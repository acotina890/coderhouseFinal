using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] weapons;

    public void ActivateWeapon(int number){
        for(int i = 0; i < weapons.Length; i++){
            weapons[i].SetActive(false);
        }
        weapons[number].SetActive(true);
    }
}
