using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class SpecialTorus : MonoBehaviour
{
    public GameObject point;
    public float handleSize = 1f;
    private Transform[] bezierPoints;
    private bool initializedArray;

    private Vector3[] bezierStartPoints = 
        { Vector3.zero, Vector3.forward, new (0.5f, 0, 0.2f), new (0.5f, 0, 0.8f) };

    private void OnDrawGizmos()
    {
        if (point != null)
        {
            if (!initializedArray)
            {
                bezierPoints = new Transform[4];
                initializedArray = true;
            }

            if (transform.childCount >= 2)
            {
                Transform point1 = transform.GetChild(0);
                Transform point2 = transform.GetChild(1);
                bezierPoints[0] = point1;
                bezierPoints[1] = point2;
                bezierPoints[2] = point1.GetChild(0);
                bezierPoints[3] = point2.GetChild(0);
            }
            else if(bezierPoints[0] == null || bezierPoints[1] == null || bezierPoints[2] == null || bezierPoints[3] == null)
            {
                ResetBezierPoints();
            }
        }
        
        Handles.DrawBezier(bezierPoints[0].position, bezierPoints[1].position, bezierPoints[2].position, 
            bezierPoints[3].position, Color.white, EditorGUIUtility.whiteTexture, 1f);
        
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(bezierPoints[0].position, handleSize);
        Gizmos.DrawSphere(bezierPoints[1].position, handleSize);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bezierPoints[2].position, handleSize);
        Gizmos.DrawSphere(bezierPoints[3].position, handleSize);
        Gizmos.DrawLine(bezierPoints[0].position, bezierPoints[2].position);
        Gizmos.DrawLine(bezierPoints[1].position, bezierPoints[3].position);
    }

    public void ResetBezierPoints()
    {
        for (int i = 0; i < bezierPoints.Length; i++)
        {
            DestroyImmediate(bezierPoints[i]?.gameObject);
            GameObject tempPoint = Instantiate(point, bezierStartPoints[i] * 10 + transform.position, quaternion.identity);
            bezierPoints[i] = tempPoint.transform;
        }

        bezierPoints[0].parent = transform;
        bezierPoints[1].parent = transform;
        bezierPoints[2].parent = bezierPoints[0];
        bezierPoints[3].parent = bezierPoints[1];
    }
}
