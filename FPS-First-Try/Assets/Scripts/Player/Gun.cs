using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text _ammoText;
    [SerializeField] private UnityEngine.UI.Image _ammoImage;

    [SerializeField] private GameObject _muzzlePrefabFlash;
    [SerializeField] private GameObject _muzzleOffset;

    [SerializeField] private AudioClip _gunShot;
    [SerializeField] private AudioClip _gunReady;
    [SerializeField] private AudioSource _audioSource;
    public enum ShootingState
    {
        Ready,
        Shooting,
        Reloading
    }

    [Header("Magazine")]
    public int ammunition;

    [Range(2.0f, 5.0f)] public float reloadTime;

    public int remainingAmmunition { get; private set; }
    private float remainingReloadTime;

    [Header("Shooting")]
    [Range(1.6f, 25.0f)] public float fireRate;
    [Range(10.0f, 100.0f)] public float roundSpeed;
    [Range(0.0f, 25.0f)] public float maxRoundVariation;

    private ShootingState shootingState = ShootingState.Ready;
    private float nextShootTime = 0;

    private void Start()
    {
        _audioSource.volume *= MenuUIManager.volume;
        remainingAmmunition = ammunition;
        _ammoText.text = $"{remainingAmmunition} / {ammunition}";
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
                if (!_ammoImage.gameObject.activeInHierarchy) _ammoImage.gameObject.SetActive(true);
                ReloadTimerDisplay();
                if (Time.time > nextShootTime)
                {
                    remainingAmmunition = ammunition;
                    _ammoText.text = $"{remainingAmmunition} / {ammunition}";
                    _ammoImage.gameObject.SetActive(false);
                    shootingState = ShootingState.Ready;
                    _audioSource.PlayOneShot(_gunReady);
                }
                break;
        }
    }
    public void Shoot()
    {
        if (shootingState == ShootingState.Ready)
        {
            GameObject spawnedBullet = ObjectPool.SharedInstance.GetPooledObject();
            if (spawnedBullet != null)
            {
                spawnedBullet.transform.position = _muzzleOffset.transform.position;
                spawnedBullet.transform.rotation = _muzzleOffset.transform.rotation;
                spawnedBullet.SetActive(true);
                Instantiate(_muzzlePrefabFlash, _muzzleOffset.transform);
            }

            spawnedBullet.transform.Rotate(new Vector3(
                    Random.Range(-1.0f, 1.0f) * maxRoundVariation,
                    Random.Range(-1.0f, 1.0f) * maxRoundVariation, 0));
               
            Rigidbody ammoRb = spawnedBullet.GetComponent<Rigidbody>();
            ammoRb.velocity = spawnedBullet.transform.forward * roundSpeed;
            _audioSource.PlayOneShot(_gunShot);   

            remainingAmmunition--;
            _ammoText.text = $"{remainingAmmunition} / {ammunition}";
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
            remainingReloadTime = reloadTime;
            _ammoText.text = $"Reloading";
            shootingState = ShootingState.Reloading;
        }
    }

    private void ReloadTimerDisplay()
    {
        remainingReloadTime -= Time.deltaTime;
        _ammoImage.fillAmount = Mathf.Clamp01(remainingReloadTime / reloadTime);

    }
}
