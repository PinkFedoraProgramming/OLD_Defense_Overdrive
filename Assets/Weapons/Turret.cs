using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Weapon
{

    public Rigidbody bullet;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(shootingJoystick.Vertical!=0||shootingJoystick.Horizontal!=0){
            if(shotDelay<=0){
                Vector3 pos = transform.position;
                pos+=transform.forward*2;

                Vector3 rot = transform.rotation.eulerAngles;
                rot.x=90;

                Rigidbody bulletClone = (Rigidbody) Instantiate(bullet, pos, Quaternion.Euler(rot.x,rot.y,rot.z));
                bulletClone.velocity=bulletClone.transform.up*30+GetComponentInParent<Rigidbody>().velocity;
                shotDelay=8;
            }
		}
    }
}
