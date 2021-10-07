using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseScript : FacePartsBaseScript
{
    public AudioClip damageSound;
    private float nextTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        audioSource.PlayOneShot(damageSound);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (damageSound && Time.time > nextTime)
        {
            audioSource.PlayOneShot(damageSound);
            nextTime = Time.time + damageSound.length;
        }
    }

    public override void FacePartsDie()
    {
        base.FacePartsDie();
        MainScript.GameOver();
    }
}
