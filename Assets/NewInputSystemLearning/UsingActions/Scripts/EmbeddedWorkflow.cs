using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TGL.Learning.NewInputSystem.UnityActions
{
    public class EmbeddedWorkflow : MonoBehaviour
    {
        [SerializeField] private InputAction jumpAction;
        [SerializeField] private InputAction moveAction;
        
        [Space(10), SerializeField] private bool isJumpingInstructionGiven;
        [Space(10), SerializeField] private bool isMovingInstructionGiven;

        #region MonobehaviourCallbacks

        private void Awake()
        {
            jumpAction.started += JumpStarted;
            jumpAction.performed += JumpPerformed;
            jumpAction.canceled += JumpCancelled;

            moveAction.performed += Move;
            // moveAction.canceled += StopMoving;
        }

        private void OnEnable()
        {
            jumpAction.Enable();
            moveAction.Enable();
        }

        private void Update()
        {
            if(Mathf.Approximately(moveAction.ReadValue<Vector2>().magnitude, 0) && isMovingInstructionGiven)
            {
                isMovingInstructionGiven = false;
            }
        }

        private void OnDisable()
        {
            jumpAction.Disable();
            moveAction.Disable();
        }

        private void OnDestroy()
        {
            jumpAction.started -= JumpStarted;
            jumpAction.performed -= JumpPerformed;
            jumpAction.canceled -= JumpCancelled;
            
            moveAction.performed -= Move;
            // moveAction.canceled -= StopMoving;
        }
        #endregion MonobehaviourCallbacks

        private void Move(InputAction.CallbackContext actionContext)
        {
            Debug.Log($"Move towards {actionContext.ReadValue<Vector2>()} direction");
            isMovingInstructionGiven = true;
        }
        // private void StopMoving(InputAction.CallbackContext actionContext)
        // {
        //     Debug.Log($"Move towards {actionContext.ReadValue<Vector2>()} direction");
        //     isMovingInstructionGiven = false;
        // }
        
        
        private void JumpStarted(InputAction.CallbackContext actionContext)
        {
            // isJumpingInstructionGiven = true;
            isJumpingInstructionGiven = actionContext.ReadValueAsButton(); 
            // while button is pressed, the current value is higher than threshold, so result bool is true
            Debug.Log($"Jump Started! - {Time.time}");
        }
        
        private void JumpPerformed(InputAction.CallbackContext actionContext)
        {
            Debug.Log($"Jumped! - {Time.time}");
        }
        
        private void JumpCancelled(InputAction.CallbackContext actionContext)
        {
            // isJumpingInstructionGiven = false;
            isJumpingInstructionGiven = actionContext.ReadValueAsButton();  
            // after button is released, the threshold is higher than current value, so result bool is false
            Debug.Log($"Jump Cancelled! - {Time.time}");
        }
    }
}