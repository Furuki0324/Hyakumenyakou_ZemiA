using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class EarScript : FacePartsBaseScript
{
    static Transform _EAR_ANCHOR;
    static Transform EAR_ANCHOR
    {
        get
        {
            if(_EAR_ANCHOR == null)
            {
                GameObject go = new GameObject("EAR_ANCHOR");
                _EAR_ANCHOR = go.transform;
            }
            return _EAR_ANCHOR;
        }
    }

    private static float volume;
    
    private enum Pattern { none, heal, damage}

    [SerializeField] AudioMixer mixer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }
    }

    void Start()
    {
        transform.SetParent(EAR_ANCHOR);
        if (transform.position.x > 0) spriteRenderer.flipX = true;

        mixer.GetFloat("BGM", out float value);
        if (value > -14)
        {
            volume = value + 1.0f;
        }
        else
        {
            volume = value;
        }

        mixer.SetFloat("BGM", volume);

        SetCache();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        VolumeControl(Pattern.damage);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        VolumeControl(Pattern.damage);
    }

    private void VolumeControl(Pattern pattern, float healAmount = 0)
    {
        switch (pattern)
        {
            case Pattern.heal:
                volume += healAmount;
                break;

            case Pattern.damage:
                volume--;
                break;

            default:
                break;
        }
        mixer.SetFloat("BGM",volume);
    }

    public override void Repaired(int amount)
    {
        base.Repaired(amount);

        VolumeControl(Pattern.heal);
    }

    public override void Repaired(float percent)
    {
        base.Repaired(percent);

        float healAmount = cacheHealth * percent / 100;
        VolumeControl(Pattern.heal, healAmount);
    }
}
