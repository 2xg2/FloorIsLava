using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    //public GameObject target;

    void Update()
    {
        if (GameManager.instance.cameraFollowTarget != null)
        {
            gameObject.transform.position = GameManager.instance.cameraFollowTarget.position + offset;
        }
    }
}
