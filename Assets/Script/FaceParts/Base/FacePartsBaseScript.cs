using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePartsBaseScript : MonoBehaviour
{
    [Header("Setting in base script")]
    public int health;
    public List<EnemyCtrl> enemiesNearby = new List<EnemyCtrl>();


    /// <summary>
    /// <para>各顔面パーツが攻撃されたときに使用されます。</para>
    /// <para>目・耳・口の各パーツのスクリプトでoverrideして使用してください。</para>
    /// </summary>
    public virtual void TakeDamage()
    {
        Debug.Log(gameObject.name + " - TakeDamage method has invoked.");
    }


    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage) { }


    public virtual void Dead()
    {
        if(enemiesNearby.Count > 0)
        {
            for(int i = 0; i < enemiesNearby.Count; i++)
            {
                Destroy(this.gameObject);
                enemiesNearby[i].ResetTarget();
            }
        }
    }
}
