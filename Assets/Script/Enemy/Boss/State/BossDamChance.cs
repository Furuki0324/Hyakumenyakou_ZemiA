using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossDamChance : MonoBehaviour, IBossStateRoot
{
    public bool First {get; set;}
    private void Start() {
        First = true;
    }
    public void attack() { }
    public void defend() { }
    public void move()
    {
        if (First)
        {
            //倒れてる状態の表現
            transform.rotation = Quaternion.FromToRotation(Vector3.left, Vector3.up);
            First = false;
        }
    }
}