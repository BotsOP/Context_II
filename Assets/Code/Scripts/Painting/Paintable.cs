using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPaintable
{
    public void Paint(Vector3 position, Color color, float hardness = 1.0f, float strength = 1.0f, float radius = 1.0f);
}
