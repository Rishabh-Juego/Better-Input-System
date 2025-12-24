using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// uses a reference to an InputActionAsset ScriptableObject to get action maps and actions<br/>
/// The names of action maps and actions are hardcoded here for demonstration purposes<br/>
/// This is a very bad way to reference actions since any name change in the asset will break this script<br/>
/// but it shows how to use InputActionAsset references in code
/// </summary>
public class UsingInputActionAssetReference : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputAction attackAction;
    private InputAction moveAction;
    private InputActionMap playerNormalActionMap;

    private void Awake()
    {
        GetAllActionReferences();
        AddListenersToActions();
    }

    private void OnEnable()
    {
        // inputActionAsset.Enable();
        //playerNormalActionMap.Enable();
        attackAction.Enable();
        moveAction.Enable();
    }

    private void OnDisable()
    {
        // inputActionAsset.Disable();
        // playerNormalActionMap.Disable();
        attackAction.Disable();
        moveAction.Disable();
    }

    private void OnDestroy()
    {
        RemoveListenersFromActions();
    }
    
    private void GetAllActionReferences()
    {
        playerNormalActionMap = inputActionAsset.FindActionMap("playerNormal");
        attackAction = playerNormalActionMap.FindAction("Attack");
        moveAction = playerNormalActionMap.FindAction("MoveHorizontal");
    }
    
    private void AddListenersToActions()
    {
        attackAction.started += AttackStarted;
        attackAction.performed += AttackPerformed;
        attackAction.canceled += AttackCancelled;

        moveAction.performed += Move;
    }
    
    private void RemoveListenersFromActions()
    {
        attackAction.started -= AttackStarted;
        attackAction.performed -= AttackPerformed;
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
