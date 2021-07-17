﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCtrl : MonoBehaviour
{
    //---------------------Public------------------
    [Header("Status")]
    public int hp;

    [Header("Property to chase target")]
    public Transform chaseTarget;
    public float speed;

    [Header("Set Trigger Target")]
    public string triggerTag;

    //---------------------Private------------------
    private Rigidbody2D rigid2D;
    private EnemyRaycaster rayCaster;

    List<Transform> transforms = new List<Transform>();

    private void ResetTarget()
    {
        transforms.Clear();

        chaseTarget = GameObject.FindGameObjectWithTag("Face_Nose").transform;
        //transform.rotation = Quaternion.FromToRotation(Vector3.right, chaseTarget.position);
        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);

        transforms = rayCaster.GetTransformsInList();

        Debug.Log(transforms.Count);

        FindClosestTarget(transforms);
    }

    private void FindClosestTarget(List<Transform> target)
    {
        

        if(target.Count > 0)
        {
            for(int i = 0; i < target.Count; i++)
            {
                if((transform.position - target[i].position).magnitude < (transform.position - chaseTarget.position).magnitude)
                {
                    chaseTarget = target[i];
                }
            }
        }
        
    }

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();

        rayCaster = GetComponentInChildren<EnemyRaycaster>();

        ResetTarget();
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

        if (collision.CompareTag(triggerTag))
        {
            hp--;

            if(hp <= 0)
            {
                Destroy(this.gameObject);

                dropCtrl = GetComponent<EnemyDropItemCtrl>();
                if (!dropCtrl) return;

                dropCtrl.DroppingItem();
            }

            
        }
    }
}
