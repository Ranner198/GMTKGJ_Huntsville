using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelObject activeLevel;
    public List<LevelObject> levelList = new List<LevelObject>();
    public List<EnemyAi> enemyAi = new List<EnemyAi>();
    public int levelsCompleted = 0;
    public float totalTimeSpent = 0f;
    public float levelTimeSpent = 0f;

    private void Start(){
    	NewLevel();

    }

    private void Update(){
    	levelTimeSpent += Time.deltaTime;
    	totalTimeSpent += Time.deltaTime;

    	//controls checking
    	if(Input.GetKeyDown(KeyCode.R))
    		ResetLevel();
    }

    public void NewLevel(){
    	//choose a random level
    	if(levelList.Count > 0){
    		activeLevel = levelList[Random.Range(0,levelList.Count-1)];
    	}
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
    }

    private IEnumerator BulletTime(float timer){
    	Time.timeScale = 0.3f;
    	yield return new WaitForSeconds(timer);
    	Time.timeScale = 1f;
    }
}

[System.Serializable]
public class LevelObject{
	public GameObject levelPrefab;
}
