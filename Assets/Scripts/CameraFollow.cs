using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.cameraFollowTarget != null)
        {
            gameObject.transform.position = GameManager.Instance.cameraFollowTarget.position + offset;
        }
    }
}
