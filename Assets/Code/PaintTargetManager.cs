using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTargetManager : MonoBehaviour
{
    private PaintTarget[] paintTargets;
    void Start()
    {
        paintTargets = FindObjectsOfType<PaintTarget>();
    }

    public float GetWorldTaintedness()
    {
        float taintedness = 0;
        foreach (var target in paintTargets)
        {
            taintedness += target.taintedness;
        }
        return taintedness / paintTargets.Length;
    }
}
