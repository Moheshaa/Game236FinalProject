using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyDieEffect;
    public Transform bullet;
    public float distanceFromPlayerToStartMove = 20f;
    public float movementSpeed_Min = 1f;
    public float movementSpeed_Max = 2f;

    private bool moveRight;
    private float movementSpeed;
    private bool isPlayerInTheRegion;

    private Transform playerTransform;

    public bool canShoot;

    void Start()
    {
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }

        movementSpeed = Random.Range(movementSpeed_Min, movementSpeed_Max);
        playerTransform = GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG).transform;
    }


    void Update()
    {
        if (playerTransform)
        {
            float distanceFromPlayer = (playerTransform.position - transform.position).magnitude;
            if (distanceFromPlayer < distanceFromPlayerToStartMove)
            {
                if (moveRight)
                {
                    Vector3 temp = transform.position;
                    temp.x += Time.deltaTime * movementSpeed;
                    transform.position = temp;
                    // transform.position = new Vector3(transform.position.x + Time.deltaTime * movementSpeed, transform.position.y, transform.position.z);
                }
                else
                {
                    Vector3 temp = transform.position;
                    temp.x += Time.deltaTime * movementSpeed;
                    transform.position = temp;
                }

                if (!isPlayerInTheRegion)
                {
                    if (canShoot)
                    {
                        InvokeRepeating("StartShooting", 0.5f, 1.5f);
                    }
                    isPlayerInTheRegion = true;

                }
            }
            else { CancelInvoke("StartShooting"); }
        }
    }

    void StartShooting()
    {
        if (playerTransform)
        {
            Vector3 bulletpos = transform.position;
            bulletpos.y += 1.5f;
            bulletpos.x -= 1f;
            Transform newBullet = (Transform)Instantiate(bullet, bulletpos, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }
    }

    void EnemyDied()
    {
        Vector3 effectPos = transform.position;
        effectPos.y += 2f;
        Instantiate(enemyDieEffect, effectPos, Quaternion.identity);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.PLAYER_BULLET_TAG)
        {
            GameplayController.instance.IncrementScore(200);
            EnemyDied();
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == Tags.PLAYER_TAG)
        {
            EnemyDied();
        }

    }


}
