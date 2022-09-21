using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    Gun gun;
    GameManager gameManager;
    [SerializeField] Text healthText, scoreText;
    [SerializeField] TextMesh playerName;
    public TextMesh ammoText;

    [Header("Basic Information")]
    public float movingSpeed, turningSpeed;
    public int maxHealth = 100;
    [SerializeField] float timeInvincible = 2.0f;

    public int health { get { return currentHealth; } }
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;
    float horizontalInput, verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>(); 
        gun = GetComponentInChildren<Gun>();
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        currentHealth = maxHealth; healthText.text = $"Health: {currentHealth}"; 
        scoreText.text = $"Score: {gameManager.currentScore}";
        playerName.text = SceneFlow.Instance.playerName;
    }

    private void Update()
    {
        if (!gameManager.m_gameOver)
        {
            if (Input.GetMouseButton(0))
            {
                gun.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                gun.Reload();
            }

            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                    isInvincible = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.m_gameOver) PlayerMove();
    }

    void PlayerMove()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * turningSpeed * Time.deltaTime);
        verticalInput = Input.GetAxis("Vertical");
        playerRb.AddRelativeForce(Vector3.forward * verticalInput * movingSpeed);
    }

    public void ChangeHealth(int amount)
    {
        if (health < 1)
        {
            gameManager.GameOver();
        }
        if (amount < 0)
        {
            if (isInvincible) return;
            {
                isInvincible = true;
                invincibleTimer = timeInvincible;
            }
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthText.text = $"Health: {currentHealth}";
    }

    public void ChangeScore(int amount)
    {
        gameManager.currentScore += amount;
        scoreText.text = $"Score: {gameManager.currentScore}";
    }
}
