using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBlob : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float hardness = 1.0f, strength = 1.0f, extraRadius;
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Paintable"))
        {
            other.gameObject.GetComponent<IPaintable>().Paint(transform.position, color, hardness, strength, transform.localScale.x + extraRadius);
            Destroy(gameObject);
        }
    }
}
