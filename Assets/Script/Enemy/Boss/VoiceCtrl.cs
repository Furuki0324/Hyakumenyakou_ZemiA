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
    BezierCurve bc;

    private FacePartsBaseScript faceScript;
    public Vector3 force;
    const int pointCount = 2;

    Vector3 firstLocalPos;
    Vector3 firstHandle;
    Vector3 secondLocalPos;
    Vector3 secondHandle;

    Vector3 firstVec;
    Vector3 firstHandleVec;
    Vector3 secondVec;
    Vector3 secondHandleVec;

    public float speed = 0.01f;
    public float curvature = 0.005f;

    private Vector3 startPos;

    void Start()
    {
        bc = gameObject.GetComponent<BezierCurve>();
        for (int i = 0; i < pointCount; i++)
        {
            bc.AddPointAt(Vector3.zero);
        }
        transform.SetParent(VOICEBULLET_ANCHOR);

        firstLocalPos = BossDeepData.GetBDpData.bRigid.transform.position;
        secondLocalPos = BossDeepData.GetBDpData.bRigid.transform.position;
        firstVec = Quaternion.AngleAxis(45.0f, Vector3.forward) * force;
        secondVec = Quaternion.AngleAxis(-45.0f, Vector3.forward) * force;
        firstHandleVec = Vector3.Cross(Vector3.forward, firstVec).normalized * BossData.bossData.mouthAttackCurve;
        secondHandleVec = Vector3.Cross(Vector3.forward, secondVec).normalized * BossData.bossData.mouthAttackCurve;

        startPos = transform.position;
    }

    void Update()
    {
        vecBezier();
        lifeRange();
    }

     void lifeRange()
    {
        if ((bc[0].localPosition - startPos).magnitude > BossData.bossData.attacksRange) Destroy(this.gameObject);
    }

    void vecBezier()
    {
        firstLocalPos += firstVec;
        secondLocalPos += secondVec;
        firstHandle += firstHandleVec;
        secondHandle += secondHandleVec;
        bc[0].localPosition = firstLocalPos;
        bc[0].handle1 = firstHandle;
        bc[1].localPosition = secondLocalPos;
        bc[1].handle1 = secondHandle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != BossDeepData.GetBDpData.toPossessParts)
        {
            if (other.gameObject.GetComponent<FacePartsBaseScript>())
            {
                faceScript = other.gameObject.GetComponent<FacePartsBaseScript>();
                faceScript.TakeDamage(BossData.bossData.mouthAttackPower);
                Destroy(this.gameObject);
            }
        }
    }
}
