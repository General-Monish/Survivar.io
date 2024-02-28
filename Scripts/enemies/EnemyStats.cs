using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObjects enemyStats;
    float currentMoveSpeed;
    float currentHealth;
    float currentdamage;

    private void Awake()
    {
        currentMoveSpeed = enemyStats.MoveSpeed;
        currentHealth = enemyStats.MaxHealth;
        currentdamage = enemyStats.Damage;
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

   public void Kill()
    {
        Destroy(gameObject);
    }
}
