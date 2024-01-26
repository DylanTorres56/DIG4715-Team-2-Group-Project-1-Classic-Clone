using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNodes : MonoBehaviour
{
    int numToSpawn = 25;

    public float currentSpawnOffset;
    public float spawnOffset = 0.296f;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.name = "Node";
        //return;
        //currentSpawnOffset = spawnOffset;
        //if (gameObject.name == "Node") 
        //{ 
            //for (int i = 0; i < numToSpawn; i++) 
            //{
                //GameObject clone = Instantiate(gameObject, new Vector3(transform.position.x + currentSpawnOffset, transform.position.y, 0), Quaternion.identity);
                //currentSpawnOffset += spawnOffset;
            //}
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
