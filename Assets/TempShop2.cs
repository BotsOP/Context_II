using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TempShop2 : MonoBehaviour, IInteractable
{
    [SerializeField] private AcidCloudSpawner acidCloudSpawner;
    [SerializeField] private float boycottValue;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject fuel;
    [SerializeField] private string displayText;
    public void Interact()
    {
        acidCloudSpawner.AddBoycottProgress(boycottValue);
        Instantiate(fuel, spawnPoint.transform.position, quaternion.identity);
    }
    public string GetInteractText()
    {
        return displayText;
    }
}
