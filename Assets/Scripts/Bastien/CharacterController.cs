using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bastien {
    public class CharacterController : MonoBehaviour
    {
    
        [SerializeField] public float speed;
        [SerializeField] public Transform headTransform;
        [SerializeField] public Transform bodyTransform;

        private void Start() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            bodyTransform.Translate(movement * (speed * Time.deltaTime));
        
            Vector3 rotation = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            bodyTransform.Rotate(0, rotation.y, 0);
            headTransform.Rotate(rotation.x, 0, 0);
        }
    }
}