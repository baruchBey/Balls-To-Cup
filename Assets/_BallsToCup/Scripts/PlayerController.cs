using UnityEngine;

namespace Baruch
{
    public class PlayerController : MonoBehaviour
    {
        Camera _main;
        Vector2 _handlePosition;
        Vector2 _clickPosition;
        float _currentZRotation;
        Transform _handle;
        Vector2 _middleOfScreen;

        Rigidbody2D _cupRb;

        [SerializeField] float rotationSpeed = 60f; // Adjust the rotation speed as needed
        Vector2 _mousePos => Input.mousePosition;
        private void Start()
        {
            Application.targetFrameRate = 60;
            _main = Camera.main;
            var tube = GameObject.Find("Tube1");
            _cupRb = tube.GetComponentInChildren<Rigidbody2D>();
            _handle = tube.transform;
            _handlePosition = _main.WorldToScreenPoint(tube.transform.position);
            _middleOfScreen = new Vector2(Screen.width, Screen.height) / 2f;
        }

        void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _currentZRotation = _handle.eulerAngles.z;
                _clickPosition = _mousePos;
            }
            else if (Input.GetMouseButton(0))
            {
                // Calculate the angle difference between the click position and the current mouse position
                var angleDelta = Vector2.SignedAngle(_clickPosition - _handlePosition, _mousePos - _handlePosition);
                angleDelta *= rotationSpeed * Time.deltaTime;

                // Update the current rotation
                _currentZRotation += angleDelta;

                // Rotate the handle around the z-axis
                _handle.eulerAngles = Vector3.forward * _currentZRotation;

                // Update the click position for the next frame
                _clickPosition = Input.mousePosition;
            }
        }
    }
}
