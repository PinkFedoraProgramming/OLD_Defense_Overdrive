using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    // Start is called before the first frame update
    Text scrapLabel;
    public int scrap=0;

    public GameObject drone;
    public GameObject bigBoy;
    public GameObject protector;
    public GameObject rc;

    

    public GameObject turret;
    public GameObject scrapCollector;
    
    float worldWidth=250f;

    public List<GameObject> scarpGameObjects;

    int round=0;

    int enemiesToSpawnRemaining=0;
    int enemiesRemaining=3; //Should be 0, but leave x for the starting enenmies I start with

    public GameObject menuPanel;   

    public GameObject buildingsMenuPanel;   

    public GameObject upgradesMenuPanel;   
    public GameObject weaponsMenuPanel;   
    
     

    public Text roundLabel;
    public Text enemiesLabel;

    public Camera cam1;
    public Camera cam2;

    
    public enum Weapons{
        TURRET=0,SHOTGUN=1,GRENADES=2,ROCKETS=3,LASER=4
    }

    void Start()
    {   
        Screen.autorotateToLandscapeLeft=false;
        Screen.autorotateToLandscapeRight=false;
        Screen.autorotateToPortrait=false;
        Screen.autorotateToPortraitUpsideDown=false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        scrapLabel = GameObject.FindGameObjectWithTag("ScrapLabel").GetComponent<Text>();
        scrapLabel.text="$"+scrap;

        InvokeRepeating("SpawnEnemies",0.5f,0.5f);
        

        startNextRound();
    }

    public void SpawnEnemies(){
        if(enemiesToSpawnRemaining>0){
            if(Random.Range(0f,10f)<1){

                int side = Random.Range(0,4);
                
                float a = Random.Range(-(worldWidth/2)*0.8f,(worldWidth/2)*0.8f);
                float x=0,y=0;

                switch(side){
                    case 0:
                        x=-worldWidth/2;
                        y=a;
                        break;
                    case 1:
                        x=a;
                        y=worldWidth/2;
                        break;
                    case 2:
                        x=worldWidth/2;
                        y=a;
                        break;
                    case 3:
                        x=a;
                        y=-worldWidth/2;
                        break;
                }

                float rand = Random.Range(0f,10f);
                
                if(enemiesToSpawnRemaining>round*5
                && rand<0.5f){//If there are more than two thirds enemies left to spawn
                    Instantiate(protector,new Vector3(x,6,y),Quaternion.Euler(0,0,0));
                    Instantiate(drone,new Vector3(x-1,6,y),Quaternion.Euler(0,0,0));
                    Instantiate(drone,new Vector3(x-1,6,y-1),Quaternion.Euler(0,0,0));
                    Instantiate(bigBoy,new Vector3(x+1,6,y+1),Quaternion.Euler(0,0,0));
                    enemiesRemaining+=3;
                    enemiesToSpawnRemaining-=3;
                    //Spawn a swarm

                }else if(rand<1f){
                    Instantiate(rc,new Vector3(x,6,y),Quaternion.Euler(0,0,0));
                }else if(rand<1.75f){
                    Instantiate(bigBoy,new Vector3(x,6,y),Quaternion.Euler(0,0,0));
                }else{
                    Instantiate(drone,new Vector3(x,6,y),Quaternion.Euler(0,0,0));
                }
                //Debug.Log("Spawned a new drone at "+newEnemy.transform.position);
                enemiesRemaining++;
                enemiesToSpawnRemaining--;
            }
        }else if(enemiesRemaining==0){
            startNextRound();
        }
    }

    void Update(){
        roundLabel.text="Round: "+round;
        enemiesLabel.text="Enemies: "+enemiesToSpawnRemaining+"/"+enemiesRemaining;

    }

    public void addScrap(int amount){
        scrap+=amount;
        scrapLabel.text="$"+scrap;
    }

    public void subtractScrap(int amount){
        scrap-=amount;
        scrapLabel.text="$"+scrap;
    }

    private void startNextRound(){
        round++;
        enemiesToSpawnRemaining=round*15;
    }

    public void enemyDestroyed(){
        enemiesRemaining--;
    }


    //Button Functions

    public void CreateTurret(){
        if(scrap>500){
            Instantiate(turret, FindObjectOfType<PlayerCar>().transform.position+Vector3.forward*4,new Quaternion());
            scrap-=500;
            scrapLabel.text="$"+scrap;
        }
        buildingsMenuPanel.SetActive(false);
    }

    public void CreateScrapCollector(){
        if(scrap>500){
            Instantiate(scrapCollector, FindObjectOfType<PlayerCar>().transform.position+Vector3.forward*4,new Quaternion());
            scrap-=500;
            scrapLabel.text="$"+scrap;
        }
        buildingsMenuPanel.SetActive(false);
    }

    public void ToggleMenu(){
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void ToggleBuildingsMenu(){
        buildingsMenuPanel.SetActive(!buildingsMenuPanel.activeSelf);
    }
    
    public void ToggleUpgradesMenu(){
        upgradesMenuPanel.SetActive(!upgradesMenuPanel.activeSelf);
    }

    public void ToggleWeaponsMenu(){
        weaponsMenuPanel.SetActive(!weaponsMenuPanel.activeSelf);
    }

    public void SwitchWeaponTo(int wid){
        PlayerCar car = FindObjectOfType<PlayerCar>();

        foreach(Weapon ww in car.weapons){
            bool setActive=false;
            switch((Weapons)wid){
                case Weapons.TURRET:
                    if(ww is Turret)setActive=true;
                break;
                case Weapons.SHOTGUN:
                    if(ww is Shotgun)setActive=true;
                break;
                case Weapons.GRENADES:
                break;
                case Weapons.ROCKETS:
                break;
                case Weapons.LASER:
                break;
            }
            ww.gameObject.SetActive(setActive);            
        }
        weaponsMenuPanel.SetActive(false);
    }

    public void SwitchCam(){
        cam1.enabled=!cam1.enabled;
        cam2.enabled=!cam2.enabled;
    }
}
