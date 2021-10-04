using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class BezierToLine : MonoBehaviour
{
    LineRenderer lr;
    BezierCurve bc;
    int lineCount;
    Vector3[] potArray;
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        bc = gameObject.GetComponent<BezierCurve>();
    }

    private void Update()
    {
        lineCount = bc.pot.Count;
        lr.positionCount = lineCount;
        potArray = bc.pot.ToArray();
        lr.SetPositions(potArray);
    }

}
