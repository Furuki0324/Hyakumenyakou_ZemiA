//#define SAVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreateFaceParts : MonoBehaviour
{

    [Header("Face Part Prefab")]
    public PrefabInfo[] prefabInfos;

    [Header("KeyCode")]
    [SerializeField] KeyCode createKey;
    [SerializeField] KeyCode switchKey;

    [SerializeField] Image image;

    /// <summary>
    /// <para>1 - Eye Prefab</para>
    /// <para>2 - Ear Prefab</para>
    /// <para>3 - Mouse Prefab</para>
    /// </summary>
    private int prefabNumber = 0;

    private float consumption;

    private GameObject[] enemyArray;

    private void Start()
    {
        consumption = 0;
        if (!image)
        {
            Debug.LogError("No image object is set to player.");
        }
        image.sprite = prefabInfos[0].prefab.sprites[0];
    }

    private void Update()
    {
        ImageFollowPlayer();
        if(Input.GetKeyDown(switchKey)) { SwitchPrefab(); }
        Consumption();

        if (Input.GetKeyDown(createKey))
        {
            CreateFaceParts();

            enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemyArray.Length; i++)
            {
                enemyArray[i].GetComponentInChildren<EnemyCtrl>().ResetTarget();
            }
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
        }
        else if(mouseWheel < 0)
        {
            consumption--;
        }
    }

    private void SwitchPrefab()
    {
        prefabNumber++;

        if(prefabNumber > prefabInfos.Length - 1) { prefabNumber = 0; }

        Debug.Log($"Switched to {prefabNumber}");

        image.sprite = prefabInfos[prefabNumber].prefab.sprites[0];
    }

    private void ImageFollowPlayer()
    {
        if (!image)
        {
            Debug.LogError("No image was found.");
        }

        image.transform.position = new Vector2(transform.position.x,transform.position.y) + new Vector2(-1, 1);
    }

    private void CreateFaceParts()
    {
        if (DropItemManager.TryToUseElementWhenCreatingFace())
        {
            FacePartsBaseScript go = Instantiate(prefabInfos[DropItemManager.GetSelectedItem()].prefab, transform.position, Quaternion.identity);
            MainScript.AddFaceObject(go.gameObject);

#if SAVE
            //セーブデータにパーツを一つ生成したことを記録
            SaveData.AchievementStep(Achievement.StepType.creator);
#endif
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
