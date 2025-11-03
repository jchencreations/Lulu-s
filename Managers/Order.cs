using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public DishRecipe dish;
    public string flavor;
    public bool wantFood;
    public string attribute;

    public Order(DishRecipe dish, bool wantFood, string flavor)
    {
        this.dish = dish;
        this.wantFood = wantFood;
        this.flavor = flavor;
    }

    public void SetAttribute(string att)
    {
        this.attribute = att;
    }

    public override string ToString()
    {
        return dish.dishName;
    }
}
