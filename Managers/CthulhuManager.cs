using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CthulhuManager : MonoBehaviour
{
    public static CthulhuManager instance { get; private set; }

    private List<PoisonRecipe> allPoisonRecipes;
    public List<Requirement> RequirementsList;
    [SerializeField] public List<string> attributes = new List<string> {"Beard", "Hat", "Glasses", "Mustache"};
    [SerializeField] private List<string> effects = new List<string> {"Cry","Morph into a fish" }; 
    
    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }

    void Start()
    {
        allPoisonRecipes = PoisonManager.instance.recipes;
        GenerateRequirements();
    }

    public List<Requirement> GenerateRequirements()
    {
        RequirementsList = new List<Requirement>();
        List<string> chosenAttributes = new List<string>();

        while (chosenAttributes.Count < Random.Range(3,5))
        {
            string attribute = attributes[Random.Range(0, attributes.Count)];
            if (!chosenAttributes.Contains(attribute))
            {
                chosenAttributes.Add(attribute);
            }
        }

        foreach (string attribute in chosenAttributes)
        {
            string effect = effects[Random.Range(0, effects.Count)];
            PoisonRecipe recipe = FindCorrespondingRecipe(effect);
            RequirementsList.Add(new Requirement(attribute, effect, recipe));
        }

        return RequirementsList;
    }
    private PoisonRecipe FindCorrespondingRecipe(string effect)
    {
        foreach (PoisonRecipe recipe in allPoisonRecipes)
            if (recipe.effect == effect)
                return recipe;
        return null;
    }

}
