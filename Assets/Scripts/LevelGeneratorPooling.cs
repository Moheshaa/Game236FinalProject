using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorPooling : MonoBehaviour
{
    [SerializeField]
    private Transform platform, platform_parent;

    [SerializeField]
    private Transform enemy, enemy_parent;

    [SerializeField]
    private Transform health_Collectable, health_Collectable_parent;

    [SerializeField]
    private int LevelLength = 100;

    [SerializeField]
    private float distance_between_platforms = 15f;

    [SerializeField]
    private float MIN_Position_Y = 0f, MAX_Position_Y = 7f;

    [SerializeField]
    private float chanceForEnemyExistence = 0.25f, chanceForHealthExistence = 0.1f;

    [SerializeField]
    private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

    private float platformLastPositionX;
    private Transform[] platform_array;

    void Start()
    {
        createPlatform();
    }

    void createPlatform()
    {
        platform_array = new Transform[LevelLength];
        for (int i = 0; i < platform_array.Length; i++)
        {
            Transform newPlatform = (Transform)Instantiate(platform, Vector3.zero, Quaternion.identity);
            platform_array[i] = newPlatform;
        }

        for (int i = 0; i < platform_array.Length; i++)
        {
            float platformLastPositionY = Random.Range(MIN_Position_Y, MAX_Position_Y);

            Vector3 platformPosition;

            if (i < 2)
            {
                platformLastPositionY = 0f;
            }
            platformPosition = new Vector3(distance_between_platforms * i, platformLastPositionY, 0);

            platformLastPositionX = platformPosition.x;

            platform_array[i].position = platformPosition;
            platform_array[i].parent = platform_parent;

            SpawnHealthandEnemy(platformPosition, i, true);
        }
    }

    public void PoolingPLatforms()
    {
        for (int i = 0; i < platform_array.Length; i++)
        {
            if (!platform_array[i].gameObject.activeInHierarchy)
            {
                platform_array[i].gameObject.SetActive(true);
                float platformPositionY = Random.Range(MIN_Position_Y, MAX_Position_Y);
                Vector3 platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, platformPositionY, 0);

                platform_array[i].position = platformPosition;
                platformLastPositionX = platformPosition.x;

                SpawnHealthandEnemy(platformPosition, i, false);
            }
        }
    }


    void SpawnHealthandEnemy(Vector3 platformPosition, int i, bool gameStarted)
    {
        if (i > 3)
        {
            if (Random.Range(0f, 1f) < chanceForEnemyExistence)
            {
                if (gameStarted)
                {
                    platformPosition = new Vector3(distance_between_platforms * i, platformPosition.y + 0.1f, 0);
                }
                else
                {
                    platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, platformPosition.y + 0.1f, 0);
                }

                Transform createEnemy = (Transform)Instantiate(enemy, platformPosition, Quaternion.Euler(0, -90, 0));

                createEnemy.parent = enemy_parent;
            }

            if (Random.Range(0f, 1f) < chanceForHealthExistence)
            {
                if (gameStarted)
                {
                    platformPosition = new Vector3(distance_between_platforms * i, platformPosition.y + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);
                }
                else
                {
                    platformPosition = new Vector3(distance_between_platforms + platformLastPositionX, platformPosition.y + Random.Range(healthCollectable_MinY, healthCollectable_MaxY), 0);
                }

                Transform createHealthCollectable = (Transform)Instantiate(health_Collectable, platformPosition, Quaternion.identity);

                createHealthCollectable.parent = health_Collectable_parent;
            }
        }
    }
}
