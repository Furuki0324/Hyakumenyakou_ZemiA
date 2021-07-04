﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemManager : MonoBehaviour
{
    //Instance prototype
    private static DropItemManager thisInstance;


    [Header("UI Text")]
    public Text eyeText;
    public Text earText;
    public Text mouseText;

    private static int eyeElements = 0, earElements = 0, mouseElements = 0;


    private void Awake()
    {
        thisInstance = this;
        thisInstance.RefleshTexts();
    }

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

        thisInstance.RefleshTexts();

        Debug.Log("EyeElements: " + eyeElements + " EarElements: " + earElements + " MouseElements: " + mouseElements);
    }

    private void RefleshTexts()
    {
        eyeText.text = eyeElements.ToString();
        earText.text = earElements.ToString();
        mouseText.text = mouseElements.ToString();
    }
}
