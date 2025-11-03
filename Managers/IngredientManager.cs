using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class IngredientManager : MonoBehaviour
{
    public GameObject spawnedIngredient;
    public GameObject emptyVersion;
    [SerializeField] private int count = 0;
    [SerializeField] private TextMeshProUGUI countText;

    public OVRHand leftHand;
    public OVRHand rightHand;

    private bool entered = false;

    private void Start()
    {
        this.gameObject.SetActive(true);
        if(emptyVersion != null) emptyVersion.SetActive(false);

        GameManager.StartDay.AddListener(DayStart);
    }

    //Invoked by GameManager Day Start UnityEvent
    public void DayStart()
    {
        entered = false;

        Debug.Log("IngredientManager: DayStart");
        foreach (GameObject ing in GameManager.instance.currentIngredients.Keys)
        {
            Debug.Log(GameManager.instance.currentIngredients.Count);
            Debug.Log(ing.name);
            if (spawnedIngredient.name == ing.name)
            {
                count = GameManager.instance.currentIngredients[ing];
            }
        }
        if(count == 0)
        {
            Swap();
        }
        else
        {
            countText.text = count.ToString();
        }       
    }

    //Grab logic
    private void OnTriggerStay(Collider other)
    {   
        if(count > 0)
        {
            if(!entered) StartCoroutine(GrabTimer(other.gameObject));
        }
    }
    private IEnumerator GrabTimer(GameObject other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            if (leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) || rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                entered = true;

                GameObject prefab = Instantiate(spawnedIngredient, other.transform.position, Quaternion.identity);
                prefab.name = spawnedIngredient.name;
                count--;
                countText.text = count.ToString();
                if(count == 0)
                {
                    Swap();
                }

                yield return new WaitForSeconds(3f);

                entered = false;
            }

        }
    }

    private void Swap()
    {
        countText.text = null;
        this.gameObject.SetActive(false);
        if (emptyVersion != null) emptyVersion.SetActive(true);
    }
}
