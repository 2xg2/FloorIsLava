using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class LevelDataClass : ScriptableObject
{
    public Bounds bounds;
    public Vector3 startPos = new Vector3(0f, 0f, 0f);
    public GameObject starPrefab;
}
