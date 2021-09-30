using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : Enemy
{
    // Start is called before the first frame update

    public float movementForce=10f;
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerBase playerBase = GameObject.FindGameObjectWithTag("PlayerBase").GetComponent<PlayerBase>();
        
        Vector3 positionDiff = GameObject.FindGameObjectWithTag("PlayerBase").transform.position - transform.position;

        if(positionDiff.magnitude>23)
            GetComponent<Rigidbody>().AddForce(positionDiff.normalized * (transform.position.y<2.5?movementForce:movementForce*2)*Time.deltaTime,ForceMode.Acceleration);
        
        
        float turnSpeed=1;
        Vector3 targetPos = playerBase.transform.position;
		Vector2 v2 = new Vector2(targetPos.x,-targetPos.z)-new Vector2(transform.position.x,-transform.position.z);
        float targetAngle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg+90;

        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,targetAngle,0),turnSpeed*Time.deltaTime);

        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag=="Bullet")Destroy(col.gameObject);
        Debug.Log("Protector on trigger enter");
    }
}
