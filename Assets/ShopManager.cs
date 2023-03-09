using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Dictionary<string, ShopScript> GoodShopDict= new Dictionary<string, ShopScript>();
    public Dictionary<string, ShopScript> BadShopDict = new Dictionary<string, ShopScript>();

    private void Start()
    {
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
}
