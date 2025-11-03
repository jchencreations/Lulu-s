using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialDeliveryTable : MonoBehaviour
{
    public static TutorialDeliveryTable instance {  get; private set; }
    private GameObject myCollision;
    private bool isServing = false;

    private void Awake()
    {
        if(instance) Destroy(this.gameObject);
        else instance = this;
    }
    private void Start()
    {
        isServing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServing)
        {
            if (collision.gameObject.CompareTag("Dish") && collision.gameObject.GetComponent<PoisonStorer>() != null)
            {
                isServing = true;
                myCollision = collision.gameObject;
                Debug.Log(myCollision.name);
                collision.gameObject.SetActive(false);

                TutorialGameManager.instance.StateChanged(TutorialGameManager.GameState.ServeDish);
            }
        }
    }

    public GameObject GetDish()
    {
        return myCollision;
    }

    public bool CheckCthulhu(Order order)
    {
        foreach (Requirement req in TutorialCthulhuManager.instance.RequirementsList)
        {
            if (req.attribute == order.attribute)
            {
                Debug.Log("Delivery Requirement Effect: " + req.poisonRecipe.effect);
                Debug.Log("Stored poison: " + myCollision.name);
                return req.poisonRecipe.poisonName == myCollision.GetComponent<PoisonStorer>().poisonRecipe.poisonName;
            }
        }

        Debug.Log("Attribute has no req");
        if (myCollision.GetComponent<PoisonStorer>().poisonRecipe.poisonName == "Water")
        {
            return true;
        }
        return false;
    }

    public bool CheckCustomer(Order order)
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
        bool b = CheckCustomer(order);

        isServing = false;
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
