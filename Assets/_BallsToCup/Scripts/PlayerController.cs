using UnityEngine;

namespace Baruch
{
    public class PlayerController : MonoBehaviour, IInit
    {
        Camera _main;
        Vector2 _handlePosition;
        Vector2 _clickPosition;
        float _currentZRotation;
        Transform _handle;


        [SerializeField] float _rotationSpeed = 60f; 
        Vector2 _mousePos => Input.mousePosition;

        public void Init()
        {
            _main = Camera.main;
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;
        }

        private void LevelManager_OnLevelBuild()
        {
           
            _handle = LevelManager.Instance.CurrentLevel.Tube;
            _handlePosition = _main.WorldToScreenPoint(_handle.position);
        }

     

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _currentZRotation = _handle.eulerAngles.z;
                _clickPosition = _mousePos;
            }
            else if (Input.GetMouseButton(0))
            {
                var angleDelta = Vector2.SignedAngle(_clickPosition - _handlePosition, _mousePos - _handlePosition);
                angleDelta *= _rotationSpeed * Time.deltaTime;

                _currentZRotation += angleDelta;

                _handle.eulerAngles = Vector3.forward * _currentZRotation;

                _clickPosition = Input.mousePosition;
            }
        }
    }
}
