using System.Runtime.CompilerServices;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player){
        // add logic to put and take objects
        if (!HasKitchenObject()) {
            //counter doesnt have kitchen object

                if (player.HasKitchenObject()) {
                    //player has a kitchenobject - give it to the counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                } else {
                    // player has nothing - nothing happens
                } 
    
        } else {
            //Counter has kitchen object
            if (player.HasKitchenObject()) {
                // switch
                this.GetKitchenObject().SwitchKitchenObjectParent(player);

            } else {
                // player does not have an object - give object to player
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }

        }

    }
}
