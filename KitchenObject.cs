using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    // the script/compoenent to place on prefabs for kitchen objects -


    // scriptable objects on a prefab, (i.e. referencing one of itself in a way, is fine because it carries other data about it)
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent newParent) {

        //first tell the current parent to clear its kitchen object
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        // set the parent of the kitchenObject
        this.kitchenObjectParent = newParent;
        // and tell that parent to set the kitchen as this
        newParent.SetKitchenObject(this);

        transform.parent = newParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public void SwitchKitchenObjectParent(IKitchenObjectParent newParent) {

        // store the new parents object
        KitchenObject newParentKitchenObject = newParent.GetKitchenObject();
        // store the current parent and delete whats inside
        IKitchenObjectParent oldParent = this.kitchenObjectParent;


        SetKitchenObjectParent(newParent);

        // Sert the old parent with the new parents object
        oldParent.SetKitchenObject(newParentKitchenObject);
        newParentKitchenObject.kitchenObjectParent = oldParent;
        //transform the newParents object to the old parent
        newParentKitchenObject.transform.parent = oldParent.GetKitchenObjectFollowTransform();
        newParentKitchenObject.transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }

    public void DestroySelf(){
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent){
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

}
