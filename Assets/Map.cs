using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    
    public GameObject enemyImage;
    public RectTransform carImage;
    
    GameObject playerCar;

    List<GameObject> enemies = new List<GameObject>();

    public Canvas canvas;
    
    float mapWidth=150f;
    float worldWidth=250f;
    // Start is called before the first frame update
    void Start()
    {
        playerCar = FindObjectOfType<PlayerCar>().gameObject;
        
        RectTransform rf = GetComponent<RectTransform>();
        
       mapWidth*=canvas.scaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        carImage.rotation = Quaternion.Euler(0,0,-playerCar.transform.rotation.eulerAngles.y);
        
        carImage.position = transform.position+(new Vector3(playerCar.transform.position.x,playerCar.transform.position.z,0)*(mapWidth/worldWidth));

        enemies.Remove(null);
        for(int i=0;i<enemies.Count;i++){
            Enemy e = enemies[i].GetComponentInChildren<Enemy>();
            e.mapIcon.position = transform.position+(new Vector3(e.transform.position.x,e.transform.position.z,0)*(mapWidth/worldWidth));
        }
        
    }

    public void AddEnemyMapIcon(GameObject gameObject){
        enemies.Add(gameObject);
        GameObject mapIcon = Instantiate(enemyImage);
        mapIcon.transform.SetParent(canvas.transform,false);
        gameObject.GetComponentInChildren<Enemy>().mapIcon = mapIcon.GetComponentInChildren<RectTransform>();
    }

    public void RemoveEnemyMapIcon(GameObject gameObject){
        Destroy(gameObject.GetComponentInChildren<Enemy>().mapIcon.gameObject);
        enemies.Remove(gameObject);
    }
}
