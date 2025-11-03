using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameState currentState;

    private int days = 7;
    [SerializeField] private int currentDay = 0;

    [SerializeField] private int sanity = 100;
    private int currentSanity;

    [SerializeField] private int[] time = { 90, 90, 90, 80, 80, 70, 70 };
    private int currentTime;

    public bool _served = false;

    private List<Order> currentOrders;
    [SerializeField] public Dictionary<GameObject, int> currentIngredients;

    public Order currentOrder;
    public int currentOrderIdx=0;

    public static UnityEvent StartDay = new UnityEvent();
    public static UnityEvent OrderTaking = new UnityEvent();

    //FMOD shits
    public FMODUnity.EventReference BGM_normal, BGM_lessNormal, BGM_lessWeird,BGM_Weirdest;
    private FMOD.Studio.EventInstance BGM_normal_instance, BGM_lessNormal_instance, BGM_lessWeird_instance, BGM_Weirdest_Instance;
    public FMODUnity.EventReference cthBad, cthGood;
    private FMOD.Studio.EventInstance cthBadInstance, cthGoodInstance;
    public FMODUnity.EventReference cthPopOut;
    private FMOD.Studio.EventInstance cthPopOutIns;

    public IEnumerator orderTimer;

    [SerializeField] private VolumeProfile gameVolume;
    private UnityEngine.Rendering.Universal.Vignette vig;
    private UnityEngine.Rendering.Universal.ChromaticAberration abb;

    public enum GameState
    {
        Menu,
        DayStart, 
        TakeOrder, 
        Cook, 
        ServeDish, 
        ServeFailed,
        CustomerLeave, 
        DayEnd, 
        PlayerDead, 
        PlayerWin
    }

    private void Awake()
    {
        Debug.Log("Awake");
        if (instance != null && instance != this)
        {
            Debug.Log("Destroyme");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("instance exists");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        currentSanity = sanity;
        currentDay = 0;

        BGM_lessNormal_instance = FMODUnity.RuntimeManager.CreateInstance(BGM_lessNormal);
        BGM_lessWeird_instance = FMODUnity.RuntimeManager.CreateInstance(BGM_lessWeird);
        BGM_normal_instance = FMODUnity.RuntimeManager.CreateInstance(BGM_normal);
        BGM_Weirdest_Instance = FMODUnity.RuntimeManager.CreateInstance(BGM_Weirdest);

        StartCoroutine(MenuTimer());

        gameVolume.TryGet(out vig);
        gameVolume.TryGet(out abb);
    }

    public void StateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.DayStart:
                currentState = state;
                DayStart();
                break;
            case GameState.TakeOrder:
                currentState = state;
                TakeOrder();
                break;
            case GameState.Cook:
                currentState = state;
                Cook();
                break;
            case GameState.ServeDish:
                currentState = state;
                ServeDish();
                break;
            case GameState.ServeFailed:
                currentState = state;
                ServeFailed();
                break;
            case GameState.CustomerLeave:
                currentState = state;
                CustomerLeave();
                break;
            case GameState.DayEnd:
                currentState = state;
                DayEnd();
                break;
            case GameState.PlayerWin:
                currentState = state;
                PlayerWin();
                break;
            case GameState.PlayerDead:
                currentState = state;
                PlayerDead();
                break;
        }
    }

    private IEnumerator MenuTimer()
    {
        yield return new WaitForSeconds(3);
        StateChanged(GameState.DayStart);
    }

    private void DayStart()
    {
        UpdateSanity(0);

        currentDay++;
        UIManager.instance.UpdateDay(currentDay);

        ChangeBGM(sanity);

        UpdateDayTime();
        if (currentOrders != null) currentOrders.Clear();
        if (currentIngredients != null) currentIngredients.Clear();

        currentOrders = OrderManager.instance.GetOrders();
        currentIngredients = OrderManager.instance.GetAllIngredients();
        currentOrderIdx = 0;

        StartDay?.Invoke();
        
        StateChanged(GameState.TakeOrder);
    }

    private void TakeOrder()
    {
        _served = false;
        
        StartCoroutine(TakeOrderTimer());
    }

    private IEnumerator TakeOrderTimer()
    {
        currentOrder = currentOrders[currentOrderIdx];

        //Display Customers
        CustomerGenerator.instance.DisplayCustomer(currentOrder.attribute);
        yield return new WaitForSeconds(3f);

        OrderTaking?.Invoke();
        StateChanged(GameState.Cook);
    }

    private void Cook()
    {
        orderTimer = OrderTimer();
        StartCoroutine(orderTimer);
    }

    private IEnumerator OrderTimer()
    {
        int timeRemaining = currentTime;
        
        while (timeRemaining >= 0)
        {
            UIManager.instance.ShowTime(timeRemaining);
            yield return new WaitForSeconds(1f);

            timeRemaining -= 1;

            //add angry sound
            if (timeRemaining == 30 || timeRemaining == 10)
            {
                CustomerGenerator.instance.EmitAngrySound();
            }
            
        }
        StateChanged(GameState.ServeFailed);
    }

    private void ServeDish()
    {
        _served = true;
        StopCoroutine(orderTimer);

        CustomerGenerator.instance.DisplayEffect(DeliveryTable.instance.GetDish());

        StartCoroutine(ServeTimer());
    }

    private IEnumerator ServeTimer()
    {
        yield return new WaitForSeconds(5f);

        CustomerGenerator.instance.HideEffect();

        if (!CheckCthulhu() || !CheckCustomer())
            StateChanged(GameState.ServeFailed);
        else
        {
            //Cth Good Response
            int SoundRand = Random.Range(0, 5);
            cthGoodInstance.setParameterByName("Random-5", SoundRand);
            cthGoodInstance.start();

            StateChanged(GameState.CustomerLeave);
        }
    }

    private void ServeFailed() 
    {
        StopCoroutine(orderTimer);

        if (!_served) UpdateSanity(-7);
        else
        {
            if (!CheckCthulhu()) UpdateSanity(-5);
            if (!CheckCustomer()) UpdateSanity(-5);
        }

        //Cth Bad Response
        int SoundRand = Random.Range(0, 5);
        cthBadInstance.setParameterByName("Random-5", SoundRand);
        cthBadInstance.start();

        StateChanged(GameState.CustomerLeave);
    }
    private void CustomerLeave()
    {
        Debug.Log("customerleave");
        UIManager.instance.ScratchOrder(currentOrderIdx);

        currentOrderIdx++;
        CustomerGenerator.instance.HideAllShit();


        // coroutine
        StartCoroutine(NextDayOrOrderDelay());
    }

    private void DayEnd()
    {
        if (currentDay >= days)
        {
            StateChanged(GameState.PlayerWin);
            return;
        }
        UpdateSanity(-10);

        //Day Switch
        ReloadScene();
        StartCoroutine(MenuTimer());
    }
    private void PlayerWin()
    {
        //Show Win, go Menu
        Debug.Log("WINNNN!!!!!!!");
        SceneTransitionManager.instance.LoadScene(4);

        DestroyMe();
    }
    private void PlayerDead()
    {
        //Show Dead
        Debug.Log("Lose.................");
        SceneTransitionManager.instance.LoadScene(5);
        DestroyMe();
    }

    private void UpdateDayTime()
    {
        currentTime = time[currentDay - 1];
    }

    private void UpdateSanity(int amount)
    {
        sanity += amount;
        if (sanity <= 0)
        {
            sanity = 0;

            StateChanged(GameState.PlayerDead);
        }
        UIManager.instance.UpdateSanity(sanity);
        UpdateDistortion();
    }

    private bool CheckCthulhu()
    {
        return DeliveryTable.instance.GetCheckCthulhu(currentOrder);
    }

    private bool CheckCustomer()
    {
        return DeliveryTable.instance.GetCheckCustomer(currentOrder);
    }

    private void ReloadScene()
    {
        SceneTransitionManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangeBGM(int _sanity)
    {
        //if(BGM_lessNormal_instance.)

        BGM_normal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM_lessWeird_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM_lessNormal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM_Weirdest_Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        if (_sanity >= 75)
        {
            BGM_normal_instance.start();
        }
        else if (_sanity < 75 && _sanity >= 50)
        {
            BGM_lessNormal_instance.start();
        }
        else if (_sanity <50 && _sanity >= 25)
        {
            BGM_lessWeird_instance.start();
        }
        else
        {
            BGM_Weirdest_Instance.start();
        }
    }

    private IEnumerator NextDayOrOrderDelay()
    {
        yield return new WaitForSeconds(3f);

        if (currentOrderIdx >= currentOrders.Count)
            DayEnd();
        else
            StateChanged(GameState.TakeOrder);
    }

    private void UpdateDistortion()
    {
        if (sanity < 70)
        {
            float vigBlend = Mathf.Lerp(0f, 1f, (70f - sanity) / 70f);
            float abbBlend = Mathf.Lerp(0f, 0.7f, (70f - sanity) / 70f);

            vig.intensity.Override(vigBlend);
            abb.intensity.Override(abbBlend);
        }
        else
        {
            vig.intensity.Override(0);
            abb.intensity.Override(0);
        }
    }

    public void DestroyMe()
    {
        BGM_normal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM_lessWeird_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM_lessNormal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        BGM_Weirdest_Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        vig.intensity.Override(0);
        abb.intensity.Override(0);

        Destroy(this.gameObject);
    }
}
