using UnityEngine;

//Kitchen object parents are the counters or the player
public interface IKitchenObjectParent
{
    // all the functions that make a KitchenObjectPArent work
    public Transform GetKitchenObjectFollowTransform();
    public void ClearKitchenObject();
    public void SetKitchenObject(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObject();
    public bool HasKitchenObject();

}
