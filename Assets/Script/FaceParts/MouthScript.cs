﻿//#define DURING_DEBUG_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MouthScript : FacePartsBaseScript
{
    static Transform _MOUTH_ANCHOR;
    static Transform MOUTH_ANCHOR
    {
        get
        {
            if(_MOUTH_ANCHOR == null)
            {
                GameObject go = new GameObject("MOUTH_ANCHOR");
                _MOUTH_ANCHOR = go.transform;
            }
            return _MOUTH_ANCHOR;
        }
    }

    private static int volume = -5;
    // Start is called before the first frame update
    [SerializeField] AudioMixer mixer;
    private AudioSource SE;
    public AudioClip SECLIP;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }


        /*
        //体力が減った時の処理
        if (hp >= cacheHp*0.8)
        {
            mixer.SetFloat("BGM", 0);
        }
        else if (hp >= cacheHp * 0.6)
        {
            mixer.SetFloat("BGM", -1);
        }
        else if (hp >= cacheHp * 0.4)
        {
            mixer.SetFloat("BGM", -2);
        }
        else if (hp > cacheHp * 0.2)
        {
            mixer.SetFloat("BGM", -3);
        }
        else if (hp == cacheHp * 0)
        {
            Destroy(gameObject);
        }
        */
    }


    private void Volume(float a)
    {
        switch (a)
        {
            case 0.8f:
                //Debug.Log("call");
                volume += 2;
                break;

            case 0.6f:
                //Debug.Log("call2");
                volume ++;
                break;

            case 0.4f:
                //Debug.Log("call3");
                volume --;
                break;
        }
        mixer.SetFloat("SE", volume);
    }


    void Start()
    {
        transform.SetParent(MOUTH_ANCHOR);

        SE = GetComponent<AudioSource>();
        mixer.SetFloat("SE", volume);

        SetCache();
    }
    //private float cacheTime = 0;
    // Update is called once per frame

#if DURING_DEBUG_ONLY
    void Update()
    {
        if (Time.time > cacheTime + 2)
        {
            hp--;
            SE.PlayOneShot(SECLIP);
            cacheTime = Time.time;

            if (Mathf.Approximately(hp, cacheHp * 0.8f)) Volume(0.8f);
            else if (Mathf.Approximately(hp, cacheHp * 0.6f)) Volume(0.6f);
            else if (Mathf.Approximately(hp, cacheHp * 0.4f)) Volume(0.4f);
        }
    }
#endif 

    public override void TakeDamage()
    {
        base.TakeDamage();

        SE.PlayOneShot(SECLIP);
        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);

    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        SE.PlayOneShot(SECLIP);
        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);

    }
}
