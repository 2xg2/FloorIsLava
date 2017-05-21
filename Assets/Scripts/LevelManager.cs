using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Bounds bounds;
    public Vector3 startPos = new Vector3(0f, 0f, 0f);

    public GameObject starPrefab;
    public Bounds starsBounds;
    public int starsCount;

    private Vector3[] starsPos;
    private List<GameObject> starsObj = new List<GameObject>();
    private GameObject starsHolder;

    public GameObject finishLinePrefab;


    void Awake()
    {
        GenerateStarPositions();
        starsHolder = Instantiate(new GameObject("StarsHolder"));

    }

    public void StartLevel()
    {
        CreateAndPosStars();
        CreateAndPosFinishLine();
    }
    void DestroyLevel()
    {
        DestroyStars();
    }

    public void Restart()
    {
        DestroyLevel();
        StartLevel();
    }

    void GenerateStarPositions()
    {
        starsPos = new Vector3[starsCount];
        for(int i = 0; i < starsCount; i++)
        {
            float x = Random.Range(starsBounds.min.x, starsBounds.max.x);
            float y = Random.Range(starsBounds.min.y, starsBounds.max.y);
            starsPos[i] = new Vector3(x, y, 0f);
        }
    }

    void CreateAndPosStars()
    {
        for(int i = 0; i < starsCount; i++)
        {
            GameObject star = Instantiate(starPrefab, starsPos[i], Quaternion.identity, starsHolder.transform);
            starsObj.Add(star.gameObject);
        }
    }
    void DestroyStars()
    {
        for(int i = 0; i < starsObj.Count; i++)
        {
            Destroy(starsObj[i]);
        }
        starsObj.Clear();
    }

    void CreateAndPosFinishLine()
    {
        Quaternion q = Quaternion.AngleAxis(90, Vector3.forward);
        Instantiate(finishLinePrefab, new Vector3(bounds.max.x, bounds.center.y, 0f), Quaternion.identity);
        Instantiate(finishLinePrefab, new Vector3(bounds.center.x, bounds.min.y, 0f), q);
        Instantiate(finishLinePrefab, new Vector3(bounds.center.x, bounds.max.y, 0f), q);
    }
}
