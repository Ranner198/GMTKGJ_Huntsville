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

	public void Kill()
	{
        currentKills++;
	}

    private void Update(){
		
    	levelTimeSpent += Time.deltaTime;
    	totalTimeSpent += Time.deltaTime;

    	//controls checking
    	if(Input.GetKeyDown(KeyCode.R))
    		ResetLevel();
    	if(Input.GetKeyDown(KeyCode.K)){
    		enemyAi[Random.Range(0,enemyAi.Count)].Kill();
    	}
    	if(Input.GetKeyDown(KeyCode.N)){
    		CompleteLevel();
    	}
    }

    int currentLevelIndex = -1;

    public void NewLevel(){

        doorOpened = false;

        /*// if not the start level
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
    	}*/

        if(currentLevelIndex == -1) {
            level = activeLevel.levelPrefab;
        }
        else {
            Destroy(level);
            activeLevel = levelList[currentLevelIndex];
            level = Instantiate(activeLevel.levelPrefab, Vector3.zero, Quaternion.identity);
        }
        currentLevelIndex++;

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

    private void ResetLevel(){
    	for(int i=0;i<enemyAi.Count;i++){
    		//reset enemies
    		enemyAi[i].Respawn();
    	}
    	levelTimeSpent = 0f;
    	currentKills = 0;

    }

    public bool OnLastKill() {
        if(currentKills == activeLevel.killsRequired - 1) {
            return true;
        }
        else {
            return false;
        }
    }

    public void SetTimeScaleBasedOnBullet(float distanceBetweenBulletAndEnemy) {
        if(distanceBetweenBulletAndEnemy < 0.5f) {
            Time.timeScale = 0.01f;
        }
        else if(distanceBetweenBulletAndEnemy < 1f) {
            Time.timeScale = 0.1f;
        }
        else if(distanceBetweenBulletAndEnemy < 1.5f) {
            Time.timeScale = 0.2f;
        }
        else {
            Time.timeScale = 0.3f;
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        print(Time.timeScale);
    }

    private IEnumerator BulletTime(float timer){
        while(timer > 0) {
            timer -= Time.deltaTime;
            if(currentKills >= activeLevel.killsRequired) {
                timer = -1f;
            }
            yield return null;
        }
        //Camera.main.transform.position = new Vector3(0, 10, -2);
    	//Time.timeScale = 1f;
    }

    private void OpenDoor()
    {
        if (currentKills == activeLevel.killsRequired)
        {
            doorOpened = true;
        }
    }

    public void ResetTimeAndCamera() {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //Camera.main.transform.position = new Vector3(0, 10, -2);
    }
}

[System.Serializable]
public class LevelObject{
	public GameObject levelPrefab;
	public int killsRequired;
	public Vector2 spawnPoint;
}