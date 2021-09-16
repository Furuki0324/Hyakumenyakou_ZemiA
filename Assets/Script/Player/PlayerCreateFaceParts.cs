using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCreateFaceParts : MonoBehaviour
{

    [Header("Face Part Prefab")]
    public PrefabInfo[] prefabInfos;

    [Header("Property")]
    [Tooltip("マウスホイールが入力されてからどれほどの間隔を空けて次の入力を受け付けるのか決定します(単位：秒)。")]
    public float interval;
    public KeyCode keyCode;

    [Header("Indicator UI")]
    public Text prefabIndicator;

    /// <summary>
    /// <para>1 - Eye Prefab</para>
    /// <para>2 - Ear Prefab</para>
    /// <para>3 - Mouse Prefab</para>
    /// </summary>
    private int prefabNumber = 0;

    private float time;

    private EnemyCtrl enemyCtrl;
    private GameObject[] enemyArray;


    // Start is called before the first frame update
    void Start()
    {
        prefabIndicator.text = prefabInfos[prefabNumber].name;
        
    }

    private void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            if (Time.fixedTime > time + interval)
            {
                SwitchPrefab(Input.GetAxis("Mouse ScrollWheel"));
                time = Time.fixedTime;
            }
        }

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

        prefabIndicator.text = prefabInfos[prefabNumber].name;
        Debug.Log(prefabNumber);
    }

    private void CreateFaceParts()
    {
        //FacePartsBaseScript go = Instantiate(prefabInfos[prefabNumber].prefab, transform.position, Quaternion.identity);
        FacePartsBaseScript go = prefabInfos[prefabNumber].prefab;
        if (DropItemManager.CanUseElements(go.gameObject.tag, prefabInfos[prefabNumber].cost))
        {
            Instantiate(go, transform.position, Quaternion.identity);
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
