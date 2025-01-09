using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractPerformed;
    public event EventHandler OnInteractAlternatePerformed;

    // the Game Input needs to be continously reading input and creating a vector
    private Vector2 inputVector;
    // to use the new input system, need to create an instance and enable it - do it on awake
    private InputSystem_Actions playerInputActions;



    private void Awake() {
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();

        //subscribe the interact event to an PlayerInteractAction
        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
    }


    private void Start() {
    }

    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        OnInteractPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void InteractAlternate_Performed(InputAction.CallbackContext context)
    {
        OnInteractAlternatePerformed?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVectorNormalized() {
        // This essentially creates a new vector every frame and 
        inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // need to normalize the diagonal with the normalized property
        inputVector = inputVector.normalized;

        return inputVector;
    }

}
