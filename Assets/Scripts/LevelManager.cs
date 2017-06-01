using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelDataClass levelData;


    //----------- stars ---------
    public Bounds starsBounds;
    public int starsCount;

    private Vector3[] starsPos;
    private List<GameObject> starsObj;
    private GameObject starsHolder;

    private GameObject boundsHolder;

    Transform levelHolder;

    void Awake()
    {
        levelHolder = new GameObject("LevelHolder").transform;
        starsObj = new List<GameObject>();
    }

    public void CreateLevel()
    {
        GenerateStarPositions();
        CreateAndPosBounds();
        CreateAndPosStars();
    }
    public void DestroyLevel()
    {
        DestroyBounds();
        DestroyStars();
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

    void CreateAndPosBounds()
    {
        boundsHolder = new GameObject("BoundsHolder");
        boundsHolder.transform.parent = levelHolder;
        if(Application.isEditor)
        {
            boundsHolder.transform.Translate(new Vector3(0f, 0f, -1f));
        }
        


        Vector3 pos1 = new Vector3(levelData.bounds.min.x, levelData.bounds.max.y, 0f);
        Vector3 pos2 = new Vector3(levelData.bounds.max.x, levelData.bounds.max.y, 0f);
        Vector3 pos3 = new Vector3(levelData.bounds.max.x, levelData.bounds.min.y, 0f);
        Vector3 pos4 = new Vector3(levelData.bounds.min.x, levelData.bounds.min.y, 0f);

        GameObject obj1 = new GameObject("line1", typeof(LineRenderer));
        obj1.transform.parent = boundsHolder.transform;
        LineRenderer line1 = obj1.GetComponent<LineRenderer>();
        line1.positionCount = 2;
        line1.SetPosition(0, pos1);
        line1.SetPosition(1, pos2);
        line1.startColor = new Color(0, 0, 0, 1);
        line1.endColor = new Color(0, 0, 0, 1);

        GameObject obj2 = new GameObject("line2", typeof(LineRenderer));
        obj2.transform.parent = boundsHolder.transform;
        LineRenderer line2 = obj2.GetComponent<LineRenderer>();
        line2.positionCount = 2;
        line2.SetPosition(0, pos2);
        line2.SetPosition(1, pos3);
        line2.startColor = new Color(1, 0, 1, 1);
        line2.endColor = new Color(0, 0, 0, 1);

        GameObject obj3 = new GameObject("line3", typeof(LineRenderer));
        obj3.transform.parent = boundsHolder.transform;
        LineRenderer line3 = obj3.GetComponent<LineRenderer>();
        line3.positionCount = 2;
        line3.SetPosition(0, pos3);
        line3.SetPosition(1, pos4);
        line3.startColor = new Color(0, 0, 0, 1);
        line3.endColor = new Color(0, 0, 0, 1);

        GameObject obj4 = new GameObject("line4", typeof(LineRenderer));
        obj4.transform.parent = boundsHolder.transform;
        LineRenderer line4 = obj4.GetComponent<LineRenderer>();
        line4.positionCount = 2;
        line4.SetPosition(0, pos4);
        line4.SetPosition(1, pos1);
        line4.startColor = new Color(0, 0, 0, 1);
        line4.endColor = new Color(0, 0, 0, 1);


    }
    void CreateAndPosStars()
    {
        starsHolder = new GameObject("StarsHolder");
        starsHolder.transform.parent = levelHolder;
        for (int i = 0; i < starsCount; i++)
        {
            GameObject star = Instantiate(levelData.starPrefab, starsPos[i], Quaternion.identity, starsHolder.transform);
            starsObj.Add(star.gameObject);
        }
    }

    void DestroyBounds()
    {
        Destroy(boundsHolder.gameObject);
    }

    void DestroyStars()
    {
        for(int i = 0; i < starsObj.Count; i++)
        {
            Destroy(starsObj[i]);
        }
        starsObj.Clear();
        Destroy(starsHolder.gameObject);
    }
}
