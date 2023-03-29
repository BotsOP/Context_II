using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyProduct : MonoBehaviour
{
    public FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;
    public FMOD.Studio.EventInstance instance2;
    public FMODUnity.EventReference fmodEvent2;
    public int workshopCounter = 0;
    public int bigCompanyCounter = 0;

    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance2 = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);
    }

    public void WorkshopProduct()
    {
        workshopCounter++;
        instance2.setParameterByName("WSFeedback", workshopCounter);
        instance2.start();
    }
    public void BigCompanyProduct()
    {
        bigCompanyCounter++;
        instance.setParameterByName("BCFeedback", bigCompanyCounter);
        instance.start();
    }
}
