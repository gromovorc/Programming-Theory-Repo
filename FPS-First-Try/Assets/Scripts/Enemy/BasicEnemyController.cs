using UnityEngine;

public class BasicEnemyController : EnemyController
{
    protected override void EnemyBehavior()
    {
        switch (state)
        {
            case States.Chasing:
                if (Time.time > nextHit)
                    _moving.Moving(_moving.GetLookDir(_player.gameObject));
                break;
            case States.DamageDone:
                if (Time.time > nextHit) state = States.Chasing;
                else gameObject.transform.position += -transform.forward * (_moving.moveSpeed / 2) * Time.deltaTime;
                break;
        }
    }

    protected override void Damaging()
    { 
        _audioSource.PlayOneShot(_attackSound);
        _player.ChangeHealth(-damage);
        nextHit = Time.time + attackDelay;
        state = States.DamageDone;
    }
    
    private override protected void Death()
    {
        _spawnManager.EnemyDead();
        _gameManager.ChangeScore(scorePoints);
        if (onDeathDrop) _spawnManager.SpawnSpecialEnemy();
        PlayDeathAnimation();
    }
}
