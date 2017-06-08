using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public float thrustMax;
    public GameObject arrowPrefab;
    public float minVelocity = 1f;

    private Vector3 startMousePos = Vector3.zero;
    private GameObject dirArrow;
    private Vector2 lastValidVelo;
    private Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        dirArrow = Instantiate(arrowPrefab, gameObject.transform.position, Quaternion.identity);
        dirArrow.transform.parent = gameObject.transform;

        lastValidVelo = Vector2.zero;
    }
    void OnDestroy()
    {
        Destroy(dirArrow);
    }

    void LateUpdate()
    {
        Vector3 velo = rigidbody.velocity;
        float fVelo = velo.magnitude;
        Debug.Log("Ball velocity: " + velo + ", velo.magnitude = " + velo.magnitude);
        if (fVelo < minVelocity)
        {
            //float boostSpeed = minVelocity - fVelo;
            //Vector3 boostVelo = boostSpeed * velo.normalized;
            //rigidbody.AddForce(boostVelo);

            if (rigidbody.bodyType != RigidbodyType2D.Static)
            {
                rigidbody.velocity = velo.normalized * minVelocity;
            }
        }
    }

    public void ApplyForce(float thrustPercent, Vector3 dir)
    {
        rigidbody.AddForce(dir * thrustMax * thrustPercent, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!(GameManager.Instance && GameManager.Instance.gameStarted))
            return;
        Vector2 velo = rigidbody.velocity;
        
        
        if(Mathf.Abs(velo.magnitude) > 0.01)
        {
            lastValidVelo = velo;
            float angleV = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
            Quaternion qV = Quaternion.AngleAxis(angleV, Vector3.forward);
            dirArrow.transform.rotation = qV;
            dirArrow.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1);
        }
    }

    public Vector3 GetMoveDir()
    {
        Vector3 dir = new Vector3(lastValidVelo.x, lastValidVelo.y, 0f);
        return dir.normalized;
    }
}
