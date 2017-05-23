using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalsPair : MonoBehaviour
{
    public GameObject orangePrefab;
    public GameObject bluePrefab;

    private Portal orangePortal;
    private Portal bluePortal;

    public void CreateAndPos(Vector3 orangePos, Vector3 bluePos)
    {
        orangePortal = Instantiate(orangePrefab, orangePos, Quaternion.identity, this.transform).GetComponent<Portal>();
        orangePortal.parentPair = this;
        bluePortal = Instantiate(bluePrefab, bluePos, Quaternion.identity, this.transform).GetComponent<Portal>();
        bluePortal.parentPair = this;
    }

    public void OnCollision(Portal portal)
    {

    }
}
