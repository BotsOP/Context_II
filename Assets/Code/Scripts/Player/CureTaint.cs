using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CureTaint : MonoBehaviour
{
    public Slider slider;
    public float fuel;
    [SerializeField] private float fuelDepletionRate = 0.1f;
    [SerializeField] private Transform sucker;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform camFollow;
    [SerializeField] private float SuckMultiplier = 1f, suckRadius = 5f, threshhold;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float SprintMultiplier = 0.1f, sprintRadius;
    private Transform target;
    private Transform[] targets;
    private Transform[] previousTargets;
    private Transform previousTarget;

    public FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    private void OnEnable()
    {
        EventSystem<float>.Subscribe(EventType.FUEL_ADDED, IncreaseFuel);
    }
    private void OnDisable()
    {
        EventSystem<float>.Unsubscribe(EventType.FUEL_ADDED, IncreaseFuel);
    }

    private void IncreaseFuel(float amountFuel)
    {
        fuel += amountFuel;
        slider.value = fuel;
    }
    private void Start()
    {
        //instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        //instance.start();
    }

    private void FixedUpdate()
    {
        target = GetPossibleTarget();
        if (Input.GetKey(KeyCode.F) && fuel >= 0 && target)
        {
            //instance.setParameterByName("Slurp", 1);
            fuel -= fuelDepletionRate;
            slider.value = fuel;
            target.GetComponent<IPaintable>().SuckTarget(sucker, SuckMultiplier);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            targets = GetAllTargets(sprintRadius);
            foreach (var target1 in targets)
            {
                target1.GetComponent<IPaintable>().SuckTarget(sucker, SprintMultiplier);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            targets = GetAllTargets(20);
            foreach (var target1 in targets)
            {
                target1.GetComponent<IPaintable>().SuckTarget(sucker, 9999999);
            }
        }
    }

    private Transform[] GetAllTargets(float radius)
    {
        return Physics.OverlapSphere(transform.position, radius, targetMask).Select(collider => collider.transform).ToArray();
    }

    private Transform GetPossibleTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, suckRadius, targetMask);

        List<Transform> possibleColliders = new List<Transform>();
        
        foreach (var check in rangeChecks)
        {
            Vector3 camToColliderDir = check.transform.position - camFollow.position;
            if (Vector3.Dot(cam.forward, camToColliderDir) > threshhold && !Physics.Raycast(camFollow.position, camToColliderDir, Mathf.Infinity, obstructionMask))
            {
                possibleColliders.Add(check.transform);
            }
        }

        if (possibleColliders.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            Transform tempTarget = null;

            for (int i = 0; i < possibleColliders.Count; i++)
            {
                if (possibleColliders[i].GetComponent<IPaintable>().GetTaintedness() <= 0)
                {
                    continue;
                }
                
                float newDistance = Vector3.Distance(transform.position, possibleColliders[i].position);
                if (newDistance < closestDistance)
                {
                    closestDistance = newDistance;
                    tempTarget = possibleColliders[i];
                }
            }

            return tempTarget;
        }

        return null;
    }
        
}
