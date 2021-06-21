using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemCtrl : MonoBehaviour
{
    //---------------------Public------------------------
    public DropItem[] dropItems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class DropItem
{
    public string name;
    public Transform item;
}