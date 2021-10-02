using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCalculate : MonoBehaviour
{
    [Header("Correct Point")]
    public Transform leftEye;
    private static Transform _correctEye;
    public Transform leftEar;
    private static Transform _correctEar;
    public Transform mouth;
    private static Transform _correctMouth;

    private void Start()
    {
        _correctEye = leftEye;
        _correctEar = leftEar;
        _correctMouth = mouth;
    }

    public static int CalculateScore(List<GameObject> faceObjects, GameObject noseAsCenter)
    {
        int score = 0;

        GameObject[] eyesOnField = FindGameObjectsWithTagInList(faceObjects, "Face_Eye");
        GameObject[] earsOnField = FindGameObjectsWithTagInList(faceObjects, "Face_Ear");
        GameObject[] mouthsOnField = FindGameObjectsWithTagInList(faceObjects, "Face_Mouth");
        
        //About Eye
        if(eyesOnField.Length == 2)
        {
            score += 10;
        }


        //About Ear
        if(earsOnField.Length == 2)
        {
            score += 10;
        }


        //About Mouth
        if(mouthsOnField.Length == 1)
        {
            score += 10;
        }


        return score;
    }

    private static GameObject[] FindGameObjectsWithTagInList(List<GameObject> list, string tag)
    {
        List<GameObject> a = new List<GameObject>();

        foreach(GameObject g in list)
        {
            if (g.tag == tag) a.Add(g);
        }

        return a.ToArray();
    }

    private static float CalculateDistanceRatio(GameObject[] faceCategory, GameObject center, bool reverse = false)
    {
        float correctDistance = 0;
       

        switch (faceCategory.ToString())
        {
            case "eyesOnField":
                correctDistance = Vector2.Distance(_correctEye.position, center.transform.position);

                foreach(GameObject g in faceCategory)
                {

                }
                break;

            case "earsOnField":
                correctDistance = Vector2.Distance(_correctEar.position, center.transform.position);
                break;

            case "mouthsOnField":
                correctDistance = Vector2.Distance(_correctMouth.position, center.transform.position);

                foreach(GameObject g in faceCategory)
                {

                }
                break;
        }

        return correctDistance;
    }
}
