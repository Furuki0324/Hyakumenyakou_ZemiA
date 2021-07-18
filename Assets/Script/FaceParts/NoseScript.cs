using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseScript : FacePartsBaseScript
{
    public int hp = 15;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }
    }

    public override void TakeDamage()
    {
        hp--;
    }
}
