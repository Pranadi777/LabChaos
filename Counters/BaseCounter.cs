using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] Transform counterTopPoint;

    private KitchenObject kitchenObject;



    public virtual void Interact(Player player){
        //This function from the base class should never be called
    }

    public virtual void InteractAlternate(Player player){
        
    }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
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
