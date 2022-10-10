using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    private protected PlayerController _player;
    private protected SpawnManager _spawnManager;
    private protected GameManager _gameManager;

    private protected EnemyMoving _moving;

    private protected Animation _animation;

    [SerializeField]private protected Transform _parentTransform;

    private protected AudioSource _audioSource;
    [SerializeField] private protected AudioClip _attackSound;

    [HideInInspector]
    public enum States
    {
        Chasing,
        SpecialAttack,
        DamageDone
    }


    [Header("Basic Information")]
    public int damage = 20, health = 100, scorePoints = 5;
    public float attackDelay = 2f;

    private protected float nextHit;
    private protected bool onDeathDrop = false;

    private protected States state = States.Chasing;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        _spawnManager = FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        _animation = GetComponent<Animation>();
        _moving = GetComponent<EnemyMoving>();
    }
    protected void Start()
    {
        _audioSource.volume *= MenuUIManager.volume;
        if (Random.Range(1, 10) == 1) onDeathDrop = true;
        OnWaveIncrease(_spawnManager.waveCount);
    }

    protected void Update()
    {
        EnemyBehavior();
    }

    public virtual void Hit(int amount)
    {
        health -= amount;
        _moving.OnHitIncrease();
        if (health < 1) Death();
    }

    private virtual protected void Death()
    {
        _spawnManager.EnemyDead();
        _gameManager.ChangeScore(scorePoints);

        PlayDeathAnimation();
    }

    private protected void PlayDeathAnimation()
    {
        _parentTransform.position = gameObject.transform.position;
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.position = Vector3.zero;
        _animation.Play("Death");
        Destroy(_parentTransform.gameObject, 1.5f);
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
