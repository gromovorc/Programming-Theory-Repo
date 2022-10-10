using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootsteps : MonoBehaviour
{
    [SerializeField]private float stepTime, specialSoundTime;
    private float nextStep, nextSpecialSoundTime;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _stepSound, _specialSound;

    public bool isDead = false;

    void Update()
    {
        if (isDead) _audioSource.Stop();
        else
        if (Time.time > nextSpecialSoundTime && !_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_specialSound);
            nextSpecialSoundTime = Time.time + specialSoundTime;
        }
        else if (Time.time > nextStep && !_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_stepSound);
            nextStep = Time.time + stepTime;
        }
    }
}
