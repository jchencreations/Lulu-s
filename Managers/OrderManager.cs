using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance { get; private set; }
    private List<DishRecipe> allDishes;

    public List<Order> orders;
    [SerializeField] private Dictionary<GameObject, int> totalIngredients;
    [SerializeField] private Dictionary<GameObject, int> neededIngredients;

    [SerializeField] private string[] flavors = {"Salty", "Sour", "Sweet", "Spicy"};
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
        orders = GenerateRandomOrders(5);

        //CustomerGenerator
        CustomerGenerator.instance.GenerateCustomers();

        for(int i = 0; i < orders.Count; i++)
        {
            Debug.Log(orders[i]);
        }
        Debug.Log("generateIng");
        totalIngredients = getNumIngredients(orders);
        
        Debug.Log("addNum");
        neededIngredients = AddNumofIngredients();
    }

    private List<Order> GenerateRandomOrders(int num)
    {
        orders = new List<Order>();
        for (int i = 0; i < num; i++)
        {
            DishRecipe chosenDish = allDishes[Random.Range(0, allDishes.Count)];
            string chosenFlavor = flavors[Random.Range(0, flavors.Length)];

            if (Random.value < 0.5f)
                orders.Add(new Order(chosenDish, true, chosenDish.taste));
            else
            {
                List<DishRecipe> dishesWithChosenFlavor = allDishes.FindAll(dish => dish.taste == chosenFlavor);
                if (dishesWithChosenFlavor.Count > 0)
                {
                    DishRecipe dish = dishesWithChosenFlavor[Random.Range(0, dishesWithChosenFlavor.Count)];
                    orders.Add(new Order(dish, false, chosenFlavor));
                }
                else
                    orders.Add(new Order(chosenDish, true, chosenDish.taste));
            }
        }
        return orders;
    }
    
    private Dictionary<GameObject, int> getNumIngredients(List<Order> orders)
    {
        totalIngredients = new Dictionary<GameObject, int>();

        foreach (Order order in orders)
        {
            foreach (GameObject item in order.dish.requiredItems)
            {
                if (totalIngredients.ContainsKey(item))
                    totalIngredients[item]++;
                else totalIngredients[item] = 1;
            }
        }

        return totalIngredients;
    }

    private Dictionary<GameObject, int> AddNumofIngredients()
    {
        neededIngredients = new Dictionary<GameObject, int>(totalIngredients);
        foreach(var ingredient in totalIngredients)
            neededIngredients[ingredient.Key] += Random.Range(1, 3);

        return neededIngredients;
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


