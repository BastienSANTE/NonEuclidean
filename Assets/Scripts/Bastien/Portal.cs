using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bastien {
    
    public class Portal : MonoBehaviour {
        /// <summary>
        /// Base class for portals. The portal uses a render texture to display the "other side".
        /// The camera is inferred from the children's components.
        /// </summary>
        
        //What information does a portal need ?
        [Header("Essentials")] 
        [SerializeField] private Portal _destination;   //Allows for portal chain creation. ends w/null

        private PortalCollider _portalCollider;         //Nested portal collider for events
        
        private Camera _playerCamera;                   //Self-explanatory. Inferred from the "Main Camera" tag
        private Camera _portalCamera;                   //Self-explanatory. One per portal

        private RenderTexture _portalRenderTexture;     //Texture showing the destination portals' side
        private Renderer _portalRenderer;
        private MeshRenderer _portalMeshRenderer;

        private void Awake() {
            _portalCollider = GetComponentInChildren<PortalCollider>();
        }

        private void OnEnable() {
            _portalCollider.OnPortalEntered += PortalEnter;
            _portalCollider.OnPortalExited += PortalExit;
        }

        private void OnDisable() {
            _portalCollider.OnPortalEntered -= PortalEnter;
            _portalCollider.OnPortalExited -= PortalExit;
        }

        private void Start() {
            // Waiting for Start() to link with other objects.
            _playerCamera = Camera.main;            
            _portalCollider = GetComponentInChildren<PortalCollider>();
            _portalCamera = GetComponentInChildren<Camera>();
            _portalRenderer = transform.Find("PortalPlane").GetComponent<Renderer>();
            _portalMeshRenderer = transform.Find("PortalPlane").GetComponent<MeshRenderer>();

            if (_destination == null) return;
            
            CreateRenderingEnvironment();
        }

        private void Update() {
            //Allows for portal chain creation. Offsets might look strange when the player is not
            //in the right rooms, but lines up with 1 portal/room
            if (_destination == null) return;
            
            Vector3 playerCamOffset = _playerCamera.transform.position - _portalCamera.transform.position;
            _destination._portalCamera.transform.localPosition = playerCamOffset;
            
            Quaternion playerCamRotation = _playerCamera.transform.rotation;
            _destination._portalCamera.transform.localRotation = playerCamRotation;
        }

        private void CreateRenderingEnvironment() {
            //Render texture size varies on resolution. Using RHalf for memory considerations.
            _portalRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
            _destination._portalCamera.targetTexture = _portalRenderTexture;
            
            _portalRenderer.material.SetTexture("_MainTex", _portalRenderTexture);
        }

        private void PortalEnter(Collider player) {
             if (_destination == null || player.CompareTag("Player") == false) return;
             
             Vector3 playerPortalOffset = transform.position - player.transform.position;
             player.transform.position = _destination.transform.position;
             Debug.Log($"POS: {player.transform.position}");
        }

        private void PortalExit(Collider player) {
            Debug.Log($"POS: {player.transform.position}");
        }
    }
}
