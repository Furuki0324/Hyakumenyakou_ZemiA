//#define SAVE

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DropItemManager : MonoBehaviour
{
    /******************************************************************
     *         このスクリプト内ではstatic変数に「_」を付けています。      *
     *         private変数の印ではありません                            *
     ******************************************************************/

    [Header("Bool")]
    private static bool _lessTextShown;

    [Header("Sound")]
    [SerializeField] private SoundInfo[] SFX;
    [SerializeField] private AudioMixerGroup sfxMixer;

    [Header("UI Image")]
    [SerializeField] private Image spendingElementCircle;

    [Header("UI Text")]
    [SerializeField] private Text eyeElement;
    [SerializeField] private Text earElement;
    [SerializeField] private Text mouthElement;
    private static Text indicatorText;
    [SerializeField] private Text lessElementWarning;

    [Header("UI Position")]
    [SerializeField] private RectTransform eyePoint;
    [SerializeField] private RectTransform earPoint;
    [SerializeField] private RectTransform mouthPoint;

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


    #region Get static variables from editor inspector
    private static Image _spendingElementCircle;
    private static Text _eyeElement, _earElement, _mouthElement, _lessElementWarning;
    private static Dictionary<SoundInfo.Pattern, AudioClip> soundDictionary = new Dictionary<SoundInfo.Pattern, AudioClip>();
    private static RectTransform _eyePoint, _earPoint, _mouthPoint;
    private static AudioMixerGroup _sfxMixer;

    #endregion


    private void Awake()
    {
        GetStaticVariables();

        RefleshTexts();
        DisableText(_lessElementWarning, 0);
    }


    private void GetStaticVariables()
    {
        //UI Image
        _spendingElementCircle = spendingElementCircle;

        //UI Text
        _eyeElement = eyeElement;
        _earElement = earElement;
        _mouthElement = mouthElement;
        _lessElementWarning = lessElementWarning;
        indicatorText = _spendingElementCircle.GetComponentInChildren<Text>();

        //AudioClip
        soundDictionary.Clear();
        foreach(SoundInfo info in SFX)
        {
            soundDictionary.Add(info.pattern, info.clip);
        }

        //Mixer
        _sfxMixer = sfxMixer;

        //RectTransform
        _eyePoint = eyePoint;
        _earPoint = earPoint;
        _mouthPoint = mouthPoint;


    }


    public static void ObtainItem(string type, int amount = 1, bool initialize = false)
    {

        switch (type)
        {
            case "EyeElement":
                if (initialize) eyeElements = 0;
                eyeElements += amount;
                break;

            case "EarElement":
                if (initialize) earElements = 0;
                earElements += amount;
                break;

            case "MouthElement":
                if (initialize) mouthElements = 0;
                mouthElements += amount;
                break;

            default:
                Debug.LogWarning("You tried to obtain an item with an error code.");
                break;
        }

        if (!initialize)
        {
            _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(soundDictionary[SoundInfo.Pattern.obtainItem], _sfxMixer);

#if SAVE
            //セーブデータに素材を一つ回収したことを記録
            SaveData.AchievementStep(Achievement.StepType.collector);
#endif
        }
        RefleshTexts();

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

            RefleshTexts();
            return true;
        }
        else
        {
            _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(soundDictionary[SoundInfo.Pattern.accessDeny], _sfxMixer);
            return false;
        }

    }

    public static bool TryToUseElement(int face, int consumption)
    {
        if(consumption <= 0)
        {
            Debug.LogWarning("No element selected.");
            return false;
        }

        if(CanUseElements(reverseDictionary[face],consumption))
        {
            _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(soundDictionary[SoundInfo.Pattern.createFace], _sfxMixer);
            RefleshTexts();
            return true;
        }

        return true;
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

            _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(soundDictionary[SoundInfo.Pattern.createFace],_sfxMixer);
            RefleshTexts();
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


    /// <summary>
    /// <para>クリックでアイテムの消費量を増加させます。</para>
    /// <para>増加消費量の初期値は1です。引数を利用してその諒を変更できます。</para>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="add"></param>
    public static void MoreSpendingElement(string type, int add = 1)
    {
        if (dictionary.TryGetValue(type,out int tryGetValue))
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
            else
            {
                ShowText("LessElement");
                _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(soundDictionary[SoundInfo.Pattern.accessDeny], _sfxMixer);
            }

            
        }

        RefleshTexts();
        //Debug.Log(spendingElement);
    }

    private static void RefleshTexts()
    {
        //elements.text = "Eye: " + eyeElements.ToString() + " Ear: " + earElements.ToString() + " Mouth: " + mouthElements.ToString();
        if (eyeElements <= 0) _eyeElement.color = Color.red;
        else _eyeElement.color = Color.white;

        if (earElements <= 0) _earElement.color = Color.red;
        else _earElement.color = Color.white;

        if (mouthElements <= 0) _mouthElement.color = Color.red;
        else _mouthElement.color = Color.white;


        _eyeElement.text = eyeElements.ToString();
        _earElement.text = earElements.ToString();
        _mouthElement.text = mouthElements.ToString();

        switch (selectedItem)
        {
            case 0:
                _spendingElementCircle.rectTransform.position = _eyePoint.position;
                break;

            case 1:
                _spendingElementCircle.rectTransform.position = _earPoint.position;
                break;

            case 2:
                _spendingElementCircle.rectTransform.position = _mouthPoint.position;
                break;

            default:
                break;
        }

        indicatorText.text = spendingElement.ToString();
    }


    private static async Task ShowText(string text)
    {
        switch (text)
        {
            case "LessElement":
                if (!_lessTextShown)
                {
                    _lessElementWarning.gameObject.SetActive(true);
                    _lessTextShown = true;
                    await DisableText(_lessElementWarning, 3000);
                    
                }
                break;
        }
    }


    private static async Task DisableText(Text text, int milliSecond)
    {
        //Debug.Log("Task started.");

        await Task.Delay(milliSecond);
        text.gameObject.SetActive(false);
        _lessTextShown = false;

        //Debug.Log("Task finished.");
    }
}

[System.Serializable]
public class SoundInfo
{
    public enum Pattern { none, obtainItem, createFace, accessDeny}
    public Pattern pattern;
    public AudioClip clip;
}