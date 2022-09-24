using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected PlayerController player;
    protected SpawnManager spawnManager;
    public enum States
    {
        Chasing,
        SpecialAttack,
        DamageDone
    }
    [Header("Movement")]
    public float moveSpeed, turnSpeed, onHitIncrease;

    [Header("Basic Information")]
    public int damage = 20, health = 100, scorePoints = 5;
    public float attackDelay = 2f;

    private protected float nextHit;

    private protected States state = States.Chasing;

    protected void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
        OnWaveIncrease(spawnManager.waveCount);
    }

    protected void Update()
    {
        EnemyBehavior();
    }

    public virtual void Hit(int amount)
    {
        health -= amount;
        moveSpeed += onHitIncrease; 
        turnSpeed += onHitIncrease;
        if (health < 1) Death();
    }

    protected Vector3 GetLookDir(GameObject target)
    {
         var look_dir = target.transform.position - gameObject.transform.position; look_dir.y = 0;
        return look_dir;
    }
    protected void Moving(Vector3 look_dir)
    {
        
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(look_dir), turnSpeed * Time.deltaTime);
        gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;

    }

    private protected virtual void Death()
    {
        spawnManager.EnemyDead();
        player.ChangeScore(scorePoints);
        Destroy(gameObject);
    }

    protected abstract void EnemyBehavior();
    
    public void OnWaveIncrease(int amount)
    {
        health += amount;
        damage += Mathf.RoundToInt(amount / 2);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Damaging();
        }
    }

    abstract protected void Damaging();
    
}
