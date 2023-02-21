using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float radius = 5f, threshhold;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    private Transform target;
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
        if (target)
        {
            if (Input.GetKey(KeyCode.F) && fuel >= 0)
            {
                fuel -= fuelDepletionRate;
                slider.value = fuel;
                target.GetComponent<IPaintable>().SuckTarget(sucker, 1);
            }
            else
            {
                target.GetComponent<IPaintable>().StoppedSucking();
            }
        }
        previousTarget = target;
        target = GetPossibleTarget();

        if (target != previousTarget && previousTarget)
        {
            previousTarget.GetComponent<IPaintable>().StoppedSucking();
        }
    }

    private Transform GetPossibleTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

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
