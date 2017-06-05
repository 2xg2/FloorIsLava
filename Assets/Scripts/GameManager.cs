using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public GameObject playerPrefab;

    public LevelManager levelMgr;

    public UIManager uiManager;

    /*[HideInInspector] */public Transform cameraFollowTarget;

    Player m_Player;

    [HideInInspector] public float totalDistance = 0f;
    [HideInInspector] public float totalTime = 0f;
    [HideInInspector] public int totalStars = 0;

    [HideInInspector] public bool applyingForce = false;

    private bool cameraOnLine = false;
    [HideInInspector] public bool gameStarted = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    public void StartGame()
    {
        m_Player = Instantiate(playerPrefab).GetComponent<Player>(); //new GameObject("Player", new[] { typeof(Player) }).GetComponent<Player>();

        totalDistance = 0f;
        totalTime = 0f;
        totalStars = 0;

        levelMgr.CreateLevel();

        gameStarted = true;
    }

    void Update()
    {
        if (m_Player && m_Player.GetCameraTarget())
        {
            cameraFollowTarget = m_Player.GetCameraTarget();
        }

        if (gameStarted)
        {
            totalDistance = m_Player.GetBallMaxDistance();
            totalTime += Time.deltaTime;
        }

    }

    public void GameWon()
    {
        gameStarted = false;
        uiManager.OnGameWon();
    }

    public void GameOver()
    {
        gameStarted = false;
        
        if(uiManager)
            uiManager.OnGameOver();
    }

    public void RestartGame()
    {
        gameStarted = false;

        Destroy(m_Player.gameObject);
        levelMgr.DestroyLevel();

        StartGame();        
    }

    public void ImpulseBall()
    {
        m_Player.ImpulseBall();
    }

    public void OnItemStar()
    {
        totalStars++;
    }
}
