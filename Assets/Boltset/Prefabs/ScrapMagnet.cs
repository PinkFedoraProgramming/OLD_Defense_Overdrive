using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMagnet : MonoBehaviour
{
    GameObject playerCar;
    public GameObject gameScript;
    float forceFactor=15f;
    Rigidbody rigidBody;
    // Start is called before the first frame update
    float maxSpawnForce=10;

    void Start()
    {
        rigidBody=GetComponent<Rigidbody>();
        rigidBody.velocity=(new Vector3(Random.Range(-maxSpawnForce,maxSpawnForce),Random.Range(-maxSpawnForce,maxSpawnForce),Random.Range(-maxSpawnForce,maxSpawnForce)));
        playerCar=GameObject.FindGameObjectWithTag("PlayerCar");
    
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 distance = playerCar.transform.position-transform.position;
        rigidBody.velocity+=(forceFactor/Mathf.Pow(distance.magnitude,2f))*distance.normalized;
    }
}
