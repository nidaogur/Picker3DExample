using System.Collections.Generic;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        
        private InputData _data;
        private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

        private float _currentVelocity;
        private float3 _moveVector;
        private Vector2? _mousePosition;
        
        #endregion

        #endregion

        private void Awake()
        {
            _data = GetInputData();
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").data;
        }

        private void OnEnable()
        {
            SubscribeEvent();
        }

        private void SubscribeEvent()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;
            //InputSignals.Instance.onInputStateChanged += OnInputStateChanged;
        }

        // private void OnInputStateChanged(bool state)
        // {
        //     _isAvailableForTouch = state;
        // }

        private void OnDisableInput()
        {
            _isAvailableForTouch = false;
        }

        private void OnEnableInput()
        {
            _isAvailableForTouch = true;
        }

        private void OnReset()
        {
            _isAvailableForTouch = false;
            _isTouching = false;
        }

        private void UnSubscribeEvent()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;
            //InputSignals.Instance.onInputStateChanged -= OnInputStateChanged;
        }

        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        private void Update()
        {
            if (!_isAvailableForTouch) return;

            HandleInput();
        }
        private void HandleInput()
        {
            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
            {
                ReleaseInput();
            }

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                TakeInput();
            }

            if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
            {
                DragInput();
            }
        }
        private void ReleaseInput()
        {
            _isTouching = false;
            InputSignals.Instance.onInputReleased?.Invoke();
        }

        private void TakeInput()
        {
            _isTouching = true;
            InputSignals.Instance.onInputTaken?.Invoke();
            if (!_isFirstTimeTouchTaken)
            {
                _isFirstTimeTouchTaken = true;
                InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
            }

            _mousePosition = (Vector2)Input.mousePosition;
        }

        private void DragInput()
        {
            if (_isTouching)
            {
                if (_mousePosition != null)
                {
                    Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;
                    if (mouseDeltaPos.x > _data.horizontalInputSpeed)
                        _moveVector.x = _data.horizontalInputSpeed / 10f * mouseDeltaPos.x;
                    else if (mouseDeltaPos.x < -_data.horizontalInputSpeed)
                        _moveVector.x = -_data.horizontalInputSpeed / 10f * -mouseDeltaPos.x;
                    else
                        _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0f, ref _currentVelocity,
                            _data.clampSpeed);

                    _moveVector.x = mouseDeltaPos.x;

                    _mousePosition = Input.mousePosition;

                    InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                    {
                        horizontalValue = _moveVector.x,
                        clampValues = _data.clampValues
                    });
                }
            }
        }
        
        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData,results);
            return results.Count > 0;
        }
    }
}