using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Gun _gun;
    private GameManager _gameManager;
    private SoundsPlayer _sound;

    [SerializeField] private Image _timerImage;

    [Header("Basic Information")]
    public float basicMovingSpeed;
    public readonly int maxHealth = 100;
    [SerializeField] float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    private int currentHealth;
    public float movingSpeed { get { return currentMovingSpeed; } }
    private float currentMovingSpeed;
    public float turningSpeed { get { return currentTurningSpeed; } }
    private float currentTurningSpeed;

    private Vector3 movementVector
    {
        get
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            return new Vector3(horizontalInput, 0.0f, verticalInput);
        }
    }


    private float invincibleTimer, debuffTime, debuffTimer, debuffMultiplier;
    public bool isInvincible;
    
    [HideInInspector]
    public enum States 
    { 
        Normal,
        Swallowed,
        Slowed,
        Boosted
    }
    [HideInInspector]
    public States state = States.Normal;

    private void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        _sound = GetComponent<SoundsPlayer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentMovingSpeed = basicMovingSpeed; currentTurningSpeed = MenuUIManager.sensitivity;
        _gameManager.healthText.text = $"Health: {currentHealth}";
    }

    private void Update()
    {
        if (!_gameManager.m_gameOver)
        {
            
            PlayerBehavior();
            MouseMove();
            if (state != States.Swallowed)
            {
                if (Input.GetMouseButton(0)) _gun.Shoot();
                if (Input.GetKeyDown(KeyCode.R) && _gun.remainingAmmunition != _gun.ammunition) _gun.Reload();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_gameManager.m_gameOver) PlayerMove();
    }

    private void PlayerBehavior()
    {

        switch (state)
        {
            case States.Normal:
                break;
            case States.Swallowed:
                TimerDisplay(Color.green);
                if (_sound.isCanStep) _sound.isCanStep = false;
                if (debuffTimer < 0)
                {
                    _sound.isCanStep = true;
                    ChangeState();
                }
                break;
            case States.Slowed:
                if (movingSpeed >= basicMovingSpeed)
                {
                    currentMovingSpeed = basicMovingSpeed;
                    ChangeSpeed(false);
                } 
                TimerDisplay(Color.magenta);
                if (debuffTimer < 0)
                {
                    _sound.ChangeToNormal();
                    ChangeState();
                }
                break;
            case States.Boosted:
                if (movingSpeed <= basicMovingSpeed)
                {
                    currentMovingSpeed = basicMovingSpeed;
                    ChangeSpeed();
                }
                TimerDisplay(Color.yellow);
                if (debuffTimer < 0)
                {
                    _sound.ChangeToNormal();
                    ChangeState();
                }
                break;
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;

            }
        }
    }

    private void PlayerMove()
    {
        transform.Translate(movementVector * movingSpeed * Time.fixedDeltaTime, Space.Self);
    }

    private void MouseMove()
    {
        float mousePos = Input.GetAxis("Mouse X");
        transform.Rotate(0.0f, mousePos * turningSpeed, 0.0f);
    }
    public void ChangeHealth(int amount)
    {
        if (health < 1)
        {
            _gameManager.GameOver();
        }
        if (amount < 0 && state != PlayerController.States.Swallowed)
        {
            if (isInvincible) return;
            {
                isInvincible = true;
                invincibleTimer = timeInvincible;
            }
        } 
        else if (amount > 0) 
        {
            _sound.PlayPickUpSound();
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        _gameManager.healthText.text = $"Health: {currentHealth}";
    }

    public void ChangeState(States neededState = States.Normal)
    {
        _timerImage.gameObject.SetActive(false);
        state = neededState;
    }
    public void ChangeState(States neededState, float duration, float multiplier = 0.0f)
    {
        state = neededState;
        debuffTime = duration;
        debuffTimer = debuffTime;
        debuffMultiplier = multiplier;
        _timerImage.gameObject.SetActive(true);
        _timerImage.GetComponentInChildren<Text>().text = neededState.ToString();
    }

    public void ChangeSpeed(bool isIncrementing = true)
    {
        if (isIncrementing)
        {
            currentMovingSpeed *= debuffMultiplier;
            currentTurningSpeed *= debuffMultiplier;
            _sound.ChangeStepTime(true);
        }
        else
        {
            currentMovingSpeed /= debuffMultiplier;
            currentTurningSpeed /= debuffMultiplier;
            _sound.ChangeStepTime(false);
        }
    }

    private void TimerDisplay(Color color)
    {
        debuffTimer -= Time.deltaTime;
        _timerImage.fillAmount = Mathf.Clamp01(debuffTimer / debuffTime);
        if (_timerImage.color != color)
        {
            _timerImage.color = color;
            _timerImage.GetComponentInChildren<Text>().color = color;
        }
    }
}
