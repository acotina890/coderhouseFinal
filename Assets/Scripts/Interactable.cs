using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private enum TypeOfInteractableObject{
        ammo,
        MedKit,
        weapon,
        doors
    }

    [SerializeField] private TypeOfInteractableObject typeOfInteractableObject;
    public Transform Player;
    public float distance;
    public int caseNumber;

    void Update(){
        distance = Vector3.Distance(Player.position, transform.position);
        if(caseNumber == 1){
            MedKit();
        }
    }

    private void SetInteractableOptions(TypeOfInteractableObject typeOfInteractableObject){
        switch (typeOfInteractableObject)
        {
            case TypeOfInteractableObject.ammo:
                caseNumber = 0;
                break;
            case TypeOfInteractableObject.MedKit:
                caseNumber = 1;
                break;
            case TypeOfInteractableObject.weapon:
                caseNumber = 2;
                break;
            case TypeOfInteractableObject.doors:
                caseNumber = 3;
                break;
        }
    }

    private void MedKit(){
        if(distance <= 2.5f && Input.GetKeyDown(KeyCode.E)){
            Destroy(gameObject, 1f);
            PlayerMovement.Healed = true;
            Player.GetComponent<PlayerMovement>().Interact();
        }
    }
}
