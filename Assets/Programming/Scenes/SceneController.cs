using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController: MonoBehaviour
{
    [SerializeField] private CanvasGroup UIToFade;

    [SerializeField] private bool quitFadeOut = false;

    private void Update()
    {
        if (quitFadeOut)
        {
            if (UIToFade.alpha >= 0)
            {
                UIToFade.alpha -= Time.deltaTime;
                if (UIToFade.alpha <= 0)
                {
                    quitFadeOut = false;
                    Quit();
                }
            }
        }
    }

    public void LoadScene(string nextScene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextScene);
    }

    public void FadeUI()
    {
        quitFadeOut = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}

