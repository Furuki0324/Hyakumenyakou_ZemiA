using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemManager : MonoBehaviour
{
    //Instance prototype
    private static DropItemManager thisInstance;


    [Header("Option")]
    public bool LOCK;

    [Header("UI Image")]
    public Image spendingElementIndicator;

    [Header("UI Text")]
    public Text elements;
    public Text eyeElement;
    public Text earElement;
    public Text mouthElement;
    private Text indicatorText;

    [Header("UI Position")]
    public RectTransform eyePoint;
    public RectTransform earPoint;
    public RectTransform mouthPoint;

    private static Dictionary<string, int> dictionary = new Dictionary<string, int>()
    {
        {"Face_Eye",0 },
        {"Face_Ear",1 },
        {"Face_Mouth",2 }
    };

    private static Dictionary<int, string> reverseDictionary = new Dictionary<int, string>()
    {
        {0,"Face_Eye" },
        {1,"Face_Ear" },
        {2,"Face_Mouth" }
    };
    private static int selectedItem;


    private static int eyeElements = 0, earElements = 0, mouthElements = 0;
    /// <summary>
    /// <para>パーツを生成するときの消費量。</para>
    /// </summary>
    private static int spendingElement = 0;
    private static int spendingElementHolder;


    private void Awake()
    {
        thisInstance = this;

        indicatorText = spendingElementIndicator.GetComponentInChildren<Text>();

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


    /// <summary>
    /// <para>使用するアイテムの素材量を確認し、素材量が消費量を満たしている場合は素材を消費してtrueを返します。</para>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="cost"></param>
    /// <returns></returns>
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

    public static bool TryToUseElementWhenCreatingFace()
    {
        if(spendingElement <= 0)
        {
            Debug.LogWarning("No element selected.");
            return false;
        }


        if (CanUseElements(reverseDictionary[selectedItem], spendingElement))
        {
            spendingElementHolder = spendingElement;
            spendingElement = 0;
            thisInstance.RefleshTexts();
            return true;
        }

        return false;
        
    }


    /// <summary>
    /// <para>注意：このメソッドはものすごい雑な考えのもとに作られています。可能な限り使わないでください。</para>
    /// <para>直前に消費した素材の量を一時保存した数値を取得します。</para>
    /// </summary>
    /// <returns></returns>
    public static int GetSpendingElementFromHolder()
    {
        return spendingElementHolder;
    }

    private static int GetReservedElement(int a)
    {
        int b = 0;

        switch (a)
        {
            case 0:
                b = eyeElements;
                break;

            case 1:
                b = earElements;
                break;

            case 2:
                b = mouthElements;
                break;
        }

        return b;
    }

    /// <summary>
    /// <para>返り値リスト</para>
    /// <para>0 - Eye</para>
    /// <para>1 - Ear</para>
    /// <para>2 - Mouth</para>
    /// </summary>
    /// <returns></returns>
    public static int GetSelectedItem()
    {
        return selectedItem;
    }

    public static int GetSpendingElementAmount()
    {
        return spendingElement;
    }


    /// <summary>
    /// <para>クリックでアイテムの消費量を増加させます。</para>
    /// <para>増加消費量の初期値は1です。引数を利用してその諒を変更できます。</para>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="add"></param>
    public static void MoreSpendingElement(string type, int add = 1)
    {
        int tryGetValue;
        if (dictionary.TryGetValue(type,out tryGetValue))
        {
            //新たに選択されたアイテムが以前のものと違う場合は消費量をリセットします
            if (!(selectedItem == tryGetValue))
            {
                spendingElement = 0;
                selectedItem = tryGetValue;
            }

            if(spendingElement < GetReservedElement(selectedItem))
            {
                spendingElement += add;
            }

            
        }

        thisInstance.RefleshTexts();
        Debug.Log(spendingElement);
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

        switch (selectedItem)
        {
            case 0:
                spendingElementIndicator.rectTransform.position = eyePoint.position;
                break;

            case 1:
                spendingElementIndicator.rectTransform.position = earPoint.position;
                break;

            case 2:
                spendingElementIndicator.rectTransform.position = mouthPoint.position;
                break;

            default:
                break;
        }

        indicatorText.text = spendingElement.ToString();
    }
}
