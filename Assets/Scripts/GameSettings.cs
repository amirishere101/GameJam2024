using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour{
    [Header("Physics")]
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public float objectKnockBack;
    public float outOfBoundsKnockBack;
    public float maxOutOfBoundsKnockback;
    public float minOutOfBoundsKnockBack;
    [Header("Values")]
    public float squirrelSpeedBoost;
    public int numPlayersExpected;
    
    [Header("Cooldowns")]
    public float dashCooldown;

    void Start(){
    
    }

    public void PauseGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
    

}
