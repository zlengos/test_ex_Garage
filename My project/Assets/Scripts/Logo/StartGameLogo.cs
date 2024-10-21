using System;
using System.Collections;
using DG.Tweening;
using Player;
using Tools;
using UnityEngine;

namespace Logo
{
    public class StartGameLogo : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CanvasGroup logoCanvas;
        [SerializeField] private RectTransform text, textMask;
        [SerializeField] private RectTransform logo, logoMask;
        [SerializeField] private RectTransform separator;
        [SerializeField] private PlayerMovement playerMovement;

        [SerializeField] private bool playOnAwake;
        [SerializeField] private float secondsAfterAnimated = 1f;

        public Action OnLogoViewEnded;

        #endregion

        private void Awake()
        {
            if (playOnAwake)
                PlayLogo();
        }

        private void PlayLogo()
        {
            playerMovement.PreventMovement(true);
            if (playOnAwake)
                logoCanvas.alpha = 1;
        
            CanvasHandler.ShowCanvas(logoCanvas, AnimateSeparator);
        }

        private void AnimateSeparator() => separator.DOScaleY(1f, 0.5f).OnComplete(AnimateTextAndLogo);
        
        private void AnimateTextAndLogo()
        {
            var logoTargetPos = new Vector3(logoMask.position.x - logoMask.rect.width / 2, logoMask.position.y, logoMask.position.z);
            var textTargetPos = new Vector3(textMask.position.x + textMask.rect.width / 2, textMask.position.y, textMask.position.z);
            
            logo.DOMove(logoTargetPos, 1f);
            text.DOMove(textTargetPos, 1f)
                .OnComplete(() => StartCoroutine(WaitAfterAnimated()));
        }

        private IEnumerator WaitAfterAnimated()
        {
            yield return new WaitForSeconds(secondsAfterAnimated);
        
            // TODO: mb sounds
        
            CanvasHandler.HideCanvas(logoCanvas, () => OnLogoViewEnded?.Invoke());
        }
    }
}
