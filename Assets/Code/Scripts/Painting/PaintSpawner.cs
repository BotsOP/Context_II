using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaintSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paintBall;
    [SerializeField] private int amountBalls = 10;
    [SerializeField] private Color color;
    [SerializeField] private float timeTillStop = 1;
    [SerializeField] private float blobRadius = 2;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask layerMask;
    private List<PaintBlob> paintBlobs = new List<PaintBlob>();
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
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < paintBlobs.Count; i++)
        {
            bool hitSomething = paintBlobs[i].Update(color, blobRadius, 0, 1, gravity, layerMask);
            if (hitSomething)
            {
                paintBlobs.RemoveAt(i);
                i--;
            }
        }
    }

    private void SpawnPaintBall()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        Vector3 spawnPos = new Vector3(x * Random.Range(0f, 1f)  * transform.localScale.x * 4, 0, y * Random.Range(0f, 1f)  * transform.localScale.x * 4);
        spawnPos += transform.position;
        GameObject paintBlob = Instantiate(paintBall, spawnPos, Quaternion.identity);
        paintBlobs.Add(new PaintBlob(paintBlob, Time.time));
    }
    
    internal struct PaintBlob
    {
        public GameObject paintObject;
        public float startTime;

        public PaintBlob(GameObject _paintObject, float _startTime)
        {
            paintObject = _paintObject;
            startTime = _startTime;
        }

        public bool Update(Color _color, float _radius, float _hardness, float _strength, float gravity, LayerMask _layerMask)
        {
            paintObject.transform.Translate(new Vector3(0, gravity, 0));

            Collider[] hitColliders = new Collider[5];
            int numColliders = Physics.OverlapSphereNonAlloc(paintObject.transform.position, 1f, hitColliders, _layerMask);

            for (int i = 0; i < numColliders; i++)
            {
                hitColliders[i].gameObject.GetComponent<IPaintable>().Paint(paintObject.transform.position, _color, _hardness, _strength, _radius);
                Destroy(paintObject);
                return true;
            }

            if (Time.time > startTime + 5)
            {
                Destroy(paintObject);
                return true;
            }

            return false;
        }
    }
}


