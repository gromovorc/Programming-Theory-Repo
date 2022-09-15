using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed, turnSpeed;
    public int damage;
    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log(collision.transform.tag);
            collision.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
        }
    }
}
