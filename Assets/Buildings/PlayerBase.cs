using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{

    // Start is called before the first frame update

    bool repairingPlayer=false;
    public PlayerCar playerCar;

    public Slider healthBar;
    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag=="Enemy"){
            if(!col.gameObject.GetComponent<Enemy>().resistCollisionDamage){
                healthBar.value-=10;
                Destroy(col.gameObject);
            }
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(repairingPlayer){
            playerCar.repair(10f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag=="PlayerCar"){
            repairingPlayer=true;
            Debug.Log("PlayerCar enter");
        }
        if(col.gameObject.tag=="Enemy"){
            if(!col.gameObject.GetComponent<Enemy>().resistCollisionDamage){
                healthBar.value-=10;
                Destroy(col.gameObject);
            }
        }
        //Debug.Log("Object with the following tag entered: "+col.gameObject.tag);
    }

    private void OnTriggerExit(Collider col){
        if(col.gameObject.tag=="PlayerCar"){
            repairingPlayer=false;
            Debug.Log("PlayerCar exit");
        }
        
    }

}
