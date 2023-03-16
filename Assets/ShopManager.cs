using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Dictionary<string, ShopScript> GoodShopDict= new Dictionary<string, ShopScript>();
    public Dictionary<string, ShopScript> BadShopDict = new Dictionary<string, ShopScript>();

    public ShopScript CurrentShop;

    private void Start()
    {
        CurrentShop = null;
        StartCoroutine(waitAndCheckContents());
    }

    private IEnumerator waitAndCheckContents()
    {
         yield return new WaitForSeconds(2f);
        foreach(KeyValuePair<string, ShopScript> shop in GoodShopDict)
        {
            Debug.Log(shop.Key);
        }
        foreach (KeyValuePair<string, ShopScript> shop in BadShopDict)
        {
            Debug.Log(shop.Key);
        }
    }

    public void ClickButton(int button)
    {
        switch (button)
        {
            case 1:
                CurrentShop.AddToBasket();
                break;
            case 2:
                CurrentShop.RemoveFromBasket();
                break;
            case 3:
                CurrentShop.ConfirmOrder();
                break;
            case 4:
                CurrentShop.CloseShopWindow();
                break;

        }
    }
}
