using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public void CreateDataAsset()
    {
        LineBase line = FindObjectOfType(typeof(LineBase)) as LineBase;
        LineRenderer lineR = line.GetComponent<LineRenderer>();
        LineDataClass lineAsset = ScriptableObject.CreateInstance<LineDataClass>();
        lineAsset.points = new Vector3[lineR.positionCount];
        for(int i = 0; i < lineR.positionCount; i++)
        {
            lineAsset.points[i] = lineR.GetPosition(i);
        }
        string path = "Assets/Data/Lines/";
        string[] files = System.IO.Directory.GetFiles(path, "Line*.asset");
        int filesCount = files.GetLength(0);
        string newFileName = "Line_" + filesCount + ".asset";
        AssetDatabase.CreateAsset(lineAsset, path + newFileName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = lineAsset;
    }
}
