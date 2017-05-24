using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ballPrefab;
    public GameObject linePrefab;

    public LevelManager levelMgr;

    [HideInInspector] public Transform cameraFollowTarget;

    /*[HideInInspector]*/ public GameObject ball;
    /*[HideInInspector]*/ public LineRendererWithInput line;

    [HideInInspector] public float totalDistance = 0f;
    [HideInInspector] public float totalTime = 0f;
    [HideInInspector] public int totalStars = 0;

    [HideInInspector] public bool applyingForce = false;

    private bool cameraOnLine = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        ball = Instantiate(ballPrefab);
        line = Instantiate(linePrefab).GetComponent<LineRendererWithInput>();

        PositionStuff();

        cameraFollowTarget = ball.transform;

        totalDistance = 0f;
        totalTime = 0f;
        totalStars = 0;

        levelMgr.CreateLevel();
    }

    void PositionStuff()
    {
        ball.transform.position = levelMgr.startPos;
        
        Renderer ballRenderer = ball.GetComponent<Renderer>();
        float ballWidth = ballRenderer.bounds.size.x;
        float lineWith = linePrefab.GetComponent<LineRenderer>().startWidth;
        float lineX = ball.transform.position.x - ballWidth / 2 - lineWith / 2;
        float lineY = ball.transform.position.y - ballWidth / 2 - lineWith / 2;
        linePrefab.transform.position = new Vector3(lineX, lineY, ball.transform.position.z);
    }

    void FixedUpdate()
    {
        //Debug.Log("ball velo: x = " + ball.GetComponent<Rigidbody2D>().velocity.x + ", y = " + ball.GetComponent<Rigidbody2D>().velocity.y);
        totalDistance = ball.transform.position.x - levelMgr.startPos.x;
        totalTime += Time.deltaTime;

        if (ball.transform.position.x > levelMgr.bounds.max.x)
        {
            Restart();
        }
    }

    public void Restart()
    {
        Destroy(ball.gameObject);
        Destroy(line.gameObject);
        levelMgr.DestroyLevel();
        Start();        
    }

    

    public void StopBall()
    {
        ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnItemStar()
    {
        totalStars++;
    }

    public void OnItemPortalOrange()
    {
        
    }

    public void OnCameraToggle()
    {
        cameraOnLine = !cameraOnLine;
        if(cameraOnLine)
        {
            cameraFollowTarget = line.endMark;
        }
        else
        {
            cameraFollowTarget = ball.transform;
        }
    }
}
