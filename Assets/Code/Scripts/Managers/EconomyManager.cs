using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public float susFuelPriceAdjustment; //sustainable fuel, adjustments are added to the standard price in the shops
    public float badFuelPriceAdjustment;

    public void UpdatePriceAdjustmentValues(int PrgoressState)
    {
        switch (PrgoressState)
        {
            case -2:
                badFuelPriceAdjustment = -1.51f;
                susFuelPriceAdjustment = 0;
                break;

            case -1:
                badFuelPriceAdjustment = -1.01f;
                susFuelPriceAdjustment = 0;
                break;

            case 0:
                badFuelPriceAdjustment = 0;
                susFuelPriceAdjustment = 0;
                break;

            case 1:
                badFuelPriceAdjustment = 0;
                susFuelPriceAdjustment = -2.5f;
                break;

            case 2:
                badFuelPriceAdjustment = 0.99f;
                susFuelPriceAdjustment = -5f;
                break;

            case 3:
                badFuelPriceAdjustment = 0.99f;
                susFuelPriceAdjustment = -7.5f;
                break;

            case 4:
                badFuelPriceAdjustment = 2.49f;
                susFuelPriceAdjustment = -11.5f;
                break;

            case 5:
                badFuelPriceAdjustment = 4.99f;
                susFuelPriceAdjustment = -11.5f;
                break;
        }
    }
}

