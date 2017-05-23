using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStar : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.OnItemStar();
        Destroy(gameObject);
    }
}
