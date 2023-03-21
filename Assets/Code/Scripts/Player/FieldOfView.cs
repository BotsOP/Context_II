using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView
{
    
    public bool canSeeTarget;
    public List<Transform> targets;
    public Transform target;
    public float radius = 100f;
    public float angle = 130;

    private Transform transformPosition;
    private Transform transformRotation;
    private Transform transformSpherePosition;
    private int frameCount;
    private LayerMask targetMask;
    private LayerMask obstructionMask;

    public FieldOfView(Transform _transform, LayerMask _targetMask, LayerMask _obstructionMask, float _radius = 100, float _angle = 130)
    {
        transformPosition = _transform;
        transformRotation = _transform;
        transformSpherePosition = _transform;
        targetMask = _targetMask;
        obstructionMask = _obstructionMask;
        radius = _radius;
        angle = _angle;
        targets = new List<Transform>();
    }
    public FieldOfView(Transform _transformPosition, Transform _transformRotation, LayerMask _targetMask, LayerMask _obstructionMask, float _radius = 100, float _angle = 130)
    {
        transformPosition = _transformPosition;
        transformRotation = _transformRotation;
        transformSpherePosition = _transformPosition;
        targetMask = _targetMask;
        obstructionMask = _obstructionMask;
        radius = _radius;
        angle = _angle;
        targets = new List<Transform>();
    }
    public FieldOfView(Transform _transformPosition, Transform _transformRotation, Transform _transformSpherePosition, LayerMask _targetMask, LayerMask _obstructionMask, float _radius = 100, float _angle = 130)
    {
        transformPosition = _transformPosition;
        transformRotation = _transformRotation;
        transformSpherePosition = _transformSpherePosition;
        targetMask = _targetMask;
        obstructionMask = _obstructionMask;
        radius = _radius;
        angle = _angle;
        targets = new List<Transform>();
    }

    public bool FieldofViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transformPosition.position, radius, targetMask);

        bool anyTargetSeen = false;
        foreach (var collider in rangeChecks)
        {
            target = collider.transform;
            Vector3 directionToTarget = (target.position - transformRotation.position).normalized;

            if (Vector3.Angle(transformRotation.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transformPosition.position, target.position);

                if (!Physics.Raycast(transformRotation.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    anyTargetSeen = true;
                    if (!targets.Contains(target))
                    {
                        targets.Add(target);
                    }
                    canSeeTarget = true;
                }
            }
        }
        if (canSeeTarget && rangeChecks.Length > 0 || !anyTargetSeen)
        {
            targets.Clear();
            canSeeTarget = false;
        }
        else
        {
            target = GetClosestTransform();
        }

        return canSeeTarget;
    }

    private Transform GetClosestTransform()
    {
        Transform closestTarget = targets[0];
        float distance = 99999;
        
        foreach (var a in targets)
        {
            float distanceToA = Vector3.Distance(transformSpherePosition.position, a.position);
            if (distanceToA < distance)
            {
                distance = distanceToA;
                closestTarget = a;
            }
        }
        
        return closestTarget;
    }
}