using System.Collections;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISlidingController : MonoBehaviour
{
    [Header("UI Panels")]
    public CanvasGroup titleScreenGroup;
    public RectTransform mainMenuPanel;
    public RectTransform levelSelectPanel;
    public RectTransform controlsPanel;

    [Header("Level Select ScrollView")]
    public GameObject levelButtonPrefab;
    public Transform levelListContent;

    [Header("Animation Settings")]
    public float fadeDuration = 1f;
    public float slideDuration = 0.5f;

    [Header("Target Positions")]
    public Vector2 mainMenuTargetPosition = new Vector2(200f, 0f);
    public Vector2 levelSelectTargetPosition = new Vector2(0f, 0f);

    public IEnumerator SlideInPanel(RectTransform panel)
    {
        Vector2 start = new Vector2(-Screen.width, 0);
        Vector2 end = GetTargetPosition(panel);
        float time = 0f;
        panel.anchoredPosition = start;

        while (time < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(start, end, time / slideDuration);
            time += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = end;
    }

    public IEnumerator SlideOutPanel(RectTransform panel)
    {
        Vector2 start = panel.anchoredPosition;
        Vector2 end = new Vector2(-Screen.width, 0);
        float time = 0f;

        while (time < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(start, end, time / slideDuration);
            time += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = end;
    }

    Vector2 GetTargetPosition(RectTransform panel)
    {
        if (panel == mainMenuPanel)
            return mainMenuTargetPosition;
        if (panel == levelSelectPanel)
            return levelSelectTargetPosition;
        return Vector2.zero;
    }
}
