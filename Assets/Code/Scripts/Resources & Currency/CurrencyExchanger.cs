using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyExchanger : MonoBehaviour
{
    public GameObject Player;
    public EconomyManager economyManager;

    public void ExchangeResources()
    {
        float totalResourceWorth = 0;

        foreach(ResourceScript resource in Player.GetComponent<PlayerInventory>().ListOfRawResources)
        {
            totalResourceWorth += resource.worth;
        }

        Player.GetComponent<PlayerInventory>().moneyInInventory += totalResourceWorth;
    }
}
