using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class LineWithCollider : MonoBehaviour
{
    LineRenderer lr;
    MeshFilter mf;
    Mesh ms;
    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        mf = gameObject.GetComponent<MeshFilter>();
        ms = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        lr.BakeMesh(ms, true);
        mf.sharedMesh = ms;
        SetPolygonCollider3D.UpdatePolygonColliders(transform);
    }
}
