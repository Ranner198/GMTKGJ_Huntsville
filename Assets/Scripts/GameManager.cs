using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	//singleton
	public static GameManager instance;


    public LevelObject activeLevel;
    private GameObject level;
    public List<LevelObject> levelList = new List<LevelObject>();
    public List<EnemyAi> enemyAi = new List<EnemyAi>();
    public PlayerMovement player; 
    public int levelsCompleted = 0;
    public float totalTimeSpent = 0f;
    public float levelTimeSpent = 0f;
    public int currentKills = 0;
    public int totalKills = 0;
    public bool doorOpened;

    private void Awake(){
    	instance = this;
    }


    private void Start(){
    	player = PlayerMovement.playerInstance;
    	NewLevel();
    }

	public static void Kill()
	{
		//Bruh
		print("Boom Head shot!");
	}

    private void Update(){
		
    	levelTimeSpent += Time.deltaTime;
    	totalTimeSpent += Time.deltaTime;

    	//controls checking
    	if(Input.GetKeyDown(KeyCode.R))
    		ResetLevel();
    	if(Input.GetKeyDown(KeyCode.K)){
    		enemyAi[Random.Range(0,enemyAi.Count)].Kill();
    		currentKills += 1;
    	}
    	if(Input.GetKeyDown(KeyCode.N)){
    		CompleteLevel();
    	}
    }

    public void NewLevel(){

        doorOpened = false;

    	// if not the start level
    	if(levelsCompleted > 0){
    		//choose a random level
    		if(levelList.Count > 0){
    			Destroy(level);
    			activeLevel = levelList[Random.Range(0,levelList.Count)];
    			level = Instantiate(activeLevel.levelPrefab,Vector3.zero,Quaternion.identity);
    		}
    	} else{
    		//active level should already be the start room
    		level = activeLevel.levelPrefab;
    	}
    	//get enemies
    	enemyAi = EnemyAi.instances;
    	totalKills += currentKills;
    	currentKills = 0;
    	player.ResetPosition(activeLevel.spawnPoint);
    }
    public void CompleteLevel(){
    	levelsCompleted += 1;
    	levelTimeSpent = 0f;
    	NewLevel();
    }

    public void StartBulletTime(){
    	StartCoroutine(BulletTime(0.75f));
    }

	public void KilledEnemy()
	{
		Debug.Log("BOOM HEADSHOT");
	}

    private void ResetLevel(){
    	for(int i=0;i<enemyAi.Count;i++){
    		//reset enemies
    		enemyAi[i].Respawn();
    	}
    	levelTimeSpent = 0f;
    	currentKills = 0;

    }

    private IEnumerator BulletTime(float timer){
    	Time.timeScale = 0.3f;
    	yield return new WaitForSeconds(timer);
    	Time.timeScale = 1f;
    }

    private void OpenDoor()
    {
        if (currentKills == activeLevel.killsRequired)
        {
            doorOpened = true;
        }
    }
}

[System.Serializable]
public class LevelObject{
	public GameObject levelPrefab;
	public int killsRequired;
	public Vector2 spawnPoint;
}