using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ResourceScript> ListOfRawResources = new List<ResourceScript>();
    public float moneyInInventory;
    public int BadFuelPacks;
    public int GoodFuelPacks;

    private void Start()
    {
        ListOfRawResources.Clear();
        moneyInInventory= 0;
    }
}
