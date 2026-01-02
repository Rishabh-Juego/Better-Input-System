using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TGL.Learning.NewInputSystem.UnityActions
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveObjectByEmbedSyntax : MonoBehaviour
    {
        [SerializeField] private InputAction moveAction;
        [SerializeField] private float moveSpeed;
        [Range(0.0f, 1.0f), SerializeField] private float dampenFactor = 0.2f;
        private Rigidbody myRigidbody;
        private Vector2 moveDirection;
        
        #region MonobehaviourCallbacks

        private void Awake()
        {
            myRigidbody = GetComponent(typeof(Rigidbody)) as Rigidbody;
            moveAction.performed += Move;
            moveAction.canceled += StopMoving;
        }

        private void OnEnable()
        {
            moveAction.Enable();
        }

        private void Update()
        {
            if (Mathf.Approximately(moveAction.ReadValue<Vector2>().magnitude, 0))
            {
                DampenMovement();
            }
        }

        private void OnDisable()
        {
            moveAction.Disable();
        }

        private void OnDestroy()
        {
            moveAction.performed -= Move;
            moveAction.canceled -= StopMoving;
        }
        #endregion MonobehaviourCallbacks
        
        private void Move(InputAction.CallbackContext actionContext)
        {
            moveDirection = actionContext.ReadValue<Vector2>().normalized;
            MoveCharacter();
        }
        private void StopMoving(InputAction.CallbackContext actionContext)
        {
            moveDirection = Vector2.zero;
            DampenMovement();
        }

        private void DampenMovement()
        {
            myRigidbody.linearVelocity = myRigidbody.linearVelocity * (1-dampenFactor);
        }

        private void MoveCharacter()
        {
            myRigidbody.linearVelocity = moveDirection * moveSpeed;
        }
    }
}