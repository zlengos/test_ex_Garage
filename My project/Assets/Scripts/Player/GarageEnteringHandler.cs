using System.Collections;
using Cinemachine;
using DG.Tweening;
using Logo;
using UnityEngine;

namespace Player
{
    public class GarageEnteringHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private StartGameLogo logo;
        [SerializeField] private BoxCollider preventToExitGarage;
        [SerializeField] private Animator gates, hand, playerCamera;
        [SerializeField] private float animPressingPercentage = 0.58f;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Transform playerStartsGamePoint;
        [SerializeField] private CinemachineFreeLook cinemachineCamera;

        #region Cache

        private static readonly int Opening = Animator.StringToHash("Opening");
        private static readonly int StartingGame = Animator.StringToHash("StartingGame");

        #endregion
        
        #endregion

        #region Listening

        private void OnEnable() => logo.OnLogoViewEnded += OnLogoViewEnded;

        private void OnDisable() => logo.OnLogoViewEnded -= OnLogoViewEnded;
        
        #endregion

        private void OnLogoViewEnded()
        {
            StartCoroutine(OpeningGarage());
        }

        private IEnumerator OpeningGarage()
        {
            hand.SetTrigger(Opening);

            yield return null;

            AnimatorStateInfo handStateInfo = hand.GetCurrentAnimatorStateInfo(0);
            while (handStateInfo.normalizedTime == 0) 
            {
                handStateInfo = hand.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }

            float stateLength = handStateInfo.length;

            yield return new WaitForSeconds(stateLength * animPressingPercentage);

            gates.SetTrigger(Opening);

            yield return null;

            AnimatorStateInfo gatesStateInfo = gates.GetCurrentAnimatorStateInfo(0);
            while (gatesStateInfo.normalizedTime == 0) 
            {
                gatesStateInfo = gates.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }

            float gatesOpeningLength = gatesStateInfo.length;

            yield return new WaitForSeconds(gatesOpeningLength);

            hand.SetTrigger(StartingGame);
            
            playerMovement.transform.DOMove(playerStartsGamePoint.position, 4f)
                .OnComplete(EnteringComplete);
        }

        private void EnteringComplete()
        {
            playerMovement.PreventMovement(false);
            cinemachineCamera.gameObject.SetActive(false);
        }
    }
}