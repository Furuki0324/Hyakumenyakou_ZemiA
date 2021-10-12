using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScript : MonoBehaviour
{

    //------------------------Public-----------------------
    public int defaultElementAmount;

    //----------------------Private-----------------------
    private static List<GameObject> faceObjects = new List<GameObject>();
    private enum Tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }
    private static TextMeshProUGUI tmp;

    private void Start()
    {
        DropItemManager.ObtainItem("EyeElement", defaultElementAmount, true);
        DropItemManager.ObtainItem("EarElement", defaultElementAmount, true);
        DropItemManager.ObtainItem("MouthElement", defaultElementAmount, true);

        //開始時点で配置されているパーツを追加
        foreach (string i in Enum.GetNames(typeof(Tags)))
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag(i);
            foreach (GameObject j in temp) if (j.layer != 5) AddFaceObject(j); ;
        }

        tmp = GameObject.FindWithTag("Finish").GetComponent<TextMeshProUGUI>();
        tmp.text = "ono";
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
        ResultData data = ResultCalculate.CalculateResultData(faceObjects, GameObject.FindWithTag("Face_Nose"));
        Debug.Log("Game Clear! Your score: " + data.totalScore
             + "\nAmountScore: \n" + "Eye: " + data.eyeAmountScore
             + "\nEar: " + data.earAmountScore
             + "\nMouth: " + data.mouthAmountScore
             + "\n\nDistanceScore: \n" + "LeftEye: " + data.leftEyeDistanceScore
             + "\nRightEye: " + data.rightEyeDistanceScore
             + "\nLeftEar: " + data.leftEarDistanceScore
             + "\nRightEar: " + data.rightEarDistanceScore
             + "\nMouth: " + data.mouthDistanceScore
             + "\n\nSectionScore:\n" + "Eye: " + data.eyeSumScore
             + "\nEar: " + data.earSumScore
             + "\nMouth: " + data.mouthSumScore);


    }

    private void Update()
    {
        ResultData data = ResultCalculate.CalculateResultData(faceObjects, GameObject.FindWithTag("Face_Nose"));

        tmp.text = "Game Clear! Your score: " + data.totalScore
                + "\nAmountScore: \n" + "Eye: " + data.eyeAmountScore
                + "\nEar: " + data.earAmountScore
                + "\nMouth: " + data.mouthAmountScore
                + "\n\nDistanceScore: \n" + "LeftEye: " + data.leftEyeDistanceScore
                + "\nRightEye: " + data.rightEyeDistanceScore
                + "\nLeftEar: " + data.leftEarDistanceScore
                + "\nRightEar: " + data.rightEarDistanceScore
                + "\nMouth: " + data.mouthDistanceScore
                + "\n\nSectionScore:\n" + "Eye: " + data.eyeSumScore
                + "\nEar: " + data.earSumScore
                + "\nMouth: " + data.mouthSumScore;
    }
}
