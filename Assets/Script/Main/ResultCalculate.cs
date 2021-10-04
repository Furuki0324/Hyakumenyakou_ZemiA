using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCalculate : MonoBehaviour
{
    [Header("Correct Point")]
    public Transform leftEye;
    public Transform leftEar;
    public Transform mouth;

    [Header("Correct Amount")]
    public int eyeAmount = 2;
    private static int _eyeAmount;
    public int earAmount = 2;
    private static int _earAmount;
    public int mouthAmount = 1;
    private static int _mouthAmount;

    [Header("Dictionary")]
    private static Dictionary<string, Transform> correctPoints = new Dictionary<string, Transform>();

    private void Awake()
    {
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


        #region Calculate about eyes

        if(info.splitFace.TryGetValue("eyes",out GameObject[] eyesOnField))
        {
            if(eyesOnField.Length == 0)
            {
                data.eyeAmountScore = 0;
            }
            else if(eyesOnField.Length > _eyeAmount)
            {
                data.eyeAmountScore -= 5 * (eyesOnField.Length - _eyeAmount);
            }
            else
            {
                data.eyeAmountScore -= 15 * (_eyeAmount - eyesOnField.Length);
            }
        }
        else
        {
            data.eyeAmountScore = 0;
        }

        data.leftEyeDistanceScore *= info.distanceRatio.leftEye;
        data.rightEyeDistanceScore *= info.distanceRatio.rightEye;

        

        #endregion

        #region Calculate about ears

        if(info.splitFace.TryGetValue("ears",out GameObject[] earsOnField))
        {
            if(earsOnField.Length == 0)
            {
                data.earAmountScore = 0;
            }
            else if(earsOnField.Length > _earAmount)
            {
                data.earAmountScore -= 5 * (earsOnField.Length - _earAmount);
            }
            else if(earsOnField.Length < _earAmount)
            {
                data.earAmountScore -= 15 * (_earAmount - earsOnField.Length);
            }
        }
        else
        {
            data.earAmountScore = 0;
        }

        data.leftEarDistanceScore *= info.distanceRatio.leftEar;
        data.rightEarDistanceScore *= info.distanceRatio.rightEar;

        
        #endregion

        #region Calculate about mouths

        if(info.splitFace.TryGetValue("mouths",out GameObject[] mouthsOnField))
        {
            if(mouthsOnField.Length == 0)
            {
                data.mouthAmountScore = 0;
            }
            else if(mouthsOnField.Length > _mouthAmount)
            {
                data.mouthAmountScore -= 5 * (mouthsOnField.Length - _mouthAmount);
            } 
            else if(mouthsOnField.Length < _mouthAmount)
            {
                data.mouthAmountScore -= 15 * (_mouthAmount - mouthsOnField.Length);
            }
        }
        else
        {
            data.mouthAmountScore = 0;
        }

        if (info.distanceRatio.mouth > 1) data.mouthDistanceScore *= (1 - (info.distanceRatio.mouth - 1));
        else data.mouthDistanceScore *= info.distanceRatio.mouth;

        #endregion

        #region Sum up

        data.eyeSumScore = Mathf.RoundToInt((data.eyeAmountScore + Mathf.RoundToInt((data.leftEyeDistanceScore + data.rightEyeDistanceScore)) / 2) / 2);
        data.earSumScore = Mathf.RoundToInt((data.earAmountScore + Mathf.RoundToInt((data.leftEarDistanceScore + data.rightEarDistanceScore)) / 2) / 2);
        data.mouthSumScore = Mathf.RoundToInt((data.mouthAmountScore + data.mouthDistanceScore) / 2);

        data.totalScore = data.eyeSumScore + data.earSumScore + data.mouthSumScore;

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
        CalculateEachDistanceRatio(splitedFace, correctPoints, noseAsCenter);
    }

    private void CalculateEachDistanceRatio(Dictionary<string,GameObject[]> splitedFace, Dictionary<string,Transform> correctPoints, GameObject noseAsCenter)
    {
        #region Eye
        
        if(splitedFace.TryGetValue("eyes",out GameObject[] eyeValue) && correctPoints.TryGetValue("eye", out Transform eyeCorrectPoint))
        {
            GameObject tempLeft = null, tempRight = null;
            float correctDistance = Vector2.Distance(eyeCorrectPoint.position, noseAsCenter.transform.position);

            foreach(GameObject g in eyeValue)
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
                    if(Vector2.Distance(tempLeft.transform.position,eyeCorrectPoint.position) > Vector2.Distance(g.transform.position, eyeCorrectPoint.position))
                    {
                        tempLeft = g;
                    }
                }
                else
                {
                    if (Vector2.Distance(tempRight.transform.position, new Vector2(-eyeCorrectPoint.position.x,eyeCorrectPoint.position.y)) > Vector2.Distance(g.transform.position, new Vector2(-eyeCorrectPoint.position.x, eyeCorrectPoint.position.y)))
                    {
                        tempRight = g;
                    }
                }
            }

            float leftDistance = 0;
            float rightDistance = 0;
            if (tempLeft) 
            { 
                leftDistance = Vector2.Distance(tempLeft.transform.position, noseAsCenter.transform.position);
            }
            
            if(tempRight) rightDistance = Vector2.Distance(tempRight.transform.position, noseAsCenter.transform.position);
            

            leftEye = leftDistance / correctDistance;
            rightEye = rightDistance / correctDistance;
        }
        else
        {
            Debug.LogWarning("No eyes found!");
            leftEye = 0;
            rightEye = 0;
        }

        #endregion

        #region Ear

        if (splitedFace.TryGetValue("ears", out GameObject[] earValue) && correctPoints.TryGetValue("ear", out Transform earCorrectPoint)) 
        {
            GameObject tempLeft = null, tempRight = null;
            float correctDistance = Vector2.Distance(earCorrectPoint.position, noseAsCenter.transform.position);

            foreach(GameObject g in earValue)
            {
                if (tempLeft == null && g.transform.position.x < 0)
                {
                    tempLeft = g;
                    continue;
                }
                else if(tempRight == null && g.transform.position.x> 0)
                {
                    tempRight = g;
                    continue;
                }

                if(g.transform.position.x <= 0)
                {
                    if(Vector2.Distance(tempLeft.transform.position,earCorrectPoint.position) > Vector2.Distance(g.transform.position, earCorrectPoint.position))
                    {
                        tempLeft = g;
                    }
                }
                else
                {
                    if(Vector2.Distance(tempRight.transform.position,new Vector2(-earCorrectPoint.position.x,earCorrectPoint.position.y)) > Vector2.Distance(g.transform.position,new Vector2(-earCorrectPoint.position.x, earCorrectPoint.position.y)))
                    {
                        tempRight = g;
                    }
                }
            }

            float leftDistance = 0;
            float rightDistance = 0;

            if(tempLeft) leftDistance = Vector2.Distance(tempLeft.transform.position, noseAsCenter.transform.position);
            if(tempRight) rightDistance = Vector2.Distance(tempRight.transform.position, noseAsCenter.transform.position);

            leftEar = leftDistance / correctDistance;
            rightEar = rightDistance / correctDistance;
        }
        else
        {
            Debug.LogWarning("No ears found!");
            leftEar = 0;
            rightEar = 0;
        }

        #endregion

        #region Mouth

        if(splitedFace.TryGetValue("mouths",out GameObject[] mouthValue) && correctPoints.TryGetValue("mouth",out Transform mouthCorrectPoint))
        {
            GameObject temp = null;
            float correctDistance = Vector2.Distance(mouthCorrectPoint.position, noseAsCenter.transform.position);

            foreach(GameObject g in mouthValue)
            {
                if(temp == null)
                {
                    temp = g;
                    continue;
                }

                if(Vector2.Distance(temp.transform.position,mouthCorrectPoint.position) > Vector2.Distance(g.transform.position, mouthCorrectPoint.position))
                {
                    temp = g;
                }
            }

            float distance = 0;

            if(temp) distance = Vector2.Distance(temp.transform.position, noseAsCenter.transform.position);

            mouth = distance / correctDistance;
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
    public DistanceRatioData distanceRatio;

    public CalculateInfo(List<GameObject> faceObjects, GameObject noseAsCenter, Dictionary<string,Transform> correctPoints)
    {
        GetEachObjectOnField(faceObjects);
        distanceRatio = new DistanceRatioData(splitFace, correctPoints, noseAsCenter);
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
    public float leftEyeDistanceScore = 100;
    public float rightEyeDistanceScore = 100;

    public int earSumScore = 0;
    public int earAmountScore = 100;
    public float leftEarDistanceScore = 100;
    public float rightEarDistanceScore = 100;

    public int mouthSumScore = 0;
    public int mouthAmountScore = 100;
    public float mouthDistanceScore = 100;
}