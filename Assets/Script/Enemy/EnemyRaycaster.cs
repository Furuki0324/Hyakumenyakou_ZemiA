using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycaster : MonoBehaviour
{
    [Header("Angle range")]
    [Tooltip("検知する角度")]
    public float detectAngle = 90;
    [Tooltip("負担軽減のために指定範囲を全て捜索せず、この数値°の間隔でレイキャストを飛ばします。")]
    public float acculacy = 5;

    [Tooltip("最大距離")]
    public float detectRange = 10;

    private int layerMask = 1 << 11;


    /// <summary>
    /// <para>このメソッドは親子関係で使われることを前提としています。専用のオブジェクトを配置してください。</para>
    /// <para>オブジェクトの前方の指定された範囲にレイキャストを発射し、検知したオブジェクトのTransformをListで返します。</para>
    /// </summary>
    /// <returns></returns>
    public List<Transform> GetTransformsInList()
    {
        transform.Rotate(0, 0, -detectAngle / 2);

        List<Transform> objectList = new List<Transform>();

        RaycastHit2D hit;

        for (int i = 0; detectAngle > acculacy * i; i++)
        {
            Ray2D ray2D = new Ray2D(origin: transform.position, direction: transform.up);
            Debug.DrawRay(ray2D.origin, ray2D.direction * detectRange, Color.blue, 2);

            hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, detectRange, layerMask);
            if(hit.collider != null && !(objectList.Contains(hit.collider.transform)))
            {
                objectList.Add(hit.collider.transform);
            }
            
            
            transform.Rotate(new Vector3(0, 0, acculacy));
        }
        return objectList;
    }
}
