using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingEnemyController : EnemyController
{
    [SerializeField] private float slowMultiplier, slowDuration;
    protected override void Damaging() 
    {
        player.ChangeHealth(-damage);
        player.ChangeState(PlayerController.States.Slowed, slowDuration, slowMultiplier);
        nextHit = Time.time + attackDelay;
        state = States.DamageDone;
    }
    protected override void EnemyBehavior()
    {
        switch (state)
        {
            case States.Chasing:
                if (Time.time > nextHit)
                    Moving(GetLookDir(player.gameObject));
                break;
            case States.DamageDone:
                if (Time.time > nextHit) state = States.Chasing;
                else Moving(-GetLookDir(player.gameObject));
                break;
        }
    }
    private protected override void Death()
    {
        spawnManager.EnemyDead();
        player.ChangeScore(scorePoints);
        if (Random.Range(1, 10) == 1) player.ChangeState(PlayerController.States.Boosted, slowDuration, slowMultiplier);
        Destroy(gameObject);
    }
}

