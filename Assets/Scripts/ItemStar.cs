using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStar : MonoBehaviour
{
    void OnTriggerEnter2D()
    {
        GameManager.instance.OnItemStar();
        Destroy(gameObject);
    }
}
