using DIContainer;
using Manager;
using UnityEngine;

namespace Player
{
    public class CameraMove : MonoBehaviour
    {
        private bool _rotating;
        private Vector3 _moveDirection;
        private InGameManager _inGameManager;
        [SerializeField] private float _speed;
        
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
            _moveDirection = new Vector3(input.x, 0, input.y);
        }
    }
}
