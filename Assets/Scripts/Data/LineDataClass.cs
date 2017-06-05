using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class LineDataClass : ScriptableObject
{
    public Vector3[] points;
    public Vector3 startBallOffset;
}
