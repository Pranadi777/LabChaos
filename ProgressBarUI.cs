using System;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{

    // The idea is that cutting progress sends an event with the actual value as the progress
    // Need to reference the cutting counter and the bar ui

    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private UnityEngine.UI.Image barImage;


    // Listen to when the cuttingcounter it is associated with sends the event
    private void Start(){
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;

        //on game start initialize to zero
        barImage.fillAmount = 0;

        // hide on start
        Hide();
    }

    private void CuttingCounter_OnProgressChanged(object sender, CuttingCounter.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.cuttingProgressNormalized;

        if (e.cuttingProgressNormalized == 0 || e.cuttingProgressNormalized == 1f){
            Hide();
        } else {
            Show();
        }
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }
}
