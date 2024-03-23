using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int levelLength;

    [SerializeField]
    private int startPlatformLength = 5, endPlatformLength = 5;

    [SerializeField]
    private int distanceBetweenPlatforms;

    [SerializeField]
    private Transform platformPrefab, platformParent;

    [SerializeField]
    private Transform enemyPrefab, enemyParent;

    [SerializeField]
    private Transform healthCollectible, healthParent;

    [SerializeField]
    private float platformPosition_minY = 0f, platformPosition_maxY = 10f;

    [SerializeField]
    private int platformLength_Min = 1, platformLength_Max = 4;

    [SerializeField]
    private float chanceForEnemy = 0.25f, chanceForCollectible = 0.2f;

    [SerializeField]
    private float healthCollectible_MinY = 1f, healthCollectible_MaxY = 3f;

    private float platformLastPositionX;

    private enum PLatformType
    {
        None,
        Flat
    }

    private class PlatformPositionInfo
    {
        public PLatformType platformType;
        public float positionY;
        public bool hasEnemy;
        public bool hasHealthCollectible;

        public PlatformPositionInfo(PLatformType type, float posY, bool has_enemy, bool has_collectible)
        {
            platformType = type;
            positionY = posY;
            hasEnemy = has_enemy;
            hasHealthCollectible = has_collectible;
        }

    }


    private void Start()
    {
        GenerateLevel(true);
    }
    void FillOutPositionInfo(PlatformPositionInfo[] platformInfo)
    {
        int currentPlatformInfoIndex = 0;
        for (int i = 0; i < startPlatformLength; i++)
        {
            platformInfo[currentPlatformInfoIndex].platformType = PLatformType.Flat;
            platformInfo[currentPlatformInfoIndex].positionY = 0f;

            currentPlatformInfoIndex++;
        }

        while (currentPlatformInfoIndex < levelLength - endPlatformLength)
        {
            if (platformInfo[currentPlatformInfoIndex - 1].platformType != PLatformType.None)
            {
                currentPlatformInfoIndex++;
                continue;
            }
            float platformPositionY = Random.Range(platformPosition_minY, platformPosition_maxY);

            int platformLength = Random.Range(platformLength_Min, platformLength_Max);

            for (int i = 0; i < platformLength; i++)
            {
                bool has_enemy = (Random.Range(0f, 1f) < chanceForEnemy);
                bool has_healthCollectible = (Random.Range(0f, 1f) < chanceForCollectible);

                platformInfo[currentPlatformInfoIndex].platformType = PLatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = platformPositionY;
                platformInfo[currentPlatformInfoIndex].hasEnemy = has_enemy;
                platformInfo[currentPlatformInfoIndex].hasHealthCollectible = has_healthCollectible;

                currentPlatformInfoIndex++;

                if (currentPlatformInfoIndex > (levelLength - endPlatformLength))
                {
                    currentPlatformInfoIndex = levelLength - endPlatformLength;
                    break;
                }

            }

            for (int i = 0; i < endPlatformLength; i++)
            {
                platformInfo[currentPlatformInfoIndex].platformType = PLatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = 0f;

                currentPlatformInfoIndex++;
            }
        }
    }

    void CreatePlatformsPositionInfo(PlatformPositionInfo[] platformPositionInfo, bool GameStarted)
    {
        for (int i = 0; i < platformPositionInfo.Length; i++)
        {
            PlatformPositionInfo positionInfo = platformPositionInfo[i];

            if (positionInfo.platformType == PLatformType.None)
            {
                continue;
            }

            Vector3 platformPosition;
            //check if the game is started or not
            if (GameStarted)
            {
                platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY, 0);
            }
            else { platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY, 0); }

            platformLastPositionX = platformPosition.x;

            Transform createBlock = Instantiate(platformPrefab, platformPosition, Quaternion.identity);
            createBlock.parent = platformParent;

            if (positionInfo.hasEnemy)
            {
                if (GameStarted)
                {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY + 0.1f, 0);
                }
                else
                {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY + 0.1f, 0);

                }

                Transform createEnemy = (Transform)Instantiate(enemyPrefab, platformPosition, Quaternion.Euler(0, -90, 0));
                createEnemy.parent = enemyParent;

            }
            if (positionInfo.hasHealthCollectible)
            {
                if (GameStarted)
                {
                    platformPosition = new Vector3(distanceBetweenPlatforms * i, positionInfo.positionY + Random.Range(healthCollectible_MinY, healthCollectible_MaxY), 0);
                }
                else
                {
                    platformPosition = new Vector3(distanceBetweenPlatforms + platformLastPositionX, positionInfo.positionY + Random.Range(healthCollectible_MinY, healthCollectible_MaxY), 0);

                }

                Transform craeteHealthCollectible = (Transform)Instantiate(healthCollectible, platformPosition, Quaternion.identity);
                craeteHealthCollectible.parent = healthParent;

            }
        }
    }

    public void GenerateLevel(bool GameStarted)
    {
        PlatformPositionInfo[] platformInfo = new PlatformPositionInfo[levelLength];
        for (int i = 0; i < platformInfo.Length; i++)
        {
            platformInfo[i] = new PlatformPositionInfo(PLatformType.None, -1f, false, false);
        }

        FillOutPositionInfo(platformInfo);
        CreatePlatformsPositionInfo(platformInfo, GameStarted);
    }
}

