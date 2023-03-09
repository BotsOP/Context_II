using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    public string ShopName;
    public string ItemName;
    public int stockAmount;
    public float basePrice;
    public float finalPrice;
    public bool isBadShop;
    public ShopManager shopManager;
    private void Start()
    {
        finalPrice = basePrice;
        if (isBadShop)
        {
            shopManager.BadShopDict.Add(ShopName, this); //does 'this' work?
        }
        else
        {
            shopManager.GoodShopDict.Add(ShopName, this);
        }
    }
}
