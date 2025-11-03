using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    public static CustomerGenerator instance { get; private set;}
    [SerializeField] private List<Canvas> fishList;
    [SerializeField] private List<Canvas> glassList;
    [SerializeField] private List<Canvas> hatList;
    [SerializeField] private List<Canvas> beardList;
    [SerializeField] private List<Canvas> mustacheList;

    [SerializeField] private List<Canvas> clothesList;

    [SerializeField] private List<Canvas> animationList;
    private bool playing = false;
    private string currentAnim;

    private int fishRand = 0;

    //initializing FMOD shits
    public FMODUnity.EventReference orderSound, angrySound,crySound;
    private FMOD.Studio.EventInstance orderSoundInstance, angrySoundInstance,crySoundInstance;
    public FMODUnity.EventReference diaPopOut;
    private FMOD.Studio.EventInstance diaPopOutIns;

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance=this;
    }

    private void Start()
    {
        HideAllShit();
        //initializing FMOD shits
        orderSoundInstance = FMODUnity.RuntimeManager.CreateInstance(orderSound);
        angrySoundInstance = FMODUnity.RuntimeManager.CreateInstance(angrySound);
        crySoundInstance = FMODUnity.RuntimeManager.CreateInstance(crySound);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(angrySoundInstance, transform);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(orderSoundInstance, transform);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(crySoundInstance, transform);

    }

    public void GenerateCustomers()
    {
        foreach(Order order in OrderManager.instance.orders)
        {
            List<string> AllAttributes = CthulhuManager.instance.attributes;
            string attribute = AllAttributes[Random.Range(0, AllAttributes.Count)];
            order.SetAttribute(attribute);
        }
    }

    public void DisplayCustomer(string attribute)
    {
        Debug.Log(attribute);

        HideAllShit();
        fishRand = Random.Range(0, fishList.Count);
        Debug.Log(fishRand);

        Canvas fish = fishList[fishRand];
        fish.enabled = true;

        switch(attribute)
        {
            case ("Beard"):
                Canvas beard = beardList[fishRand];
                beard.enabled = true;
                break;
            case ("Hat"):
                Canvas hat = hatList[fishRand];
                hat.enabled = true;
                break;
            case ("Glasses"):
                Canvas glass = glassList[fishRand];
                glass.enabled = true;
                break;
            case ("Mustache"):
                Canvas mustache = mustacheList[fishRand];
                mustache.enabled = true;
                break;
            default:
                break;
        }

        int rand = Random.Range(0,clothesList.Count);
        Canvas clothe = clothesList[rand];
        clothe.enabled = true;

        // play order sound
        orderSoundInstance.setParameterByName("Face", fishRand);
        orderSoundInstance.start();

    }

    public void DisplayEffect(GameObject dish)
    {
        Debug.Log("Displaying");
        if(dish.GetComponent<PoisonStorer>().poisonRecipe.poisonName != "Water")
        {
            Debug.Log(dish.GetComponent<PoisonStorer>().poisonRecipe.effect);
            Debug.Log(animationList[fishRand].GetComponentInChildren<Animator>().name);

            currentAnim = dish.GetComponent<PoisonStorer>().poisonRecipe.effect;
            animationList[fishRand].gameObject.GetComponentInChildren<Animator>().SetBool(currentAnim, true);

            if(currentAnim == "Morph to a fry") HideAllShit();
            else if(currentAnim == "Cry")
            {
                crySoundInstance.setParameterByName("Face", fishRand);
                crySoundInstance.start();
            }
            playing = true;
        }
    }

    public void HideEffect()
    {
        if (playing)
        {
            Debug.Log("HideEffect");

            animationList[fishRand].gameObject.GetComponentInChildren<Animator>().SetBool(currentAnim, false);
            playing = false;
        }
    }

    private void HideShit(List<Canvas> c)
    {
        foreach(Canvas canvas in c)
        {
            canvas.enabled = false;
        }
    }
    public void HideAllShit()
    {
        HideShit(fishList);
        HideShit(glassList);
        HideShit(hatList);
        HideShit(beardList);
        HideShit(mustacheList);
        HideShit(clothesList);
    }

    public void EmitAngrySound()
    {
        angrySoundInstance.setParameterByName("Face", fishRand);
        angrySoundInstance.start();
    }

}
