using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItemCtrl : MonoBehaviour
{
    //---------------------Public------------------------
    [Tooltip("ドロップするアイテムの個数")]
    public int amount = 1;
    public DropItem[] dropItems;

    
    public void DroppingItem()
    {
        for(int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, dropItems.Length);
            DropItemScript item = dropItems[index].item;

            Instantiate(item, transform.position, Quaternion.identity);
        }
        
    }
}

[System.Serializable]
public class DropItem
{
    public string name;
    public DropItemScript item;
}