using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoisonRecipe
{
    public string poisonName;
    public string effect;
    public List<GameObject> requiredItems;

    public PoisonRecipe(string n, string f, List<GameObject> r)
    {
        poisonName = n;
        effect = f;
        requiredItems = r;
    }
}
