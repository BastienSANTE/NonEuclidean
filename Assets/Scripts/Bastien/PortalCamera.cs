using UnityEngine;

namespace Bastien {
    public class PortalCamera : MonoBehaviour {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private Transform _inPortal;
        [SerializeField] private Transform _outPortal;

        private Vector3 _playerPortalOffset;
    
        void Update() {
            _playerPortalOffset = _playerCamera.position - _inPortal.position;
            transform.position = _outPortal.position + _playerPortalOffset;

            transform.rotation = _playerCamera.rotation;
        }
    }
}
