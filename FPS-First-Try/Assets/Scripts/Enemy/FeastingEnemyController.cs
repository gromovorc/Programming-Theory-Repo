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

                _moving.Moving(_moving.GetLookDir(_player.gameObject));
                break;
            case States.SpecialAttack:
                {
                    if (_player.isInvincible)
                    {
                        nextHit = Time.time + attackDelay;
                        state = States.DamageDone;
                       
                    }
                    else
                    {
                        if (_player.state != PlayerController.States.Swallowed)
                        {
                            _player.ChangeState(PlayerController.States.Swallowed, (float)damageTicks);
                            _audioSource.Stop();
                            _audioSource.PlayOneShot(_attackSound);
                        }
                        gameObject.transform.position = _player.transform.position + Vector3.back;
                        if (damageTicks > 0)
                        {
                            nextTick -= Time.deltaTime;
                            if (nextTick < 0)
                            {
                                _player.ChangeHealth(-damage);
                                nextTick = 1.0f;
                                damageTicks--;
                            }
                        }
                        else
                        {
                            _player.ChangeState();
                            _audioSource.Stop();
                            damageTicks = 2;
                            nextHit = Time.time + attackDelay;
                            state = States.DamageDone;

                        }
                    }
                    break;
                }
                
            case States.DamageDone:
                if (Time.time > nextHit) state = States.Chasing;
                else _moving.Moving(-_moving.GetLookDir(_player.gameObject));
                break;
        }

    }
     protected override void Damaging()
    {
        if (_player.state != PlayerController.States.Swallowed)
        {
            state = States.SpecialAttack;
        }
        
    }
}