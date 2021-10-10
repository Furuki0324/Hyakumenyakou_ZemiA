#define PHASE_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCtrl : EnemyBaseScript
{
    //---------------------Public------------------
    /*この3ステータスはベーススクリプトへ移行しました。
    [Header("Status")]
    public int hp;
    public int attackPower = 1;
    public float attackInterval = 1;
    */

    [Header("Property to chase target")]
    public float limitAngle;
    public Transform chaseTarget;
    public float speed;

    [Header("Set Trigger Tag")]
    public string triggerTag;

    [Header("Aim core only")]
    public bool coreAim;


    //---------------------Private------------------
    private Rigidbody2D rigid2D;
    private EnemyOverlapper overLapper;
    private FacePartsBaseScript faceScript;
    private Vector3 scale;

    public void SetFaceScript(FacePartsBaseScript face)
    {
        faceScript = face;
    }

    public FacePartsBaseScript GetFacePart()
    {
        return faceScript;
    }


    List<Transform> transforms = new List<Transform>();
    private Transform[] transformArray;
    private List<Transform> transformList = new List<Transform>();
    private Transform lastTarget;

    /// <summary>
    /// <para>TransformのListをクリアした後、新たなターゲットを定めます。</para>
    /// </summary>
    public void ResetTarget()
    {
        transforms.Clear();

        chaseTarget = GameObject.FindGameObjectWithTag("Face_Nose").transform;

        /*
        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, diff);
        */

        transforms = overLapper.GetChaseTargetInList();

        //Debug.Log(transforms.Count);

        if (!coreAim) FindClosestTarget(transforms);
    }

    /// <summary>
    /// <para>Listで渡されたTransformからpositionを検索し、現在の座標から最も近い顔パーツをターゲットに定めます。</para>
    /// </summary>
    /// <param name="target"></param>
    private void FindClosestTarget(List<Transform> target)
    {
        if (target.Count > 0)
        {
            for (int i = 0; i < target.Count; i++)
            {
                float heightDiff = transform.position.y - target[i].position.y;
                float distance = (transform.position - target[i].position).magnitude;

                if (Mathf.Asin(distance / heightDiff) > Mathf.Abs(limitAngle / 2))
                {
                    Debug.Log(Mathf.Asin(distance / heightDiff));
                    continue;
                }

                if ((transform.position - target[i].position).magnitude < (transform.position - chaseTarget.position).magnitude)
                {
                    chaseTarget = target[i];
                }
            }
        }

        /*
        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, diff);
        */
    }

    void Start()
    {
        scale = transform.localScale;

        rigid2D = GetComponent<Rigidbody2D>();
        overLapper = GetComponentInChildren<EnemyOverlapper>();
        ResetTarget();

        PhaseManager.AddEnemyList(this);
    }

    void Update()
    {
        Chase();
        FlipFlop();
    }

    private void Chase()
    {
        if (faceScript)
        {
            rigid2D.velocity = Vector2.zero;
            return;
        }

        if (!chaseTarget)
        {
            ResetTarget();
            return;
        }

        Vector2 force = (chaseTarget.position - transform.position).normalized * speed;
        rigid2D.velocity = force;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FacePartsBaseScript>() && lastTarget != collision.transform)
        {
            faceScript = collision.gameObject.GetComponent<FacePartsBaseScript>();
            lastTarget = collision.transform;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        Debug.Log("Called: " + gameObject.name);
        while (faceScript)
        {
            
            if (attackEffect)
            {
                _effect.transform.position = faceScript.transform.position;
                _effect.Play();
            }
       
            //faceScript.TakeDamage();
            faceScript.TakeDamage(attackPower);

            if(faceScript.health <= 0)
            {
                faceScript = null;
                yield break;
            }

            //インターバルをはさんだ後に同じ処理を繰り返します
            yield return new WaitForSeconds(attackInterval);
        }

    }

    private void FlipFlop()
    {
        Vector3 newScale = scale;
        if (transform.position.x <= 0) newScale.x = -scale.x;
        else newScale.x = scale.x;

        transform.localScale = newScale;
    }
}
