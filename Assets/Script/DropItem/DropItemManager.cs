using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    

    private static int eyeElements = 0, earElements = 0, mouseElements = 0;

    public static void ObtainItem(string type)
    {
        switch (type)
        {
            case "EyeElement":
                eyeElements++;
                break;

            case "EarElement":
                earElements++;
                break;

            case "MouseElement":
                mouseElements++;
                break;

            default:
                Debug.LogWarning("You tried to obtain an item with an error code.");
                break;
        }

        Debug.Log("EyeElements: " + eyeElements + " EarElements: " + earElements + " MouseElements: " + mouseElements);
    }
}
