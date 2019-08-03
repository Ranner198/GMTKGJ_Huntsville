﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelObject activeLevel;
    public List<LevelObject> levelList = new List<LevelObject>();
    public int levelsCompleted = 0;
    public float totalTimeSpent = 0f;
    public float levelTimeSpent = 0f;

    private void Start(){
    	NewLevel();

    }

    private void Update(){
    	levelTimeSpent += Time.deltaTime;
    	totalTimeSpent += Time.deltaTime;
    }

    public void NewLevel(){
    	//choose a random level
    	activeLevel = levelList[random.range(0,levelList.count-1)];
    }
    public void CompleteLevel(){
    	levelsCompleted += 1;
    	levelTimeSpent = 0f;
    	NewLevel();
    }

    public void StartBulletTime(){
    	StartCoroutine(BulletTime(0.75f));
    }

    private IEnumerator BulletTime(float timer){
    	Time.timescale = 0.3f;
    	yield return new WaitForSeconds(timer);
    	Time.timescale = 1f;
    }
}

[System.Serializable]
public class LevelObject{
	public GameObject levelPrefab;
}
