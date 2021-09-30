using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform car;
    
    public Vector3 offset;
    public Quaternion quatOff;
    
    void Start(){
        offset = transform.position-car.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=car.position+new Vector3(0,30,0);
        transform.rotation=Quaternion.Euler(90,0,0);
        
    }
}

