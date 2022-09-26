using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    Gun gun;
    GameManager gameManager;
    [SerializeField]Image timerImage;

    int currentScore;

    [Header("Basic Information")]
    public float basicMovingSpeed = 5.0f, basicTurningSpeed, knockForce;
    public readonly int maxHealth = 100;
    [SerializeField] float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;
    public float movingSpeed { get { return currentMovingSpeed; } }
    float currentMovingSpeed;
    public float turningSpeed { get { return currentTurningSpeed; } }
    float currentTurningSpeed;

    float invincibleTimer, debuffTime, debuffTimer, debuffMultiplier;
    public bool isInvincible;
    
    [HideInInspector]
    public enum States 
    { 
        Normal,
        Swallowed,
        Slowed,
        Boosted
    }
    public States state = States.Normal;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<Gun>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        currentHealth = maxHealth;
        currentMovingSpeed = basicMovingSpeed; currentTurningSpeed = basicTurningSpeed;
        gameManager.healthText.text = $"Health: {currentHealth}";
    }

    private void Update()
    {
        if (!gameManager.m_gameOver)
        {
            PlayerBehavior();
            if (state != States.Swallowed)
            {
                if (Input.GetMouseButton(0)) gun.Shoot();
                if (Input.GetKeyDown(KeyCode.R) && gun.remainingAmmunition != gun.ammunition) gun.Reload();
            }
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.m_gameOver) PlayerMove();
    }

    void PlayerBehavior()
    {

        switch (state)
        {
            case States.Normal:
                break;
            case States.Swallowed:
                TimerDisplay(Color.green);
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
                    ChangeSpeed();
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
                    ChangeSpeed(false);
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

    void PlayerMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * turningSpeed * Time.deltaTime);
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddRelativeForce(Vector3.forward * verticalInput * movingSpeed);
    }

    public void ChangeHealth(int amount)
    {
        if (health < 1)
        {
            gameManager.GameOver();
        }
        if (amount < 0 && state != PlayerController.States.Swallowed)
        {
            if (isInvincible) return;
            {
                isInvincible = true;
                invincibleTimer = timeInvincible;
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        gameManager.healthText.text = $"Health: {currentHealth}";
        playerRb.AddForce(Vector3.Normalize(Vector3.zero + gameObject.transform.position) * knockForce, ForceMode.Impulse);
    }

    public void ChangeState(States neededState = States.Normal)
    {
        timerImage.gameObject.SetActive(false);
        state = neededState;
    }
    public void ChangeState(States neededState, float duration, float multiplier = 0.0f)
    {
        state = neededState;
        debuffTime = duration;
        debuffTimer = debuffTime;
        debuffMultiplier = multiplier;
        timerImage.gameObject.SetActive(true);
        timerImage.GetComponentInChildren<Text>().text = neededState.ToString();
    }

    public void ChangeSpeed(bool isIncrementing = true)
    {
        if (isIncrementing)
        {
            currentMovingSpeed *= debuffMultiplier;
            currentTurningSpeed *= debuffMultiplier;
        }
        else
        {
            currentMovingSpeed /= debuffMultiplier;
            currentTurningSpeed /= debuffMultiplier;
        }
    }

    public void ChangeScore(int amount)
    {
        currentScore += amount;
        gameManager.scoreText.text = $"Score: {currentScore}";
    }

    private void TimerDisplay(Color color)
    {
        debuffTimer -= Time.deltaTime;
        timerImage.fillAmount = Mathf.Clamp01(debuffTimer / debuffTime);
        if (timerImage.color != color)
        {
            timerImage.color = color;
            timerImage.GetComponentInChildren<Text>().color = color;
        }
    }
}
