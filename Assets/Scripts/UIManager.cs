﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject totalDistance;
    public GameObject totalTime;
    public GameObject totalStars;

    void Update()
    {
        totalDistance.GetComponent<Text>().text = "Distance: " + GameManager.instance.totalDistance.ToString("F2");
        totalTime.GetComponent<Text>().text = "Time: " + GameManager.instance.totalTime.ToString("F2");
        totalStars.GetComponent<Text>().text = "Stars: " + GameManager.instance.totalStars.ToString();
    }
}