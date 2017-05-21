using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Follow : MonoBehaviour
{
    public Sprite on;
    public Sprite off;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GameManager.instance.line.GetComponent<LineRenderer>();
        GetComponent<SpriteRenderer>().sprite = off;
        SetPosition();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) 
            && (EventSystem.current.currentSelectedGameObject == null)
            && (GameManager.instance.applyingForce == false))
        {
            GetComponent<SpriteRenderer>().sprite = on;
        }
        if(Input.GetMouseButtonUp(0) && (EventSystem.current.currentSelectedGameObject == null))
        {
            GetComponent<SpriteRenderer>().sprite = off;
        }
        if (Input.GetMouseButton(0)
            && (EventSystem.current.currentSelectedGameObject == null)
            && (GameManager.instance.applyingForce == false))
        {
            SetPosition();
        }
    }
    void SetPosition()
    {
        gameObject.transform.position = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(lineRenderer.positionCount - 1));
    }
}
