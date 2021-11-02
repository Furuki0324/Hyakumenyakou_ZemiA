#define PHASE_TEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCtrl : EnemyBaseScript
{
    //---------------------Public------------------

    [Header("Property to chase target")]
    public float limitAngle;
    public Transform chaseTarget;
    public float speed;

    [Header("Set Trigger Tag")]
    public string triggerTag;

    [Header("Aim core only")]
    public bool coreAim;

    [Header("Sprite direction")]
    public bool leftForward;

    //---------------------Private------------------
    private Rigidbody2D rigid2D;
    private EnemyOverlapper overLapper;
    private FacePartsBaseScript faceScript;
    private Vector3 scale;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        //攻撃対象に接触している間は移動を停止します。
        if (faceScript)
        {
            rigid2D.velocity = Vector2.zero;
            return;
        }

        //何らかの理由で追跡対象を見つけられていない場合は再度検索します。
        if (!chaseTarget)
        {
            ResetTarget();
            return;
        }

        Vector2 force = (chaseTarget.position - transform.position).normalized * speed;
        rigid2D.velocity = force;
    }

    /// <summary>
    /// <para>インスペクターで設定できるleftForward(イラストが左向き)というboolを基に</para>
    /// <para>ターゲットとの座標を比較してイラストを反転させます。</para>
    /// </summary>
    private void FlipFlop()
    {
        if (leftForward)
        {
            if (transform.position.x <= chaseTarget.position.x) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
        }
        else
        {
            if (transform.position.x <= chaseTarget.position.x) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;
        }
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
        //Debug.Log("Called: " + gameObject.name);
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
}
