using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCthulhuManager : MonoBehaviour
{
    public static TutorialCthulhuManager instance { get; private set; }

    private List<PoisonRecipe> allPoisonRecipes;
    public List<Requirement> RequirementsList;
    
    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }

    void Start()
    {
        allPoisonRecipes = PoisonManager.instance.recipes;
        RequirementsList = new List<Requirement>{new Requirement("Beard", "Cry", FindCorrespondingRecipe("Cry"))};
    }

    private PoisonRecipe FindCorrespondingRecipe(string effect)
    {
        foreach (PoisonRecipe recipe in allPoisonRecipes)
            if (recipe.effect == effect)
                return recipe;
        return null;
    }

}
