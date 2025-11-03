using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{
    public static TutorialGameManager instance { get; private set; }
    public GameState currentState;

    public bool _served = false;

    [SerializeField] public Dictionary<GameObject, int> currentIngredients;

    public List<Order> currentOrders;
    public Order currentOrder;

    public static UnityEvent StartDay = new UnityEvent();
    public static UnityEvent OrderTaking = new UnityEvent();

    public FMODUnity.EventReference BGM_normal, potion_fail, dish_fail;

    public FMOD.Studio.EventInstance BGM_normal_instance, potion_fail_ins, dish_fail_ins;

    

    


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
        }
    }
    private void Start()
    {
        BGM_normal_instance = FMODUnity.RuntimeManager.CreateInstance(BGM_normal);
        potion_fail_ins = FMODUnity.RuntimeManager.CreateInstance(potion_fail);
        dish_fail_ins = FMODUnity.RuntimeManager.CreateInstance(dish_fail);

        StartCoroutine(MenuTimer());
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
        }
    }

    private IEnumerator MenuTimer()
    {
        yield return new WaitForSeconds(3);
        StateChanged(GameState.DayStart);
    }

    private void DayStart()
    {
        BGM_normal_instance.start();

        if (currentIngredients != null) currentIngredients.Clear();

        currentOrders = TutorialOrderManager.instance.GetOrders();
        currentIngredients = TutorialOrderManager.instance.GetAllIngredients();

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
        //Display Customers
        currentOrder = currentOrders[0];
        TutorialCustomerGenerator.instance.DisplayCustomer(currentOrder.attribute);
        yield return new WaitForSeconds(3f);

        OrderTaking?.Invoke();
        StateChanged(GameState.Cook);
    }

    private void Cook()
    {

    }

    private void ServeDish()
    {
        _served = true;

        TutorialCustomerGenerator.instance.DisplayEffect(TutorialDeliveryTable.instance.GetDish());
        if (!TutorialDeliveryTable.instance.CheckCthulhu(currentOrder)) potion_fail_ins.start(); //Play dialogue for failed poison
        else if (!TutorialDeliveryTable.instance.CheckCustomer(currentOrder)) dish_fail_ins.start();// Play dialogue for failed dish

        StartCoroutine(ServeTimer());
    }

    private IEnumerator ServeTimer()
    {
        Debug.Log("ServeTimer");
        yield return new WaitForSeconds(5f);
        TutorialCustomerGenerator.instance.HideEffect();


        if (!CheckCthulhu() || !CheckCustomer())
            StateChanged(GameState.ServeFailed);
        else StateChanged(GameState.CustomerLeave);
    }

    private void ServeFailed() 
    {
        Debug.Log("ServeFailed");
        ReloadScene();
    }

    private void CustomerLeave()
    {
        TutorialCustomerGenerator.instance.HideAllShit();

        BGM_normal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneTransitionManager.instance.LoadScene(1); //go to second menu scene
    }

    private bool CheckCthulhu()
    {
        return TutorialDeliveryTable.instance.GetCheckCthulhu(currentOrder);
    }

    private bool CheckCustomer()
    {
        return TutorialDeliveryTable.instance.GetCheckCustomer(currentOrder);
    }
    private void ReloadScene()
    {
        BGM_normal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
       SceneTransitionManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        BGM_normal_instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
