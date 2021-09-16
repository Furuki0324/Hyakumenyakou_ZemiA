using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemManager : MonoBehaviour
{
    //Instance prototype
    private static DropItemManager thisInstance;


    [Header("UI Text")]
    public Text elements;


    private static int eyeElements = 0, earElements = 0, mouthElements = 0;


    private void Awake()
    {
        thisInstance = this;
        thisInstance.RefleshTexts();
    }

    public static void ObtainItem(string type, int amount = 1)
    {

        switch (type)
        {
            case "EyeElement":
                eyeElements += amount;
                break;

            case "EarElement":
                earElements += amount;
                break;

            case "MouthElement":
                mouthElements += amount;
                break;

            default:
                Debug.LogWarning("You tried to obtain an item with an error code.");
                break;
        }

        thisInstance.RefleshTexts();

        Debug.Log("EyeElements: " + eyeElements + " EarElements: " + earElements + " MouthElements: " + mouthElements);
    }

    public static void CreateFaceParts(string type, int cost)
    {
        switch (type)
        {
            case "Face_Eye":
                if(!(eyeElements - cost < 0)) eyeElements -= cost;
                break;

            case "Face_Ear":
                if(!(earElements - cost < 0)) earElements -= cost;
                break;

            case "Face_Mouth":
                if(!(mouthElements - cost < 0)) mouthElements -= cost;
                break;
        }

        thisInstance.RefleshTexts();
    }

    public static bool CanUseElements(string type, int cost)
    {
        int eye, ear, mouth;
        eye = eyeElements;
        ear = earElements;
        mouth = mouthElements;

        switch (type)
        {
            case "Face_Eye":
                eye -= cost;
                break;

            case "Face_Ear":
                ear -= cost;
                break;

            case "Face_Mouth":
                mouth -= cost;
                break;
        }

        if(eye >= 0 && ear >= 0 && mouth >= 0)
        {
            eyeElements = eye;
            earElements = ear;
            mouthElements = mouth;

            thisInstance.RefleshTexts();
            Debug.Log("Use elements granted.");
            return true;
        }
        else
        {
            Debug.Log("Use elements denied.");
            return false;
        }

    }

    private void RefleshTexts()
    {
        elements.text = "Eye: " + eyeElements.ToString() + " Ear: " + earElements.ToString() + " Mouth: " + mouthElements.ToString();
    }
}
