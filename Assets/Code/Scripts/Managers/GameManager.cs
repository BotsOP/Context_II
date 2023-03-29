using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float HealedVegPercentage; //total percentage of vegetation that has been 'healed'
    public int CurrentProgressState; //ranges from -2 to 5 and marks the amount of progress the player has made
    private PaintTarget[] paintTargets;

    public EconomyManager economyManager;

    private void Awake()
    {
        paintTargets = FindObjectsOfType<PaintTarget>();
    }

    private void Update()
    {
        HealedVegPercentage = CalculateHealedVegPercentage();
        CheckProgressState();

    }

    private float CalculateHealedVegPercentage()
    {
        float vegPercentage = 0;
        foreach (var target in paintTargets)
        {
            vegPercentage += target.taintedness;
        }
        vegPercentage /= paintTargets.Length;
        return vegPercentage * 100;
    }

    private void CheckProgressState()
    {
        if(HealedVegPercentage >= 0 && HealedVegPercentage < 10)
        {
            //trigger bad ending
            return;
        }
        else if (HealedVegPercentage >= 10 && HealedVegPercentage < 20)
        {
            CurrentProgressState = -2; 
        }
        else if (HealedVegPercentage >= 20 && HealedVegPercentage < 30)
        {
            CurrentProgressState = -1;
        }
        else if (HealedVegPercentage >= 30 && HealedVegPercentage < 40)
        {
            CurrentProgressState = 0;
        }
        else if (HealedVegPercentage >= 40 && HealedVegPercentage < 50)
        {
            CurrentProgressState = 1;
        }
        else if (HealedVegPercentage >= 50 && HealedVegPercentage < 60)
        {
            CurrentProgressState = 2;
        }
        else if (HealedVegPercentage >= 60 && HealedVegPercentage < 70)
        {
            CurrentProgressState = 3;
        }
        else if (HealedVegPercentage >= 70 && HealedVegPercentage < 80)
        {
            CurrentProgressState = 4;
        }
        else if (HealedVegPercentage >= 80 && HealedVegPercentage < 90)
        {
            CurrentProgressState = 5;
        }
        else if (HealedVegPercentage >= 90 && HealedVegPercentage <= 100 && Time.timeSinceLevelLoad > 10)
        {
            //trigger good ending
            return;
        }

        economyManager.UpdatePriceAdjustmentValues(CurrentProgressState);
    }

}
