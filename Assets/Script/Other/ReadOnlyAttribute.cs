using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyAttribute : PropertyAttribute
{
    
}


[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
#if UNITY_EDITOR
public class ReadOnlyAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }
}
#endif