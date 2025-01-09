using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{


    //singleton - static makes the variable belong to the class itself - It is a property by the way. must set it in the awake
    public static Player Instance{ get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }



    [SerializeField] GameInput gameInput;
    [SerializeField] private int moveSpeed;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldingPoint;



    private bool isWalking;
    private UnityEngine.Vector3 lastInteractDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("there is more than one player instance");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractPerformed += GameInput_OnInteractionPerformed;
        gameInput.OnInteractAlternatePerformed += GameInput_OnInteractionAlternatePerformed;
    }


    private void Update() {

        HandleMovement();
        HandleInteractions();
    }

    private void GameInput_OnInteractionPerformed(object sender, EventArgs e)
    {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractionAlternatePerformed(object sender, EventArgs e)
    {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }    
    }

    private void HandleInteractions() {
        // need to ray cast infront of us and detect the objects - based on the inputVector and MoveDir from movemoent
        UnityEngine.Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // convert the vector2 from inputs to vector3 
        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x, 0f, inputVector.y);

        //moveDir will be zero if no movement, but we want to maintain the line os sight in front of us
        if (moveDir != UnityEngine.Vector3.zero) {
            lastInteractDirection = moveDir;
        }

        //raycat and return any hit transfor
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit rayCastHit, interactDistance, countersLayerMask)) {
            // check if the interaction is a base counter
            if (rayCastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (selectedCounter != baseCounter) {
                    SetSelectedCounter(baseCounter);
                }

            } else {
                //there is no counter in front of the player
                SetSelectedCounter(null);
            }
        } else {
            //there is nothing in front of the player
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
            this.selectedCounter = selectedCounter;

            OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
                selectedCounter = selectedCounter
            });
    }

    private void HandleMovement() {

        // update this (player) position vector with the gameInput moveDir
        UnityEngine.Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // convert the vector2 from inputs to vector3 
        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x, 0f, inputVector.y);

        // If moveDir is not zero, then this motherfucker is walking
        isWalking = moveDir != UnityEngine.Vector3.zero;

        // return a bool based on a raycast - if raycast is false, canMove
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + UnityEngine.Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        // whenmoving diagnal agaisnt a surf, will not move, but should move along the side
        if (!canMove) {
            //check to see if you can move on the x
            UnityEngine.Vector3 moveDirX = new UnityEngine.Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDirX.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + UnityEngine.Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                moveDir = moveDirX;

            }  else {
                //try moving in the y direction
                UnityEngine.Vector3 moveDirZ = new UnityEngine.Vector3(0f, 0f, moveDir.z).normalized;
                canMove = moveDirX.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + UnityEngine.Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }
        
        if (canMove) {
            // add to the transform position vector 
            this.transform.position += moveDir * moveSpeed * Time.deltaTime;
        }


        int rotateSpeed = 10;
        // change the direction of the transform.forward
        this.transform.forward = UnityEngine.Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    public bool IsWalking() {
        return isWalking;
    }




    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldingPoint;
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
