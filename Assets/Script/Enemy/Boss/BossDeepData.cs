using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeepData
{
    public Transform toPossessParts;
    public List<Transform> transforms = new List<Transform>();
    //初回ですか？　それならtrue
    public bool hsFirst = true;
    public Rigidbody2D bRigid;

    private static BossDeepData _bossDpData;
    public static BossDeepData GetBDpData
    {
        get
        {
            if (_bossDpData == null)
            {
                _bossDpData = new BossDeepData();
            }
            if (_bossDpData == null)
            {
                Debug.LogError("not found");
            }
            return _bossDpData;
        }
    }
}
