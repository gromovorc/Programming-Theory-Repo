using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootsteps : MonoBehaviour
{
    [SerializeField]private float stepTime, specialSoundTime;
    private float nextStep, nextSpecialSoundTime;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip stepSound, specialSound;

    void Update()
    {
        if (Time.time > nextSpecialSoundTime && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(specialSound);
            nextSpecialSoundTime = Time.time + specialSoundTime;
        }
        else if (Time.time > nextStep && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(stepSound);
            nextStep = Time.time + stepTime;
        }
    }
}
