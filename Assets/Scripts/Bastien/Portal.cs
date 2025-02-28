using UnityEngine;
using UnityEngine.UI;

namespace Bastien {
    public class Portal : MonoBehaviour
    {
        //What information does a portal need ?
        [Header("Essentials")]
        [SerializeField] private Transform _inPortal; 
        [SerializeField] private Transform _outPortal;              // Two surfaces to enter/exit
        [SerializeField] private bool _isOneWay;                    // Self-explanatory
        [SerializeField] private Camera _inCamera, _outCamera;      // Pair of cameras for the renderTextures

        [Header("Debugging")]
        public RawImage _inRenderUITex, _outRenderUITex;            //Sprites to convert the render textures for UI
        [SerializeField] private bool _debugCanvas;                 // Is debug info enabled ?

        public RenderTexture _inPortalTexture, _outPortalTexture;  // Pair of RenderTextures
        private Transform _inPlane, _outPlane;                      // Actual teleportation planes
        private Camera _mainCamera;                                 //Player Camera
        private int _textureW, _textureH;                           //Texture dimensions
    
        private Vector3 mainCameraOfs, mainCameraRot;
    
        private void Awake() {
            _textureW = Camera.main.pixelWidth;     //Set W/H of texture to camera resolution
            _textureH = Camera.main.pixelHeight;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {
            _mainCamera = Camera.main;
        
            //
            _inPlane = _inPortal.GetComponentInChildren<TeleportPlane>().gameObject.transform;
            _outPlane = _outPortal.GetComponentInChildren<TeleportPlane>().gameObject.transform;
            
            //
            _inCamera = _inPortal.GetComponentInChildren<Camera>();
            _outCamera = _outPortal.GetComponentInChildren<Camera>();
            
            // 
            _inPortalTexture  = new RenderTexture(_textureW, _textureH, 24);
            _outPortalTexture = new RenderTexture(_textureW, _textureH, 24);

            _inCamera.targetTexture = _outPortalTexture;
            _outCamera.targetTexture = _inPortalTexture;
        
            _inPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", _outCamera.targetTexture);
            _outPlane.GetComponent<Renderer>().material.SetTexture("_MainTex", _inCamera.targetTexture);
        }

        // Update is called once per frame
        void Update() {
            mainCameraOfs = _mainCamera.transform.position - _inPortal.transform.position;
            mainCameraRot = _mainCamera.transform.rotation.eulerAngles;

            _inCamera.transform.localPosition = mainCameraOfs; 
            _inCamera.transform.localRotation = Quaternion.Euler(mainCameraRot); 
            _outCamera.transform.localPosition = mainCameraOfs;
            _outCamera.transform.localRotation = Quaternion.Euler(mainCameraRot);
        }
    
        private void OnGUI() {
            if (_debugCanvas) {
                _inRenderUITex.texture = _inPortalTexture;
                _outRenderUITex.texture = _outPortalTexture;
            }
        }
    }
}
