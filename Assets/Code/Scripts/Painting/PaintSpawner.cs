using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaintSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paintBall;
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(transform.localScale.x, 0, transform.localScale.z));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnPaintBall();
        }
        //SpawnPaintBall();
    }

    private void SpawnPaintBall()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Sin(angle) * transform.localScale.x;
        float y = Mathf.Cos(angle) * transform.localScale.x;
        Vector3 spawnPos = new Vector3(x * Random.Range(0f, 1f), 0, y * Random.Range(0f, 1f));
        spawnPos += transform.position;
        Instantiate(paintBall, spawnPos, Quaternion.identity);
    }
}
