using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoiceCtrl : MonoBehaviour
{
    static Transform _VOICEBULLET_ANCHOR;
    static Transform VOICEBULLET_ANCHOR
    {
        get
        {
            if (_VOICEBULLET_ANCHOR == null)
            {
                GameObject go = new GameObject("VOICEBULLET_ANCHOR");
                _VOICEBULLET_ANCHOR = go.transform;
            }
            return _VOICEBULLET_ANCHOR;
        }
    }
    private Transform coreParts;
    private FacePartsBaseScript faceScript;

    BezierCurve bc;
    const int pointCount = 2;

    Vector3 voiceForward;

    Vector3 firstLocalPos;
    Vector3 firstHandle;
    Vector3 secondLocalPos;
    Vector3 secondHandle;

    Quaternion f;
    Quaternion s;

    public float speed = 0.01f;
    public float curvature = 0.005f;

    void Start()
    {
        coreParts = GameObject.FindGameObjectWithTag("Face_Nose").transform;
        voiceForward = (Vector3.up + Vector3.right).normalized;
        bc = gameObject.GetComponent<BezierCurve>();
        for (int i = 0; i < pointCount; i++)
        {
            bc.AddPointAt(Vector3.zero);
        }
        transform.SetParent(VOICEBULLET_ANCHOR);
        firstLocalPos = new Vector3(1,1,1);
        f = Quaternion.AngleAxis(180.0f,Vector3.forward);
        firstLocalPos = f * firstLocalPos;
    }

    // Update is called once per frame
    void Update()
    {
        moveBezier();
        Vector2 force = (coreParts.position - transform.position).normalized;
        //transform.rotation = Quaternion.FromToRotation(Vector3.up, force);
    }

    void moveBezier()
    {
        firstLocalPos.x += speed;
        secondLocalPos.y += speed;
        firstHandle.y += curvature;
        secondHandle.x += curvature;
        bc[0].localPosition = firstLocalPos;
        bc[0].handle2 = firstHandle;
        bc[1].localPosition = secondLocalPos;
        bc[1].handle1 = secondHandle;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.transform != BossDeepData.GetBDpData.toPossessParts)
    //     {
    //         if (other.gameObject.GetComponent<FacePartsBaseScript>())
    //         {
    //             faceScript = other.gameObject.GetComponent<FacePartsBaseScript>();
    //             faceScript.TakeDamage(BossData.bossData.mouthAttackPower);
    //             Destroy(this.gameObject);
    //         }
    //     }
    // }
}
