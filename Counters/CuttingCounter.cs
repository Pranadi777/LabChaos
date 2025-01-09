using System;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs {
        public float cuttingProgressNormalized;
    }
    public event EventHandler OnCut;


    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        // add logic to put and take objects
        if (!HasKitchenObject()) {
            //counter doesnt have kitchen object

                if (player.HasKitchenObject()) {
                    //player has a kitchenobject - give it to the counter  - if the object carried can be switched
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                        // player carruing object has a cutting reicpe
                        player.GetKitchenObject().SetKitchenObjectParent(this);

                        ResetCuttingProgress();

                    }
                } else {
                    // player has nothing - nothing happens
                } 
    
        } else {
            //Counter has kitchen object
            if (player.HasKitchenObject()) {
                // switch - if the object carried can be switched
                this.GetKitchenObject().SwitchKitchenObjectParent(player);

                ResetCuttingProgress();

            } else {
                // player does not have an object - give object to player
                this.GetKitchenObject().SetKitchenObjectParent(player);
                ResetCuttingProgress();
            }

        }
    }

    public override void InteractAlternate(Player player) {
        // If the object can be cut
        // destroy the object and spawn the cut
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {

            OnCut?.Invoke(this, EventArgs.Empty);

            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{
                cuttingProgressNormalized = (float) cuttingProgress/cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax){

                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // dont get the kitchenobject with this.kitchenObject, becasue the base class already has it, and use a function to get it
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO){
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO){
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (inputKitchenObjectSO == cuttingRecipeSO.input) {
                return cuttingRecipeSO;
            }
        }
        return null; 
    }

    private void ResetCuttingProgress(){
        cuttingProgress = 0;

        // set the progress to zero 
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{ cuttingProgressNormalized = 0});
    }

}
