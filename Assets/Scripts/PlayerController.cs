using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] GameSettings settings;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject playerObject;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Animator animator;
    Vector3 _input;
    Vector3 forward, right;
    bool capVelocityY;

    public AudioSource src;
    public AudioClip dashSfx;

    // Start is called before the first frame update
    void Start(){
        playerStats.canMove = true;
        playerStats.canDash = true;
        rb.freezeRotation = true;
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        right = Quaternion.Euler(0, 90, 0) * forward;
    }

    //use for all physics based things
    void FixedUpdate(){
        //read movementInput and then move the player
        _input = new Vector3(playerStats.horizontalInput, 0, playerStats.verticalInput);
        if(playerStats.canMove){
            MovePlayer();
        }

        //Cap the players upwards velocity so they do not fly out of the map
        if(rb.velocity.y > playerStats.currentMaxSpeed){
            rb.velocity = new Vector3(rb.velocity.x, playerStats.currentMaxSpeed/4, rb.velocity.z);
        }

        //Soft cap the players upwards max height
        if(transform.position.y > 15 && capVelocityY){
            //maybe add a lerp to the gravity so its not so sudden???
            rb.velocity = rb.velocity = new Vector3(rb.velocity.x, Physics.gravity.y, rb.velocity.z);
        } 

        //Cap player velocity so they do not fly around the map like crazy
        if(rb.velocity.magnitude > playerStats.currentMaxSpeed){
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, playerStats.currentMaxSpeed * 2);
        }
    }

    void OnCollisionEnter(Collision other) {
        //when players collide with another player
        if(other.gameObject.tag == "Player"){
            Rigidbody p2Rb = other.rigidbody;
            Vector3 dir = other.transform.position - transform.position;
            dir = dir.normalized;
            p2Rb.AddForce(dir * playerStats.currentBumpPower * 10f);
        }
        
        //when players collide with a squirrel
        if(other.gameObject.tag == "Squirrel"){
            playerStats.numSquirrels++;
            playerStats.currentMaxSpeed += settings.squirrelSpeedBoost;
            SquirrelAI squirrelAI = other.gameObject.GetComponent<SquirrelAI>();
            playerStats.CheckChangeState();
            squirrelAI.ChangeDeathType();
            squirrelAI.Die();
            Debug.Log(squirrelAI.currentSquirrelBounceForce);
            rb.AddExplosionForce(squirrelAI.currentSquirrelBounceForce * 10, other.contacts[0].point, 10);
        }

        //when players collide with a game object that has the "Object" tag
        if(other.gameObject.tag == "Object"){
            float knockback = settings.objectKnockBack;
            rb.AddExplosionForce(knockback, other.contacts[0].point, 10);
        }
    }

    //handles players movement
    void MovePlayer(){
        rb.drag = playerStats.drag;
        Physics.gravity = settings.gravity;
        Vector3 rightMovement = right * CalculateAcceleration() * Time.deltaTime * playerStats.horizontalInput;
        Vector3 upMovement = forward * CalculateAcceleration() * Time.deltaTime * playerStats.verticalInput;
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        if(rb.velocity.magnitude <= playerStats.currentMaxSpeed){
            rb.AddForce(playerObject.transform.forward * CalculateAcceleration());
        }
        if(_input != Vector3.zero){
            transform.forward = heading;
        }
        playerStats.currentSpeed = rb.velocity.magnitude;
    }

    //is called after the dash cooldown has finished
    void AllowDash(){
        playerStats.canDash = true;
    }

    //calculates players acceleration based on physics factors
    float CalculateAcceleration(){
        if(playerStats.drag >= 1){
            return playerStats.drag * playerStats.acceleration * (Physics.gravity.y/-9.81f);
        }
        return playerStats.acceleration * (Physics.gravity.y/-9.81f); 
    }


    ///////////////////////
   /// *INPUT ACTIONS* ///
  ///////////////////////
  
  //OPEN THE REGIOHN IF IT IS CLOSED
    #region
    //reads input from keyboard
    public void Move(InputAction.CallbackContext _context){
        playerStats.horizontalInput = _context.ReadValue<Vector2>().x;
        playerStats.verticalInput = _context.ReadValue<Vector2>().y;
    }

    //called when the dash button is pressed on the keyboard
    public void Dash(){
        if(playerStats.canDash){
            //add code that you want to happen when the dash is pressed in here
            playerStats.canDash = false;
            Vector3 forceToApply = playerObject.transform.forward * playerStats.dashPower;
            rb.AddForce(forceToApply * 10f);
            Invoke(nameof(AllowDash), settings.dashCooldown);

            src.clip = dashSfx;
            src.Play();
        }
    }
    #endregion
}
