using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TGL.Learning.NewInputSystem.DirectReadingDeviceStates
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveObjectByDirectInput : MonoBehaviour
    {
        #region InputTypes
        private Keyboard myKeyboard;
        private bool keyboardConnected;
        
        private Gamepad myGamepad;
        private bool gamepadConnected;
        
        private Mouse myMouse;
        private bool mouseConnected;
        #endregion InputTypes

        #region PrivateVariables
        [SerializeField] private float speed = 5f;
        [SerializeField] private float inputX, inputY;
        [SerializeField] private Vector2 inputDirection;
        [SerializeField] private Vector2 gamepadDirection;
        Rigidbody myRigidbody;
        private Vector2 mousePosition;
        #endregion PrivateVariables
        
        
        #region ValidationOfInputTypes
        private void ReadInputsTypes()
        {
            keyboardConnected = UserHasKeyboard();
            gamepadConnected = UserHasGamePad();
            mouseConnected = UserHasMouse();
        }

        private bool UserHasKeyboard()
        {
            myKeyboard = Keyboard.current;
            if (myKeyboard == null)
            {
                // Debug.LogWarning("No keyboard connected!");
                return false;
            }
            return true;
        }

        private bool UserHasGamePad()
        {
            myGamepad = Gamepad.current;
            if (myGamepad == null)
            {
                // Debug.LogWarning("No GamePad connected!");
                return false;
            }
            return true;
        }

        private bool UserHasMouse()
        {
            myMouse = Mouse.current;
            if (myMouse == null)
            {
                // Debug.LogWarning("No Mouse connected!");
                return false;
            }
            return true;
        }
        #endregion ValidationOfInputTypes


        #region ReadInputs
        private void ReadKeyboardInput()
        {
            if (myKeyboard == null)
            {
                Debug.LogError($"keyboard is not found, cannot read input.");
                return;
            }
            
            // horizontal movement
            if (myKeyboard.aKey.isPressed || myKeyboard.leftArrowKey.isPressed)
            {
                inputX -= 1f;
            }
            if (myKeyboard.dKey.isPressed || myKeyboard.rightArrowKey.isPressed)
            {
                inputX += 1f;
            }
            
            // vertical movement
            if (myKeyboard.wKey.isPressed || myKeyboard.upArrowKey.isPressed)
            {
                inputY += 1f;
            }
            if (myKeyboard.sKey.isPressed || myKeyboard.downArrowKey.isPressed)
            {
                inputY -= 1f;
            }
        }

        private void ReadGamepadInput()
        {
            if (myGamepad == null)
            {
                Debug.LogError($"Gamepad is not found, cannot read input.");
                return;
            }
            
            if (!Mathf.Approximately(myGamepad.rightStick.ReadValue().magnitude, 0))
            {
                gamepadDirection = myGamepad.rightStick.ReadValue();
                inputX += gamepadDirection.x;
                inputY += gamepadDirection.y;
            }
        }

        private void ReadMouseInput()
        {
            if (myMouse == null)
            {
                Debug.LogError($"Mouse is not found, cannot read input.");
                return;
            }
        }
        #endregion ReadInputs

        #region MonoBehaviourMethods
        private void Awake()
        {
            myRigidbody = GetComponent(typeof(Rigidbody)) as Rigidbody; 
        }

        private void Update()
        {
            ReadInputsTypes();
            inputX = 0;
            inputY = 0;
            
            if (keyboardConnected) { ReadKeyboardInput(); }
            if (gamepadConnected) { ReadGamepadInput(); }
            if (mouseConnected) { ReadMouseInput(); }
            
            inputDirection = new Vector2(inputX, inputY).normalized;
        }

        private void FixedUpdate()
        {
            myRigidbody.linearVelocity = new Vector3(inputDirection.x, inputDirection.y, 0) * speed;
        }

        #endregion MonoBehaviourMethods
    }
}

