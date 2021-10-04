using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarBulletCtrl : MonoBehaviour
{
    static Transform _EARBULLET_ANCHOR;
    static Transform EARBULLET_ANCHOR
    {
        get
        {
            if (_EARBULLET_ANCHOR == null)
            {
                GameObject go = new GameObject("EARBULLET_ANCHOR");
                _EARBULLET_ANCHOR = go.transform;
            }
            return _EARBULLET_ANCHOR;
        }
    }

    public Vector2 force;

    private Rigidbody2D rigid;
    private FacePartsBaseScript faceScript;
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        transform.SetParent(EARBULLET_ANCHOR);
    }

    void Update()
    {
        //円形なのでいらない
        //transform.rotation = Quaternion.FromToRotation(Vector3.down, force);
        rigid.velocity = force;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != BossDeepData.GetBDpData.toPossessParts)
        {
            if (other.gameObject.GetComponent<FacePartsBaseScript>())
            {
                faceScript = other.gameObject.GetComponent<FacePartsBaseScript>();
                faceScript.TakeDamage(BossData.bossData.earAttackPower);
                Destroy(this.gameObject);
            }
        }
    }
}
