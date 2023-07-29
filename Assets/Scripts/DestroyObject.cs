using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    float health = 500f;
    float speed = 5f;
   


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            speed = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

            health = health - (100f * speed);
            print(health);

        }


        if (other.gameObject.CompareTag("Player") && health <= 0)
        {
            Destroy(gameObject);
        }
        

    }

}
