using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed, turnSpeed;
    public int damage, health = 100;

    private void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log(other.transform.tag);
            other.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
        }
    }

    public void Hit(int amount)
    {
        health -= amount;
    }
}
