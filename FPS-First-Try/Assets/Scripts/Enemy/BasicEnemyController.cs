using UnityEngine;

public class BasicEnemyController : EnemyController
{
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
                else gameObject.transform.position += -transform.forward * (moveSpeed / 2) * Time.deltaTime;
                break;
        }
    }

    protected override void Damaging()
    { 
        audioSource.PlayOneShot(attackSound);
        player.ChangeHealth(-damage);
        nextHit = Time.time + attackDelay;
        state = States.DamageDone;
    }
    
    private override protected void Death()
    {
        spawnManager.EnemyDead();
        gameManager.ChangeScore(scorePoints);
        if (onDeathDrop) spawnManager.SpawnSpecialEnemy();
        Destroy(gameObject);
    }
}
