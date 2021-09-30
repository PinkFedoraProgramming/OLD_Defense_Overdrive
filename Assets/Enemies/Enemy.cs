using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    public int maxHealth;
    int health;
    protected Slider healthBar;

    public GameObject healthBarGameObject;

    public GameObject explosion;

    public int scrap;

    public RectTransform mapIcon;

    public bool resistCollisionDamage=false;

    Canvas canvas;
    // Start is called before the first frame update
    protected void Start()
    {
        healthBar=Instantiate(healthBarGameObject).GetComponent<Slider>();
        canvas = FindObjectOfType<Canvas>();
        healthBar.transform.SetParent(canvas.transform,false);
        
        healthBar.maxValue=maxHealth;
        healthBar.value=maxHealth;

        FindObjectOfType<Map>().AddEnemyMapIcon(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.gameObject.transform.position=Camera.main.WorldToScreenPoint(transform.position)+new Vector3(-20f*canvas.scaleFactor,10f,0f);
        
        if(transform.position.y<-100)Destroy(this.gameObject);
    }

    
    public void hit(float damage){
        healthBar.value-=damage;
        if(healthBar.value<=0)
            Destroy(gameObject);
        
    }

    protected void OnDestroy(){
        Destroy(healthBar.gameObject);
        FindObjectOfType<Map>().RemoveEnemyMapIcon(gameObject);
        Instantiate(explosion,gameObject.transform.position,new Quaternion());
        FindObjectOfType<GameScript>().enemyDestroyed();

        GameScript gameScript = FindObjectOfType<GameScript>();
        for(int i=0;i<scrap;i++)
            Instantiate(gameScript.scarpGameObjects[Random.Range(0,2)], transform.position + (Vector3.up*3), new Quaternion());
        
    }
}
