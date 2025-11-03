using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public static SummonManager instance { get; private set; }
    public List<DishRecipe> allDishes;
    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }

    public GameObject CheckSummoning(List<GameObject> items)
    {
        Debug.Log("Prepare for checking");
        foreach (DishRecipe recipe in allDishes)
        {
            Debug.Log(recipe.dishName);
            if (IsMatch(recipe.requiredItems, items))
                return recipe.dishPrefab;
        }
        return null;
    }

    private bool IsMatch(List<GameObject> requiredItems, List<GameObject> items)
    {
        List<GameObject> noCopies = items.Distinct().ToList();

        if (requiredItems.Count != noCopies.Count)
            return false;

        foreach (GameObject item in requiredItems)
        {
            Debug.Log("Current item in the recipe: " + item.name);
            if (!ContainsItem(item, noCopies))
            {
                return false;
            }
            Debug.Log("Table contains " + item.name);
        }
        return true;
    }

    private bool ContainsItem(GameObject key, List<GameObject> list)
    {
        int i = 0;
        foreach(GameObject item in list)
        {
            if(item.name == key.name)
            {
                i++;
            }
        }
        return (i>0);
    }
}
