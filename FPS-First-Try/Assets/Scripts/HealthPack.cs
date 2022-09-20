using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int healthAmount = 20;
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.health < player.maxHealth)
            {
                player.ChangeHealth(healthAmount);
                Destroy(gameObject);
            }
            
        }
        
    }
}
