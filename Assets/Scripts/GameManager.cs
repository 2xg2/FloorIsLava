using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject ballPrefab;
    public GameObject linePrefab;

    public LevelManager levelMgr;

    /*[HideInInspector] */public Transform cameraFollowTarget;

    GameObject ball;
    LineBaseWithInput line;

    [HideInInspector] public float totalDistance = 0f;
    [HideInInspector] public float totalTime = 0f;
    [HideInInspector] public int totalStars = 0;

    [HideInInspector] public bool applyingForce = false;

    private bool cameraOnLine = false;
    private bool gameStarted = false;

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
        line = Instantiate(linePrefab).GetComponent<LineBaseWithInput>();

        PositionStuff();

        cameraFollowTarget = /*line.endMark.transform;//*/ball.transform;

        totalDistance = 0f;
        totalTime = 0f;
        totalStars = 0;

        levelMgr.CreateLevel();

        gameStarted = true;
    }

    void PositionStuff()
    {
        ball.transform.position = levelMgr.levelData.startPos;
        
        Renderer ballRenderer = ball.GetComponent<Renderer>();
        float ballWidth = ballRenderer.bounds.size.x;
        float lineWith = line.GetComponent<LineRenderer>().startWidth;
        float lineX = ball.transform.position.x - ballWidth / 2 - lineWith / 2;
        float lineY = ball.transform.position.y - ballWidth / 2 - lineWith / 2;
        line.transform.position = new Vector3(lineX, lineY, ball.transform.position.z);
    }

    void Update()
    {
        if(gameStarted)
        {
            totalDistance = ball.transform.position.x - levelMgr.levelData.startPos.x;
            totalTime += Time.deltaTime;

            if (ball.transform.position.x > levelMgr.levelData.bounds.max.x)
            {
                Restart();
            }
        }

    }

    public void Restart()
    {
        gameStarted = false;

        Destroy(ball.gameObject);
        Destroy(line.gameObject);
        levelMgr.DestroyLevel();
        Start();        
    }

    

    public void StopBall()
    {
        if (gameStarted)
        {
            ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void ImpulseBall()
    {
        ball.GetComponent<BallImpulse>().ApplyForce(1f, ball.GetComponent<Rigidbody2D>().velocity.normalized);
    }

    public void OnItemStar()
    {
        totalStars++;
    }

    public void OnCameraToggle()
    {
        if (gameStarted)
        { 
            cameraOnLine = !cameraOnLine;
            if (cameraOnLine)
            {
                cameraFollowTarget = line.endMark.transform;
            }
            else
            {
                cameraFollowTarget = ball.transform;
            }
        }
    }
}
