using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage=10;

    public bool playerOwned=true;

    void OnCollisionEnter(Collision col){
        if(playerOwned){
            if(col.gameObject.tag=="Enemy"){
                col.gameObject.GetComponent<Enemy>().hit(damage);
                
                //Destroy(this.gameObject);
            }
            GetComponent<Rigidbody>().useGravity=true;
        }else{
            if(col.gameObject.tag=="PlayerCar"){
                col.gameObject.GetComponent<PlayerCar>().healthBar.value-=1;
                
            }else{
                Debug.Log("Enemy bullet collided with: "+col.gameObject.tag);
            }
        }
        //if(col.gameObject.tag!="Bullet")
       
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove",30f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Remove(){
        Destroy(gameObject);
    }
}
