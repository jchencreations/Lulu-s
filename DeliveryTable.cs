using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DeliveryTable : MonoBehaviour
{
    public static DeliveryTable instance {  get; private set; }
    private GameObject myCollision;
    public bool isServing = false;

    private void Awake()
    {
        if(instance) Destroy(this.gameObject);
        else instance = this;
    }
    void Start()
    {
        isServing = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(isServing);
        if (!isServing)
        {
            if (collision.gameObject.CompareTag("Dish") && collision.gameObject.GetComponent<PoisonStorer>() != null)
            {
                isServing = true;
                myCollision = collision.gameObject;
                Debug.Log(myCollision.name);
                collision.gameObject.SetActive(false);

                GameManager.instance.StateChanged(GameManager.GameState.ServeDish);
            }
        }
    }

    public GameObject GetDish()
    {
        return myCollision;
    }

    private bool CheckCthulhu(Order order)
    {
        foreach (Requirement req in CthulhuManager.instance.RequirementsList)
        {
            if (req.attribute == order.attribute)
            {
                Debug.Log("Delivery Requirement Effect: " + req.poisonRecipe.effect);
                Debug.Log("Stored poison: " + myCollision.name);
                return req.poisonRecipe.poisonName == myCollision.GetComponent<PoisonStorer>().poisonRecipe.poisonName;
            }
        }

        Debug.Log("Attribute has no req");
        if(myCollision.GetComponent<PoisonStorer>().poisonRecipe.poisonName == "Water")
        {
            return true;
        }
        return false;
    }

    private bool CheckCustomer(Order order)
    {
        if (order.wantFood)
        {
            Debug.Log(myCollision.name);
            return myCollision.name == order.dish.dishName;

        }
        else
        {
            Debug.Log(GetDishFlavor());
            return order.flavor == GetDishFlavor();
        }

    }

    public bool GetCheckCthulhu(Order order)
    {
        return CheckCthulhu(order);
    }

    public bool GetCheckCustomer(Order order)
    {
        isServing = false;
        bool b = CheckCustomer(order);

        Destroy(myCollision);

        return b;
    }

    private string GetDishFlavor()
    {
        foreach (DishRecipe recipe in SummonManager.instance.allDishes)
            if (recipe.dishName == myCollision.name)
                return recipe.taste;
        return null;
    }
}
