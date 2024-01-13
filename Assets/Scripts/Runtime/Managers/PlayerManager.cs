using System;
using Runtime.Commands.Player;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private PlayerMeshController playerMeshController;
        [SerializeField] private PlayerPhysicsController playerPhysicsController;


        #endregion

        #region Public Variables

        public byte stageValue;
        internal ForceBallsToPoolCommand forceCommand;

        #endregion

        #region Private Variables

        private PlayerData _data;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPlayerData();
            SendDataControllers();
            Init();
        }

        private PlayerData GetPlayerData()
        {
            return Resources.Load<CD_Player>("Data/CD_Player").data;
        }
        private void SendDataControllers()
        {
            playerMovementController.SetData(_data.playerMovementData);
            playerMeshController.SetData(_data.playerMeshData);
        }
        private void Init()
        {
            forceCommand = new ForceBallsToPoolCommand(this, _data.playerForceData);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += OnInputTaken;
            InputSignals.Instance.onInputReleased += OnInputReleased;
            InputSignals.Instance.onInputDragged += OnInputDragged;
            UISignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccesful;
            CoreGameSignals.Instance.onLevelFailed += OnLevelFail;
            CoreGameSignals.Instance.onStageAreaEntered += OnStageAreaEnter;
            CoreGameSignals.Instance.onStageAreaSuccessful += OnStageSuccessful;
            CoreGameSignals.Instance.onFinishAreaEntered += OnFinishArea;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void OnInputTaken()
        {
            playerMovementController.IsReadyToMove(true);
        }

        private void OnInputReleased()
        {
            playerMovementController.IsReadyToMove(false);
        }

        private void OnInputDragged(HorizontalInputParams inputParams)
        {
            playerMovementController.UpdateInputParams(inputParams);
        }

        private void OnPlay()
        {
            playerMovementController.IsReadyToPlay(true);
        }

        private void OnLevelSuccesful()
        {
            playerMovementController.IsReadyToPlay(false);
        }

        private void OnLevelFail()
        {
            playerMovementController.IsReadyToPlay(false);
        }

        private void OnStageAreaEnter()
        {
            playerMovementController.IsReadyToPlay(false);
        }

        private void OnStageSuccessful(byte value)
        {
            stageValue=++value;
            playerMovementController.IsReadyToPlay(true);
            playerMeshController.ScaleUpPlayer();
            playerMeshController.PlayConfetiParticle();
            playerMeshController.ShowUpText();
        }

        private void OnFinishArea()
        {
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
            //Mini Game 
        }

        private void OnReset()
        {
            stageValue = 0;
            playerMovementController.OnReset();
            playerPhysicsController.OnReset();
            playerMeshController.OnReset();
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= OnInputTaken;
            InputSignals.Instance.onInputReleased -= OnInputReleased;
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            UISignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccesful;
            CoreGameSignals.Instance.onLevelFailed -= OnLevelFail;
            CoreGameSignals.Instance.onStageAreaEntered -= OnStageAreaEnter;
            CoreGameSignals.Instance.onStageAreaSuccessful -= OnStageSuccessful;
            CoreGameSignals.Instance.onFinishAreaEntered -= OnFinishArea;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}