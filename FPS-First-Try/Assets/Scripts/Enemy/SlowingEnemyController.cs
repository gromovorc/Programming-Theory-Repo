using UnityEngine;

public class SlowingEnemyController : EnemyController
{
    [SerializeField] private float slowMultiplier, slowDuration;
    protected override void Damaging() 
    {
        if (player.state != PlayerController.States.Swallowed)
        {
            player.ChangeHealth(-damage);
            audioSource.PlayOneShot(attackSound);
            player.ChangeState(PlayerController.States.Slowed, slowDuration, slowMultiplier);
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
                    Moving(GetLookDir(player.gameObject));
                break;
            case States.DamageDone:
                if (Time.time > nextHit) state = States.Chasing;
                else Moving(-GetLookDir(player.gameObject));
                break;
        }
    }
    private override protected void Death()
    {
        spawnManager.EnemyDead();
        gameManager.ChangeScore(scorePoints);
        Destroy(gameObject);
    }

    public override void Hit(int amount)
    {
        health -= amount;
        if (health < 1)
        {
             if (onDeathDrop) player.ChangeState(PlayerController.States.Boosted, slowDuration, slowMultiplier);
             Death();
        }
    }
}

