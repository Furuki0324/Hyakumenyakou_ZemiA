using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarScript : FacePartsBaseScript
{
    // Start is called before the first frame update
    [SerializeField]
    private float hp = 15; //体力
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("ダメージを受けた！");
            //Enemy、EnemyManegimentはとりあえずで決めたものなので、変更してもらって大丈夫です。
            hp -= collision.gameObject.GetComponent<EnemyManager>().powerEnemy;
        }

        //体力が減った時の処理
        if(hp >= 12)
        {

        }
        else if(hp >=9 && hp<=11)
        {

        }
        else if (hp >= 6 && hp <= 8)
        {

        }
        else if (hp >= 3 && hp <= 5)
        {

        }
        else if (hp >= 0 && hp <= 2)
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
        base.TakeDamage();
    }
}
