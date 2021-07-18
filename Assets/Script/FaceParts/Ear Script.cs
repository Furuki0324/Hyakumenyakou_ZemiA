using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ear : FacePartsBaseScript
{
    // Start is called before the first frame update
    [SerializeField]
    private float hp = 20; //体力
    [SerializeField] BaseAudioMixer[] snapshots;
    [SerializeField] AudioMixer mixer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }

        //体力が減った時の処理
        if(hp >= 16)
        {

        }
        else if(hp >=11 && hp<=15)
        {

        }
        else if (hp >= 6 && hp <= 10)
        {

        }
        else if (hp >= 0 && hp <= 5)
        {

        }
        else if (hp == 0)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage()
    {
        hp--;
    }
}
