using System;
using DG.Tweening;
using Runtime.Data.ValueObjects;
using Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private TextMeshPro scaleText;
        [SerializeField] private ParticleSystem confettiParticle;

        #endregion

        #region Private Variables
        
        [ShowInInspector] private PlayerMeshData _data;

        #endregion

        #endregion
        

        public void SetData(PlayerMeshData scaleData)
        {
            _data = scaleData;
        }

        internal void ScaleUpPlayer()
        {
            renderer.transform.DOScaleX(_data.scaleCounter, 1).SetEase(Ease.Flash);
        }

        internal void ShowUpText()
        {
            scaleText.gameObject.SetActive(true);
            scaleText.DOFade(1, 0f).SetEase(Ease.Flash).OnComplete(() => { scaleText.DOFade(0, 0).
                SetEase(Ease.Flash).SetDelay(.65f);
            });
            scaleText.rectTransform.DOAnchorPosY(.85f, .65f).SetRelative(true).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                scaleText.rectTransform.DOAnchorPosY(-.85f, .65f).SetRelative(true);
            });
        }

        internal void PlayConfetiParticle()
        {
            confettiParticle.Play();
        }

        internal void OnReset()
        {
            renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
        }

    }
}