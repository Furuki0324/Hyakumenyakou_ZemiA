using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossCtrl : EnemyBaseScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //とりあえずボスが倒されたらゲームクリアのメソッドを呼ぶ記述をしていますが、必要に応じて変更してください。
    //ボスが撃破されたらゲームクリア
    public override void EnemyDie()
    {
        base.EnemyDie();
        MainScript.GameClear(); //まだデバッグ出力がされるのみです。
    }
}
