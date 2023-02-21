using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerEatBubble : MonoBehaviour
{
   // public CinemachineFreeLook freelook;
    [SerializeField]
    private float bubbleAlphaDepletion;
    [SerializeField]
    private float bubbleSizeDepletion;
    [SerializeField]
    private float eatRadius;
    [SerializeField] 
    private LayerMask bubbleLayerMask;
    private int bubbleID;
    private Material bubbleMat;
    private Bubble bubbleScript;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2 + eatRadius);
    }
    private void FixedUpdate()
    {
        //freelook.m_Orbits[1].m_Radius = transform.localScale.x + 10f;
        int maxColliders = 1;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.x / 2 + eatRadius, hitColliders, bubbleLayerMask);
        for (int i = 0; i < numColliders; i++)
        {
            int colliderID = hitColliders[i].GetInstanceID();
            if (colliderID != bubbleID)
            {
                bubbleID = colliderID;
                bubbleMat = hitColliders[i].GetComponent<MeshRenderer>().material;
                bubbleScript = hitColliders[i].GetComponent<Bubble>();
            }
            // float alpha = bubbleMat.GetFloat("alpha");
            // bubbleMat.SetFloat("alpha", alpha - bubbleAlphaDepletion);
            // if (alpha <= 0)
            // {
            //     Destroy(gameObject);
            // }
            if (transform.localScale.x > hitColliders[i].transform.localScale.x)
            {
                bubbleScript.sizeIncrease -= bubbleSizeDepletion;
                if (bubbleScript.sizeIncrease <= 0)
                {
                    Destroy(hitColliders[i].gameObject);
                }
                transform.localScale = new Vector3(transform.localScale.x + bubbleSizeDepletion, transform.localScale.y  + bubbleSizeDepletion, transform.localScale.z  + bubbleSizeDepletion);
            }
        }
    }
}
