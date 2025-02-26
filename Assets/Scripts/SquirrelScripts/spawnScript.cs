using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class spawnScript : MonoBehaviour
{
    public GameObject spawnedObject;

    public float MaxPosX = 0f;
    public float MaxPosY = 0f;
    public float MaxPosZ = 0f;

    public float MinPosX = 0f;
    public float MinPosY = 0f;   
    public float MinPosZ = 0f;

    public float spawnCooldownTimer = 5f;
    public float spawnLimit = 5f;
    private float currentSpawnNum = 0f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnCooldownTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawnNum < spawnLimit)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Vector3 spawnPos = new Vector3(Random.Range(transform.position.x - MinPosX, transform.position.x + MaxPosX), 
                Random.Range(transform.position.y - MinPosY, transform.position.y + MaxPosY), 
                Random.Range(transform.position.z - MinPosZ, transform.position.z + MaxPosZ));

                Instantiate(spawnedObject, spawnPos, transform.rotation);

                currentSpawnNum++;

                timer = spawnCooldownTimer;
            }
        }

        else
        {
            timer = spawnCooldownTimer;
        }
        
    }
}
