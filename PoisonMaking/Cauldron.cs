using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField] private List<GameObject> currentItems = new List<GameObject>();

    //[SerializeField] private List<Material> materialList;

    [SerializeField] private bool match = false;
    [SerializeField] private bool created = false;
    private PoisonRecipe createdPoisonRecipe = null;

    [SerializeField] MeshRenderer rend;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material correctMaterial;
    [SerializeField] private List<Material> materialList;

    [SerializeField] private MeshRenderer ladleRend;
    private bool ladleEntered;

    [SerializeField] private Light light;

    private void Start()
    {
        ladleRend.enabled = false;
        light.intensity = 0.2f;
    }

    public void AddPoison(GameObject other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Poison") && !created)
        {
            switch (other.name)
            {
                case "SulfuricAcid":
                    rend.material = materialList[0];
                    break;
                case "TartaricAcid":
                    rend.material = materialList[1];
                    break;
                case "Mercury":
                    rend.material = materialList[2];
                    break;
                case "ChloricAcid":
                    rend.material = materialList[3];
                    break;
                default:
                    break;
            }
            Debug.Log("poison");
            if (!currentItems.Contains(other))
            {
                currentItems.Add(other);
            }

            match = PoisonManager.instance.CheckSummoning(currentItems);
        }
        else if (other.CompareTag("PoisonBinder"))
        {
            if (created)
            {
                //If you created something, then reset to normal w binder
                ResetWater();
            }
            else if (match)
            {
                //Get the right recipe
                createdPoisonRecipe = PoisonManager.instance.GetCurrRecipe();
                rend.material = correctMaterial;
                created = true;
                light.intensity = 0.6f;
                Debug.Log("Created: " + created);
            }
            else
            {
                //If you bind a non-match, reset to normal w binder
                ResetWater();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + other.tag);
        if (other.CompareTag("Ladle"))
        {
            if (!ladleEntered)
            {
                ladleEntered = true;
                ladleRend.enabled = true;
                ladleRend.material = rend.material;

                other.GetComponentInParent<LadlePourDetector>().isEmpty = false;

                Debug.Log(other.tag);

                if (created) other.GetComponentInParent<PoisonStorer>().poisonRecipe = createdPoisonRecipe;
                else other.GetComponentInParent<PoisonStorer>().Reset();

                StartCoroutine(LadleTimer());
            }
        }
    }

    private IEnumerator LadleTimer()
    {
        yield return new WaitForSeconds(1f);
        ladleEntered = false;
    }

    public void ResetWater()
    {
        rend.material = normalMaterial;
        currentItems.Clear();
        match = false;
        created = false;
        createdPoisonRecipe = null;
        light.intensity = 0.2f;
    }

}
