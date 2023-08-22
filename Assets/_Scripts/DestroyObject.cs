using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour, IDamagable
{
   
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; } = 500f;

    bool gracePeriod = true;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        Invoke("GracePeriodEnd", 1f);
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth-= damageAmount;

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        string otherTag = other.gameObject.tag;
        if ((otherTag == "Bird" || otherTag == "Pig" || otherTag == "Blocks") && !gracePeriod)
        {

            var speed = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

            
            Damage(10f * speed);
            if(otherTag == "Bird")
            print(10f * speed + " - " + other.gameObject.name);
            
            

        }


    }

    void GracePeriodEnd()
    {
        gracePeriod = false;
    }

}
