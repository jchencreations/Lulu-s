using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStorer : MonoBehaviour
{
    public PoisonRecipe poisonRecipe;
    private PoisonRecipe defaultRecipe;

    private void Start()
    {
        defaultRecipe = new PoisonRecipe("Water", "Water", null);
        Reset();
    }
    public void Reset()
    {
        poisonRecipe = defaultRecipe;
    }
}
