using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteAlways]
[CanEditMultipleObjects]
public class GameObjectLocker : MonoBehaviour
{
    private bool isLocked = false;
    private Vector3 position;

    // Update is called once per frame
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            Transform transform = GetComponent<Transform>();
            if (isLocked)
            {
                transform.hideFlags = HideFlags.NotEditable;
                if (transform.hasChanged)
                {
                    Debug.LogWarning("You cannot move " + this.name + ".");
                    transform.position = position;
                    transform.hasChanged = false;
                }
            }
            else
            {
                transform.hideFlags = HideFlags.None;
            }
        }
    }

    private void SwitchLock()
    {
        Transform transform = GetComponent<Transform>();

        if (!isLocked)
        {
            position = gameObject.transform.position;
            
            transform.hideFlags = HideFlags.NotEditable;
        }
        else
        {
            transform.hideFlags = HideFlags.None;
        }
        isLocked = !isLocked;
    }



    [CustomEditor(typeof(GameObjectLocker))]

    class GameObjectLockerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            string text;
            if (Instance.isLocked) text = "Unlock";
            else text = "Lock";

            if (GUILayout.Button(text))
            {
                Instance.SwitchLock();
            }
        }

        GameObjectLocker Instance
        {
            get { return (GameObjectLocker)target; }
        }
    }
}
#endif