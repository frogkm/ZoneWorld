using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{

    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private string interactText = "Press {0} to interact";

    public void Interact() {
        onInteract.Invoke();
    }

    public string getInteractText(KeyCode interactKey) {
        return string.Format(interactText, interactKey.ToString());
    }


}
