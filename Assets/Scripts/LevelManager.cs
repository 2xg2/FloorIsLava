using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Bounds bounds;
    public Vector3 startPos = new Vector3(0f, 0f, 0f);

    //----------- stars ---------
    public GameObject starPrefab;
    public Bounds starsBounds;
    public int starsCount;

    private Vector3[] starsPos;
    private List<GameObject> starsObj = new List<GameObject>();
    private GameObject starsHolder;

    //----------- portals ---------
    class PortalsPairPosition
    {
        public Vector3 orangePosition;
        public Vector3 bluePosition;

        public PortalsPairPosition(Vector3 orangePos, Vector3 bluePos)
        {
            orangePosition = orangePos;
            bluePosition = bluePos;
        }
    }
    [HideInInspector] public GameObject portalsPairPrefab;
    private int portalsPairsCount = 0;
    private PortalsPairPosition[] portalsPairsPos;
    private List<GameObject> portalsPairsObj = new List<GameObject>();
    private GameObject portalsHolder;


    [HideInInspector]  public GameObject finishLinePrefab;


    void Awake()
    {
        GenerateStarPositions();
        starsHolder = Instantiate(new GameObject("StarsHolder"));
        //sportalsHolder = Instantiate(new GameObject("PortalsHolder"));
    }

    public void StartLevel()
    {
        CreateAndPosStars();
        //CreateAndPosPortals();
        //CreateAndPosFinishLine();
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

    void CreateAndPosPortals()
    {
        portalsPairsCount = 1;
        portalsPairsPos = new PortalsPairPosition[1];
        Vector3 orangePos = new Vector3(8f, -2f, 0f);
        Vector3 bluePos = new Vector3(10f, 2f, 0f);
        portalsPairsPos[0] = new PortalsPairPosition(orangePos, bluePos);

        PortalsPair pp = Instantiate(portalsPairPrefab, portalsHolder.transform).GetComponent<PortalsPair>();
        pp.CreateAndPos(portalsPairsPos[0].orangePosition, portalsPairsPos[0].bluePosition);
    }
}
