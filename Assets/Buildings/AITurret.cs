using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurret : MonoBehaviour
{

    public Rigidbody bullet;
    public GameObject turretHead;

    GameObject target;
    
    bool rotating=false;


    float shotDelay=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shotDelay>0)shotDelay-=25f*Time.deltaTime;
        if(target!=null){
			float turnSpeed=2;
            Vector3 targetPos = target.transform.position+target.GetComponentInParent<Rigidbody>().velocity;
			Vector2 v2 = new Vector2(targetPos.x,-targetPos.z)-new Vector2(turretHead.transform.position.x,-turretHead.transform.position.z);
            float targetAngle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg+90;

            turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation,Quaternion.Euler(0,targetAngle,0),turnSpeed*Time.deltaTime);

            if(shotDelay<=0){
                Vector3 pos = turretHead.transform.position;
                
                Vector3 rot = turretHead.transform.rotation.eulerAngles;
                rot.x=90;


                Rigidbody bulletClone = (Rigidbody) Instantiate(bullet, pos, Quaternion.Euler(rot.x,rot.y,rot.z));
                bulletClone.velocity=(bulletClone.transform.up*(new Vector3(0,0,30)).magnitude);
                bulletClone.gameObject.GetComponent<Bullet>().damage=3;
                shotDelay=8;
            }
		}
    }

    private void OnTriggerEnter(Collider col){
        if(target==null && col.gameObject.tag=="Enemy")target=col.gameObject;
    }

    private void OnTriggerExit(Collider col){
        if(col.gameObject==target)target=null;
    }

    
}
