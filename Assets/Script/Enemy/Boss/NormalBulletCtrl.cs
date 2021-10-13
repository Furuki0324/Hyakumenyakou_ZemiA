using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletCtrl : MonoBehaviour
{
    static Transform _NORMALBULLET_ANCHOR;
    static Transform NORMALBULLET_ANCHOR
    {
        get
        {
            if (_NORMALBULLET_ANCHOR == null)
            {
                GameObject go = new GameObject("NORMALBULLET_ANCHOR");
                _NORMALBULLET_ANCHOR = go.transform;
            }
            return _NORMALBULLET_ANCHOR;
        }
    }

    public Vector2 force;

    public int damage;
    private Rigidbody2D rigid;
    private FacePartsBaseScript faceScript;
    private Vector3 startPos;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        transform.SetParent(NORMALBULLET_ANCHOR);
        startPos = transform.position;
    }

    void Update()
    {
        //円形なのでいらない
        //transform.rotation = Quaternion.FromToRotation(Vector3.down, force);
        rigid.velocity = force;
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
                faceScript.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
