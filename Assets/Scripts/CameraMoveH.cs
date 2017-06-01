using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveH : MonoBehaviour
{
    public float cameraSpeed;

    void LateUpdate()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.001f)
        {
            float dx = Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed;
            Camera.main.transform.Translate(new Vector3(dx, 0f, 0f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.001f)
        {
            float dy = Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed;
            Camera.main.transform.Translate(new Vector3(0f, dy, 0f));
        }
    }
}