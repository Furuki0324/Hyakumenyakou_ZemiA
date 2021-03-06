using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairFace : MonoBehaviour
{
    //Public
    public KeyCode repairKey;
    public float radius;

    [Header("Number")]
    [Tooltip("指定したパーセント分だけ修理対象を回復させます")]
    [SerializeField] private float healPercent;

    [Header("Effect")]
    [SerializeField] UnityEngine.Video.VideoPlayer effect;
    [SerializeField] Transform parentCanvas;

    [Header("Particle")]
    public ParticleSystem repairParticle;

    //private 
    [Header("READ ONLY")]
    [SerializeField, ReadOnly] private FacePartsBaseScript repairTarget;
    private ContactFilter2D filter2D;
    [SerializeField, ReadOnly] private List<Collider2D> faceList = new List<Collider2D>();

    private void Start()
    {
        filter2D.SetLayerMask(LayerMask.GetMask("Face"));
    }

    private void Update()
    {
        Physics2D.OverlapCircle(GetPosition2D(transform), radius, filter2D, faceList);

        if(faceList.Count > 0)
        {
            foreach(Collider2D col in faceList)
            {
                if (!repairTarget)
                {
                    repairTarget = col.gameObject.GetComponent<FacePartsBaseScript>();
                    continue;
                }

                float oldDistance = (GetPosition2D(transform) - GetPosition2D(repairTarget.transform)).magnitude;
                float newDistance = (GetPosition2D(transform) - GetPosition2D(col.transform)).magnitude;

                if(newDistance < oldDistance)
                {
                    repairTarget = col.GetComponent<FacePartsBaseScript>();
                }
            }
        }
        else
        {
            repairTarget = null;
        }

        if(Input.GetKeyDown(repairKey) && repairTarget)
        {
            if(repairTarget.Damaged() && DropItemManager.CanUseElements(repairTarget.tag, 1))
            {
                //Instantiate(repairParticle, repairTarget.transform.position, Quaternion.identity);
                Instantiate(effect, repairTarget.transform.position, Quaternion.identity, parentCanvas);
                repairTarget.Repaired(healPercent);
            }
        }
    }

    private Vector2 GetPosition2D(Transform t)
    {
        return new Vector2(t.position.x, t.position.y);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);

        if (repairTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(repairTarget.transform.position, 1);
        }
    }
#endif
}
