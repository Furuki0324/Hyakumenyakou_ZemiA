using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] Image cover;
    [SerializeField] float fadeDuration;
    [SerializeField] string nextSceneName;

    public void Receiver()
    {
        _ = SceneLoad();
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
