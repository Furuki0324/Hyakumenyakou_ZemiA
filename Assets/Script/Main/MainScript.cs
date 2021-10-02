using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{

    //------------------------Public-----------------------
    public int defaultElementAmount;

    //----------------------Private-----------------------
    private static List<GameObject> faceObjects = new List<GameObject>();
    private enum tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }


    private void Start()
    {
        DropItemManager.ObtainItem("EyeElement", defaultElementAmount);
        DropItemManager.ObtainItem("EarElement", defaultElementAmount);
        DropItemManager.ObtainItem("MouthElement", defaultElementAmount);

        //開始時点で配置されているパーツを追加
        foreach (string i in Enum.GetNames(typeof(tags)))
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag(i);
            foreach (GameObject j in temp) if (j.layer != 5) AddFaceObject(j); ;
        }
    }

    //ここから顔パーツの情報

    /// <summary>
    /// <para>対象をリストに追加します</para>
    /// </summary>
    /// <param name="gameObject">リストに追加される対象</param>
    public static void AddFaceObject(GameObject gameObject)
    {
        faceObjects.Add(gameObject);
    }

    /// <summary>
    /// <para>対象をリストから削除します</para>
    /// </summary>
    /// <param name="gameObject">リストから削除される対象</param>
    public static void RemoveFaceObject(GameObject gameObject)
    {
        faceObjects.Remove(gameObject);
    }

    /// <summary>
    /// <para>リストに登録されている顔パーツのTransformを配列で返します。</para>
    /// </summary>
    /// <returns></returns>
    public static Transform[] GetFaceObjectTransformsInArray()
    {
        List<Transform> transforms = new List<Transform>();

        for (int i = 0; i < faceObjects.Count; i++)
        {
            transforms[i] = faceObjects[i].transform;
        }

        return transforms.ToArray();
    }

    /// <summary>
    /// <para>顔パーツリストに登録されているゲームオブジェクトのTransformをリストで返します。</para>
    /// </summary>
    /// <returns></returns>
    public static List<Transform> GetFaceObjectTransformInList()
    {
        List<Transform> transforms = new List<Transform>();

        for (int i = 0; i < faceObjects.Count; i++)
        {
            transforms.Add(faceObjects[i].transform);
        }

        return transforms;
    }

    //ここまで顔パーツの情報

    public static void GameOver()
    {
        Debug.Log("Game Over");
    }

    public static void GameClear()
    {
        int score = ResultCalculate.CalculateScore(faceObjects, GameObject.FindWithTag("Face_Nose"));
        Debug.Log("Game Clear! Your score: " + score);
    }
}
