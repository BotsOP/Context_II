using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureTaint : MonoBehaviour
{
    [SerializeField] private Transform sucker;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform camFollow;
    [SerializeField] private float radius = 5f, threshhold;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    private void Awake()
    {
        //fov = new FieldOfView(camFollow, cam, transform, targetMask, obstructionMask, radius, angle);
    }

    private void LateUpdate()
    {
        Transform target = GetPossibleTarget();
        if (target)
        {
            if (Input.GetKey(KeyCode.L))
            {
                target.GetComponent<IPaintable>().SuckTarget(sucker, 1);
            }
        }
        
    }

    private Transform GetPossibleTarget()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        Transform[] possibleColliders = new Transform[rangeChecks.Length];
        int amountPossibleColliders = 0;
        
        foreach (var collider in rangeChecks)
        {
            Vector3 camToColliderDir = collider.transform.position - camFollow.position;
            if (Vector3.Dot(cam.forward, camToColliderDir) > threshhold && !Physics.Raycast(camFollow.position, camToColliderDir, Mathf.Infinity, obstructionMask))
            {
                possibleColliders[amountPossibleColliders] = collider.transform;
                amountPossibleColliders++;
            }
        }

        if (amountPossibleColliders > 0)
        {
            float closestDistance = Mathf.Infinity;
            Transform target = null;
            foreach (var trans in possibleColliders)
            {
                float newDistance = Vector3.Distance(transform.position, trans.position);
                if (newDistance < closestDistance)
                {
                    closestDistance = newDistance;
                    target = trans;
                }
            }
    
            //Debug.Log($"{target.gameObject.name}");
            return target;
        }

        return null;
    }
        
}
