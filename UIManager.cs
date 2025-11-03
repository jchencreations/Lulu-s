using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    [SerializeField] private TextMeshProUGUI requirementShown;
    [SerializeField] private TextMeshProUGUI orderShown;
    [SerializeField] private TextMeshProUGUI timeShown;

    [SerializeField] private TextMeshProUGUI orderOne;
    [SerializeField] private TextMeshProUGUI orderTwo;
    [SerializeField] private TextMeshProUGUI orderThree;
    [SerializeField] private TextMeshProUGUI orderFour;
    [SerializeField] private TextMeshProUGUI orderFive;
    [SerializeField] private GameObject scratchImage;


    [SerializeField] private GameObject sanityParent;
    private List<GameObject> sanityList = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI dayShown;
    [SerializeField] private GameObject dayScratch;


    [SerializeField] private GameObject scratch;
    private List<string> displayedOrders = new List<string>();


    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }

    void Start()
    {
        orderShown.text = "";
        dayScratch.SetActive(false);

        sanityList.Clear();
        for(int i = 0; i<sanityParent.transform.childCount; i++)
        {
            sanityList.Add(sanityParent.transform.GetChild(i).gameObject);
        }

        GameManager.StartDay.AddListener(ShowRequirements);
        GameManager.OrderTaking.AddListener(ShowCurrentOrder);
    }

    public void ShowRequirements()
    {
        List<Requirement> requirements = CthulhuManager.instance.RequirementsList;
        requirementShown.text = "";
        Debug.Log("get requirements");
        int i = 1;
        foreach (Requirement requirement in requirements)
        {
            requirementShown.text += "" + i + ". with " + requirement.attribute + "\n  Effect: " + requirement.effect + "\n";
            i++;
        }
    }

    public void ShowCurrentOrder()
    {

        //if (GameManager.instance.currentOrder.wantFood) orderShown.text += "" + GameManager.instance.currentOrder.ToString() + "\n";
        //else orderShown.text += "" + GameManager.instance.currentOrder.flavor + "\n";

        int currentOrderIdx = GameManager.instance.currentOrderIdx;
        Order currentOrder = GameManager.instance.currentOrder;

        string orderText = currentOrder.wantFood ? currentOrder.dish.dishName : currentOrder.flavor;

        switch (currentOrderIdx) {
            case 0:
                orderOne.text = orderText;
                break;
            case 1:
                orderTwo.text = orderText;
                break;
            case 2:
                orderThree.text = orderText;
                break;
            case 3:
                orderFour.text = orderText;
                break;
            case 4: 
                orderFive.text = orderText;
                break;
        }
    }

    public void ScratchOrder(int orderIndex)
    {
        TextMeshProUGUI orderText = null;

        switch (orderIndex)
        {
            case 0:
                orderText = orderOne;
                break;
            case 1:
                orderText = orderTwo;
                break;
            case 2:
                orderText = orderThree;
                break;
            case 3:
                orderText = orderFour;
                break;
            case 4:
                orderText = orderFive;
                break;
        }

        if (orderText != null)
        {
            GameObject scratchInstance = Instantiate(scratchImage, orderText.transform.parent);
            RectTransform orderRectTransform = orderText.GetComponent<RectTransform>();
            RectTransform scratchRectTransform = scratchInstance.GetComponent<RectTransform>();

            scratchRectTransform.position = orderRectTransform.position;
            //scratchRectTransform.sizeDelta = orderRectTransform.sizeDelta;

            scratchImage.gameObject.SetActive(true);
        }
    }


    public void ShowTime(float time)
    {
        timeShown.text = time.ToString();
    }

    public void UpdateSanity(int sanity)
    {
        foreach(GameObject c in sanityList)
        {
            c.SetActive(false);
        }
        int sanityInt = sanity / 10;
        sanityList[sanityInt].SetActive(true);
    }

    public void UpdateDay(int day)
    {
        string dashes = "";
        for(int i = 0; i < day; i++)
        {
            dashes += "| ";
        }
        dayShown.text = "Day:\n" + dashes;
        if(day == 5)
        {
            dayScratch.SetActive(true);
        }

    }
}
