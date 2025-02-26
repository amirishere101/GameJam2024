using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;

public class SquirrelAI : MonoBehaviour {
    [SerializeField] float normalSquirrelBounceForce, explosiveSquirrelBounceForce;
    public float currentSquirrelBounceForce;
    float flyAwayForce = 25f;
    public float explosionChance;
    public float bomberChance;
    float projectileForce = 3f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] public List <AudioClip> deathSounds;
    [SerializeField] public AudioSource squirrelDeathAudio;
    [SerializeField] float detectPlayerRange;
    [SerializeField] float walkPointRange;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject squirrel;
    [SerializeField] GameObject squirrelPrefab;
    [SerializeField] LayerMask whatIsPlayer, whatIsGround;
    bool playerIsInRange;
    bool walkPointSet;
    public float walkSpeed; 
    public float sprintSpeed;
    [SerializeField] ParticleSystem ps;
    bool isDead;
    float deathType;
    public Type squirrelType;

 
    public float deathSpeed = 100f; 
    [SerializeField] AudioSource explosionSound;
    
    // Put the running object here
    public Rigidbody squirrelRb;
    bool alreadyAttacked;
    bool allowBomb;

    
    [SerializeField] GameSettings settings;
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    Vector3 walkPoint;

    public enum Type {
        Bomber, Normal, Exploder
    }

    // Start is called before the first frame update
    void Start()
    {
        walkPointSet = false;
    }

    void OnEnable(){
       walkPointSet = false;
       allowBomb = true; 
       squirrelType = Type.Bomber;
       deathType = Random.Range(1, 101);

        if(deathType <= explosionChance){
            currentSquirrelBounceForce = explosiveSquirrelBounceForce;
            squirrelType = Type.Exploder;
        } else if(deathType > explosionChance && deathType <= explosionChance + bomberChance){
            currentSquirrelBounceForce = normalSquirrelBounceForce;
            squirrelType = Type.Bomber;
        } else {
            currentSquirrelBounceForce = normalSquirrelBounceForce;
            squirrelType = Type.Normal;
        }
    }

    void Patrol() {
        if(allowBomb == false && !alreadyAttacked){
            allowBomb = true;
            CancelInvoke(nameof(ThrowBomb));
        }
        alreadyAttacked = false;
        agent.speed = walkSpeed;
        if(!walkPointSet){
            SearchWalkPoint();
        }
        if(walkPointSet){
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }

    void Runaway() {
        walkPointSet = false;
        agent.speed = sprintSpeed;
        walkPoint = squirrel.transform.position + (squirrel.transform.position - player.transform.position);
        agent.SetDestination(walkPoint);
    }

    public void ChangeDeathType(){
        deathType = Random.Range(1, 11);

        if(deathType <= explosionChance){
            currentSquirrelBounceForce = explosiveSquirrelBounceForce;
        } else {
            currentSquirrelBounceForce = normalSquirrelBounceForce;
        }
    }

    public void Die() {
        col.enabled = false;
        isDead = true;
        agent.enabled = false;
        int deathSoundIndex = Random.Range(0, deathSounds.Count);
        squirrelDeathAudio.clip = deathSounds[deathSoundIndex];
        squirrelDeathAudio.Play();
        if(squirrelType == Type.Exploder){
            ps.transform.position = squirrel.transform.position - new Vector3(0, 0.5f, 0);
            explosionSound.Play();
            ps.Play();
            rb.velocity = new Vector3(rb.velocity.x, flyAwayForce, rb.velocity.z);
        } else {
            if(squirrelType == Type.Bomber && !alreadyAttacked){
                CancelInvoke(nameof(ThrowBomb));
                ps.transform.position = squirrel.transform.position - new Vector3(0, 0.5f, 0);
                explosionSound.Play();
                ps.Play();
                rb.velocity = new Vector3(rb.velocity.x, flyAwayForce, rb.velocity.z);
            } else {
                rb.velocity = new Vector3(rb.velocity.x, flyAwayForce, rb.velocity.z);
            }
        }
        Invoke(nameof(DestroySquirrel), 3);
    }

    void DestroySquirrel(){
        Destroy(squirrelPrefab);
    }

    void AttackPlayer(){
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);
        if(allowBomb){
            allowBomb = false;
            Invoke(nameof(ThrowBomb), 1f);  
        }
        walkPointSet = false;
        agent.speed = walkSpeed + 1;
        walkPoint = squirrel.transform.position + (squirrel.transform.position - player.transform.position);
        agent.SetDestination(walkPoint);
    }

    void ThrowBomb(){
        alreadyAttacked = true;
        Rigidbody rb = Instantiate(projectilePrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity).GetComponentInChildren<Rigidbody>();
        rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse); 
    }

    void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }
    }

    void FixedUpdate() {
        if(!isDead){
            playerIsInRange = Physics.CheckSphere(transform.position, detectPlayerRange, whatIsPlayer);
            if(!playerIsInRange){
                Patrol();
            } else if(playerIsInRange && squirrelType == Type.Bomber && !alreadyAttacked){
                AttackPlayer();
            } else {
                Runaway();
            }
        } else {
            transform.Rotate(20f, 0f, 20f, Space.Self);
        }
    }
}
