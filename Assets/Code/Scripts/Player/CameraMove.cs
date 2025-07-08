using System.Collections;
using DG.Tweening;
using Manager;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class CameraMove : MonoBehaviour
    {
        private Vector3 _rotation;
        private Tween _rotateTween;
        private Vector3 _moveDirection;
        private InGameManager _inGameManager;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed;
        
        //初期化処理を書く
        private void Start()
        {
            _rotation = transform.eulerAngles;
            var a = ServiceLocator.Instance.TryGetClass(out _inGameManager);
            var b = ServiceLocator.Instance.TryGetClass(out PlayerOperationManager playerOperationManager);
            playerOperationManager.OnMoveAction += OnMoveInput;
            playerOperationManager.OnNextAction += OnNextInput;
            playerOperationManager.OnPreviousAction += OnPreviousInput;
        }

        //前後左右へのカメラ移動、
        private void FixedUpdate()
        {
            if (_inGameManager == null) return;
            if (_inGameManager.DayState != 0) return;
            transform.position += _moveDirection * _speed;
        }

        //操作受付時の処理
        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            var inputDirection = new Vector3(input.x, 0, input.y);
            float yRotation = transform.eulerAngles.y;
            Quaternion yOnlyRotation = Quaternion.Euler(0f, yRotation, 0f);
            _moveDirection = yOnlyRotation * inputDirection;
        }

        private void OnPreviousInput(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started) return;
            _rotation += new Vector3(0, -90, 0);
            transform.DORotate(_rotation, 1.0f);
        }

        private void OnNextInput(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started) return;
            _rotation += new Vector3(0, 90, 0);
            transform.DORotate(_rotation, 1.0f);
        }
    }
}