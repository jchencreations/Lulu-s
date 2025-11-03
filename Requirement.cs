using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Requirement
{
    public string attribute;
    public string effect;
    public PoisonRecipe poisonRecipe;

    public Requirement(string attribute, string effect, PoisonRecipe poisonRecipe)
    {
        this.attribute = attribute;
        this.effect = effect;
        this.poisonRecipe = poisonRecipe;
    }
}
