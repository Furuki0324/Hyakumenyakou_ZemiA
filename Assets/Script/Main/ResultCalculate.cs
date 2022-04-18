using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ResultCalculate : MonoBehaviour
{
    [Header("Correct Point")]
    [SerializeField] Transform leftEye;
    [SerializeField] Transform leftEar;
    [SerializeField] Transform mouth;

    [Header("Correct Amount")]
    [SerializeField] int eyeAmount = 2;
    [SerializeField] int earAmount = 2;
    [SerializeField] int mouthAmount = 1;


    [Header("Dictionary")]
    private static Dictionary<string, Transform> correctPoints = new Dictionary<string, Transform>();

    private void Awake()
    {
        correctPoints.Clear();
        correctPoints.Add("eye", leftEye);
        correctPoints.Add("ear", leftEar);
        correctPoints.Add("mouth", mouth);
    }

    private static string GetStar(int score)
    {
        if (score >= 95) return "★★★";
        else if (score >= 50) return "★★☆";
        else if (score >= 25) return "★☆☆";
        else return "☆☆☆";
    }

    public ResultData CalculateResultData(GameObject noseAsCenter)
    {
        ResultData data = new ResultData();
        //SetTextUI(data);
        //MakeTextInvisible(data);

        CalculateInfo info = new CalculateInfo(noseAsCenter, correctPoints);

        #region Calculate about eyes

        if(info.splitFace.TryGetValue("eyes",out GameObject[] eyesOnField))
        {
            if(eyesOnField.Length == 0)
            {
                data.eyeAmountScore = 0;
            }
            else if(eyesOnField.Length > eyeAmount)
            {
                data.eyeAmountScore = 100 - 5 * (eyesOnField.Length - eyeAmount);
            }
            else
            {
                data.eyeAmountScore = 100 - 15 * (eyeAmount - eyesOnField.Length);
            }
        }
        else
        {
            data.eyeAmountScore = 0;
        }

        data.leftEyeDistanceScore = 100 * info.distanceRatio.leftEye;
        data.rightEyeDistanceScore = 100 * info.distanceRatio.rightEye;

        #endregion

        #region Calculate about ears

        if(info.splitFace.TryGetValue("ears",out GameObject[] earsOnField))
        {
            if(earsOnField.Length == 0)
            {
                data.earAmountScore = 0;
            }
            else if(earsOnField.Length > earAmount)
            {
                data.earAmountScore = 100 - 5 * (earsOnField.Length - earAmount);
            }
            else if(earsOnField.Length < earAmount)
            {
                data.earAmountScore = 100 - 15 * (earAmount - earsOnField.Length);
            }
        }
        else
        {
            data.earAmountScore = 0;
        }

        data.leftEarDistanceScore = 100 * info.distanceRatio.leftEar;
        data.rightEarDistanceScore = 100 * info.distanceRatio.rightEar;
        
        #endregion

        #region Calculate about mouths

        if(info.splitFace.TryGetValue("mouths",out GameObject[] mouthsOnField))
        {
            if(mouthsOnField.Length == 0)
            {
                data.mouthAmountScore = 0;
            }
            else if(mouthsOnField.Length > mouthAmount)
            {
                data.mouthAmountScore = 100 - 5 * (mouthsOnField.Length - mouthAmount);
            } 
            else if(mouthsOnField.Length < mouthAmount)
            {
                data.mouthAmountScore = 100 - 15 * (mouthAmount - mouthsOnField.Length);
            }
        }
        else
        {
            data.mouthAmountScore = 0;
        }

        if (info.distanceRatio.mouth > 1) data.mouthDistanceScore = 100 * (1 - (info.distanceRatio.mouth - 1));
        else data.mouthDistanceScore = 100 * info.distanceRatio.mouth;

        #endregion

        #region Sum up

        //目と耳はそれぞれ左右の距離得点の平均を求める
        int meanOfEyeDistance = Mathf.RoundToInt((data.leftEyeDistanceScore + data.rightEyeDistanceScore) / 2);
        int meanOfEarDistance = Mathf.RoundToInt((data.leftEarDistanceScore + data.rightEarDistanceScore) / 2);

        data.eyeSumScore = Mathf.RoundToInt((data.eyeAmountScore + meanOfEyeDistance) / 2);
        data.earSumScore = Mathf.RoundToInt((data.earAmountScore + meanOfEarDistance) / 2);
        data.mouthSumScore = Mathf.RoundToInt((data.mouthAmountScore + data.mouthDistanceScore) / 2);

        //スコアを利用して獲得する★の量を決定
        data.eyeStar_A = GetStar(data.eyeAmountScore);
        data.eyeStar_P = GetStar(meanOfEyeDistance);
        data.earStar_A = GetStar(data.earAmountScore);
        data.earStar_P = GetStar(meanOfEarDistance);
        data.mouthStar_A = GetStar(data.mouthAmountScore);
        data.mouthStar_P = GetStar(Mathf.RoundToInt(data.mouthDistanceScore));

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
        //フィールド上に配置されているパーツ群(splitedFace)と正解位置群(correctPoints)から目の要素を抜き取る
        if(splitedFace.TryGetValue("eyes",out GameObject[] eyeValue) && correctPoints.TryGetValue("eye", out Transform eyeCorrectPoint))
        {
            GameObject tempLeft = null, tempRight = null;
            //採点時に中心となる鼻と正解位置との距離を求める
            float correctDistance = Vector2.Distance(eyeCorrectPoint.position, noseAsCenter.transform.position);

            foreach(GameObject g in eyeValue)
            {
                //採点対象となるパーツを探しだす
                if(tempLeft == null || tempRight == null)
                {
                    if(g.transform.position.x <= 0)
                    {
                        if(tempLeft == null)
                        {
                            tempLeft = g;
                            continue;
                        } else if(tempRight == null)
                        {
                            tempRight = g;
                            continue;
                        }
                    }
                    else
                    {
                        if(tempRight == null)
                        {
                            tempRight = g;
                            continue;
                        } else if(tempLeft == null)
                        {
                            tempLeft = g;
                            continue;
                        }
                    }
                }

                //より正解位置に近いパーツが見つかった場合は採点対象をそちらに変更する
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
                leftDistance = Vector2.Distance(tempLeft.transform.position, eyeCorrectPoint.transform.position);
            }

            if (tempRight)
            {
                rightDistance = Vector2.Distance(tempRight.transform.position, new Vector2(-eyeCorrectPoint.position.x, eyeCorrectPoint.position.y));
            }


            if (leftDistance != 0)
            {
                leftEye = 1 - leftDistance / correctDistance;
            }
            else
            {
                leftEye = 0;
            }

            if (rightDistance != 0)
            {
                rightEye = 1 - rightDistance / correctDistance;
            }
            else
            {
                rightEye = 0;
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

        if (splitedFace.TryGetValue("ears", out GameObject[] earValue) && correctPoints.TryGetValue("ear", out Transform earCorrectPoint)) 
        {
            GameObject tempLeft = null, tempRight = null;
            float correctDistance = Vector2.Distance(earCorrectPoint.position, noseAsCenter.transform.position);

            foreach(GameObject g in earValue)
            {
                if(tempLeft == null || tempRight == null)
                {
                    if(g.transform.position.x <= 0)
                    {
                        if(tempLeft == null)
                        {
                            tempLeft = g;
                            Debug.Log("Left ear initialized.");
                            continue;
                        } else if(tempRight == null)
                        {
                            tempRight = g;
                            Debug.Log("Right ear initialized.");
                            continue;
                        }
                    }
                    else
                    {
                        if(tempRight == null)
                        {
                            tempRight = g;
                            Debug.Log("Right ear initialized.");
                            continue;
                        } else if(tempLeft == null)
                        {
                            tempLeft = g;
                            Debug.Log("Left ear initialized.");
                            continue;
                        }
                    }
                }


                if(g.transform.position.x <= 0 && tempLeft)
                {
                    if(Vector2.Distance(tempLeft.transform.position,earCorrectPoint.position) > Vector2.Distance(g.transform.position, earCorrectPoint.position))
                    {
                        tempLeft = g;
                        Debug.Log("Left ear switched.");
                    }
                }
                else if(tempRight)
                {
                    if(Vector2.Distance(tempRight.transform.position,new Vector2(-earCorrectPoint.position.x,earCorrectPoint.position.y)) > Vector2.Distance(g.transform.position,new Vector2(-earCorrectPoint.position.x, earCorrectPoint.position.y)))
                    {
                        tempRight = g;
                        Debug.Log("Right ear switched.");
                    }
                }
            }

            float leftDistance = 0;
            float rightDistance = 0;

            if(tempLeft) leftDistance = Vector2.Distance(tempLeft.transform.position, earCorrectPoint.transform.position);
            if(tempRight) rightDistance = Vector2.Distance(tempRight.transform.position, new Vector2(-earCorrectPoint.position.x, earCorrectPoint.position.y));


            if (leftDistance != 0)
                leftEar = 1 - leftDistance / correctDistance;
            else 
                leftEar = 0;

            if (rightDistance != 0)
                rightEar = 1 - rightDistance / correctDistance;
            else
                rightEar = 0;

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

            if(temp) distance = Vector2.Distance(temp.transform.position, mouthCorrectPoint.transform.position);

            if (distance != 0)
                mouth = 1 - distance / correctDistance;
            else
                mouth = 0;
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

    public CalculateInfo(GameObject noseAsCenter, Dictionary<string,Transform> correctPoints)
    {
        GetEachObjectOnField();
        distanceRatio = new DistanceRatioData(splitFace, correctPoints, noseAsCenter);
    }

    private void GetEachObjectOnField()
    {
        
        GameObject[] eyesOnField = GameObject.FindGameObjectsWithTag("Face_Eye");
        GameObject[] earsOnField = GameObject.FindGameObjectsWithTag("Face_Ear");
        GameObject[] mouthsOnField = GameObject.FindGameObjectsWithTag("Face_Mouth");

        splitFace.Add("eyes", eyesOnField);
        splitFace.Add("ears", earsOnField);
        splitFace.Add("mouths", mouthsOnField);

    }
}

public class ResultData
{
    //総合点
    public int totalScore = 0;

    //目の各種スコア
    public int eyeSumScore = 0;
    public int eyeAmountScore = 100;
    public float leftEyeDistanceScore = 100;
    public float rightEyeDistanceScore = 100;
    public string eyeStar_A = "";
    public string eyeStar_P = "";

    //耳の各種スコア
    public int earSumScore = 0;
    public int earAmountScore = 100;
    public float leftEarDistanceScore = 100;
    public float rightEarDistanceScore = 100;
    public string earStar_A = "";
    public string earStar_P = "";

    //口の各種スコア
    public int mouthSumScore = 0;
    public int mouthAmountScore = 100;
    public float mouthDistanceScore = 100;
    public string mouthStar_A = "";
    public string mouthStar_P = "";
}