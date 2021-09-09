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
                eyeElements -= cost;
                break;

            case "Face_Ear":
                earElements -= cost;
                break;

            case "Face_Mouth":
                mouthElements -= cost;
                break;
        }

        thisInstance.RefleshTexts();
    }

    private void RefleshTexts()
    {
        elements.text = "Eye: " + eyeElements.ToString() + " Ear: " + earElements.ToString() + " Mouth: " + mouthElements.ToString();
    }
}
