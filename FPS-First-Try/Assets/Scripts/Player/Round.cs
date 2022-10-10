using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    public int damage;
    private TrailRenderer _trail;

    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        _trail.Clear();
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
