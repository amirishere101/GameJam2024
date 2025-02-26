using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaitingForPlayers : MonoBehaviour
{
     [SerializeField] GameObject obj1, obj2;

   public Canvas waitingForPlayersScreen, askNumPlayersScreen, playerUI;
   public GameSettings settings;
   public PlayerInputManager inputManager;
   public PlayerManager playerManager;

   void OnEnable(){
        waitingForPlayersScreen.enabled = false;
        askNumPlayersScreen.enabled = true;
        if(playerUI != null){
          playerUI.enabled = false;
        }
   }

   public void WaitForPlayers(){
        inputManager.EnableJoining();
        waitingForPlayersScreen.enabled = true;
        askNumPlayersScreen.enabled = false;
        settings.PauseGame();
        if(playerManager.players.Count == settings.numPlayersExpected){
            StartGame();
        }
   }

   public void StartGame(){
     if(playerUI != null){
          playerUI.enabled = true;
     }
        inputManager.DisableJoining();
        waitingForPlayersScreen.enabled = false;
        askNumPlayersScreen.enabled = false;
        Destroy(obj1);
        Destroy(obj2);
        settings.ResumeGame();
   }

   public void askNumPlayers(){
        inputManager.DisableJoining();
        askNumPlayersScreen.enabled = true;
        waitingForPlayersScreen.enabled = false;
        settings.PauseGame();
   }

   public void UpdateNumPlayers(int numPlayers){
    settings.numPlayersExpected = numPlayers;
   }
}
