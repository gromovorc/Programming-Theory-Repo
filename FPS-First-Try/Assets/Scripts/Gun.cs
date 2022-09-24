using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] TextMesh ammoText;
    public enum ShootingState
    {
        Ready,
        Shooting,
        Reloading
    }

    private float muzzleOffset;

    [Header("Magazine")]
    public GameObject round;
    public int ammunition;

    [Range(2f, 5.0f)] public float reloadTime;

    private int remainingAmmunition;

    [Header("Shooting")]
    [Range(1.6f, 25.0f)] public float fireRate;
    [Range(10f, 100f)] public float roundSpeed;
    [Range(0, 25)] public float maxRoundVariation;

    private ShootingState shootingState = ShootingState.Ready;
    private float nextShootTime = 0;

    private void Start()
    {
        muzzleOffset = GetComponent<MeshRenderer>().bounds.max.z + 0.3f;
        remainingAmmunition = ammunition;
        ammoText.text = $"{remainingAmmunition} / {ammunition}";
    }
    private void Update()
    {
        switch(shootingState)
        {
            case ShootingState.Shooting:
                if (Time.time > nextShootTime)
                {
                    shootingState = ShootingState.Ready;
                }
                break;
            case ShootingState.Reloading:
                if (Time.time > nextShootTime)
                {
                    remainingAmmunition = ammunition;
                    ammoText.text = $"{remainingAmmunition} / {ammunition}";
                    shootingState = ShootingState.Ready;
                }
                break;
        }
    }
    public void Shoot()
    {
        if (shootingState == ShootingState.Ready)
        {
                GameObject spawnedRound = Instantiate(
                    round,
                    transform.position + transform.forward * muzzleOffset, 
                    transform.rotation);

                spawnedRound.transform.Rotate(new Vector3(
                    Random.Range(-1.0f, 1.0f) * maxRoundVariation,
                    Random.Range(-1.0f, 1.0f) * maxRoundVariation, 0));

                Rigidbody ammoRb = spawnedRound.GetComponent<Rigidbody>();
                ammoRb.velocity = spawnedRound.transform.forward * roundSpeed;

            remainingAmmunition--;
            ammoText.text = $"{remainingAmmunition} / {ammunition}";
            if (remainingAmmunition > 0)
            {
                nextShootTime = Time.time + (1 / fireRate);
                shootingState = ShootingState.Shooting;
            }
            else
            {
                Reload();
            }
        }
    }

    public void Reload()
    {
        if (shootingState == ShootingState.Ready)
        {
            nextShootTime = Time.time + reloadTime;
            ammoText.text = $"Reloading";
            shootingState = ShootingState.Reloading;
        }
    }
}
