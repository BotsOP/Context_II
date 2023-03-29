using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyExchanger : MonoBehaviour
{
    public GameObject Player;
    public EconomyManager economyManager;
    private bool InRange;

    public GameObject InteractWindow;
    private bool InteractWindowIsOpen;

    private void Start()
    {
        InRange = false;
        InteractWindowIsOpen = false;
    }

    private void Update()
    {
        if (InRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Exchanging");
                ExchangeResources();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractWindow.SetActive(true);
        if(other.gameObject.layer == 8)
        {
            InRange= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractWindow.SetActive(false);
        InRange= false;
    }

    public void ExchangeResources()
    {
        float totalResourceWorth = 0;

        foreach(ResourceScript resource in Player.GetComponent<PlayerInventory>().ListOfRawResources)
        {
            Debug.Log("Increased total worth");
            totalResourceWorth += resource.worth;
            Debug.Log("Total resource worth is" + totalResourceWorth);
        }

        Player.GetComponent<PlayerInventory>().ListOfRawResources.Clear();

        Player.GetComponent<PlayerInventory>().moneyInInventory += totalResourceWorth;
        Debug.Log(Player.GetComponent<PlayerInventory>().moneyInInventory);
    }
}
