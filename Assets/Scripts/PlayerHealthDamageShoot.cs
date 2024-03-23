using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDamageShoot : MonoBehaviour
{
    [SerializeField]
    private Transform playerBullet;
    private float distanceBeforeNewPlatform = 120f;

    private LevelGenerator levelGenerator;
    private LevelGeneratorPooling levelGenerator_Pooling;

    [HideInInspector]
    public bool canShoot;

    private Button ShootBTN;

    // Start is called before the first frame update
    void Awake()
    {
        levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR_OBJ).GetComponent<LevelGenerator>();

        levelGenerator_Pooling = GameObject.Find(Tags.LEVEL_GENERATOR_OBJ).GetComponent<LevelGeneratorPooling>();

        ShootBTN = GameObject.Find(Tags.SHOOT_BUTTON).GetComponent<Button>();
        ShootBTN.onClick.AddListener(() => Shoot());
    }


    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 bulletpos = transform.position;
            bulletpos.y += 1.5f;
            bulletpos.x += 1f;
            Transform newBullet = (Transform)Instantiate(playerBullet, bulletpos, Quaternion.identity);

            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }
    }

    public void Shoot()
    {
        Vector3 bulletpos = transform.position;
        bulletpos.y += 1.5f;
        bulletpos.x += 1f;
        Transform newBullet = (Transform)Instantiate(playerBullet, bulletpos, Quaternion.identity);

        newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
        newBullet.parent = transform;
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.ENEMY_BULLET_TAG)
        {
            GameplayController.instance.TakeDamage();
            Destroy(gameObject);
        }

        // if (target.tag == "die")
        // {
        //     GameplayController.instance.TakeDamage();
        //     Destroy(gameObject);
        // }

        if (target.tag == Tags.HEALTH_TAG)
        {
            GameplayController.instance.incrementHealth();
            target.gameObject.SetActive(false);
        }
        if (target.tag == Tags.MORE_PLATFORMS)
        {
            Vector3 temp = target.transform.position;
            temp.x += distanceBeforeNewPlatform;
            target.transform.position = temp;

            // levelGenerator.GenerateLevel(false);
            levelGenerator_Pooling.PoolingPLatforms();
        }

    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == Tags.ENEMY_TAG)
        {
            GameplayController.instance.TakeDamage();
            Destroy(gameObject);
        }

    }
}
