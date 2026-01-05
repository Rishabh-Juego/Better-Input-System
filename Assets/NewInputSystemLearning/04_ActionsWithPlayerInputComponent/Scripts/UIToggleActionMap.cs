using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UIToggleActionMap : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private string uiActionMapName;
    [HideInInspector, SerializeField] private InputActionMap uiActionMap;
    [SerializeField] private string uiToggleActionMapName;
    [HideInInspector, SerializeField] private InputActionMap uiToggleActionMap;
    [SerializeField] private string uiToggleActionName;
    [HideInInspector, SerializeField] private InputAction uiToggleAction;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private bool showingUiMap = false;
    private InputActionMap savedActionMap;

    private void Awake()
    {
        playerInput = GetComponent(typeof(PlayerInput)) as PlayerInput;
        uiActionMap = playerInput.actions.FindActionMap(uiActionMapName);
        if (uiActionMap == null)
        {
            Debug.LogError($"No UI action map found on {gameObject.name} with name as '{uiActionMapName}'", this);
            return;
        }
        uiToggleActionMap = playerInput.actions.FindActionMap(uiToggleActionMapName);
        if (uiToggleActionMap == null)
        {
            Debug.LogError($"No UI Toggle action map found on {gameObject.name} with name as '{uiToggleActionMapName}'", this);
            return;
        }
        uiToggleAction = uiToggleActionMap.FindAction(uiToggleActionName);
        if (uiToggleAction == null)
        {
            Debug.LogError($"No UI Toggle action found on {gameObject.name} with name as '{uiToggleActionName}'", this);
            return;
        }
        savedActionMap = null;
        uiToggleAction.performed += ToggleUiActionMapPerformed;
        uiCanvas.gameObject.SetActive(showingUiMap);
    }

    private void OnEnable()
    {
        uiToggleActionMap.Enable();
        uiActionMap.Enable();
    }

    private void OnDisable()
    {
        uiToggleActionMap.Disable();
        uiActionMap.Disable();
    }

    private void OnDestroy()
    {
        uiToggleAction.performed -= ToggleUiActionMapPerformed;
    }

    private void ToggleUiActionMapPerformed(InputAction.CallbackContext obj)
    {
        ToggleUiActionMap();
    }

    private void ToggleUiActionMap()
    {
        if (savedActionMap == uiActionMap)
        {
            Debug.LogError($"Saved action map is the same as UI action map on {gameObject.name}. Cannot toggle action maps.", this);
            return;
        }
        else 
        {
            if(showingUiMap)
            {
                playerInput.SwitchCurrentActionMap(savedActionMap.name);
                savedActionMap.Enable();
                Debug.Log($"Switched to {savedActionMap.name}", this);
                Debug.Log($"Now active map is {playerInput.currentActionMap.name}", this);
                savedActionMap = null;
                showingUiMap = false;
                uiActionMap.Disable();
            }
            else
            {
                savedActionMap = playerInput.currentActionMap;
                savedActionMap.Disable();
                playerInput.SwitchCurrentActionMap(uiActionMap.name);
                Debug.Log($"Switched to {uiActionMap.name}", this);
                Debug.Log($"Now active map is {playerInput.currentActionMap.name}", this);
                showingUiMap = true;
                uiActionMap.Enable();
            }
            uiCanvas.gameObject.SetActive(showingUiMap);
        }
    }
}
