using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "die")
        {
            GameplayController.instance.TakeDamage();
            Destroy(gameObject);
            Debug.Log("player");
        }
    }
}
