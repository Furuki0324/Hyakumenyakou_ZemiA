using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHighSpeed : MonoBehaviour, IBossStateRoot
{
    //---------------------Private------------------
    private FacePartsBaseScript faceScript;
    [SerializeField]
    private Transform hsChaseTarget;

    public void SetFaceScript(FacePartsBaseScript face)
    {
        faceScript = face;
    }

    public FacePartsBaseScript GetFacePart()
    {
        return faceScript;
    }
    public bool First { get; set; }
    public void attack() { }
    public void defend() { }
    public void move()
    {
        if (First)
        {
            ResetTarget();
        }
        chase();
    }
    public void stopAllCoroutine(){ }

    public void ResetTarget()
    {
        BossDeepData.GetBDpData.transforms.Clear();
        hsChaseTarget = GameObject.FindGameObjectWithTag("Face_Nose").transform;

        Vector3 diff = (hsChaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, diff);
        BossDeepData.GetBDpData.transforms = MainScript.GetFaceObjectTransformInList();
        FindRandomTarget(BossDeepData.GetBDpData.transforms);
        First = false;
    }

    void FindRandomTarget(List<Transform> target)
    {
        if (target.Count > 0)
        {
            hsChaseTarget = target[Random.Range(0, target.Count)];
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
