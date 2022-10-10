using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepSounds;
    [SerializeField] private AudioClip _pickUpSound;

    private float stepTime;
    public bool isCanStep;
    private readonly float stepWidth = 0.3f;
    private float stepDuration;
    private readonly float basicStepDuration = 0.7f;

    private void Start()
    {
        stepDuration = basicStepDuration;
        _audioSource.volume *= MenuUIManager.volume;
    }

    private void Update()
    {
        if (isCanStep && Mathf.Abs(Input.GetAxis("Horizontal")) > stepWidth || Mathf.Abs(Input.GetAxis("Vertical")) > stepWidth)
        {
            {
                if (Time.time > stepTime)
                {
                    _audioSource.PlayOneShot(_stepSounds[Random.Range(0, _stepSounds.Length)]);
                    stepTime = Time.time + stepDuration;
                }
            }
        }
    }

    public void ChangeStepTime(bool isIncrementing = true)
    {
        if (isIncrementing)
        {
            stepDuration = _stepSounds[0].length;
        }
        else
        {
            stepDuration = _stepSounds[0].length * 2;
        }
    }

    public void ChangeToNormal() => stepDuration = basicStepDuration;
    
    public void PlayPickUpSound() => _audioSource.PlayOneShot(_pickUpSound);
}
