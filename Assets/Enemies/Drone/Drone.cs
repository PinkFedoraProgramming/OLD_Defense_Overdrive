using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone : Enemy{
    // Start is called before the first frame update

    public float speed;
    void Start()
    {   
        base.Start();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        
        
        GetComponent<Rigidbody>().AddForce((GameObject.FindGameObjectWithTag("PlayerBase").transform.position - transform.position).normalized * (transform.position.y<2.5?speed:140f)*Time.deltaTime,ForceMode.Acceleration);
        
    }

}
