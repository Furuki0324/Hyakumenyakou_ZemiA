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
    public Text eyeElement;
    public Text earElement;
    public Text mouthElement;


    private static int eyeElements = 0, earElements = 0, mouthElements = 0;
    /// <summary>
    /// <para>パーツを生成するときの消費量。</para>
    /// </summary>
    private static int spendingElement = 0;
    private static string[] spendingType = { "none", "eye", "ear", "mouth" };


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

    public static void MoreSpendingElement()
    {

    }

    private void RefleshTexts()
    {
        //elements.text = "Eye: " + eyeElements.ToString() + " Ear: " + earElements.ToString() + " Mouth: " + mouthElements.ToString();
        if (eyeElements <= 0) eyeElement.color = Color.red;
        else eyeElement.color = Color.white;

        if (earElements <= 0) earElement.color = Color.red;
        else earElement.color = Color.white;

        if (mouthElements <= 0) mouthElement.color = Color.red;
        else mouthElement.color = Color.white;


        eyeElement.text = eyeElements.ToString();
        earElement.text = earElements.ToString();
        mouthElement.text = mouthElements.ToString();
    }
}
