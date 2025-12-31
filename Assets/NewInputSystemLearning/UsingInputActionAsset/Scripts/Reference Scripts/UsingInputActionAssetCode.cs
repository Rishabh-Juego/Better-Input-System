using UnityEngine;
using UnityEngine.InputSystem;


namespace TGL.Learning.NewInputSystem.UsingInputActionAsset
{
    public class UsingInputActionAssetCode : MonoBehaviour
    {
        private InputControls inputControls;

        private InputAction attackAction;
        private InputAction moveAction;

        /// <summary>
        /// default type, can be used just like any InputActionMap
        /// </summary>
        private InputActionMap playerNormalActionMap;

        /// <summary>
        /// strongly typed version, this allows us to access actions directly as a datatype.
        /// </summary>
        private InputControls.PlayerNormalActions _playerNormalActionMap;

        private void Awake()
        {
            GetAllActionReferences();
            AddListenersToActions();
        }

        private void OnEnable()
        {
            // inputControls.Enable();
            // inputControls.playerNormal.Enable();
            // playerNormalActionMap.Enable();
            // _playerNormalActionMap.Enable();
            // inputControls.playerNormal.MoveHorizontal.Enable();
            attackAction.Enable();
            moveAction.Enable();
        }

        private void OnDisable()
        {
            // inputControls.Disable();
            // inputControls.playerNormal.Disable();
            // playerNormalActionMap.Disable();
            // _playerNormalActionMap.Disable();
            // inputControls.playerNormal.MoveHorizontal.Disable();
            attackAction.Disable();
            moveAction.Disable();
        }

        private void OnDestroy()
        {
            RemoveListenersFromActions();
        }

        private void GetAllActionReferences()
        {
            inputControls = new InputControls();
            playerNormalActionMap = inputControls.playerNormal; // default type
            _playerNormalActionMap = inputControls.playerNormal; // strongly typed version
            attackAction = playerNormalActionMap.FindAction("Attack"); // you can also use: inputControls.playerNormal.Attack
            moveAction = _playerNormalActionMap.MoveHorizontal; // you can also use: playerNormalActionMap.FindAction("MoveHorizontal");
        }

        private void AddListenersToActions()
        {
            _playerNormalActionMap.Attack.started += AttackStarted; // we can use strongly typed version here
            inputControls.playerNormal.Attack.performed += AttackPerformed;
            attackAction.canceled += AttackCancelled; // we can use default type here as well

            moveAction.performed += Move;
        }

        private void RemoveListenersFromActions()
        {
            _playerNormalActionMap.Attack.started -= AttackStarted;
            inputControls.playerNormal.Attack.performed -= AttackPerformed;
            attackAction.canceled -= AttackCancelled;

            moveAction.performed -= Move;
        }


        private void AttackStarted(InputAction.CallbackContext context)
        {
            Debug.Log("Attack Started");
        }

        private void AttackPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Attack Performed");
        }

        private void AttackCancelled(InputAction.CallbackContext context)
        {
            Debug.Log("Attack Cancelled");
        }

        private void Move(InputAction.CallbackContext context)
        {
            Debug.Log($"Move towards {context.ReadValue<Vector2>()} direction");
        }
    }
}