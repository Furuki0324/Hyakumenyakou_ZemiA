using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスの状態を憑依中、ダメージチャンス、高速移動、パーツ無し時の四つに分類
//共通部分をインターフェースに
public interface IBossStateRoot
{
    bool First{get; set;}
    void attack();
    void defend();
    void move();
    void stopHavingAllCoroutine();
}