using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DishRecipe
{
    public string dishName;
    public string taste;
    public GameObject dishPrefab;
    public List<GameObject> requiredItems;
}
