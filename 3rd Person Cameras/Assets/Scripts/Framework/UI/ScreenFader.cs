﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScreenFader : SingletonMono<ScreenFader>
{
    public CanvasGroup faderCanvasGroup;
    public CanvasGroup loadingCanvasGroup;
    public CanvasGroup gameOverCanvasGroup;
    public float fadeDuration = 1f;

    protected bool m_IsFading;

    const int k_MaxSortingLayer = 32767;

    public enum FadeType
    {
        Black, Loading, GameOver,
    }
    

    public static bool IsFading
    {
        get { return Instance.m_IsFading; }
    }


    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
    {
        m_IsFading = true;
        canvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        canvasGroup.alpha = finalAlpha;
        m_IsFading = false;
        canvasGroup.blocksRaycasts = false;
    }


    public static void SetAlpha(float alpha)
    {
        Instance.faderCanvasGroup.alpha = alpha;
    }


    public static IEnumerator FadeSceneIn()
    {
        CanvasGroup canvasGroup;
        if (Instance.faderCanvasGroup.alpha > 0.1f)
            canvasGroup = Instance.faderCanvasGroup;
        else if (Instance.gameOverCanvasGroup.alpha > 0.1f)
            canvasGroup = Instance.gameOverCanvasGroup;
        else
            canvasGroup = Instance.loadingCanvasGroup;

        yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));

        canvasGroup.gameObject.SetActive(false);
    }

      public static IEnumerator FadeSceneOut (FadeType fadeType = FadeType.Black)
        {
            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.faderCanvasGroup;
                    break;
                case FadeType.GameOver:
                    canvasGroup = Instance.gameOverCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    break;
            }
            
            canvasGroup.gameObject.SetActive (true);
            
            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        }
}
