using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeastingEnemyController : EnemyController
{
    private float nextTick = 1.0f;
    [SerializeField] private int damageTicks = 2;

    protected override void EnemyBehavior()
    {

        switch (state)
        {
            case States.Chasing:
                
                Moving(GetLookDir(player.gameObject));
                break;
            case States.SpecialAttack:
                {
                    if (player.isInvincible)
                    {
                        nextHit = Time.time + attackDelay;
                        state = States.DamageDone;
                       
                    }
                    else
                    {
                        if(player.state != PlayerController.States.Swallowed) 
                            player.ChangeState(PlayerController.States.Swallowed, (float)damageTicks);
                        gameObject.transform.position = player.transform.position + Vector3.back;
                        if (damageTicks > 0)
                        {
                            nextTick -= Time.deltaTime;
                            if (nextTick < 0)
                            {
                                player.ChangeHealth(-damage);
                                nextTick = 1.0f;
                                damageTicks--;
                            }
                        }
                        else
                        {
                            player.ChangeState();
                            damageTicks = 2;
                            nextHit = Time.time + attackDelay;
                            state = States.DamageDone;

                        }
                    }
                    break;
                }
                
            case States.DamageDone:
                if (Time.time > nextHit) state = States.Chasing;
                else Moving(-GetLookDir(player.gameObject));
                break;
        }

    }
     protected override void Damaging()
    {
        if (player.state != PlayerController.States.Swallowed)
        {
            state = States.SpecialAttack;
        }
        
    }
}