using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager instance { get; private set; }
    [SerializeField] private TextMeshProUGUI requirementShown;
    [SerializeField] private TextMeshProUGUI orderShown;
    [SerializeField] private TextMeshProUGUI timeShown;
    [SerializeField] private TextMeshProUGUI dayShown;


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

        TutorialGameManager.StartDay.AddListener(ShowRequirements);
        TutorialGameManager.OrderTaking.AddListener(ShowCurrentOrder);
    }

    public void ShowRequirements()
    {
        List<Requirement> requirements = TutorialCthulhuManager.instance.RequirementsList;
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
        if (TutorialGameManager.instance.currentOrder.wantFood) orderShown.text += "" + TutorialGameManager.instance.currentOrder.ToString() + "\n";
        else orderShown.text += "" + TutorialGameManager.instance.currentOrder.flavor + "\n";
    }

    private void AddScratch(string text)
    {
        string[] lines = text.Split("\n");
        float height = orderShown.fontSize;
        float y = - height / 2;

        for (int i = 0; i < lines.Length; i++)
        {
            if(string.IsNullOrEmpty(lines[i])) continue;

            GameObject scratchShown = Instantiate(scratch, orderShown.transform);
            RectTransform rectTransform = scratchShown.GetComponent<RectTransform>();

            float width = GetWidth(lines[i], orderShown);
            rectTransform.sizeDelta = new Vector2(width, height/10);
            rectTransform.anchoredPosition = new Vector2(width / 2, y - (i * height));
        }
    }

    private float GetWidth(string text, TextMeshProUGUI tmp)
    {
        tmp.text = text;
        return tmp.preferredWidth;
    }
}
