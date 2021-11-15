using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleControl : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] Image cover;
    private Color coverColor;

    private void Start()
    {
        Time.timeScale = 1;

        coverColor = cover.color;
        coverColor.a = 0;
        cover.color = coverColor;
    }

    public void SceneLoading()
    {
        _ = SceneLoad();
        //SceneManager.LoadScene("Test_Field");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        Application.Quit();

#endif
    }

    private async Task SceneLoad()
    {
        AsyncOperation nextScene = SceneManager.LoadSceneAsync("Test_Field", LoadSceneMode.Single);
        nextScene.allowSceneActivation = false;
        
        float alpha;
        for(float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / fadeDuration);

            coverColor.a = alpha;
            cover.color = coverColor;

            await Task.Delay(10);
        }

        nextScene.allowSceneActivation = true;
    }
}
