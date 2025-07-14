using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VProject.Utils
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputActionReference _clickAction;

        public static event Action OnClickAction;

        private bool _isClicked = false;
        
        public bool Enabled { get; set; }

        private void OnEnable()
        {
            _clickAction.action.performed += ClickAction_performed;
            _clickAction.action.Enable();
            Enabled = true;
        }

        private void OnDisable()
        {
            _clickAction.action.performed -= ClickAction_performed;
            _clickAction.action.Disable();
            Enabled = false;
        }

        private void ClickAction_performed(InputAction.CallbackContext context)
        {
            if (Enabled == false)
                return;

            if (_isClicked)
            {
                _isClicked = false;
                OnClickAction?.Invoke();
            }
            else
            {
                _isClicked = true;
            }
                
        }
    }

}