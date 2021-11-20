using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR
public class DrawGizmo : MonoBehaviour
{
    private enum ColorPattern { black, white, red, green, blue, cyan, gray, magenta, yellow}

    [SerializeField] Color sphereColor = Color.red;
    [SerializeField] int textSize = 15;
    [SerializeField] ColorPattern textColor = ColorPattern.blue;
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;

        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(transform.position, 1);
        UnityEditor.Handles.Label(transform.position + Vector3.one, $"<size={textSize}> <color={textColor}>" + name + "</color></size>", style);
    }
}
#endif