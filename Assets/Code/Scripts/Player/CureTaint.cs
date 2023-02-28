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

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.F) && fuel >= 0)
        {
            Debug.Log($"target hit");
            fuel -= fuelDepletionRate;
            slider.value = fuel;
            if(target) { target.GetComponent<IPaintable>().SuckTarget(sucker, SuckMultiplier); }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && targets.Length > 0)
        {
            foreach (var target1 in targets)
            {
                target1.GetComponent<IPaintable>().SuckTarget(sucker, SprintMultiplier);
            }
        }
        // else if(!Input.GetKey(KeyCode.F))
        // {
        //     if(target) { target.GetComponent<IPaintable>().StoppedSucking(); }
        // }
        // else if(!Input.GetKey(KeyCode.LeftShift) && targets.Length > 0)
        // {
        //     foreach (var target1 in targets)
        //     {
        //         if(target1 == target) { continue; }
        //         target1.GetComponent<IPaintable>().StoppedSucking();
        //     }
        // }
        
        previousTarget = target;
        target = GetPossibleTarget();
        previousTargets = targets;
        targets = GetAllTargets();

        if (target != previousTarget && previousTarget)
        {
            Debug.Log($"single target");
            previousTarget.GetComponent<IPaintable>().StoppedSucking();
        }
        else if (previousTargets.Length > 0 && !Input.GetKey(KeyCode.F))
        {
            foreach (var previousTarget1 in previousTargets)
            {
                bool shouldStopSucking = false;
                foreach (var target1 in targets)
                {
                    if (target1 != previousTarget1)
                    {
                        shouldStopSucking = true;
                    }
                }

                if (shouldStopSucking)
                {
                    Debug.Log($"multy target");
                    previousTarget1.GetComponent<IPaintable>().StoppedSucking();
                }
            }
        }
    }

    private Transform[] GetAllTargets()
    {
        return Physics.OverlapSphere(transform.position, sprintRadius, targetMask).Select(collider => collider.transform).ToArray();
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
