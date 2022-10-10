using UnityEngine;

public class SlowingEnemyController : EnemyController
{
    [SerializeField] private float slowMultiplier, slowDuration;
    protected override void Damaging() 
    {
        if (_player.state != PlayerController.States.Swallowed)
        {
            _player.ChangeHealth(-damage);
            _audioSource.PlayOneShot(_attackSound);
            _player.ChangeState(PlayerController.States.Slowed, slowDuration, slowMultiplier);
            nextHit = Time.time + attackDelay;
            state = States.DamageDone;
        }
    }
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
                else _moving.Moving(-_moving.GetLookDir(_player.gameObject));
                break;
        }
    }
    private override protected void Death()
    {
        _spawnManager.EnemyDead();
        _gameManager.ChangeScore(scorePoints);
        PlayDeathAnimation();
    }

    public override void Hit(int amount)
    {
        health -= amount;
        if (health < 1)
        {
             if (onDeathDrop) _player.ChangeState(PlayerController.States.Boosted, slowDuration, slowMultiplier);
             Death();
        }
    }
}

