using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFlipper : MonoBehaviour
{
    private bool inArea = false;
    private bool isPlaying = false;

    [SerializeField] private GameObject animPage;
    private Animator anim;

    [SerializeField] private GameObject pages;
    private int pageNum = 0;

    [SerializeField] private GameObject book;

    //public FMODUnity.EventReference bookFlipping;
    //public FMOD.Studio.EventInstance bookFlippingInstance;

    private void Start()
    {
        animPage.SetActive(false);
        anim = animPage.GetComponent<Animator>();

        for(int i = 0; i < pages.transform.childCount; i++)
        {
            pages.transform.GetChild(i).GetComponent<Canvas>().enabled = false;
        }
        pages.transform.GetChild(0).GetComponent<Canvas>().enabled = true;
        //bookFlippingInstance = FMODUnity.RuntimeManager.CreateInstance(bookFlipping);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            inArea = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            inArea = false;
        }
    }


    public void RightFlip()
    {
        Debug.Log("RightHandFlip");
        if (inArea)
        {
            if (pageNum < pages.transform.childCount - 1)
            {
                if (!isPlaying)
                {
                    StartCoroutine(RightFlipTimer());
                }
            }
        }
    }
    private IEnumerator RightFlipTimer()
    {
        isPlaying = true;

        animPage.SetActive(true);

        pages.transform.GetChild(pageNum).GetComponent<Canvas>().enabled = false;
        book.GetComponent<bookSound>().flippingSound();

        anim.SetTrigger("FlipLeft");
        pageNum++;

        yield return new WaitForSeconds(2.1f);

        pages.transform.GetChild(pageNum).GetComponent<Canvas>().enabled = true;

        animPage.SetActive(false);

        isPlaying = false;
    }

    public void LeftFlip()
    {
        Debug.Log("LeftHandFlip");
            
        if (inArea)
        {
            if (pageNum >0)
            {
                if (!isPlaying)
                {
                    StartCoroutine(LeftFlipTimer());
                }
            }
        }
    }

    private IEnumerator LeftFlipTimer()
    {
        isPlaying=true;

        animPage.SetActive(true);

        pages.transform.GetChild(pageNum).GetComponent<Canvas>().enabled = false;
        book.GetComponent<bookSound>().flippingSound();

        anim.SetTrigger("FlipRight");
        pageNum--;

        yield return new WaitForSeconds(2.1f);

        pages.transform.GetChild(pageNum).GetComponent<Canvas>().enabled = true;

        animPage.SetActive(false);

        isPlaying = false;
    }

}
