using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemCtrl : MonoBehaviour
{
    //---------------------Public------------------------
    public DropItem[] dropItems;

    
    public void DroppingItem()
    {
        int index = Random.Range(0, dropItems.Length);
        GameObject item = dropItems[index].item;

        Instantiate(item, transform.position, Quaternion.identity);
    }
}

[System.Serializable]
public class DropItem
{
    public string name;
    public GameObject item;
}