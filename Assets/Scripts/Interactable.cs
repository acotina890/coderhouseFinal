using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    private enum TypeOfInteractableObject{
        ammo,
        MedKit,
        axe,
        doors,
        gasGallon,
        rifle
    }

    [SerializeField] private TypeOfInteractableObject typeOfInteractableObject;
    public Transform Player;
    private Inventory inventoryScript;
    public float distance;
    public int caseNumber;


    public static int gasGallonsCollected = 0;
    public TextMeshProUGUI gasGallonsQuantityText;

    void Start(){
        gameObject.SetActive(true);
        inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update(){
        distance = Vector3.Distance(Player.position, transform.position);
        if(caseNumber == 1){
            MedKit();
        }
        else if(caseNumber == 2){
            axe();
        }
        else if(caseNumber == 4){
            gasGallon();
        }
        else if(caseNumber == 5){
            rifle();
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
            case TypeOfInteractableObject.axe:
                caseNumber = 2;
                break;
            case TypeOfInteractableObject.doors:
                caseNumber = 3;
                break;
            case TypeOfInteractableObject.gasGallon:
                caseNumber = 4;
                break;
            case TypeOfInteractableObject.rifle:
                caseNumber = 5;
                break;
        }
    }

    private void MedKit(){
        if(distance <= 2.5f && Input.GetKeyDown(KeyCode.E)){
            Destroy(gameObject, 1f);
            PlayerMovement.health = 255;
            PlayerMovement.Healed = true;
            Player.GetComponent<PlayerMovement>().Interact();
        }
    }

    private void gasGallon(){
        if(distance <= 2.5f && Input.GetKeyDown(KeyCode.E)){
            gameObject.SetActive(false);
            gasGallonsCollected++;
            gasGallonsQuantityText.text = gasGallonsCollected.ToString();
            Player.GetComponent<PlayerMovement>().Interact();
        }
        if(gasGallonsCollected > 9){
            Destroy(gameObject);
        }
    }

    private void axe(){
        if(distance <= 2.5f && Input.GetKeyDown(KeyCode.E)){
            Destroy(gameObject, 1f);
            inventoryScript.ActivateWeapon(0);
            Player.GetComponent<PlayerMovement>().Interact();
            Player.GetComponent<PlayerMovement>().ActivateAxe();
        }
    }

    private void rifle(){
        if(distance <= 2.5f && Input.GetKeyDown(KeyCode.E)){
            Destroy(gameObject, 1f);
            inventoryScript.ActivateWeapon(1);
            Player.GetComponent<PlayerMovement>().Interact();
            Player.GetComponent<PlayerMovement>().ActivateRifle();
        }
    }
}
