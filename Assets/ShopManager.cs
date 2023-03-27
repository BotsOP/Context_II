using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ShopManager : MonoBehaviour
{
    public FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;
    public FMOD.Studio.EventInstance instance2;
    public FMODUnity.EventReference fmodEvent2;
    public int goodShop = 0;
    public int badShop = 0;

    public Dictionary<string, ShopScript> GoodShopDict= new Dictionary<string, ShopScript>();
    public Dictionary<string, ShopScript> BadShopDict = new Dictionary<string, ShopScript>();

    public ShopScript CurrentShop;

    private void Start()
    {
        CurrentShop = null;
        StartCoroutine(waitAndCheckContents());
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance2 = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);
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
                if (BadShopDict.ContainsValue(CurrentShop))
                {
                    badShop++;
                    instance.setParameterByName("BCFeedback", badShop);
                    instance.start();
                }
                else if (GoodShopDict.ContainsValue(CurrentShop))
                {
                    goodShop++;
                    instance2.setParameterByName("WSFeedback", goodShop);
                    instance2.start();
                }
                break;
            case 4:
                CurrentShop.CloseShopWindow();
                break;

        }
    }

}
