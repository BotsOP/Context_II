using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public float susFuelPriceAdjustment; //sustainable fuel, adjustments are added to the standard price in the shops
    public float badFuelPriceAdjustment;

    public float baseResourceValue;

    public GameManager gameManager;
    public ShopManager shopManager;

    private int PreviousState;

    private void Start()
    {
        PreviousState= 0;
    }

    public void UpdatePriceAdjustmentValues(int ProgressState)
    {
        switch (ProgressState)
        {
            case -2:
                badFuelPriceAdjustment = -2.51f;
                susFuelPriceAdjustment = 0;
                break;

            case -1:
                badFuelPriceAdjustment = -1.01f;
                susFuelPriceAdjustment = 0;

                if(PreviousState == -2)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState= ProgressState;
                break;

            case 0:
                badFuelPriceAdjustment = 0;
                susFuelPriceAdjustment = 0;

                if (PreviousState == -1)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState = ProgressState;
                break;

            case 1:
                badFuelPriceAdjustment = 0;
                susFuelPriceAdjustment = -2.5f;
                if (PreviousState == 0)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState = ProgressState;
                break;

            case 2:
                badFuelPriceAdjustment = 0.99f;
                susFuelPriceAdjustment = -5f;
                if (PreviousState == 1)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState = ProgressState;
                break;

            case 3:
                badFuelPriceAdjustment = 0.99f;
                susFuelPriceAdjustment = -7.5f;
                if (PreviousState == 2)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState = ProgressState;
                break;

            case 4:
                badFuelPriceAdjustment = 2.49f;
                susFuelPriceAdjustment = -11.5f;
                if (PreviousState == 3)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState = ProgressState;
                break;

            case 5:
                badFuelPriceAdjustment = 4.99f;
                susFuelPriceAdjustment = -11.5f;
                if (PreviousState == 4)
                {
                    RestockGoodShops();
                }
                else
                {
                    RestockBadShops();
                }
                PreviousState = ProgressState;
                break;
        }
        foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
        {
            shop.Value.finalPrice = shop.Value.basePrice + susFuelPriceAdjustment;
        }
        foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
        {
            shop.Value.finalPrice = shop.Value.basePrice + badFuelPriceAdjustment;
        }

    }

    public void RestockGoodShops()
    {
        switch (gameManager.CurrentProgressState)
        {
            case -1:
               foreach(KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 5;
                }
                break;

            case 0:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 5;
                }
                break;

            case 1:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 10;
                }
                break;

            case 2:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 10;
                }
                break;

            case 3:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 15;
                }
                break;

            case 4:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 15;
                }
                break;

            case 5:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.GoodShopDict)
                {
                    shop.Value.stockAmount = 20;
                }
                break;
        }
    }

    public void RestockBadShops()
    {
        switch (gameManager.CurrentProgressState)
        {
            case -2:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 1000;
                }
                break;

            case -1:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 500;
                }
                break;

            case 0:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 100;
                }
                break;

            case 1:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 100;
                }
                break;

            case 2:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 50;
                }
                break;

            case 3:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 50;
                }
                break;

            case 4:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 25;
                }
                break;

            case 5:
                foreach (KeyValuePair<string, ShopScript> shop in shopManager.BadShopDict)
                {
                    shop.Value.stockAmount = 10;
                }
                break;
        }
    }
}

