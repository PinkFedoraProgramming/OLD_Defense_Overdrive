using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelskid : MonoBehaviour
{
    
    [SerializeField]float intensityMultiplier=1.5f;

    Skidmarks skidMarkController;

    PlayerCar playerCar;

    int lastSkidId=-1;

    // Start is called before the first frame update
    void Start()
    {
        skidMarkController=FindObjectOfType<Skidmarks>();
        playerCar=FindObjectOfType<PlayerCar>();
    }

    void LateUpdate(){
        float intensity = playerCar.SideSlipAmount;

        if(intensity<0)intensity=-intensity;

        if(intensity>0.2f){
            lastSkidId=skidMarkController.AddSkidMark(transform.position,transform.up,intensity*intensityMultiplier,lastSkidId);
        }else{
            lastSkidId=-1;
        }
    }
}
