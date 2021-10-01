using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class LineWithCollider : MonoBehaviour
{
    LineRenderer lr;
    BezierCurve bc;
    float wireWidth;
    List<Vector2> bezierPoints;
    List<Vector2> temp;
    PolygonCollider2D pCol;
    // Start is called before the first frame update
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        bc = gameObject.GetComponent<BezierCurve>();
        pCol = gameObject.GetComponent<PolygonCollider2D>();
        bezierPoints = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {

        wireWidth = lr.startWidth;
        bezierPoints.Clear();
        bc.pot.ForEach(v => bezierPoints.Add(v));

        //コライダ用のVector2ポイント
        List<Vector2> edgePoints = new List<Vector2>();

        //ベジェ曲線のポイント分繰り返す
        for (int j = 1; j < bezierPoints.Count; j++)
        {
            //ポイントごとの距離、その距離と前方ベクトルに直交するベクトルの計算
            Vector2 distanceBetweenPoints = bezierPoints[j - 1] - bezierPoints[j];
            Vector3 crossProduct = Vector3.Cross(distanceBetweenPoints, Vector3.forward);

            //両側に伸ばす
            Vector2 up = (wireWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + bezierPoints[j - 1];
            Vector2 down = -(wireWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + bezierPoints[j - 1];

            //先頭にdown、末尾にupを追加
            edgePoints.Insert(0, down);
            edgePoints.Add(up);

            //最後のポイントだけ計算を変更
            if (j == bezierPoints.Count - 1)
            {
                // Compute the values for the last point on the Bezier curve
                up = (wireWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + bezierPoints[j];
                down = -(wireWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + bezierPoints[j];

                edgePoints.Insert(0, down);
                edgePoints.Add(up);
            }
        }

        //コライダポイントを代入
        pCol.points = edgePoints.ToArray();
    }
}