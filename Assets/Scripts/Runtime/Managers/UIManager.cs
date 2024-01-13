using System;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onStageAreaSuccessful += OnStageAreaSuccessful;
        }

        private void OnStageAreaSuccessful(byte stageValue)
        {
            UISignals.Instance.onSetStageColor?.Invoke(stageValue);
        }

        private void OnLevelFailed()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2);
        }

        private void OnLevelSuccessful()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
        }


        private void OnLevelInitialize(byte levelValue)
        {
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelTypes.Level,0);
            UISignals.Instance.onSetLevelValue.Invoke(levelValue);
        }
        private void OnReset()
        {
            CoreUISignals.Instance.onCloseAllPanel.Invoke();
            CoreUISignals.Instance.onOpenPanel(UIPanelTypes.Start, 1);
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        public void Play()
        {
            UISignals.Instance.onPlay?.Invoke();
            CoreUISignals.Instance.onClosePanel?.Invoke(1);
            InputSignals.Instance.onEnableInput?.Invoke();
            CameraSignals.Instance.onSetCameraTarget?.Invoke();
        }
        public void NextLevel()
        {
            CoreGameSignals.Instance.onNextLevel.Invoke();
        }
        public void RestartLevel()
        {
            CoreGameSignals.Instance.onRestartLevel.Invoke();
        }
    }
}