using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerController player;
    SpawnManager spawnManager;
    public enum States
    {
        Chasing,
        DamageDealt
    }
    [Header("Movement")]
    public float moveSpeed, turnSpeed, onHitIncrease;
    
    [Header("BasicInformation")]
    public int damage = 20, health = 100;
    public float attackDelay = 2f;

    private float nextHit;

    private States state = States.Chasing;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
    }
    private void Update()
    {
        var look_dir = player.transform.position - gameObject.transform.position; look_dir.y = 0;
        if (health <= 0)
        {
            spawnManager.enemysLeft--;
            Destroy(gameObject);
        }
        switch(state)
        {
            case States.Chasing:
                if (Time.time > nextHit)
                gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(look_dir), turnSpeed * Time.deltaTime);
                gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
                break;
            case States.DamageDealt:
                if (Time.time > nextHit) state = States.Chasing;
                break;
        }
           
    }
    private void OnCollisionStay(Collision other)
{
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
            nextHit = Time.time + attackDelay;
            state = States.DamageDealt;
        }
    }

    public void Hit(int amount)
    {
        health -= amount;
        moveSpeed += onHitIncrease; turnSpeed += onHitIncrease;
    }
}
