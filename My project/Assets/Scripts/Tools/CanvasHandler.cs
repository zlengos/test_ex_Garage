using System;
using DG.Tweening;
using UnityEngine;

namespace Tools
{
    public static class CanvasHandler
    {
        public static void ShowCanvas(CanvasGroup canvasGroup, Action onComplete = null, float duration = 0.5f)
        {
            ToggleCanvas(canvasGroup, true, onComplete);
        }

        public static void HideCanvas(CanvasGroup canvasGroup, Action onComplete = null, float duration = 0.5f)
        {
            ToggleCanvas(canvasGroup, false, onComplete);
        }

        public static void ToggleCanvas(CanvasGroup canvasGroup, bool isActive, Action onComplete = null, float duration = 0.5f)
        {
            if (canvasGroup == null) return;
            
            var endValue = isActive ? 1f : 0f;
                
            canvasGroup.DOFade(endValue, duration).OnComplete(() =>
            {
                canvasGroup.gameObject.SetActive(isActive);
                canvasGroup.interactable = isActive;
                canvasGroup.blocksRaycasts = isActive;
                    
                onComplete?.Invoke();
                    
                canvasGroup.gameObject.SetActive(isActive);
            });
        }
    }
}