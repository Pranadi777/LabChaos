using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

// // Note about the set up
// SelectedCounter is Justify a copy of the ClearCounterVisual 1% larger
// Selected is not unchecked, but the KitchenCounter child is
// ThemeStyleSheet KitchenCounter will have the MAterial for Select attached onit


public class SelectedCounter : MonoBehaviour
{


    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start() {
        Player.Instance.OnSelectedCounterChanged += PlayerInstance_OnSelectedCounterEvent;
    }

    private void PlayerInstance_OnSelectedCounterEvent(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (baseCounter == e.selectedCounter){
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
