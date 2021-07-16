using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaycaster : MonoBehaviour
{
    [Header("Angle range")]
    [Tooltip("検知する角度")]
    public float detectAngle = 180;
    [Tooltip("負担軽減のために指定範囲を全て捜索せず、この数値°の間隔でレイキャストを飛ばします。")]
    public float acculacy = 5;

    [Tooltip("最大距離")]
    public float detectRange = 10;

    private int layerMask = 1 << 11;
    

    /// <summary>
    /// <para>このメソッドは親子関係で使われることを前提としています。専用のオブジェクトを配置してください。</para>
    /// <para>オブジェクトの前方の指定された範囲にレイキャストを発射し、検知したオブジェクトのTransformを配列で返します。</para>
    /// </summary>
    /// <returns></returns>
    public Transform[] GetRaycastHitInTransform()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 0, -(detectAngle / 2)));

        List<Transform> objectList = new List<Transform>();
        
        RaycastHit2D hit;
        
        for (int i = 0; detectAngle > i; i++)
        {
            
            Ray ray = new Ray(origin: transform.position,
                direction: transform.right);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 2);
            /*
            if(hit = Physics2D.Raycast(origin: transform.position,
                direction: transform.right,
                distance: detectRange,
                layerMask: layerMask))
            {
                objectList.Add(hit.collider.transform);
            }
            */
            transform.Rotate(new Vector3(0, 0, i));
        }
        
        return objectList.ToArray();
    }

    public List<Transform> GetTransformsInList()
    {
        List<Transform> objectList = new List<Transform>();

        RaycastHit2D hit;

        for (int i = 0; 5 > i; i++)
        {
            /*
            Ray ray = new Ray(origin: transform.position,
                direction: transform.right);
            Debug.DrawRay(ray.origin, ray.direction * detectRange, Color.red, 2);
            */
            Ray2D ray2D = new Ray2D(origin: transform.position, direction: transform.right);
            Debug.DrawRay(ray2D.origin, ray2D.direction * detectRange, Color.blue, 2);
            /*
            if(hit = Physics2D.Raycast(origin: transform.position,
                direction: transform.right,
                distance: detectRange,
                layerMask: layerMask))
            {
                objectList.Add(hit.collider.transform);
            }
            */
            hit = Physics2D.Raycast(ray2D.origin, ray2D.direction, detectRange, layerMask);
            if(hit.collider != null && !(objectList.Contains(hit.collider.transform)))
            {
                objectList.Add(hit.collider.transform);
            }
            
            transform.Rotate(new Vector3(0, 0, i));
        }

        return objectList;
    }
}
