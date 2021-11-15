//#define SAVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreateFaceParts : MonoBehaviour
{

    [Header("Face Part Prefab")]
    public PrefabInfo[] prefabInfos;

    [Header("Property")]
    public KeyCode keyCode;


    /// <summary>
    /// <para>1 - Eye Prefab</para>
    /// <para>2 - Ear Prefab</para>
    /// <para>3 - Mouse Prefab</para>
    /// </summary>
    private int prefabNumber = 0;

    private float time;
    private float interval;

    private GameObject[] enemyArray;



    private void Update()
    {
        /*
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            if (Time.fixedTime > time + interval)
            {
                SwitchPrefab(Input.GetAxis("Mouse ScrollWheel"));
                time = Time.fixedTime;
            }
        }
        */

        if (Input.GetKeyDown(keyCode))
        {
            CreateFaceParts();

            enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemyArray.Length; i++)
            {
                enemyArray[i].GetComponentInChildren<EnemyCtrl>().ResetTarget();
            }
        }
    }

    private void SwitchPrefab(float a)
    {
        if (a > 0)
        {
            prefabNumber++;
            if (prefabNumber > prefabInfos.Length - 1) prefabNumber = 0;
        }
        else
        {
            prefabNumber--;
            if (prefabNumber < 0) prefabNumber = prefabInfos.Length - 1;
        }

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
