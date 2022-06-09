using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneChangeButton : MonoBehaviour
{
    [Tooltip("画面遷移の際の目隠しに使います。")]
    [SerializeField] Image cover;
    [Tooltip("目隠しを何秒で表示するかを設定します。")]
    [SerializeField] float fadeDuration;
    [SerializeField] string nextSceneName;

    /// <summary>
    /// ボタンの入力を検知します。
    /// </summary>
    public void Receiver()
    {
        StartCoroutine(ChangeScene());
        //_ = SceneLoad();
    }

    private IEnumerator ChangeScene()
    {
        AsyncOperation nextScene = SceneManager.LoadSceneAsync(nextSceneName);
        nextScene.allowSceneActivation = false;

        cover.gameObject.SetActive(true);
        Color coverColor = cover.color;
        coverColor.a = 0;
        cover.color = coverColor;

        for(float f = 0; f < fadeDuration; f += Time.unscaledDeltaTime)
        {
            coverColor.a = Mathf.Lerp(0, 1, f / fadeDuration);
            cover.color = coverColor;

            yield return null;
        }

        nextScene.allowSceneActivation = true;
    }

    public async Task SceneLoad()
    {
        AsyncOperation nextScene = SceneManager.LoadSceneAsync(nextSceneName);
        nextScene.allowSceneActivation = false;

        cover.gameObject.SetActive(true);
        Color coverColor = cover.color;
        coverColor.a = 0;
        cover.color = coverColor;

        float alpha;
        for(float i = 0; i < fadeDuration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / fadeDuration);
            coverColor.a = alpha;
            cover.color = coverColor;

            await Task.Delay(10);
        }

        nextScene.allowSceneActivation = true;
    }
}
