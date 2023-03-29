using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintTargetManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    static public PaintTarget[] paintTargets;
    void Start()
    {
        paintTargets = FindObjectsOfType<PaintTarget>();
    }

    private void Update()
    {
        if (Time.frameCount % 60 == 0)
        {
            slider.value = GetWorldTaintedness();
        }
    }

    static public float GetWorldTaintedness()
    {
        float taintedness = 0;
        foreach (var target in paintTargets)
        {
            taintedness += target.taintedness;
        }
        return taintedness / paintTargets.Length;
    }
}
