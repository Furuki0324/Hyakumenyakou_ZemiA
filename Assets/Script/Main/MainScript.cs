using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    //------------------Singleton--------------------
    private static MainScript _S;
    public static MainScript S
    {
        get { return _S; }
        set { _S = value; }
    }

    //------------------------Public-----------------------
    public int defaultElementAmount;

    private static List<EnemyCtrl> enemies = new List<EnemyCtrl>();

    //----------------------Private-----------------------
    private static List<GameObject> faceObjects = new List<GameObject>();



    private void Start()
    {
        DropItemManager.ObtainItem("EyeElement", defaultElementAmount);
        DropItemManager.ObtainItem("EarElement", defaultElementAmount);
        DropItemManager.ObtainItem("MouthElement", defaultElementAmount);
    }

    //ここから顔パーツの情報

    /// <summary>
    /// <para>対象をリストに追加します</para>
    /// </summary>
    /// <param name="gameObject">リストに追加される対象</param>
    public static void AddFaceObject(GameObject gameObject)
    {
        if (!gameObject.GetComponent<EnemyCtrl>()) return;

        faceObjects.Add(gameObject);
    }

    /// <summary>
    /// <para>対象をリストから削除します</para>
    /// </summary>
    /// <param name="gameObject">リストから削除される対象</param>
    public static void RemoveFaceObject(GameObject gameObject)
    {
        faceObjects.Remove(gameObject);
    }

    /// <summary>
    /// <para>リストに登録されている顔パーツのTransformを配列で返します。</para>
    /// </summary>
    /// <returns></returns>
    public static Transform[] GetFaceObjectTransformsInArray()
    {
        List<Transform> transforms = new List<Transform>();

        for(int i = 0; i < faceObjects.Count; i++)
        {
            transforms[i] = faceObjects[i].transform;
        }

        return transforms.ToArray();
    }

    /// <summary>
    /// <para>顔パーツリストに登録されているゲームオブジェクトのTransformをリストで返します。</para>
    /// </summary>
    /// <returns></returns>
    public static List<Transform> GetFaceObjectTransformInList()
    {
        List<Transform> transforms = new List<Transform>();

        for(int i = 0; i < faceObjects.Count; i++)
        {
            transforms[i] = faceObjects[i].transform;
        }

        return transforms;
    }

    //ここまで顔パーツの情報

    public static void AddEnemyList(EnemyCtrl newObject)
    {
        enemies.Add(newObject);
    }

    public static void RemoveFromEnemyList(EnemyCtrl removeObject)
    {
        enemies.Remove(removeObject);
    }

    public static void GameOver()
    {
        
    }

    public static void GameClear()
    {

    }
}
