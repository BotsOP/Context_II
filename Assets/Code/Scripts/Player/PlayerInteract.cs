using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform camFollow;
    [SerializeField] private float radius = 5f, threshhold;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    private Transform target;
    private void Update()
    {
        target = GetPossibleTarget();
        
        if (target)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            text.gameObject.SetActive(true);
            text.text = interactable.GetInteractText();
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interact();
            }
        }
        else
        {
            text.gameObject.SetActive(false);
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
