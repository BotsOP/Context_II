using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureTaint : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform camFollow;
    [SerializeField] private float radius = 5f, angle = 60f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    private FieldOfView fov;

    private void Awake()
    {
        fov = new FieldOfView(camFollow, cam, transform, targetMask, obstructionMask, radius, angle);
    }

    private void Update()
    {
        Debug.Log($"{fov.FieldofViewCheck()} {fov?.target.name}");
    }
}
