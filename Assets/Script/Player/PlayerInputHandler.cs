using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController _playerController;
    
    private bool _isGroundPoundUnlocked;
    private bool _isDoubleJumpUnlocked;
    private bool _isDoubleDashUnlocked;
    private bool _isInteracting;
    
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        InputManager.actionMap.PlayerInput.Interaction.started += InteractStarted;
        InputManager.actionMap.PlayerInput.Interaction.canceled += InteractCanceled;
    }
    private void OnDisable()
    {
        InputManager.actionMap.PlayerInput.Interaction.performed -= InteractStarted;
        InputManager.actionMap.PlayerInput.Interaction.canceled -= InteractCanceled;
    }
    
    

    private void InteractStarted(UnityEngine.InputSystem.InputAction.CallbackContext interact)
    {
        _isInteracting = true;
    }
    private void InteractCanceled(UnityEngine.InputSystem.InputAction.CallbackContext nullInteract)
    {
        _isInteracting = false;
    }
    
    
}
