//#define DURING_DEBUG_ONLY
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

    private static int volume = -5;
    
    // Start is called before the first frame update

    [SerializeField] AudioMixer mixer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }
    }


    private void Volume(float a)
    {
        switch (a)
        {
            case 0.8f:
                Debug.Log("call");
                volume += 2;
                break;

            case 0.6f:
                Debug.Log("call2");
                volume ++;
                break;

            case 0.4f:
                Debug.Log("call3");
                volume --;
                break;
        }
        mixer.SetFloat("BGM", volume);
    }
    void Start()
    {
        transform.SetParent(EAR_ANCHOR);
        if (transform.position.x > 0) spriteRenderer.flipX = true;

        mixer.SetFloat("BGM", volume);

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

        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);

    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);
    }
}
