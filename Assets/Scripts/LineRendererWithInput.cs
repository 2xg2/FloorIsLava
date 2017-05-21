using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineRendererWithInput : MonoBehaviour {

    public float baseSpeed;
    LayerMask whatIsGround;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    Vector3 lastMousePos;


    void Awake ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
	}
	
	void Update ()
    {
//#if UNITY_ANDROID || UNITY_IOS || UNITY_WP8
//#else
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) 
            && (EventSystem.current.currentSelectedGameObject == null)
            && (GameManager.instance.applyingForce == false))
        {
            float dx = Time.deltaTime * baseSpeed;
            float dy = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - Camera.main.ScreenToWorldPoint(lastMousePos).y;

            //if(Mathf.Abs(dy) > Mathf.Abs(dx))
            //{
            //    if (dy >= 0)
            //        dy = Mathf.Abs(dx);
            //    else
            //        dy = -Mathf.Abs(dx);
            //}

            Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Vector3 nextPosition = new Vector3(lastPosition.x + dx, lastPosition.y + dy, lastPosition.z);

            AddNewPosition(nextPosition);

            lastMousePos = Input.mousePosition;
        }
//#endif
        //if (Input.touchCount == 1)
        //{
        //    if (Input.touches[0].phase == TouchPhase.Moved)
        //    {
        //        float dx = Time.deltaTime * baseSpeed;
        //        float dy = Input.touches[0].deltaPosition.y;

        //        //if(Mathf.Abs(dy) > Mathf.Abs(dx))
        //        //{
        //        //    if (dy >= 0)
        //        //        dy = Mathf.Abs(dx);
        //        //    else
        //        //        dy = -Mathf.Abs(dx);
        //        //}

        //        Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        //        Vector3 nextPosition = new Vector3(lastPosition.x + dx, lastPosition.y + dy, lastPosition.z);

        //        AddNewPosition(nextPosition);

        //        lastMousePos = Input.mousePosition;
        //    }
        //}
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
