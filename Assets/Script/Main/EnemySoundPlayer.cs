using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySoundPlayer : MonoBehaviour
{
    private static AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    
    public static void PlayEnemySFX(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
