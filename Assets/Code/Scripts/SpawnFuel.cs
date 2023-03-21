using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFuel : MonoBehaviour
{
    [SerializeField] private GameObject fuel;
    private GameObject fuelInstance;

    private void Update()
    {
        if (!fuelInstance)
        {
            fuelInstance = Instantiate(fuel, transform.position, Quaternion.identity);
        }
    }
}
