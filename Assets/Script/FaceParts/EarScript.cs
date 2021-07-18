using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class EarScript : FacePartsBaseScript
{
    private static int volume = -5;
    // Start is called before the first frame update
    [SerializeField]
    private float hp = 20; //体力
    private float cacheHp;
    [SerializeField] AudioMixer mixer;
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

    private void Volume()
    {
        if (hp >= cacheHp * 0.8)
        {
            mixer.SetFloat("BGM", 0);
            volume += 2;
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
    }
    void Start()
    {
        mixer.SetFloat("BGM", -10);
        cacheHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        Volume();       
    }

    public override void TakeDamage()
    {
        hp--;
    }
}
