using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter(Collision other)
    {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

        if (enemy != null && enemy.health > 0)
        {
            enemy.Hit(damage);
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }
}
