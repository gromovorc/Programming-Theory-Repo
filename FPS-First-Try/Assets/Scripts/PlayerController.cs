using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    Gun gun;
    [SerializeField] Text healthText;

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
        currentHealth = maxHealth; healthText.text = $"Health: {currentHealth}";

        
    }

    private void Update()
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

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
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
}
