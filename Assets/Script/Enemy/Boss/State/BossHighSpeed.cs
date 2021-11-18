using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BossHighSpeed : MonoBehaviour, IBossStateRoot
{
    //---------------------Private------------------
    private FacePartsBaseScript faceScript;
    [SerializeField] private Transform hsChaseTarget;


    public void SetFaceScript(FacePartsBaseScript face)
    {
        faceScript = face;
    }

    public FacePartsBaseScript GetFacePart()
    {
        return faceScript;
    }

    public bool First { get; set; }

    public void attack()
    {
    }

    public void defend()
    {
    }

    public void move()
    {
        if (First)
        {
            ResetTarget();
        }

        chase();
    }

    public void stopHavingAllCoroutine()
    {
    }

    public void ResetTarget()
    {
        hsChaseTarget = GameObject.FindGameObjectWithTag("Face_Nose").transform;

        Vector3 diff = (hsChaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, diff);
        FindRandomTarget(BossDeepData.GetBDpData.Transforms);
        First = false;
    }

    void FindRandomTarget(List<Transform> target)
    {
        if (target.Count <= 0) return;
        if (target[0] == BossCtrl.formerPossess && target.Count == 1) return;
        while (true)
        {
            hsChaseTarget = target[Random.Range(0, target.Count)];
            if (hsChaseTarget != BossCtrl.formerPossess) break;
        }
    }

    void chase()
    {
        if (faceScript)
        {
            BossDeepData.GetBDpData.bRigid.velocity = Vector2.zero;
            return;
        }

        if (!hsChaseTarget)
        {
            ResetTarget();
            return;
        }

        Vector2 force = (hsChaseTarget.position -
                         transform.position).normalized *
                        BossData.bossData.hsSpeed;

        BossDeepData.GetBDpData.bRigid.velocity = force;
    }
}