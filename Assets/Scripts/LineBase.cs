using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineBase : MonoBehaviour
{
    LineRenderer m_LineRenderer;
    EdgeCollider2D m_EdgeCollider;
    List<Vector3> m_Points;

    int m_nFirstVisibleIndex;
    int m_nLastVisibleIndex;

    void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_EdgeCollider = GetComponent<EdgeCollider2D>();
        m_Points = new List<Vector3>();

        m_nFirstVisibleIndex = -1;
        m_nLastVisibleIndex = -1;

        if (m_LineRenderer)
        {
            for(int i = 0; i < m_LineRenderer.positionCount; i++)
            {
                m_Points.Add(m_LineRenderer.GetPosition(i));
            }
        }

        if(m_Points.Count > 0)
        {
            m_nFirstVisibleIndex = 0;
            m_nLastVisibleIndex = m_Points.Count - 1;
        }
    }

    public void AddNewPoint(Vector3 point)
    {
        float viewHalfHeight = Camera.main.orthographicSize;
        float viewHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height);
        float maxValidPointX = Camera.main.transform.position.x + viewHalfWidth;
        float minValidPointY = Camera.main.transform.position.y - viewHalfHeight;
        float maxValidPointY = Camera.main.transform.position.y + viewHalfHeight;

        if (point.x > maxValidPointX || point.y < minValidPointY || point.y > maxValidPointY)
            return;


        m_Points.Add(point);

        m_LineRenderer.positionCount = m_LineRenderer.positionCount + 1;
        m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, point);

        int edgeColliderPointsCount = m_EdgeCollider.points.Length;
        Vector2[] newColliderPoints = new Vector2[edgeColliderPointsCount + 1];
        for (int i = 0; i < edgeColliderPointsCount; i++)
        {
            newColliderPoints[i] = m_EdgeCollider.points[i];
        }
        newColliderPoints[edgeColliderPointsCount] = new Vector2(point.x, point.y);
        m_EdgeCollider.points = newColliderPoints;
    }

    public void InitWithPoints(Vector3[] points)
    {
        m_Points.Clear();
        for(int i = 0; i < points.Length; i++)
        {
            m_Points.Add(points[i]);
        }

        m_LineRenderer.positionCount = m_Points.Count;
        m_LineRenderer.SetPositions(points);

        Vector2[] points2D = new Vector2[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            points2D[i] = new Vector2(points[i].x, points[i].y);
        }

        m_EdgeCollider.points = points2D;
    }

    public Vector3 GetFirstPos()
    {
        Vector3 pos = Vector3.zero;
        if (m_Points.Count > 0)
            pos = m_Points[0];
        return pos;
    }

    public Vector3 GetLastPos()
    {
        Vector3 pos = Vector3.zero;
        if (m_Points.Count > 0)
            pos = m_Points[m_Points.Count - 1];
        return pos;
    }

    public Vector3 GetFirstDirection()
    {
        Vector3 dir = Vector3.zero;
        if(m_Points.Count >= 2)
        {
            Vector3 p1 = m_Points[1];
            Vector3 p2 = m_Points[0];            
            dir = (p2 - p1).normalized;
        }

        return dir;
    }

    public Vector3 GetLastDirection()
    {
        Vector3 dir = Vector3.zero;
        if (m_Points.Count >= 2)
        {
            Vector3 p1 = m_Points[m_Points.Count - 2];
            Vector3 p2 = m_Points[m_Points.Count - 1];
            dir = (p2 - p1).normalized;
        }

        return dir;
    }

    public void UpdateVisiblePoints(float minValidX, float maxValidX)
    {
        if (m_Points.Count == 0)
            return;

        List<Vector3> validPoints = new List<Vector3>();

        if (m_nFirstVisibleIndex == -1)
            m_nFirstVisibleIndex = 0;

        //find new first visible
        if (m_Points[m_nFirstVisibleIndex].x < minValidX)
        {
            for (int i = m_nFirstVisibleIndex; i < m_Points.Count; i++)
            {
                if (m_Points[i].x >= minValidX)
                {
                    m_nFirstVisibleIndex = i;
                    break;
                }
            }
        }
        else if (m_Points[m_nFirstVisibleIndex].x > minValidX)
        {
            int i = 0;
            for (i = m_nFirstVisibleIndex; i >= 0; i--)
            {
                if (m_Points[i].x < minValidX)
                {
                    break;
                }
            }
            m_nFirstVisibleIndex = i + 1;
        }
        if (m_nFirstVisibleIndex < 0)
            m_nFirstVisibleIndex = 0;
        if (m_nFirstVisibleIndex > m_Points.Count - 1)
            m_nFirstVisibleIndex = m_Points.Count - 1;

        //find new last visible
        {
            int i = 0;
            for (i = m_nFirstVisibleIndex; i < m_Points.Count; i++)
            {
                if (m_Points[i].x > maxValidX)
                {
                    break;
                }
            }

            if(i < m_Points.Count)
            {
                m_nLastVisibleIndex = i - 1;
            }
            else
            {
                m_nLastVisibleIndex = m_Points.Count - 1;
            }
        }
        if (m_nLastVisibleIndex < 0)
            m_nLastVisibleIndex = 0;
        if (m_nLastVisibleIndex > m_Points.Count - 1)
            m_nLastVisibleIndex = m_Points.Count - 1;

        for (int i = m_nFirstVisibleIndex; i <= m_nLastVisibleIndex; i++)
        {
            validPoints.Add(m_Points[i]);
        }

        Vector3[] arrValidPoints = validPoints.ToArray();

        m_LineRenderer.positionCount = arrValidPoints.Length;
        m_LineRenderer.SetPositions(arrValidPoints);

        Vector2[] arrValidPoints2 = new Vector2[arrValidPoints.Length];
        for(int i = 0; i < arrValidPoints.Length; i++)
        {
            arrValidPoints2[i] = new Vector2(arrValidPoints[i].x, arrValidPoints[i].y);
        }
        m_EdgeCollider.points = arrValidPoints2;

    }

    public Vector3 GetFirstLinePos()
    {
        Vector3 pos = Vector3.zero;
        if (m_LineRenderer.positionCount > 0)
            pos = m_LineRenderer.GetPosition(0);
        return pos;
    }

    public Vector3 GetFirstLineDirection()
    {
        Vector3 dir = Vector3.zero;
        if (m_LineRenderer.positionCount >= 2)
        {
            Vector3 p1 = m_LineRenderer.GetPosition(0);
            Vector3 p2 = m_LineRenderer.GetPosition(1);
            dir = (p2 - p1).normalized;
        }

        return dir;
    }
}
