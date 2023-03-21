using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaintSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paintBall;
    [SerializeField] private int amountBalls = 10;
    [SerializeField] private float timeTillStop = 1;
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(transform.localScale.x, 0, transform.localScale.z));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < amountBalls; i++)
            {
                SpawnPaintBall();
            }
        }
        if (Time.timeSinceLevelLoad < timeTillStop)
        {
            for (int i = 0; i < amountBalls; i++)
            {
                SpawnPaintBall();
            }
        }
        //SpawnPaintBall();
    }

    private void SpawnPaintBall()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        Vector3 spawnPos = new Vector3(x * Random.Range(0f, 1f)  * transform.localScale.x * 4, 0, y * Random.Range(0f, 1f)  * transform.localScale.x * 4);
        spawnPos += transform.position;
        Instantiate(paintBall, spawnPos, Quaternion.identity);
    }
}
