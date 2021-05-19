using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    PlayerMovement pm;
    [SerializeField] GameObject panel; 

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventoryPanel;
    bool active = false;

    private void Start() {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();

        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){

            if(active){
                inventory.SetActive(true);
                Time.timeScale = 1;
                pm.canToggleInventory = true;
                if(!inventoryPanel.activeSelf){
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    pm.canMove = true;
                }
                panel.SetActive(true);
                pauseMenu.SetActive(false);
                active = false;
            } else {
                pm.canMove = false;
                pm.canToggleInventory = false;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                inventory.SetActive(false);
                panel.SetActive(false);
                pauseMenu.SetActive(true);
                active = true;
            }
        }
    }

    public void QuitGame(){
        SceneManager.LoadScene(0);
    }
}
