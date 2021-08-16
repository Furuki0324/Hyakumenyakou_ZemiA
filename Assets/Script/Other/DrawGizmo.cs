using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
        UnityEditor.Handles.Label(transform.position + Vector3.one, "<size=15> <color=blue>" + name + "</color></size>", style);
    }
}
