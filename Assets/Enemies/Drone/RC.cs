using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC : Enemy
{
    
    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
	[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

    private WheelCollider[] m_Wheels;

	public GameObject turretHead;

    GameObject playerCar;

	public Rigidbody bullet;

	GameObject shootingTarget;

	float shotDelay=0;

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag=="PlayerCar"){
			//col.gameObject.GetComponent<PlayerCar>().healthBar.value-=5;
			//Destroy(gameObject);
		}
		
	}

    void Start()
    {
        base.Start();
        playerCar = GameObject.FindGameObjectWithTag("PlayerCar");

        m_Wheels = GetComponentsInChildren<WheelCollider>();
		
		for (int i = 0; i < m_Wheels.Length; ++i) 
		{
			var wheel = m_Wheels [i];

			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
    }

    void FixedUpdate()
	{	
		float torque=0;
		float handBrake=0;

		Debug.Log(GetComponent<Rigidbody>().velocity.magnitude);

		if(GetComponent<Rigidbody>().velocity.magnitude>20){
			Destroy(gameObject);
			return;
		}

        Vector3 targetPos = playerCar.transform.position;//+playerCar.GetComponentInParent<Rigidbody>().velocity;
		Vector2 v2 = new Vector2(targetPos.x,-targetPos.z)-new Vector2(transform.position.x,-transform.position.z);
        float targetAngle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg+90;
		
		float distanceToTarget = (targetPos-transform.position).magnitude;

		if(distanceToTarget>10)
			torque=maxTorque*Random.Range(0.9f,1.1f);

		
		float currentAngle = transform.rotation.eulerAngles.y;

		float angle=Mathf.Clamp(Mathf.DeltaAngle(currentAngle,targetAngle),-45,45);

		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
		
		foreach (WheelCollider wheel in m_Wheels)
		{
			// A simple car where front wheels steer while rear ones drive.
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			if (wheel.transform.localPosition.z < 0)
			{
				wheel.brakeTorque = handBrake;
			}

			if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
			{
				wheel.motorTorque = torque;
			}

			if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
			{
				wheel.motorTorque = torque;
			}

			// Update visual wheels if any.
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// Assume that the only child of the wheelcollider is the wheel shape.
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}

		shootingTarget = GameObject.FindGameObjectWithTag("PlayerCar");

		if(shotDelay>0)shotDelay-=25f*Time.deltaTime;
		if(shootingTarget!=null && distanceToTarget<10){
			float turnSpeed=2;
            targetPos = shootingTarget.transform.position+shootingTarget.GetComponentInParent<Rigidbody>().velocity;
			v2 = new Vector2(targetPos.x,-targetPos.z)-new Vector2(turretHead.transform.position.x,-turretHead.transform.position.z);
            targetAngle = Mathf.Atan2(v2.y, v2.x)*Mathf.Rad2Deg+90;

            turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation,Quaternion.Euler(0,targetAngle,0),turnSpeed*Time.deltaTime);

            if(shotDelay<=0){
                Vector3 pos = turretHead.transform.position;
                
                Vector3 rot = turretHead.transform.rotation.eulerAngles;
                rot.x=90;


                Rigidbody bulletClone = (Rigidbody) Instantiate(bullet, pos, Quaternion.Euler(rot.x,rot.y,rot.z));
                bulletClone.velocity=(bulletClone.transform.up*(new Vector3(0,0,30)).magnitude);
                bulletClone.gameObject.GetComponent<Bullet>().damage=3;
                shotDelay=20;
            }
		}
	}

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag=="Bullet" && col.gameObject.GetComponent<Bullet>().playerOwned)Destroy(gameObject);
		
        //Debug.Log("RC Collider Enter by "+col.gameObject.tag);
    }
}
