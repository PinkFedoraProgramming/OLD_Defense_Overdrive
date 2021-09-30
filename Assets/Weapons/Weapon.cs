using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    protected float shotDelay=0;
    protected FixedJoystick shootingJoystick;

    protected void Start()
    {
        shootingJoystick=GameObject.FindGameObjectWithTag("ShootingJoystick").GetComponent<FixedJoystick>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(shotDelay>0)shotDelay-=25*Time.deltaTime;
        if(shootingJoystick.Vertical!=0||shootingJoystick.Horizontal!=0){
			float turnSpeed=10;
			float targetAngle =  Mathf.Atan2(-shootingJoystick.Vertical, shootingJoystick.Horizontal - 0) * (180/Mathf.PI)+90;
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,targetAngle,0),turnSpeed*Time.deltaTime);
        }
    }
}
