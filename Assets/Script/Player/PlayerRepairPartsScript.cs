﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairPartsScript : MonoBehaviour
{
    //Public
    public KeyCode repairKey;
    public float radius;

    //Private
    [ReadOnly, SerializeField]private FacePartsBaseScript repairTarget;
    private ContactFilter2D filter2D;
    [SerializeField, ReadOnly]private List<Collider2D> faceList = new List<Collider2D>();

    private void Start()
    {
        filter2D.SetLayerMask(LayerMask.GetMask("Face"));
    }

    private void Update()
    {
        Physics2D.OverlapCircle(GetPosition2D(transform), radius, filter2D, faceList);
        
        if(faceList.Count > 0)
        {
            foreach(Collider2D t in faceList)
            {
                if (!repairTarget)
                {
                    repairTarget = t.gameObject.GetComponent<FacePartsBaseScript>();
                    continue;
                }

                float oldDistance = (GetPosition2D(transform) - GetPosition2D(repairTarget.transform)).magnitude;
                float newDistance = (GetPosition2D(transform) - GetPosition2D(t.transform)).magnitude;
                
                if(oldDistance > newDistance)
                {
                    repairTarget = t.gameObject.GetComponent<FacePartsBaseScript>();
                }
            }
        }
        else
        {
            repairTarget = null;
        }

        if(Input.GetKeyDown(repairKey) && repairTarget)
        {
            //メソッドの名前は違うけれど流用できるためこれを使用
            DropItemManager.CreateFaceParts(repairTarget.gameObject.tag, 1);
            repairTarget.Repaired(1);
        }
    }

    private Vector2 GetPosition2D(Transform t)
    {
        return new Vector2(t.position.x, t.position.y);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}