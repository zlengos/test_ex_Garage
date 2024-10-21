using UnityEngine;

namespace Player
{
    public class PlayerItemPickupHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float pickupRange;
        [SerializeField] private float throwForce = 500f;
        [SerializeField] private Transform holdPoint;
        [SerializeField] private LayerMask objectLayerMask;

        #region Runtime

        private Rigidbody _pickedObject;
        private bool _isObjectHeld;

        private Camera _camera;

        #endregion

        #endregion

        private void Start()
        {
            _camera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (!_isObjectHeld)
                {
                    TryPickup();
                }
                else
                {
                    MoveObjectToHoldPoint();
                }
            }
            else if (_isObjectHeld)
            {
                DropObject();
            }

            if (Input.GetMouseButtonDown(1) && _isObjectHeld)
            {
                _isObjectHeld = false;
                _pickedObject.useGravity = true;
                ThrowObject();
            }

        }

        private void TryPickup()
        {
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, objectLayerMask))
            {
                _pickedObject = hit.rigidbody;
                _isObjectHeld = true;
                _pickedObject.useGravity = false;
                _pickedObject.freezeRotation = true;
            }
        }

        private void DropObject()
        {
            _isObjectHeld = false;
            _pickedObject.useGravity = true;
            _pickedObject.freezeRotation = false;
            _pickedObject = null;
        }

        private void ThrowObject()
        {
            _pickedObject.AddForce(_camera.transform.forward * throwForce);
            _pickedObject.freezeRotation = false;
            _pickedObject = null;
        }

        private void MoveObjectToHoldPoint()
        {
            _pickedObject.velocity = (holdPoint.position - _pickedObject.position) * 10;

            if (Vector3.Distance(_pickedObject.transform.position, _camera.transform.position) > pickupRange)
                DropObject();
        }
    }
}