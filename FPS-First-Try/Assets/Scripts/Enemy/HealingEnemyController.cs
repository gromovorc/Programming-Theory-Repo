using System;
using UnityEngine;

public class HealingEnemyController : EnemyController
{
    HealthPack healthPack;
    [SerializeField] private int healAmount;
    [SerializeField] private float radius, healingRate;
    [SerializeField] private AudioClip _healSound;

    private bool needHealing;

    new private void Start()
    {
        _audioSource.volume *= MenuUIManager.volume;
        InvokeRepeating(nameof(Damaging), 0.5f, 10.0f);
        OnWaveIncrease(_spawnManager.waveCount);
        if (UnityEngine.Random.Range(1, 20) == 1) onDeathDrop = true;
    }

    protected override void EnemyBehavior()
    {
        switch (state)
        {
            case States.Chasing:
                _moving.Moving(_moving.GetLookDir(_player.gameObject));
                break;
            case States.SpecialAttack:
                {
                    if (needHealing)
                    {
                        try
                        {
                            healthPack = FindObjectOfType<HealthPack>().GetComponent<HealthPack>();
                            _moving.Moving(_moving.GetLookDir(healthPack.gameObject));
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
                    else _moving.Moving(-_moving.GetLookDir(_player.gameObject));
                    break;
                }
                }
        }
  
    protected override void Damaging()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);
        _audioSource.PlayOneShot(_healSound);
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
            _moving.moveSpeed += 0.1f;
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
        _gameManager.ChangeScore(scorePoints);
        if (onDeathDrop) _spawnManager.SpawnHealthPack();
        PlayDeathAnimation();
    }
}
