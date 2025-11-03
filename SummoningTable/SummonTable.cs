using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;

public class SummonTable : MonoBehaviour
{
    public SummonManager summonManager;
    [SerializeField] private List<GameObject> currentItems = new List<GameObject>();

    public FMODUnity.EventReference foodSpawning;
    public FMOD.Studio.EventInstance foodSpawningInstance;

    public VisualEffect dissolveEffect;

    private void Start()
    {
        if (dissolveEffect != null)
            dissolveEffect.Stop();
        foodSpawningInstance = FMODUnity.RuntimeManager.CreateInstance(foodSpawning);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Ingredient"))
        {
            if(other.gameObject.transform.parent != null)
            {
                Debug.Log("add parent");
                currentItems.Add(other.gameObject.transform.parent.gameObject);
            }
            else
            {
                Debug.Log("add this");
                currentItems.Add(other.gameObject);
            }
            CheckSummoning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            if (other.gameObject.transform.parent != null)
            {
                currentItems.Remove(other.gameObject.transform.parent.gameObject);
            }
            else
            {
                Debug.Log("add this");
                currentItems.Remove(other.gameObject);
            }
        }
    }

    private void CheckSummoning()
    {
        GameObject resultPrefab = summonManager.CheckSummoning(currentItems);
        if (resultPrefab != null)
        {
            Summon(resultPrefab);
            Debug.Log("sum");
        }
    }

    private void Summon(GameObject resultPrefab)
    {
        Debug.Log("ins");

        StartCoroutine(ProcessItems(resultPrefab));
    }

    private IEnumerator ProcessItems(GameObject resultPrefab)
    {
        List<Dissolving> dissolvings = new List<Dissolving>();
        List<bool> dissolveCompleteFlags = new List<bool>();

        if (dissolveEffect != null)
            dissolveEffect.Play();

        foreach (GameObject child in currentItems)
        {
            Debug.Log(child.name);
            Dissolving dissolving = child.GetComponent<Dissolving>();
            if (dissolving != null)
            {
                dissolvings.Add(dissolving);
                dissolveCompleteFlags.Add(false);

                dissolving.OnDissolveComplete = () =>
                {
                    int index = dissolvings.IndexOf(dissolving);
                    dissolveCompleteFlags[index] = true;
                };
                Dissolving.StartDissolveObject(child);
            }
            else
            {
                dissolving = child.GetComponentInChildren<Dissolving>(); 
            }
        }

        bool allDissolved = false;
        while (!allDissolved)
        {
            allDissolved = dissolveCompleteFlags.All(flag => flag);
            yield return null;
        }

        foreach (GameObject child in currentItems)
            if (child != null)
                Destroy(child);

        currentItems.Clear();

        GameObject obj = Instantiate(resultPrefab, transform.position, transform.rotation);
        obj.name = resultPrefab.name;
        foodSpawningInstance.start();

    }


}