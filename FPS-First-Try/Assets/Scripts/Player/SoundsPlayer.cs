using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioClip pickUpSound;

    private float stepTime;
    public bool isCanStep;
    private readonly float stepWidth = 0.3f;
    private float stepDuration;
    private readonly float basicStepDuration = 0.7f;

    private void Start()
    {
        stepDuration = basicStepDuration;
        audioSource.volume *= MenuUIManager.volume;
    }

    private void Update()
    {
        if (isCanStep && Mathf.Abs(Input.GetAxis("Horizontal")) > stepWidth || Mathf.Abs(Input.GetAxis("Vertical")) > stepWidth)
        {
            {
                if (Time.time > stepTime)
                {
                    audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
                    stepTime = Time.time + stepDuration;
                }
            }
        }
    }

    public void ChangeStepTime(bool isIncrementing = true)
    {
        if (isIncrementing)
        {
            stepDuration = stepSounds[0].length;
        }
        else
        {
            stepDuration = stepSounds[0].length * 2;
        }
    }

    public void ChangeToNormal() => stepDuration = basicStepDuration;
    
    public void PlayPickUpSound() => audioSource.PlayOneShot(pickUpSound);
}
