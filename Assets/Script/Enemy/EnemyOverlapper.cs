using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOverlapper : MonoBehaviour
{
    [Header("Angle range")]
    [Tooltip("検知する角度")]
    public float detectAngle = 90;

    [Tooltip("最大距離")]
    public float detectRange = 100;

    private LayerMask layerMask;

    [SerializeField]
    private ContactFilter2D conf;
    [SerializeField]
    Vector3 minAng, maxAng;
    public List<Collider2D> colList;

    /// <summary>
    /// <para>このメソッドは親子関係で使われることを前提としています。専用のオブジェクトを配置してください。</para>
    /// <para>OverlapCircleで疑似Circle Colliderを作り、その内部のcolliderをcolListに一度纏め、NormalAngleの内部かどうかを別途boolで見て場合分けして、objectListに戻している。</para>
    /// </summary>
    /// <returns></returns>

    public List<Transform> GetChaseTargetInList()
    {
        layerMask = LayerMask.GetMask("Face");
        List<Transform> objectList = new List<Transform>();

        conf.SetNormalAngle((180 - detectAngle / 2)+transform.parent.localEulerAngles.z, (180 + detectAngle / 2)+transform.parent.localEulerAngles.z);
        conf.SetLayerMask(layerMask);
        conf.useOutsideNormalAngle = true;

        minAng = new Vector3(detectRange * Mathf.Cos(conf.minNormalAngle * Mathf.Deg2Rad),
                            detectRange * Mathf.Sin(conf.minNormalAngle * Mathf.Deg2Rad),
                            0.0f);
        maxAng = new Vector3(detectRange * Mathf.Cos(conf.maxNormalAngle * Mathf.Deg2Rad),
                            detectRange * Mathf.Sin(conf.maxNormalAngle * Mathf.Deg2Rad),
                            0.0f);
        Debug.DrawRay(transform.position, minAng, Color.blue, 2.0f);
        Debug.DrawRay(transform.position, maxAng, Color.blue, 2.0f);

        Physics2D.OverlapCircle(point: transform.position,
                                        radius: detectRange,
                                        contactFilter: conf,
                                        results: colList);

        for (int i = 0; i < colList.Count; i++)
        {
            if (conf.IsFilteringNormalAngle(colList[i].transform.position - transform.position) &&
                    !(objectList.Contains(colList[i].transform)))
            {
                objectList.Add(colList[i].transform);
            }
        }
        return objectList;
    }
}