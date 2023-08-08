using UnityEngine;

namespace Baruch
{
    public class PlayerController : MonoBehaviour, IInit
    {
        Camera _main;
        Vector2 _handlePosition;
        Vector2 _clickPosition;
        Vector2 _middlePosition;
        float _currentZRotation;
        Transform _handle;


        [SerializeField] float _rotationSpeed = 1f;
        [SerializeField] bool _fromHandle = true;
        Vector2 _mousePos => Input.mousePosition;

        public void Init()
        {
            _main = Camera.main;
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;
            _middlePosition = new Vector2(Screen.width, Screen.height) / 2f;
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
                var angleDelta = Vector2.SignedAngle(_clickPosition - (_fromHandle ? _handlePosition : _middlePosition), _mousePos - (_fromHandle ? _handlePosition : _middlePosition));
                angleDelta *= _rotationSpeed * Time.deltaTime * 60;

                _currentZRotation += angleDelta;

                _handle.eulerAngles = Vector3.forward * _currentZRotation;

                _clickPosition = Input.mousePosition;
            }
        }
    }
}
