using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapCollector : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col){
        if(col.tag=="Scrap"){
            Vector3 differenceInPosition = transform.position-col.transform.position;
            if(differenceInPosition.magnitude<2){
                FindObjectOfType<GameScript>().addScrap(5);//Collect
                Destroy(col.gameObject);
            }
            else
                col.GetComponent<Rigidbody>().AddForce((transform.position-col.transform.position).normalized*150f*Time.deltaTime);
        }
    }
}
