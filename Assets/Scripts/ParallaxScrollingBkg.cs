using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrollingBkg : MonoBehaviour
{
    public float backgroundSizeX;
    public float backgroundSizeY;

    public bool hasScrollingX = false;
    private bool hasScrollingY = false;

    public bool hasParallaxX = false;
    public bool hasParallaxY = false;
    public float parallaxSpeed;

    List<Transform> tilesX;
    int leftIndexX = 0;
    int rightIndexX = 0;
    Vector3 lastCamPos;

    void Start()
    {
        tilesX = new List<Transform>();
        foreach (Transform t in gameObject.transform)
        {
            tilesX.Add(t);
        }
        if(tilesX.Count < 2 && hasScrollingX)
        {
            Debug.LogError("tiles.Count = " + tilesX.Count + " too small for hasScrolling, must be min 2");
            hasScrollingX = false;
        }

        for (int i = 0; i < tilesX.Count; i++)
        {
            float x = transform.position.x - backgroundSizeX * tilesX.Count / 2 + backgroundSizeX / 2 + i * backgroundSizeX;
            tilesX[i].transform.position = new Vector3(x, tilesX[i].transform.position.y, tilesX[i].transform.position.z);
        }

        leftIndexX = 0;
        rightIndexX = tilesX.Count - 1;



        lastCamPos = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        if(hasScrollingX || hasScrollingY)
        {
            UpdateScrolling();
        }

        if (hasParallaxX || hasParallaxY)
        {
            UpdateParallax();
        }
    }

    float GetViewHalfWidth()
    {
        float viewHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / (float)Screen.height);
        return viewHalfWidth;
    }

    void UpdateScrolling()
    {
        if(hasScrollingX)
        {
            float viewHalfWidth = GetViewHalfWidth();
            if ((Camera.main.transform.position.x - viewHalfWidth) < (tilesX[leftIndexX].position.x - backgroundSizeX / 2))
            {
                ScrollLeft();
            }
            if ((Camera.main.transform.position.x + viewHalfWidth) > (tilesX[rightIndexX].position.x + backgroundSizeX / 2))
            {
                ScrollRight();
            }
        }
    }

    void ScrollLeft()
    {
        tilesX[rightIndexX].position = tilesX[leftIndexX].position - new Vector3(backgroundSizeX, 0f, 0f);
        leftIndexX = rightIndexX;
        rightIndexX--;
        if (rightIndexX < 0)
            rightIndexX = tilesX.Count - 1;

        transform.position = new Vector3(transform.position.x - backgroundSizeX, transform.position.y, transform.position.z);
        for(int i = 0; i < tilesX.Count; i++)
        {
            tilesX[i].transform.position = new Vector3(tilesX[i].transform.position.x + backgroundSizeX, tilesX[i].transform.position.y, tilesX[i].transform.position.z);
        }
    }

    void ScrollRight()
    {
        tilesX[leftIndexX].position = tilesX[rightIndexX].position + new Vector3(backgroundSizeX, 0f, 0f);
        rightIndexX = leftIndexX;
        leftIndexX++;
        if (leftIndexX > tilesX.Count - 1)
            leftIndexX = 0;

        transform.position = new Vector3(transform.position.x + backgroundSizeX, transform.position.y, transform.position.z);
        for (int i = 0; i < tilesX.Count; i++)
        {
            tilesX[i].transform.position = new Vector3(tilesX[i].transform.position.x - backgroundSizeX, tilesX[i].transform.position.y, tilesX[i].transform.position.z);
        }
    }
  
    void UpdateParallax()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        if (hasParallaxX)
        {
            float dx = Camera.main.transform.position.x - lastCamPos.x;
            x += dx * parallaxSpeed;

            if (!hasScrollingX)
            {
                float viewHalfWidth = GetViewHalfWidth();

                if (Camera.main.transform.position.x - viewHalfWidth < x - backgroundSizeX * tilesX.Count / 2)
                {
                    x = Camera.main.transform.position.x - viewHalfWidth + backgroundSizeX * tilesX.Count / 2;
                }
                else if (Camera.main.transform.position.x + viewHalfWidth > x + backgroundSizeX * tilesX.Count / 2)
                {
                    x = Camera.main.transform.position.x + viewHalfWidth - backgroundSizeX * tilesX.Count / 2;
                }
            }
        }

        if(hasParallaxY)
        {
            float dy = Camera.main.transform.position.y - lastCamPos.y;
            y += dy * parallaxSpeed;
            int tilesYCount = 1;
            if (!hasScrollingY)
            {
                float viewHalfHeight = Camera.main.orthographicSize;

                if (Camera.main.transform.position.y - viewHalfHeight < y - backgroundSizeY * tilesYCount / 2)
                {
                    y = Camera.main.transform.position.y - viewHalfHeight + backgroundSizeY * tilesYCount / 2;
                }
                else if (Camera.main.transform.position.y + viewHalfHeight > y + backgroundSizeY * tilesYCount / 2)
                {
                    y = Camera.main.transform.position.y + viewHalfHeight - backgroundSizeY * tilesYCount / 2;
                }
            }
        }
        
        transform.position = new Vector3(x, y, transform.position.z);

        lastCamPos = Camera.main.transform.position;
    }
}
