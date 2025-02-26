using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornBomb : MonoBehaviour
{
    public float despawnRange;
    public float radius = 5f;
    public float explosionForce = 700;
    public LayerMask whatIsPlayer;
    public GameObject bombPrefab;
    public GameObject explosion;
    public GameObject acornModel;
    public AudioSource explosionSound;
    public float bombTimer = 1f;
    public int squirrelPenalty;

    bool hasExploded;
    bool allowExplode;
    float countDown;
    bool canHitSquirrels;

    void OnEnable(){
        countDown = bombTimer;
        canHitSquirrels = false;
        allowExplode = true;
        Invoke(nameof(DeleteBomb), 10f);
    }

    void FixedUpdate() {
        if(!Physics.CheckSphere(transform.position, despawnRange, whatIsPlayer)){
            allowExplode = false;
        } else {
            allowExplode = true;
        }
        countDown -= Time.deltaTime;
        if(countDown <= 0f){
            canHitSquirrels = true;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player" || (other.gameObject.tag == "Squirrel" && canHitSquirrels) || other.gameObject.tag == "Bomb"){
            if(allowExplode && !hasExploded){
                explosionSound.Play();
                Explode();
            }
        }
    }

    void Explode(){
        Instantiate(explosion, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider nearbyObj in colliders){
            if(nearbyObj.GetComponent<Rigidbody>() != null){
                Debug.Log("Boom!");
                Rigidbody rb = nearbyObj.GetComponent<Rigidbody>();
                if(nearbyObj.tag == "Squirrel"){
                    SquirrelAI squirrelAI = nearbyObj.GetComponent<SquirrelAI>();
                    squirrelAI.Die();
                } else {
                    rb.AddExplosionForce(explosionForce * 5f, transform.position, radius);
                }
            }
            if(nearbyObj.GetComponent<PlayerStats>() != null){
                PlayerStats stats = nearbyObj.GetComponent<PlayerStats>();
                stats.SubtractSquirrels(squirrelPenalty);
            }
        }
        hasExploded = true;
        Invoke(nameof(DeleteBombModel), 0f);
        Invoke(nameof(DeleteBomb), 3f);
    }

    void DeleteBomb(){
        Destroy(bombPrefab);
    }

    void DeleteBombModel(){
        Destroy(acornModel);
    }
}
