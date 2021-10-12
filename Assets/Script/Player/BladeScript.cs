using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBaseScript enemy = collision.gameObject.GetComponent<EnemyBaseScript>();
        if (enemy)
        {
            enemy.EnemyTakeDamage();
        }
    }
}
