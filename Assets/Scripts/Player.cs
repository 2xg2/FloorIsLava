using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject linePrefab;
    public LineDataClass lineData;
    public bool gameOverOnGoingBack = false;
    //public float gameOverOnGoingBackTime = 2;

    public GameObject playerBall;
    public PlayerLine playerLine;

    float totalDistance = 0f;

    void Start()
    {
        CreateBall(ballPrefab);
        CreateLine(linePrefab, lineData);
        playerBall.GetComponent<PlayerBall>().ApplyForce(0.3f, Camera.main.transform.right);

        totalDistance = 0f;
    }

    public void CreateBall(GameObject ballPrefab)
    {
        playerBall = Instantiate(ballPrefab);
        playerBall.transform.parent = gameObject.transform;

        Vector3 startPos = GameManager.Instance.levelMgr.levelData.startPos;
        playerBall.transform.position = new Vector3(startPos.x, startPos.y, playerBall.transform.position.z);
    }

    public void CreateLine(GameObject linePrefab, LineDataClass lineAsset)
    {
        playerLine = Instantiate(linePrefab).GetComponent<PlayerLine>();
        playerLine.transform.parent = gameObject.transform;

        if (lineAsset)
        {
            playerLine.InitWithPoints(lineAsset.points);
        }

        if (playerBall)
        {
            Renderer ballRenderer = playerBall.GetComponent<Renderer>();
            float ballWidth = ballRenderer.bounds.size.x;
            float lineX = playerBall.transform.position.x;
            float lineY = playerBall.transform.position.y - ballWidth / 2;
            if (lineData)
            {
                lineX += lineData.startBallOffset.x;
                lineY += lineData.startBallOffset.y;
            }
            playerLine.transform.position = new Vector3(lineX, lineY, playerLine.transform.position.z);
        }
    }

    void OnDestroy()
    {
        if(playerBall)
            Destroy(playerBall.gameObject);
        if(playerLine)
            Destroy(playerLine.gameObject);
    }

    bool CheckGameOver()
    {
        bool gameOver = false;

        //ball falls over line
        RaycastHit2D[] hitsLast = Physics2D.RaycastAll(playerLine.transform.position + playerLine.GetOrCreateCurrentLine().GetLastPos(), playerLine.GetOrCreateCurrentLine().GetLastDirection());
        foreach (RaycastHit2D hit in hitsLast)
        {
            if (hit.collider != null && hit.collider.tag == "Ball")
            {
                gameOver = true;
            }
        }
        RaycastHit2D[] hitsFirst = Physics2D.RaycastAll(playerLine.transform.position + playerLine.GetOrCreateCurrentLine().GetFirstPos(), playerLine.GetOrCreateCurrentLine().GetFirstDirection());
        foreach (RaycastHit2D hit in hitsFirst)
        {
            if (hit.collider != null && hit.collider.tag == "Ball")
            {
                gameOver = true;
            }
        }

        //ball goes back
        if (gameOverOnGoingBack)
        {
            if (playerBall)
            {
                Vector2 velo = playerBall.GetComponent<Rigidbody2D>().velocity;
                if (velo.x < 0)
                {
                    gameOver = true;
                }
            }
        }

        if (gameOver)
        {
            StopBall();
            GameManager.Instance.GameOver();
        }

        return gameOver;
    }

    bool CheckGameWon()
    {
        bool gameWon = false;

        if (playerBall)
        {
            if(GameManager.Instance && GameManager.Instance.levelMgr && GameManager.Instance.levelMgr.levelData && GameManager.Instance.levelMgr.levelData.bounds.max.x < 0)
            {
                if (playerBall.transform.position.x > GameManager.Instance.levelMgr.levelData.bounds.max.x)
                {
                    gameWon = true;
                }
            }

        }

        if(gameWon)
        {
            StopBall();
            GameManager.Instance.GameWon();
        }

        return gameWon;
    }

    void Update()
    {
        CheckGameOver();
        CheckGameWon();
    }

    void StopBall()
    {
        if (playerBall)
        {
            playerBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //playerBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //playerBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void ImpulseBall()
    {
        playerBall.GetComponent<PlayerBall>().ApplyForce(1f, playerBall.GetComponent<PlayerBall>().GetMoveDir());
    }

    public Transform GetCameraTarget()
    {
        if (playerBall)
            return playerBall.transform;
        else
            return transform;
    }

    public float GetBallMaxDistance()
    {
        if (playerBall)
        {
            float dist = playerBall.transform.position.x - GameManager.Instance.levelMgr.levelData.startPos.x;

            if (totalDistance < dist)
                totalDistance = dist;
        }

        return totalDistance;
    }
}
