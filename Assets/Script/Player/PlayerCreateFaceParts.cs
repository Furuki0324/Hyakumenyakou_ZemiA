//#define SAVE
#define MOUSE_WHEEL

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerCreateFaceParts : MonoBehaviour
{

    [Header("Face Part Prefab")]
    public PrefabInfo[] prefabInfos;

    [Header("KeyCode")]
    [SerializeField] KeyCode createKey;
    [SerializeField] KeyCode switchKey;

    [Header("Sound")]
    [SerializeField] AudioClip createFaceSound;
    [SerializeField] AudioMixerGroup sfxMixerGroup;

    [Header("UI")]
    [SerializeField] Image image;
    private Image previous, next;
    private Text currentText, previousText, nextText;

    /// <summary>
    /// <para>0 - Eye Prefab</para>
    /// <para>1 - Ear Prefab</para>
    /// <para>2 - Mouse Prefab</para>
    /// </summary>
    private int prefabNumber = 0;

    private int consumption;
    [SerializeField] private Text consumptionText;

    [Header("Preview")]
    [SerializeField] private SpriteRenderer facePositionPreview;
    [SerializeField] private float previewDistance = 2.5f;
    private GameObject[] enemyArray;
    public static bool needReflesh;

    private void Start()
    {
        consumption = 0;
        if (!image)
        {
            Debug.LogError("No image object is set to player.");
        }
        image.sprite = prefabInfos[0].prefab.sprites[0];
        currentText = image.gameObject.GetComponentInChildren<Text>();
        currentText.text = DropItemManager.GetElement(0).ToString(); ;

        previous = image.transform.GetChild(1).GetComponent<Image>();
        previous.sprite = prefabInfos[2].prefab.sprites[0];
        previousText = previous.gameObject.GetComponentInChildren<Text>();
        previousText.text = DropItemManager.GetElement(1).ToString();

        next = image.transform.GetChild(2).GetComponent<Image>();
        next.sprite = prefabInfos[1].prefab.sprites[0];
        nextText = next.gameObject.GetComponentInChildren<Text>();
        nextText.text = DropItemManager.GetElement(2).ToString();

        facePositionPreview.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(switchKey)) { SwitchPrefab(); }
        if(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0) Consumption();

        if(needReflesh)
        {
            SetNewSprite();
            SetNewText();
            needReflesh = false;
        }

        if (Input.GetKeyDown(createKey))
        {
            CreateFaceParts();
        }
    }

    /// <summary>
    /// <para>素材アイテムの消費量を調節します</para>
    /// </summary>
    private void Consumption()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if(mouseWheel > 0)
        {
            consumption++;
            Debug.Log(consumption);
        }
        else if(mouseWheel < 0)
        {
            consumption--;
            Debug.Log(consumption);
        }

        int currentElement = DropItemManager.GetElement(prefabNumber);
        if(consumption > currentElement)
        {
            consumption = currentElement;
            Debug.LogWarning("You cannot use more element.");
        }
        else if(consumption <= 0)
        {
            consumption = 0;
        }


        needReflesh = true;
    }

    public static void Receiver()
    {
        needReflesh = true;
    }

    private void SwitchPrefab()
    {
        prefabNumber++;

        if(prefabNumber > prefabInfos.Length - 1) { prefabNumber = 0; }

        Debug.Log($"Switched to {prefabNumber}");

        SetNewSprite();
        SetNewText();
    }

    private void SetNewSprite()
    {
        int previousIdx = prefabNumber + 2;
        if (previousIdx > prefabInfos.Length - 1) { previousIdx -= prefabInfos.Length; }

        int nextIdx = prefabNumber + 1;
        if (nextIdx > prefabInfos.Length - 1) { nextIdx -= prefabInfos.Length; }

        image.sprite = prefabInfos[prefabNumber].prefab.sprites[0];
        previous.sprite = prefabInfos[previousIdx].prefab.sprites[0];
        next.sprite = prefabInfos[nextIdx].prefab.sprites[0];
    }

    private void SetNewText()
    {
        int previousIdx = prefabNumber + 2;
        if (previousIdx > prefabInfos.Length - 1) { previousIdx -= prefabInfos.Length; }

        int nextIdx = prefabNumber + 1;
        if (nextIdx > prefabInfos.Length - 1) { nextIdx -= prefabInfos.Length; }

        currentText.text = DropItemManager.GetElement(prefabNumber).ToString();
        previousText.text = DropItemManager.GetElement(previousIdx).ToString();
        nextText.text = DropItemManager.GetElement(nextIdx).ToString();

        //変更後の素材量を消費量が上回らないようにチェック
        int currentElement = DropItemManager.GetElement(prefabNumber);
        if (consumption > currentElement)
        {
            consumption = currentElement;
        }

        consumptionText.text = consumption.ToString();
    }

    private void CreateFaceParts()
    {
        if(consumption <= 0)
        {
            Debug.LogWarning("Zero Consumption.");
            return;
        }

#if MOUSE_WHEEL
        if (DropItemManager.TryToUseElement(prefabNumber,consumption))
#else
        if (DropItemManager.TryToUseElementWhenCreatingFace())
#endif
        {
            //FacePartsBaseScript go = Instantiate(prefabInfos[prefabNumber].prefab, transform.position, Quaternion.identity);
            StartCoroutine(DecideFacePosition());

#if SAVE
            //セーブデータにパーツを一つ生成したことを記録
            SaveData.AchievementStep(Achievement.StepType.creator);
#endif
        }
    }

    private IEnumerator DecideFacePosition()
    {
        facePositionPreview.enabled = true;

        Vector2 previewPosition = new Vector2();
        while(!Input.GetKeyUp(createKey))
        {
            previewPosition.x = Input.GetAxis("Horizontal") * previewDistance;
            previewPosition.y = Input.GetAxis("Vertical") * previewDistance;
            facePositionPreview.gameObject.transform.localPosition = previewPosition;

            yield return null;
        }

        FacePartsBaseScript parts = Instantiate(
            prefabInfos[prefabNumber].prefab,
            facePositionPreview.gameObject.transform.position,
            Quaternion.identity
            );
        parts.Initialize(consumption);
        NonSpatialSFXPlayer.PlayNonSpatialSFX(createFaceSound, sfxMixerGroup);
        consumption = 0;
        MainScript.AddFaceObject(parts.gameObject);

        needReflesh = true;
        facePositionPreview.enabled = false;

        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyArray.Length <= 0) { yield break; }
        for (int i = 0; i < enemyArray.Length; i++)
        {
            EnemyCtrl ctrl = enemyArray[i].GetComponentInChildren<EnemyCtrl>();
            if (ctrl)
            {
                ctrl.ResetTarget();
            }
        }
    }
}

[System.Serializable]
public class PrefabInfo
{
    [Tooltip("この名前は選択中のパーツを示すために画面上に表示されます")]
    public string name;
    public FacePartsBaseScript prefab;
    public int cost;
}
