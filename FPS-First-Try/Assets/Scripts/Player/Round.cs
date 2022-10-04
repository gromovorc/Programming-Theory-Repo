using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    public int damage;
    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        trail.Clear();
    }
    private void OnCollisionEnter(Collision other)
    {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

        if (enemy != null && enemy.health > 0)
        {
            enemy.Hit(damage);
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(false);
    }
}
