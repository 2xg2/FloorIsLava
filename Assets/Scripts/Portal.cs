using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public PortalsPair parentPair { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ball")
        {
            parentPair.OnCollision(this);
        }
    }
}
