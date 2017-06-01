using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallImpulse : MonoBehaviour
{
    public float thrustMax;
    public GameObject arrowPrefab;

    private Vector3 startMousePos = Vector3.zero;
    private GameObject arrow;
    private float arrowWidth = 0f;
    private Vector3 v;

    void Start()
    {
        arrow = Instantiate(arrowPrefab);//, gameObject.transform.position, Quaternion.identity);
        //arrow.transform.parent = gameObject.transform;
        //arrow.transform.Translate(0f, 0f, -1f);
    }
    void OnDestroy()
    {
        Destroy(arrow);
    }

    public void ApplyForce(float thrustPercent, Vector3 dir)
    {
        GetComponent<Rigidbody2D>().AddForce(dir * thrustMax * thrustPercent, ForceMode2D.Impulse);
    }

    void Update()
    {
        Debug.Log("Ball velocity: " + GetComponent<Rigidbody2D>().velocity);
        Vector2 velo = GetComponent<Rigidbody2D>().velocity;
        float angleV = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
        Quaternion qV = Quaternion.AngleAxis(angleV, Vector3.forward);
        arrow.transform.rotation = qV;
        arrow.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1);
        return;

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y), Vector2.zero);
            if(hit)
            {
                if(hit.transform.gameObject == gameObject)
                {
                    GameManager.Instance.applyingForce = true;
                    startMousePos = mousePos;
                }
            }
        }
        if(Input.GetMouseButton(0) && GameManager.Instance.applyingForce)
        {
            if(!arrow)
            {
                arrow = Instantiate(arrowPrefab, gameObject.transform.position, Quaternion.identity);
                arrowWidth = arrow.GetComponent<Renderer>().bounds.size.x;
                v = arrow.transform.position - gameObject.transform.position;
                arrow.transform.localScale = Vector3.zero;                
            }

            Vector3 mousePos = Input.mousePosition;
            Vector3 mousePosW = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 startMousePosW = Camera.main.ScreenToWorldPoint(startMousePos);
            Vector3 mouseDistanceW = mousePosW - startMousePosW;
            float distanceW = mouseDistanceW.magnitude;

            float arrowScale = distanceW / arrowWidth;
            if (arrowScale > 1f)
                arrowScale = 1f;
            arrow.transform.localScale = new Vector3(arrowScale, arrowScale, 0f);

            //Vector3 rotation = new Vector3(0f, 0f, mouseDistanceW.z);
            //arrow.transform.RotateAround(gameObject.transform.position, mouseDistanceW, 20);

            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            arrow.transform.position = gameObject.transform.position + q * v;
            arrow.transform.rotation = q;

        }
        if(Input.GetMouseButtonUp(0) && GameManager.Instance.applyingForce)
        {
            if(arrow)
            {
                GameManager.Instance.applyingForce = false;
                startMousePos = Vector3.zero;

                Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gameObject.transform.position);
                dir.Normalize();
                ApplyForce(arrow.transform.localScale.x, dir);
                Destroy(arrow);                
            }
        }
    }
}
