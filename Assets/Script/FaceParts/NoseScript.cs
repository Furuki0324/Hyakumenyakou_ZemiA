using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseScript : FacePartsBaseScript
{
    [SerializeField] AudioSource noseSource;
    public AudioClip damageSound;
    private float nextTime;

    private void Awake()
    {
        Initialize(0);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        noseSource.PlayOneShot(damageSound);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (damageSound && Time.time > nextTime)
        {
            noseSource.PlayOneShot(damageSound);
            nextTime = Time.time + damageSound.length;
        }
    }

    public override void FacePartsDie()
    {
        base.FacePartsDie();
        MainScript.GameOver();
    }
}
