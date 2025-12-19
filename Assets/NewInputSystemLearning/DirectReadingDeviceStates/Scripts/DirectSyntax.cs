using UnityEngine;
using UnityEngine.InputSystem;

namespace TGL.Learning.NewInputSystem.DirectReadingDeviceStates
{
    public class DirectSyntax : MonoBehaviour
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
            if (myKeyboard.spaceKey.wasPressedThisFrame) // Input.GetKeyDown(KeyCode.Space)
            {
                Debug.Log($"Space was pressed this frame at {Time.time} seconds.");
            }
            else if (myKeyboard.spaceKey.isPressed)  // Input.GetKey(KeyCode.Space)
            {
                Debug.Log($"Space is currently pressed.");
            }
            else if (myKeyboard.spaceKey.wasReleasedThisFrame)  // Input.GetKeyUp(KeyCode.Space)
            {
                Debug.Log($"Space was released this frame at {Time.time} seconds.");
            }
            else if(myKeyboard.anyKey.wasPressedThisFrame)
            {
                Debug.Log($"Some key was pressed this frame at {Time.time} seconds.");
            }
        }

        private void ReadGamepadInput()
        {
            if (myGamepad == null)
            {
                Debug.LogError($"Gamepad is not found, cannot read input.");
                return;
            }
            if (myGamepad.buttonWest.wasPressedThisFrame) // Old: Input.GetKeyDown(KeyCode.JoystickButton2) - Xbox X / PS Square
            {
                Debug.Log($"Gamepad West Button was pressed this frame at {Time.time} seconds.");
            }
            else if (myGamepad.dpad.left.wasPressedThisFrame)  // Old: Input.GetKeyDown(KeyCode.JoystickButton6)
            {
                Debug.Log($"Gamepad DPad left Button is pressed this frame.");
            }
            else if (!Mathf.Approximately(myGamepad.leftStick.ReadValue().magnitude, 0))  // Old: Input.GetAxis("Horizontal") and Input.GetAxis("Vertical")
            {
                Debug.Log($"Gamepad Left Stick is being moved. Value: {myGamepad.leftStick.ReadValue()}");
            }
        }

        private void ReadMouseInput()
        {
            if (myMouse == null)
            {
                Debug.LogError($"Mouse is not found, cannot read input.");
                return;
            }

            // button click
            if (myMouse.leftButton.wasPressedThisFrame) // Input.GetmouseButtonDown(0)
            {
                Debug.Log($"Mouse Left Button was pressed this frame at {Time.time} seconds.");
            }
            
            // Scrolling
            if (!Mathf.Approximately(myMouse.scroll.ReadValue().y, 0)) // Input.GetAxis("Mouse ScrollWheel")
            {
                Debug.Log($"Mouse Scroll is being moved vertically. Value: {myMouse.scroll.ReadValue().y}");
            }
            
            // mouse position
            if (Vector2.Distance(mousePosition, myMouse.position.ReadValue()) > 0.1f) // Input.mousePosition
            {
                Debug.Log($"Mouse Position: {myMouse.position.ReadValue()}");
                mousePosition = myMouse.position.ReadValue();
            }
        }

        #endregion ReadInputs

        #region MonoBehaviourMethods

        private void Update()
        {
            ReadInputsTypes();
            
            if (keyboardConnected)
            {
                ReadKeyboardInput();
            }

            if (gamepadConnected)
            {
                ReadGamepadInput();
            }

            if (mouseConnected)
            {
                ReadMouseInput();
            }
        }

        #endregion MonoBehaviourMethods
    }
}