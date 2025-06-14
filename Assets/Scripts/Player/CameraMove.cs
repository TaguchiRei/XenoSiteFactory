using System.Collections;
using DG.Tweening;
using DIContainer;
using Manager;
using UnityEngine;

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
            DiContainer.Instance.TryGet(out _inGameManager);
            DiContainer.Instance.TryGet(out PlayerOperationManager playerOperationManager);
            playerOperationManager.OnMoveAction += OnMoveInput;
        }

        //前後左右へのカメラ移動、
        private void FixedUpdate()
        {
            if (_inGameManager.DayState != 0) return;

            transform.position += _moveDirection * _speed;
        }

        //操作受付時の処理
        private void OnMoveInput(Vector2 input)
        {
            var inputDirection = new Vector3(input.x, 0, input.y);
            float yRotation = transform.eulerAngles.y;
            Quaternion yOnlyRotation = Quaternion.Euler(0f, yRotation, 0f);
            _moveDirection = yOnlyRotation * inputDirection;
        }

        private void OnPreviousInput()
        {
            _rotation += new Vector3(0, 90, 0);
            _rotation.y %= 360f;
            transform.DORotate(_rotation, 1.0f);
        }

        private void OnNextInput()
        {
            _rotation += new Vector3(0, -90, 0);
            _rotation.y %= 360f;
            transform.DORotate(_rotation, 1.0f);
        }
    }
}