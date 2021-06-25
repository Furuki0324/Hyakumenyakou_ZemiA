using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCtrl : MonoBehaviour
{
    //---------------------Public------------------
    public Transform chaseTarget;
    public float speed;

    //---------------------Private------------------
    private Rigidbody2D rigid2D;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        chaseTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Chase();
    }

    private void Chase()
    {
        Vector2 force = (chaseTarget.position - transform.position) * speed;
        rigid2D.velocity = force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDropItemCtrl dropCtrl;

        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);

            dropCtrl = GetComponent<EnemyDropItemCtrl>();
            if (!dropCtrl) return;

            dropCtrl.DroppingItem();
        }
    }
}
