using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonManager : MonoBehaviour
{
    public static PoisonManager instance {  get; private set; }
    [SerializeField] public List<PoisonRecipe> recipes;
    [SerializeField] public PoisonRecipe normalRecipe;
    private PoisonRecipe madeRecipe;

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }
    public bool CheckSummoning(List<GameObject> items)
    {
        Debug.Log("Prepare for checking");
        if(items.Count == 2)
        {
            foreach (PoisonRecipe recipe in recipes)
            {
                if (IsMatch(recipe.requiredItems, items))
                {
                    madeRecipe = recipe;
                    return true;
                }
            }
        }    
        return false;
    }

    private bool IsMatch(List<GameObject> requiredItems, List<GameObject> items)
    {
        if (requiredItems.Count != items.Count)
            return false;
        Debug.Log(requiredItems.Count != items.Count);

        foreach (GameObject requiredItem in requiredItems)
        {
            Debug.Log("match items");
            foreach (GameObject item in items)
            {
                if (item.name.Equals(requiredItem.name))
                {
                    Debug.Log("True");
                    return true;
                }
            }
        }
        return false;
    }

    public PoisonRecipe GetCurrRecipe()
    {
        return madeRecipe;
    }
}
