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

    [Header("Correct Amount")]
    public int eyeAmount = 2;
    private static int _eyeAmount;
    public int earAmount = 2;
    private static int _earAmount;
    public int mouthAmount = 1;
    private static int _mouthAmount;

    [Header("Dictionary")]
    private static Dictionary<string, Transform> correctPoints = new Dictionary<string, Transform>();

    private void Start()
    {
        _correctEye = leftEye;
        _correctEar = leftEar;
        _correctMouth = mouth;

        correctPoints.Add("eye", leftEye);
        correctPoints.Add("ear", leftEar);
        correctPoints.Add("mouth", mouth);

        _eyeAmount = eyeAmount;
        _earAmount = earAmount;
        _mouthAmount = mouthAmount;
    }

    public static ResultData CalculateResultData(List<GameObject> faceObjects, GameObject noseAsCenter)
    {
        ResultData data = new ResultData();
        CalculateInfo info = new CalculateInfo(faceObjects, noseAsCenter, correctPoints);
        GameObject[] eyesOnField, earsOnField, mouthsOnField;


        #region Calculate about eyes

        if(info.splitFace.TryGetValue("eyes",out eyesOnField))
        {
            if(eyesOnField.Length > _eyeAmount)
            {
                data.eyeAmountScore -= 5 * (eyesOnField.Length - _eyeAmount);
            }
            else
            {
                data.eyeAmountScore -= 15 * (_eyeAmount - eyesOnField.Length);
            }
        }

        #endregion

        #region Calculate about ears
        if(info.splitFace.TryGetValue("ears",out earsOnField))
        {

        }

        #endregion

        #region Calculate about mouths
        if(info.splitFace.TryGetValue("mouths",out mouthsOnField))
        {

        }

        #endregion

        #region Sum up


        #endregion

        return data;
    }
}

class DistanceRatioData
{
    public float leftEye;
    public float rightEye;

    public float leftEar;
    public float rightEar;

    public float mouth;

    public DistanceRatioData(Dictionary<string,GameObject[]> splitedFace, Dictionary<string,Transform> correctPoints, GameObject noseAsCenter)
    {

    }

    private void CalculateEachDistanceRatio(Dictionary<string,GameObject[]> splitedFace, Dictionary<string,Transform> correctPoints, GameObject noseAsCenter)
    {
        #region Eye
        GameObject tempLeft = null, tempRight = null;
        GameObject[] value;
        Transform correctPoint;

        //Left eye
        if(splitedFace.TryGetValue("eyes",out value) && correctPoints.TryGetValue("eye", out correctPoint))
        {
            foreach(GameObject g in value)
            {
                if(tempLeft == null  && g.transform.position.x <= 0)
                {
                    tempLeft = g;
                    continue;
                } 
                else if(tempRight == null && g.transform.position.x > 0)
                {
                    tempRight = g;
                    continue;
                }

                if(g.transform.position.x <= 0)
                {
                    if(Vector2.Distance(tempLeft.transform.position,correctPoint.position) > Vector2.Distance(g.transform.position, correctPoint.position))
                    {
                        tempLeft = g;
                    }
                }
                else
                {
                    if (Vector2.Distance(tempRight.transform.position, new Vector2(-correctPoint.position.x,correctPoint.position.y)) > Vector2.Distance(g.transform.position, new Vector2(-correctPoint.position.x, correctPoint.position.y)))
                    {
                        tempRight = g;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No eyes found!");
            leftEye = 0;
            rightEye = 0;
        }

        #endregion

        #region Ear
        if(splitedFace.TryGetValue("ears",out value))
        {

        }
        else
        {
            Debug.LogWarning("No ears found!");
            leftEar = 0;
            rightEar = 0;
        }

        #endregion

        #region Mouth
        if(splitedFace.TryGetValue("mouths",out value))
        {

        }
        else
        {
            Debug.LogWarning("No mouths found!");
            mouth = 0;
        }

        #endregion
    }
}

class CalculateInfo
{
    public Dictionary<string, GameObject[]> splitFace = new Dictionary<string, GameObject[]>();
    public DistanceRatioData distanceRaio;

    public CalculateInfo(List<GameObject> faceObjects, GameObject noseAsCenter, Dictionary<string,Transform> correctPoints)
    {
        GetEachObjectOnField(faceObjects);
        distanceRaio = new DistanceRatioData(splitFace, correctPoints, noseAsCenter);
    }

    private void GetEachObjectOnField(List<GameObject> faceObjects)
    {
        GameObject[] eyesOnField = FindGameObjectsWithTagInList(faceObjects, "Face_Eye");
        GameObject[] earsOnField = FindGameObjectsWithTagInList(faceObjects, "Face_Ear");
        GameObject[] mouthsOnField = FindGameObjectsWithTagInList(faceObjects, "Face_Mouth");

        splitFace.Add("eyes", eyesOnField);
        splitFace.Add("ears", earsOnField);
        splitFace.Add("mouths", mouthsOnField);

    }

    private GameObject[] FindGameObjectsWithTagInList(List<GameObject> list, string tag)
    {
        List<GameObject> a = new List<GameObject>();

        foreach (GameObject g in list)
        {
            if (g.tag == tag) a.Add(g);
        }

        return a.ToArray();
    }
}

public class ResultData
{
    public int totalScore = 0;

    public int eyeSumScore = 0;
    public int eyeAmountScore = 100;
    public int eyeDistanceScore = 100;

    public int earSumScore = 0;
    public int earAmountScore = 100;
    public int earDistanceScore = 100;

    public int mouthSumScore = 0;
    public int mouthAmountScore = 100;
    public int mouthDistanceScore = 100;
}