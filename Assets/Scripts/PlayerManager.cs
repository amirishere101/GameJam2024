using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour {
    public List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> startingPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private GameSettings settings;
    [SerializeField] private WaitingForPlayers waitingPlayers;

    void Start(){
        settings.PauseGame();
        inputManager.DisableJoining();
    }

    public void AddPlayer(PlayerInput player){
        players.Add(player);
        Debug.Log("player joined: " + players.Count + " in lobby. " + settings.numPlayersExpected + " player(s) expected");

        player.transform.parent.position = startingPoints[players.Count - 1].position;

        if(players.Count == settings.numPlayersExpected){
            waitingPlayers.StartGame();
        }
    }

}
