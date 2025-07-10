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

        private void OnEnable()
        {
            _clickAction.action.performed += ClickAction_performed;
            _clickAction.action.Enable();
        }

        private void OnDisable()
        {
            _clickAction.action.performed -= ClickAction_performed;
            _clickAction.action.Disable();
        }

        private void ClickAction_performed(InputAction.CallbackContext context)
        {
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