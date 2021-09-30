using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]


public class PlayerCar : MonoBehaviour
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
	public FixedJoystick steeringJoystick;
	
	Vector3 lastPosition;
	float sideSlipAmount=0;

	public Slider healthBar;

	GameScript gameScript;

	public GameObject explosion; //Todo, move to GS

	bool destroyed=false;

	public Weapon[] weapons;

	void OnCollisionEnter(Collision col){   
        if(col.gameObject.tag=="Scrap"){
            Destroy(col.gameObject);
            gameScript.addScrap(5);
            return; // No damage for collecting scrap
        }
        float damageForce = (col.impulse.magnitude/1000)+1;
        
        if(col.gameObject.tag=="Enemy" || col.gameObject.tag=="ForceField"){
            Enemy e = col.gameObject.GetComponent<Enemy>();
			if(e==null)e=col.gameObject.GetComponentInParent<Enemy>();
			e.hit(col.relativeVelocity.magnitude);

			//healthBar.value-=1;
        }else{
			
			//Debug.Log("Car hit "+col.gameObject.name);
		}
		healthBar.value-=damageForce;
		Debug.Log("That hit had a force of "+col.impulse.magnitude);
    }

    // Find all the WheelColliders down in the hierarchy.
	void Start()
	{
		gameScript=FindObjectOfType<GameScript>();

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
		lastPosition=transform.position;
	}

	// This is a really simple approach to updating wheels.
	// We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
	// This helps us to figure our which wheels are front ones and which are rear.
	void FixedUpdate()
	{	
		if(destroyed)return;
		if(healthBar.value<=0){
			Instantiate(explosion,gameObject.transform.position,new Quaternion());
			foreach(MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
				mr.enabled=false;
			}
			GetComponent<Rigidbody>().velocity=Vector3.zero;
			destroyed=true;
			return;
		}

		float torque=0;
		float handBrake=0;

		if(steeringJoystick.Vertical!=0||steeringJoystick.Horizontal!=0){
			float turnSpeed=200;
			float targetAngle =  Mathf.Atan2(-steeringJoystick.Vertical, steeringJoystick.Horizontal - 0) * (180/Mathf.PI)+90;
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,targetAngle,0),turnSpeed*Time.deltaTime);
			torque=maxTorque*new Vector2(steeringJoystick.Horizontal,steeringJoystick.Vertical).magnitude;

		}else{
			handBrake=brakeTorque;
		}
		
		m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
		
		float angle = 0;


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
		SetSideSlip();
	}

	private void SetSideSlip(){
		Vector3 direction = transform.position - lastPosition;
		Vector3 movement = transform.InverseTransformDirection(direction);
		lastPosition=transform.position;
		sideSlipAmount=movement.x;
	}

	public float SideSlipAmount{
		get{
			return sideSlipAmount;
		}
	}

	void OnDestroy(){
		
	}

	public void repair(float value){
		if(healthBar.value<healthBar.maxValue){
                healthBar.value+=value;
                if(healthBar.value>healthBar.maxValue)healthBar.value=healthBar.maxValue;
            }
	}
}
