using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeepData
{
    public Transform toPossessParts;
    private enum Tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }
    // private static List<Transform> tempList;
    private List<Transform> _transforms;
    public List<Transform> Transforms
    {
        get
        {
            List<Transform> tempList = new List<Transform>();
            foreach (string name in Enum.GetNames(typeof(Tags)))
            {
                GameObject[] tempObj = GameObject.FindGameObjectsWithTag(name);
                foreach (GameObject j in tempObj) if (j.layer != 5) tempList.Add(j.transform);
            }
            _transforms = tempList;
            return _transforms;
        }
    }
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
