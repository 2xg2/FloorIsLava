using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameStartPanel;
    public GameObject gameOverPanel;
    public GameObject gameWonPanel;

    public GameObject totalDistance;
    public GameObject totalTime;
    public GameObject totalStars;

    void Start()
    {
        if(gameStartPanel)
        {
            gameStartPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            gameWonPanel.SetActive(false);
        }
    }

    public void OnGameStart()
    {
        gameStartPanel.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void OnGameOver_Restart()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    public void OnGameWon()
    {
        gameWonPanel.SetActive(true);
    }

    public void OnGameWon_Restart()
    {
        gameWonPanel.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    void Update()
    {
        totalDistance.GetComponent<Text>().text = "Distance: " + GameManager.Instance.totalDistance.ToString("F2");
        totalTime.GetComponent<Text>().text = "Time: " + GameManager.Instance.totalTime.ToString("F2");
        totalStars.GetComponent<Text>().text = "Stars: " + GameManager.Instance.totalStars.ToString();
    }
}
