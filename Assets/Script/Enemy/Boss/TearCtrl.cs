using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TearCtrl : MonoBehaviour
{
    static Transform _TEARBULLET_ANCHOR;
    static Transform TEARBULLET_ANCHOR
    {
        get
        {
            if (_TEARBULLET_ANCHOR == null)
            {
                GameObject go = new GameObject("TEARBULLET_ANCHOR");
                _TEARBULLET_ANCHOR = go.transform;
            }
            return _TEARBULLET_ANCHOR;
        }
    }
    private Transform coreParts;
    private Rigidbody2D tearRigid;
    private FacePartsBaseScript faceScript;
    private Vector3 startPos;

    void Start()
    {
        coreParts = GameObject.FindGameObjectWithTag("Face_Nose").transform;
        tearRigid = GetComponent<Rigidbody2D>();
        transform.SetParent(TEARBULLET_ANCHOR);

        startPos = transform.position;
        
    }

    void Update()
    {
        Vector2 force = (coreParts.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, force);
        force *= BossData.bossData.eyeAttackSpeed;
        tearRigid.velocity = force;
        lifeRange();
    }

     void lifeRange()
    {
        if ((transform.position - startPos).magnitude > BossData.bossData.attacksRange) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != BossDeepData.GetBDpData.toPossessParts)
        {
            if (other.gameObject.GetComponent<FacePartsBaseScript>())
            {
                faceScript = other.gameObject.GetComponent<FacePartsBaseScript>();
                faceScript.TakeDamage(BossData.bossData.eyeAttackPower);
                Destroy(this.gameObject);
            }
        }
    }
}
