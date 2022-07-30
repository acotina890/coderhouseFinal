using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    private bool pauseActive;
    public GameObject pauseMenu;

    void Update()
    {
        TogglePause();
    }

    private void TogglePause(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseActive){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }

    private void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pauseActive = false;
    }

    private void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        pauseActive = true;
    }
}
