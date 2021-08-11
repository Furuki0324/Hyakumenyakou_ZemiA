using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCtrl : MonoBehaviour
{
    //---------------------Public------------------
    [Header("Status")]
    public int hp;
    public float attackInterval = 1;

    [Header("Property to chase target")]
    public float limitAngle;
    public Transform chaseTarget;
    public float speed;

    [Header("Set Trigger Target")]
    public string triggerTag;

    

    //---------------------Private------------------
    private Rigidbody2D rigid2D;
    private EnemyRaycaster rayCaster;
    private FacePartsBaseScript faceScript;

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

    /// <summary>
    /// <para>TransformのListをクリアした後、新たなターゲットを定めます。</para>
    /// </summary>
    public void ResetTarget()
    {
        transforms.Clear();
        transformList.Clear();

        chaseTarget = GameObject.FindGameObjectWithTag("Face_Nose").transform;
        
        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, diff);

        transforms = rayCaster.GetTransformsInList();
        //transformArray = MainScript.GetFaceObjectTransformsInArray();
        transformList = MainScript.GetFaceObjectTransformInList();

        Debug.Log(transforms.Count);

        FindClosestTarget(transformList);
    }

    /// <summary>
    /// <para>Listで渡されたTransformからpositionを検索し、現在の座標から最も近い顔パーツをターゲットに定めます。</para>
    /// </summary>
    /// <param name="target"></param>
    private void FindClosestTarget(List<Transform> target)
    {
        if(target.Count > 0)
        {
            for(int i = 0; i < target.Count; i++)
            {
                float heightDiff = transform.position.y - target[i].position.y;
                float distance = (transform.position - target[i].position).magnitude;

                if (Mathf.Asin(distance / heightDiff) > Mathf.Abs(limitAngle / 2))
                {
                    Debug.Log(Mathf.Asin(distance / heightDiff));
                    continue;
                }

                if((transform.position - target[i].position).magnitude < (transform.position - chaseTarget.position).magnitude)
                {
                    chaseTarget = target[i];
                }
            }
        }

        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.left, diff);
    }

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();

        rayCaster = GetComponentInChildren<EnemyRaycaster>();

        ResetTarget();

    }

    void Update()
    {
        Chase();


    }

    private void Chase()
    {
        if (faceScript)
        {
            rigid2D.velocity = Vector2.zero;
            return;
        }

        Vector2 force = (chaseTarget.position - transform.position).normalized * speed;
        rigid2D.velocity = force;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FacePartsBaseScript>())
        {
            faceScript = collision.gameObject.GetComponent<FacePartsBaseScript>();
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        if (!faceScript) yield break;
        faceScript.TakeDamage();

        //インターバルをはさんだ後に同じ処理を繰り返します
        yield return new WaitForSeconds(attackInterval);
        StartCoroutine(Attack());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDropItemCtrl dropCtrl;

        if (collision.CompareTag(triggerTag))
        {
            hp--;

            if(hp <= 0)
            {
                Destroy(this.gameObject);
                MainScript.RemoveFromEnemyList(this);

                if(faceScript != null)
                {
                    faceScript.RemoveEnemyFromList(this);
                }

                dropCtrl = GetComponent<EnemyDropItemCtrl>();
                if (!dropCtrl) return;

                dropCtrl.DroppingItem();
            }

            
        }
    }
}
