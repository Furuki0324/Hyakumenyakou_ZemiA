using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    #region public variables

    [SerializeField] AudioClip defence, boss;

    #endregion

    #region private variables

    private static AudioSource audioSource;
    private static AudioClip _defence, _boss;

    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _defence = defence;
        _boss = boss;

        audioSource.clip = _defence;
        audioSource.loop = true;
        audioSource.Play();
    }

    public static void ChangeBGM()
    {
        audioSource.Stop();

        audioSource.clip = _boss;
        audioSource.loop = true;
        audioSource.Play();
    }
}
