using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraFuel : MonoBehaviour, IInteractable
{
    [SerializeField] private float amountFuelToAdd;
    [SerializeField] private string displayText;
    public void Interact()
    {
        EventSystem<float>.RaiseEvent(EventType.FUEL_ADDED, amountFuelToAdd);
        Destroy(gameObject);
    }
    public string GetInteractText()
    {
        return displayText;
    }
}
