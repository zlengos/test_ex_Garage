using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float swayAmount = 0.1f;
        [SerializeField] private float swaySpeed = 5f;
        [SerializeField] private float mouseSensitivity = 5f;
        [SerializeField] private Rigidbody rb;

        #region Runtime

        private Vector3 _movementInput;
        private Vector3 _targetSway;
        private float _rotationY;
        private Camera _camera;

        #endregion

        #region API

        private bool _preventMovement;
        public void PreventMovement(bool isPrevent) => _preventMovement = isPrevent;

        #endregion

        #endregion


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_preventMovement) return;
            
            HandleInput();
            HandleMouseLook();

            if (_movementInput != Vector3.zero)
                HandleSway();
            
            Move();
        }

        private void HandleInput()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            _movementInput = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        }

        private void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            _rotationY -= mouseY;
            _rotationY = Mathf.Clamp(_rotationY, -80f, 80f);
            _camera.transform.localRotation = Quaternion.Euler(_rotationY, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void Move()
        {
            if (_movementInput != Vector3.zero)
            {
                Vector3 movement = transform.TransformDirection(_movementInput) * (moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + movement);
            }
        }

        private void HandleSway()
        {
            float swayX = 0f;
            float swayY = 0f;

            if (_movementInput.magnitude > 0)
            {
                float stepProgress = Mathf.PingPong(Time.time * swaySpeed, 1f);

                if (_movementInput.z > 0)
                {
                    swayX = Mathf.Sin(stepProgress * Mathf.PI) * swayAmount;
                    swayY = Mathf.Abs(Mathf.Cos(stepProgress * Mathf.PI)) * swayAmount * 0.5f;
                }
                else if (_movementInput.z < 0)
                {
                    swayX = -Mathf.Sin(stepProgress * Mathf.PI) * swayAmount;
                    swayY = Mathf.Abs(Mathf.Cos(stepProgress * Mathf.PI)) * swayAmount * 0.5f;
                }
            }

            _targetSway = new Vector3(swayX, swayY, 0f);
            _camera.transform.localPosition = _targetSway;
        }
    }
}