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

    [Header("Property to chase target")]
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

    /// <summary>
    /// <para>TransformのListをクリアした後、新たなターゲットを定めます。</para>
    /// </summary>
    public void ResetTarget()
    {
        transforms.Clear();

        chaseTarget = GameObject.FindGameObjectWithTag("Face_Nose").transform;
        
        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);

        transforms = rayCaster.GetTransformsInList();

        Debug.Log(transforms.Count);

        FindClosestTarget(transforms);
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
                if((transform.position - target[i].position).magnitude < (transform.position - chaseTarget.position).magnitude)
                {
                    chaseTarget = target[i];
                }
            }
        }

        Vector3 diff = (chaseTarget.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
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
        if (faceScript) return;

        Vector2 force = (chaseTarget.position - transform.position).normalized * speed;
        rigid2D.velocity = force;
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
