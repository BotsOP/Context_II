using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingFactoryScript : MonoBehaviour
{
    private Transform target;
    public float speed;

    private float Xmin = 350;
    private float Xmax = 650;
    private float Zmin = 275;
    private float Zmax = 725;

    public float PathDuration;
    private float CurrentTime;
    private float NextPathTime;

    private void Start()
    {
        target = transform;
        GetRandomLocation();
        NextPathTime = Time.time + PathDuration;
    }
    void Update()
    {
        Debug.Log(target);
        CurrentTime = Time.time;

        if (CurrentTime >= NextPathTime)
        {
            GetRandomLocation();
            NextPathTime= Time.time + PathDuration;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    private void GetRandomLocation()
    {
        target.position = new Vector3(Random.Range(Xmin, Xmax), 210, Random.Range(Zmin, Zmax));
    }
}
