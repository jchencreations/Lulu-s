using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOrderManager : MonoBehaviour
{
    public static TutorialOrderManager instance { get; private set; }
    private List<DishRecipe> allDishes;

    [SerializeField] public List<Order> orders;
    [SerializeField] private Dictionary<GameObject, int> totalIngredients;
    [SerializeField] private Dictionary<GameObject, int> neededIngredients;

    [SerializeField] private DishRecipe tutorialDish;
    [SerializeField] private List<GameObject> tutorialIngredients;

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }
    void Start()
    {
        allDishes = SummonManager.instance.allDishes;
        StartGeneratingOrders();
    }

    public void StartGeneratingOrders()
    {
        Debug.Log("generaterandom");
        orders = new List<Order>{new Order(tutorialDish, true, tutorialDish.taste)};
        neededIngredients = new Dictionary<GameObject, int>(tutorialIngredients.Count);
        foreach(GameObject ing in tutorialIngredients)
        {
            neededIngredients[ing] = 1;
            neededIngredients[ing] += 2;
        }

        //CustomerGenerator
        TutorialCustomerGenerator.instance.GenerateCustomers();

        Debug.Log(orders[0]);
    }

    public List<Order> GetOrders()
    {
        return orders;
    }

    public Dictionary<GameObject, int> GetAllIngredients()
    {
        return neededIngredients;
    }
}


