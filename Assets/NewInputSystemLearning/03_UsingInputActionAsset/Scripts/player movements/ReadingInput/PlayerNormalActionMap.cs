using System;
using UnityEngine;

public class PlayerNormalActionMap : MonoBehaviour
{
    private InputControls inputControls;
    private InputControls.PlayerNormalActions playerNormalActionMap;
    // Public Actions for other classes to listen to
    public Action PlayerNormalActionMapPerformJump;
    public Action<float> PlayerNormalActionMapMoveHorizontal;
    public Action<float> PlayerNormalActionMapMoveVertical;
    public Action PlayerNormalActionMapPerformAttack;

    #region Monobehaviour_Methodss
    private void Awake()
    {
        GetAllActionReferences();
        AddListenersToActions();
    }
    
    private void OnEnable()
    {
        playerNormalActionMap.Enable();
    }

    private void Update()
    {
        //Read input and call the actions
        ProcessMovementInput();
    }

    private void OnDisable()
    {
        playerNormalActionMap.Disable();
    }
    
    private void OnDestroy()
    {
        RemoveListenersFromActions();
    }
    #endregion Monobehaviour_Methodss

    #region Action_Listeners
    private void GetAllActionReferences()
    {
        inputControls = new InputControls();
        playerNormalActionMap = inputControls.playerNormal;
    }
    
    private void AddListenersToActions()
    {
        playerNormalActionMap.Jump.performed += JumpPerformed;
        playerNormalActionMap.Attack.performed += AttackPerformed;
    }
    
    private void RemoveListenersFromActions()
    {
        playerNormalActionMap.Jump.performed -= JumpPerformed;
        playerNormalActionMap.Attack.performed -= AttackPerformed;
    }
    #endregion Action_Listeners
    
    #region Action_Methods

    private void JumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        PlayerNormalActionMapPerformJump?.Invoke();
    }
    
    private void AttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        PlayerNormalActionMapPerformAttack?.Invoke();
    }

    private void ProcessMovementInput()
    {
        float horizontalAxisVal = playerNormalActionMap.MoveHorizontal.ReadValue<float>();
        float verticalAxisVal = playerNormalActionMap.MoveVertical.ReadValue<float>();
        
        if (horizontalAxisVal != 0)
        {
            PlayerNormalActionMapMoveHorizontal?.Invoke(horizontalAxisVal);
        }

        if (verticalAxisVal != 0)
        {
            PlayerNormalActionMapMoveVertical?.Invoke(verticalAxisVal);
        }
    }
    #endregion Action_Methods
}
