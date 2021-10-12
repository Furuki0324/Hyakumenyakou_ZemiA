using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FaceSoundPlayer : MonoBehaviour
{
    private static AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlayFaceSFX(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
