using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLine : MonoBehaviour
{
    public float speedX = 8;
    public float maxSpeedY = 8;

    public GameObject lineTrunkPrefab;

    Transform m_lineTrunksParent;
    List<LineBase> m_Lines;
    int m_nCurrentLineIndex = -1;

    Vector3 firstMousePos;
    Vector3 lastMousePos;

    GameObject m_Head;
    GameObject m_Tail;

    void Awake()
    {
        m_Lines = new List<LineBase>();
        m_lineTrunksParent = transform.Find("trunks");
        foreach(Transform t in m_lineTrunksParent)
        {
            LineBase line = t.gameObject.GetComponent<LineBase>();
            if(line)
            {
                m_Lines.Add(line);
            }
        }

        if(m_Lines.Count > 0)
        {
            m_nCurrentLineIndex = m_Lines.Count - 1;
        }

        m_Head = transform.Find("head").gameObject;
        m_Tail = transform.Find("tail").gameObject;
    }

    public void InitWithPoints(Vector3[] points)
    {
        LineBase line = null;
        if(m_nCurrentLineIndex != -1)
        {
            line = m_Lines[m_nCurrentLineIndex];
        }
        else
        {
            line = GetOrCreateCurrentLine();
        }

        line.InitWithPoints(points);
        UpdateVisiblePoints();
    }

    void Update()
    {
        if (!(GameManager.Instance && GameManager.Instance.gameStarted))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = Input.mousePosition;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)
            && (EventSystem.current.currentSelectedGameObject == null))
        {
            float dx = Time.deltaTime * speedX;

            float speedY = ((Input.mousePosition.y - firstMousePos.y) / Screen.height) * maxSpeedY;
            float dy = Time.deltaTime * speedY;

            LineBase line = GetOrCreateCurrentLine();

            Vector3 lastPosition = line.GetLastPos();
            Vector3 nextPosition = new Vector3(lastPosition.x + dx, lastPosition.y + dy, 0f);

            line.AddNewPoint(nextPosition);

            lastMousePos = Input.mousePosition;
        }

        UpdateVisiblePoints();
    }

    public Vector3 GetCurrentLineHead()
    {
        Vector3 head = Vector3.zero;
        if(m_nCurrentLineIndex != -1)
        {
            if(m_nCurrentLineIndex < m_Lines.Count)
            {
                if(m_Lines[m_nCurrentLineIndex])
                {
                    head = m_Lines[m_nCurrentLineIndex].GetLastPos();
                }
            }
        }
        return head;
    }

    public LineBase GetOrCreateCurrentLine()
    {
        LineBase line = null;

        if (m_nCurrentLineIndex != -1)
        {
            line = m_Lines[m_nCurrentLineIndex];
        }
        else
        {
            line = Instantiate(lineTrunkPrefab).GetComponent<LineBase>();
            line.transform.parent = m_lineTrunksParent;
            
            m_Lines.Add(line);
            m_nCurrentLineIndex = 0;
        }

        return line;
    }

    void UpdateHeadPos()
    {
        m_Head.transform.position = transform.position + GetOrCreateCurrentLine().GetLastPos();
        Vector3 dir = GetOrCreateCurrentLine().GetLastDirection();

        float angleV = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion qV = Quaternion.AngleAxis(angleV, Vector3.forward);
        m_Head.transform.rotation = qV;
    }

    void UpdateTailPos()
    {
        m_Tail.transform.position = transform.position + GetOrCreateCurrentLine().GetFirstLinePos();
        Vector3 dir = GetOrCreateCurrentLine().GetFirstLineDirection();

        float angleV = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion qV = Quaternion.AngleAxis(angleV, Vector3.forward);
        m_Tail.transform.rotation = qV;
    }

    void OnDestroy()
    {
        for(int i = 0; i < m_Lines.Count; i++)
        {
            Destroy(m_Lines[i].gameObject);
        }
        m_Lines.Clear();
    }

    void UpdateVisiblePoints()
    {
        float viewHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height);
        float minValidPointX = Camera.main.transform.position.x - viewHalfWidth;
        float maxValidPointX = Camera.main.transform.position.x + viewHalfWidth;

        for (int i = 0; i < m_Lines.Count; i++)
        {
            m_Lines[i].UpdateVisiblePoints(minValidPointX, maxValidPointX);
        }

        UpdateHeadPos();
        UpdateTailPos();
    }
}
