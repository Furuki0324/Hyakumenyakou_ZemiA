using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControl : MonoBehaviour
{
    public void SceneLoading()
    {
        SceneManager.LoadScene("Test_Field");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        Application.Quit();

#endif
    }
}
