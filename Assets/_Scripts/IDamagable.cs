using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    float CurrentHealth { get; set; }
    float MaxHealth { get; set; }

    void Damage(float damageAmount);

    void Die();
}
