using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineBaseWithInput : MonoBehaviour
{

    public float speedX = 2;
    public float maxSpeedY = 2;

    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    public Transform endMark;

    Vector3 firstMousePos;
    Vector3 lastMousePos;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        endMark = transform.Find("endMark"); //GetComponentInChildren<SpriteRenderer>();

        if (lineRenderer.positionCount > 0)
        {
            endMark.transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = Input.mousePosition;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)
            && (EventSystem.current.currentSelectedGameObject == null))
        {
            float dx = Time.deltaTime * speedX;

            //Debug.Log("Screen h = " + Screen.height);
            float speedY = ((Input.mousePosition.y - firstMousePos.y) / Screen.height) * maxSpeedY;
            float dy = Time.deltaTime * speedY;

            //float dy = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - Camera.main.ScreenToWorldPoint(lastMousePos).y;

            Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Vector3 nextPosition = new Vector3(lastPosition.x + dx, lastPosition.y + dy, lastPosition.z);

            AddNewPosition(nextPosition);
            endMark.transform.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

            lastMousePos = Input.mousePosition;
        }
    }

    void AddNewPosition(Vector3 point)
    {
        lineRenderer.positionCount = lineRenderer.positionCount + 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);

        int edgeColliderPointsCount = edgeCollider.points.Length;
        Vector2[] newColliderPoints = new Vector2[edgeColliderPointsCount + 1];
        for (int i = 0; i < edgeColliderPointsCount; i++)
        {
            newColliderPoints[i] = edgeCollider.points[i];
        }
        newColliderPoints[edgeColliderPointsCount] = new Vector2(point.x, point.y);
        edgeCollider.points = newColliderPoints;
    }
}
