using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    [Header("Movement")]
    public float crankyAcceleration = 15;
    public float normalAcceleration = 23;
    public float excitedAcceleration = 30;
    public float crankyBumpPower = 125;
    public float normalBumpPower = 175;
    public float excitedBumpPower = 225;
    public float acceleration;
    public float startingMaxSpeed;
    public float currentMaxSpeed;
    public float dashPower;
    public float currentBumpPower;
    public float drag;

    [Header("Flags")]
    public DogState currentState;
    public bool canMove;
    public bool canDash;

    [Header("Inputs")]
    public float currentSpeed;
    public float horizontalInput;
    public float verticalInput;
    [Header("Scores")]
    public int numSquirrels = 0;
    public int numSquirrelsNormal = 1;
    public int numSquirrelsExcited = 2;
    public GameSettings settings;

    public enum DogState{
        Cranky, Normal, Excited
    } 

    void OnEnable(){
        currentMaxSpeed = startingMaxSpeed;
        currentState = DogState.Cranky;
        acceleration = crankyAcceleration;
        currentBumpPower = crankyBumpPower;
    }

    public void CheckChangeState(){
        if(numSquirrels >= numSquirrelsExcited){
            currentState = DogState.Excited;
            acceleration = excitedAcceleration;
            currentBumpPower = excitedBumpPower;
        } else if(numSquirrels >= numSquirrelsNormal){
            currentState = DogState.Normal;
            acceleration = normalAcceleration;
            currentBumpPower = normalBumpPower;
        } else {
            currentState = DogState.Cranky;
            acceleration = crankyAcceleration;
            currentBumpPower = crankyBumpPower;
        }
    }

    public void SubtractSquirrels(int numToSubtract){
        numSquirrels -= numToSubtract;
        if(numSquirrels < 0){
            numSquirrels = 0;
        }
    }

    private void FixedUpdate() {
        currentMaxSpeed = startingMaxSpeed + (numSquirrels * settings.squirrelSpeedBoost);
    }
}
