using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public static MessageBox singletonInstance;

    [System.Serializable]
    public struct Message
    {
        public Message(string text, float time)
        {
            message = text;
            duration = time;
        }

        [TextArea]
        public string message;
        public float duration;
    }

    private Text text;
    private bool isShowing;
    private List<Message> pendingMessageList = new List<Message>();

    private void Awake()
    {
        if(singletonInstance == null)
        {
            singletonInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // ほかのクラスのStart()からShowMessage()を呼ぶため、
        // Start()より先にコンポーネントを取得する。
        isShowing = false;
        text = GetComponentInChildren<Text>();
    }

    /// <summary>
    /// <para>メッセージを表示します。</para>
    /// <para>既に表示中のものがある場合は待機列に追加し、順番に表示します。</para>
    /// </summary>
    /// <param name="message"></param>
    public void ShowMessage(Message message)
    {
        pendingMessageList.Add(message);
        if (!isShowing)
        {
            isShowing = true;
            StartCoroutine(Show());
        }
    }

    public void ShowMessage(string text, float duration)
    {
        Message message = new Message(text, duration);
        pendingMessageList.Add(message);
        if (!isShowing)
        {
            isShowing = true;
            StartCoroutine(Show());
        }
    }

    private IEnumerator Show()
    {
        Vector3 position = new Vector3(-520, transform.position.y, transform.position.z);
        transform.position = position;

        Message message;
        float duration;
        while(pendingMessageList.Count > 0)
        {
            message = pendingMessageList[0];
            pendingMessageList.RemoveAt(0);

            duration = message.duration;
            // durationに不正な数値が入力されていた場合、
            // 明らかに問題のある動作をさせる。
            if(duration <= 0)
            {
                duration = 0.1f;
            }

            text.text = message.message;
            // メッセージボックスは1秒でスライドする
            for(float time = 0; time < 1.0f; time += Time.deltaTime)
            {
                position.x = Mathf.Lerp(-520, 5, time / 1.0f);
                transform.position = position;

                yield return null;
            }

            yield return new WaitForSeconds(duration);

            for (float time = 0; time < 1.0f; time += Time.deltaTime)
            {
                position.x = Mathf.Lerp(5, -520, time / 1.0f);
                transform.position = position;

                yield return null;
            }

            yield return null;
        }

        isShowing = false;
    }
}