using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseScript : FacePartsBaseScript
{


    public AudioClip damageSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void TakeDamage()
    {
        health--;

        audioSource.PlayOneShot(damageSound);
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;

        audioSource.PlayOneShot(damageSound);
    }
}
