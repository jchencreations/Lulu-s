using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    public TutorialData tutorialData;
    public List<Button> sectionButtonsUnselected;
    public List<Button> sectionButtonsSelected;
    public GameObject imgMenu, imgPrev, imgNext, imgPinch, imgPointer;
    public GameObject bookCursor, sommonCursor, cauldronCursor, shelfCursor, boardCursor, orderCursor, deliverCursor;
    public GameObject poison1, poison2a, poison2b, poison3;
    public List<GameObject> imgs = new List<GameObject>();  


    public TextMeshProUGUI pageTitleText;
    public TextMeshProUGUI contentText;
    public Button prevButton;
    public Button nextButton;

    private int currentSectionIdx = 0;
    private int currentPageIdx = 0;

    public Button exitButton;
    private void Start()
    {
        InitializeSections();
        DisplaySection(0);
    }

    private void InitializeSections()
    {
        for (int i = 0; i < tutorialData.sections.Count; i++)
        {
            int index = i;
            Button buttonUnselected = sectionButtonsUnselected[i];
            Button buttonSelected = sectionButtonsSelected[i];

            buttonUnselected.GetComponentInChildren<TextMeshProUGUI>().text = tutorialData.sections[i].sectionTitle;
            buttonSelected.GetComponentInChildren<TextMeshProUGUI>().text = tutorialData.sections[i].sectionTitle;

            buttonUnselected.onClick.AddListener(() => OnSectionButtonClicked(index));

            buttonSelected.gameObject.SetActive(false);
            buttonUnselected.gameObject.SetActive(true);
        }

        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    public void DisplaySection(int index)
    {
        foreach (var button in sectionButtonsSelected)
            button.gameObject.SetActive(false);
        foreach (var button in sectionButtonsUnselected)
            button.gameObject.SetActive(true);

        sectionButtonsUnselected[index].gameObject.SetActive(false);
        sectionButtonsSelected[index].gameObject.SetActive(true);

        currentSectionIdx = index;
        currentPageIdx = 0;
        UpdateContent();
    }

    public void OnSectionButtonClicked(int index)
    {
        if (index != currentSectionIdx)
            DisplaySection(index);
    }

    public void OnPrevButtonClicked()
    {
        var currentSection = tutorialData.sections[currentSectionIdx];

        if (currentPageIdx > 0)
        {
            currentPageIdx--;
            UpdateContent();
        }
        else if (currentSectionIdx > 0)
        {
            currentSectionIdx--;
            var previousSection = tutorialData.sections[currentSectionIdx];
            currentPageIdx = previousSection.pages.Count - 1;
            DisplaySection(currentSectionIdx);
        }
    }

    public void OnNextButtonClicked()
    {
        var currentSection = tutorialData.sections[currentSectionIdx];

        if (currentPageIdx < currentSection.pages.Count - 1)
        {
            currentPageIdx++;
            UpdateContent();
        }
        else if (currentSectionIdx < tutorialData.sections.Count - 1)
        {
            currentSectionIdx++;
            DisplaySection(currentSectionIdx);
        }
    }

    private void UpdateContent()
    {
        var currentPage = tutorialData.sections[currentSectionIdx].pages[currentPageIdx];
        pageTitleText.text = currentPage.title;
        contentText.text = currentPage.content;
        prevButton.interactable = currentPageIdx > 0 || currentSectionIdx > 0;
        nextButton.interactable = currentPageIdx < tutorialData.sections[currentSectionIdx].pages.Count - 1 || currentSectionIdx < tutorialData.sections.Count - 1;
        
        this.imgMenu.gameObject.SetActive(currentPage.imgMenu);
        this.imgPrev.gameObject.SetActive(currentPage.imgPrev);
        this.imgNext.gameObject.SetActive(currentPage.imgNext);
        this.imgPinch.SetActive(currentPage.imgPinch);
        this.imgPointer.SetActive(currentPage.imgPointer);


        this.bookCursor.gameObject.SetActive(currentPage.bookCursor);
        this.sommonCursor.gameObject.SetActive(currentPage.sommonCursor);
        this.shelfCursor.gameObject.SetActive(currentPage.shelfCursor);
        this.orderCursor.gameObject.SetActive(currentPage.orderCursor);
        this.cauldronCursor.gameObject.SetActive(currentPage.cauldronCursor);
        this.deliverCursor.gameObject.SetActive(currentPage.deliverCursor);
        this.boardCursor.gameObject.SetActive(currentPage.boardCursor);

        this.poison1.gameObject.SetActive(currentPage.poison1);
        this.poison2a.gameObject.SetActive(currentPage.poison2a);
        this.poison2b.gameObject.SetActive(currentPage.poison2b);
        this.poison3.gameObject.SetActive(currentPage.poison);

        exitButton.gameObject.SetActive(currentPage.showExitButton);
    }
}
