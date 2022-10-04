using System;
using UnityEngine;

public class HealingEnemyController : EnemyController
{
    HealthPack healthPack;
    [SerializeField] private int healAmount;
    [SerializeField] private float radius, healingRate;
    [SerializeField] private AudioClip healSound;

    private bool needHealing;

    new private void Start()
    {
        audioSource.volume *= MenuUIManager.volume;
        InvokeRepeating(nameof(Damaging), 0.5f, 10.0f);
        OnWaveIncrease(spawnManager.waveCount);
        if (UnityEngine.Random.Range(1, 20) == 1) onDeathDrop = true;
    }

    protected override void EnemyBehavior()
    {
        switch (state)
        {
            case States.Chasing:
                Moving(GetLookDir(player.gameObject));
                break;
            case States.SpecialAttack:
                {
                    if (needHealing)
                    {
                        try
                        {
                            healthPack = FindObjectOfType<HealthPack>().GetComponent<HealthPack>();
                            Moving(GetLookDir(healthPack.gameObject));
                        }
                        catch (Exception)
                        {
                            state = States.Chasing;
                            needHealing = false;
                        }
                    }
                    else state = States.Chasing;
                    break;
                    }
            case States.DamageDone:
                {
                    if (Time.time > nextHit) state = States.Chasing;
                    else Moving(-GetLookDir(player.gameObject));
                    break;
                }
                }
        }
  
    protected override void Damaging()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
        audioSource.PlayOneShot(healSound);
        foreach (Collider collider in hitColliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Hit(-healAmount);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            moveSpeed += 0.1f;
            nextHit = Time.time + attackDelay;
            state = States.DamageDone;
        }
    }

    override public void Hit(int amount)
    {
        health -= amount;
        if (health < 1) Death();
        else if (health < 35)
        {
            needHealing = true;
            state = States.SpecialAttack;
        }
    }

    private override protected void Death()
    {
        gameManager.ChangeScore(scorePoints);
        if (onDeathDrop) spawnManager.SpawnHealthPack();
        Destroy(gameObject);
    }
}
