using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ResultCalculate : MonoBehaviour
{
    /*****************************************************************
    *         このスクリプト内ではstatic変数に「_」を付けています。      *
    *         private変数の印ではありません                            *
    ******************************************************************/

    [Header("Correct Point")]
    [SerializeField] Transform leftEye;
    [SerializeField] Transform leftEar;
    [SerializeField] Transform mouth;

    [Header("Correct Amount")]
    [SerializeField] int eyeAmount = 2;
    private static int _eyeAmount;
    [SerializeField] int earAmount = 2;
    private static int _earAmount;
    [SerializeField] int mouthAmount = 1;
    private static int _mouthAmount;

    [Header("UI")]
    [SerializeField] private Text T_totalScore;
    [SerializeField] private Text T_eye;
    [SerializeField] private Text T_ear;
    [SerializeField] private Text T_mouth;
    #region Static UI
    private static Text _T_totalScore, _T_eyeSum, _T_eyeStar_A, _T_eyeStar_P,
        _T_earSum, _T_earStar_A, _T_earStar_P,
        _T_mouthSum, _T_mouthStar_A, _T_mouthStar_P;
    #endregion


    [Header("Dictionary")]
    private static Dictionary<string, Transform> correctPoints = new Dictionary<string, Transform>();




    private void Awake()
    {
        correctPoints.Clear();
        correctPoints.Add("eye", leftEye);
        correctPoints.Add("ear", leftEar);
        correctPoints.Add("mouth", mouth);

        GetStaticUI();

        _eyeAmount = eyeAmount;
        _earAmount = earAmount;
        _mouthAmount = mouthAmount;
    }

    private static string GetStar(int score)
    {
        if (score >= 95) return "★★★";
        else if (score >= 50) return "★★☆";
        else if (score >= 25) return "★☆☆";
        else return "☆☆☆";
    }

    private void GetStaticUI()
    {
        _T_totalScore = T_totalScore.transform.GetChild(0).GetComponent<Text>();

        _T_eyeSum = T_eye.transform.GetChild(0).GetComponent<Text>();
        _T_eyeStar_A = T_eye.transform.GetChild(1).GetComponent<Text>();
        _T_eyeStar_P = T_eye.transform.GetChild(2).GetComponent<Text>();

        _T_earSum = T_ear.transform.GetChild(0).GetComponent<Text>();
        _T_earStar_A = T_ear.transform.GetChild(1).GetComponent<Text>();
        _T_earStar_P = T_ear.transform.GetChild(2).GetComponent<Text>();

        _T_mouthSum = T_mouth.transform.GetChild(0).GetComponent<Text>();
        _T_mouthStar_A = T_mouth.transform.GetChild(1).GetComponent<Text>();
        _T_mouthStar_P = T_mouth.transform.GetChild(2).GetComponent<Text>();
    }

    private static void SetTextUI(ResultData data)
    {
        data.T_totalScore = _T_totalScore;

        data.T_eyeSumScore = _T_eyeSum;
        data.T_eyeStar_A = _T_eyeStar_A;
        data.T_eyeStar_P = _T_eyeStar_P;

        data.T_earSumScore = _T_earSum;
        data.T_earStar_A = _T_earStar_A;
        data.T_earStar_P = _T_earStar_P;

        data.T_mouthSumScore = _T_mouthSum;
        data.T_mouthStar_A = _T_mouthStar_A;
        data.T_mouthStar_P = _T_mouthStar_P;
    }


    private static void MakeTextInvisible(ResultData data)
    {
        data.T_totalScore.color = new Color(1, 1, 1, 0);

        data.T_eyeSumScore.color = new Color(1, 1, 1, 0);
        data.T_eyeStar_A.color = new Color(1, 1, 1, 0);
        data.T_eyeStar_P.color = new Color(1, 1, 1, 0);

        data.T_earSumScore.color = new Color(1, 1, 1, 0);
        data.T_earStar_A.color = new Color(1, 1, 1, 0);
        data.T_earStar_P.color = new Color(1, 1, 1, 0);

        data.T_mouthSumScore.color = new Color(1, 1, 1, 0);
        data.T_mouthStar_A.color = new Color(1, 1, 1, 0);
        data.T_mouthStar_P.color = new Color(1, 1, 1, 0);
    }



    public static ResultData CalculateResultData(GameObject noseAsCenter)
    {
        ResultData data = new ResultData();
        SetTextUI(data);
        MakeTextInvisible(data);

        CalculateInfo info = new CalculateInfo(noseAsCenter, correctPoints);


        #region Calculate about eyes

        if(info.splitFace.TryGetValue("eyes",out GameObject[] eyesOnField))
        {
            if(eyesOnField.Length == 0)
            {
                data.eyeAmountScore = 0;
            }
            else if(eyesOnField.Length > _eyeAmount)
            {
                data.eyeAmountScore = 100 - 5 * (eyesOnField.Length - _eyeAmount);
            }
            else
            {
                data.eyeAmountScore = 100 - 15 * (_eyeAmount - eyesOnField.Length);
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
            else if(earsOnField.Length > _earAmount)
            {
                data.earAmountScore = 100 - 5 * (earsOnField.Length - _earAmount);
            }
            else if(earsOnField.Length < _earAmount)
            {
                data.earAmountScore = 100 - 15 * (_earAmount - earsOnField.Length);
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
            else if(mouthsOnField.Length > _mouthAmount)
            {
                data.mouthAmountScore = 100 - 5 * (mouthsOnField.Length - _mouthAmount);
            } 
            else if(mouthsOnField.Length < _mouthAmount)
            {
                data.mouthAmountScore = 100 - 15 * (_mouthAmount - mouthsOnField.Length);
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

        //UIのテキストを変更
        data.T_totalScore.text = data.totalScore.ToString();
        data.T_eyeSumScore.text = data.eyeSumScore.ToString();
        data.T_eyeStar_A.text = data.eyeStar_A;
        data.T_eyeStar_P.text = data.eyeStar_P;
        data.T_earSumScore.text = data.earSumScore.ToString();
        data.T_earStar_A.text = data.earStar_A;
        data.T_earStar_P.text = data.earStar_P;
        data.T_mouthSumScore.text = data.mouthSumScore.ToString();
        data.T_mouthStar_A.text = data.mouthStar_A;
        data.T_mouthStar_P.text = data.mouthStar_P;

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
            
            if(tempRight) rightDistance = Vector2.Distance(tempRight.transform.position, new Vector2(-eyeCorrectPoint.position.x, eyeCorrectPoint.position.y));


            if (leftDistance != 0) 
                leftEye = 1 - leftDistance / correctDistance;
            else 
                leftEye = 0;

            if (rightDistance != 0) 
                rightEye = 1 - rightDistance / correctDistance;
            else 
                rightEye = 0;
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
    //各種UI
    public Text T_totalScore, T_eyeSumScore, T_eyeStar_A, T_eyeStar_P,
        T_earSumScore, T_earStar_A, T_earStar_P,
        T_mouthSumScore, T_mouthStar_A, T_mouthStar_P;

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