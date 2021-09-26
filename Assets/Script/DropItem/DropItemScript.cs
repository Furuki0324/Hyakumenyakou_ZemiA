using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemScript : MonoBehaviour
{
    private Vector3 startPos = Vector3.zero, destination = Vector3.zero;
    private float startTime = 0;
    internal float deltaX = 0, deltaY = 0;

    [Tooltip("アイテム出現時のアクションにかける時間")]
    public float moveTime = 0.2f;
    public float allowableError = 0.5f;

    public void SetDelta(float DeltaX, float DeltaY)
    {
        deltaX = DeltaX;
        deltaY = DeltaY;

        startPos = transform.position;
        destination = transform.position + new Vector3(deltaX, deltaY, 0);

        Debug.Log("When SetDelta: " + destination);
       
    }

    private void OnEnable()
    {
        Debug.Log("When OnEnable" + destination);
        startTime = Time.time;
        //StartCoroutine(PopAction());
    }
    /*
    private void Start()
    {
        Debug.Log(deltaX + " : " + deltaY);
        startPos = transform.position;
        destination = transform.position + new Vector3(1, 1, 0);

        startTime = Time.time;

        StartCoroutine(PopAction());
    }
    */

    IEnumerator PopAction()
    {
        while (!ReachToDestination(destination))
        {
            transform.position = Vector3.Slerp(startPos, destination, CalcMoveRatio());
            yield return null;
        }
    }

    private float CalcMoveRatio()
    {
        float a = (Time.time - startTime) * (1 / moveTime);
        return a;
    }

    private bool ReachToDestination(Vector3 destination)
    {
        if ((transform.position - destination).magnitude < allowableError)
        {
            Debug.Log("Reached.");
            return true;
        }
        else return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DropItemManager.ObtainItem(gameObject.tag);
            Destroy(this.gameObject);
        }
    }
}
