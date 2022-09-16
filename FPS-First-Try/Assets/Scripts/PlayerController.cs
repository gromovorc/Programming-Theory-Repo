using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    Gun gun;
    [SerializeField] TextMeshProUGUI healthText;

    public float movingSpeed, turningSpeed;
    float horizontalInput, verticalInput;
    public int playerHealth = 100;
    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        healthText.SetText($"Health: {playerHealth}");
        gun = GetComponentInChildren<Gun>();
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
        if (!gameOver && playerHealth > 0)
        {
            playerHealth += amount;
            healthText.SetText($"Health: {playerHealth}");
        }
    }
}
