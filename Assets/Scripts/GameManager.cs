using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    //singleton
    public static GameManager instance;


    public LevelObject activeLevel;
    public LevelObject winnerLevel;
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
    public GameObject youDiedText;
    public Text enemiesRemainingText;
    public Text scoreText;
    public Camera maincam;
    private float shakeAmt;
    private Vector3 cameraOrigin;
    private int totalScore = 0;
    private int levelScore = 0;
    private float timeMultiplier;
    public int numOfDeaths;
    public Text DeathText;
    public Text totalTimeText;
    public bool finished = false;
    private void Awake() {
        instance = this;
        cameraOrigin = maincam.transform.position;
    }


    private void Start() {
        finished = false;
        player = PlayerMovement.playerInstance;
        NewLevel();
        youDiedText.SetActive(false);
    }

    public void GetTimePlayerShot(float _time) {
        timeMultiplier = Mathf.Max(10f - _time, 1);
    }

    public void Kill(float _distance) {
        currentKills++;
        UpdateKillsRemaining();
        Score(_distance);
    }

    private void Score(float _distance) {
        levelScore += (int)(10 * _distance * timeMultiplier);
        scoreText.text = "Score: " + (totalScore + levelScore);
    }

    private void Update() {

        if (!finished)
        {
            levelTimeSpent += Time.deltaTime;
            totalTimeSpent += Time.deltaTime;

            totalTimeText.text = "Total Time: " + string.Format("{0:0.##}", totalTimeSpent);
        }

        //controls checking
        if (Input.GetKeyDown(KeyCode.R))
            ResetLevel();
        if (Input.GetKeyDown(KeyCode.K)) {
            //enemyAi[Random.Range(0, enemyAi.Count)].Kill(0);
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            //CompleteLevel();
        }
    }

    int currentLevelIndex = 0;

    public void NewLevel() {
        levelScore = 0;
        doorOpened = false;

        if (levelsCompleted > 0) {
            if (currentLevelIndex < levelList.Count) {
                if (level != null)
                    Destroy(level);
                activeLevel = levelList[currentLevelIndex];
                level = Instantiate(activeLevel.levelPrefab, Vector3.zero, Quaternion.identity);

                currentLevelIndex++;
                //currentLevelIndex = Random.Range(0, levelList.Count);

                //get enemies
                enemyAi = EnemyAi.instances;

                totalKills += currentKills;
                currentKills = 0;
            }
            else {
                if (level != null)
                    Destroy(level);
                finished = true;
                activeLevel = winnerLevel;
                activeLevel.levelPrefab.SetActive(true);
                level = activeLevel.levelPrefab;
                DeathText.text = "Deaths: " + numOfDeaths.ToString();
                DeathText.gameObject.SetActive(true);
            }
            player.ResetPosition(activeLevel.spawnPoint);
            player.GetComponent<ShootingController>().ResetAmmo();
            maincam.transform.position = cameraOrigin;
        }
        else {
            level = activeLevel.levelPrefab;
        }
        UpdateKillsRemaining();
    }

    private void UpdateKillsRemaining() {
        enemiesRemainingText.text = "Kills Until Portal Opens: " + Mathf.Max(activeLevel.killsRequired - currentKills, 0);
        if (currentKills == activeLevel.killsRequired) {
            doorOpened = true;
        }
    }

    public void CompleteLevel() {
        levelsCompleted += 1;
        levelTimeSpent = 0f;
        totalScore += levelScore;
        NewLevel();
    }

    public void StartBulletTime() {
        StartCoroutine(BulletTime(0.75f));
    }

    private void ResetLevel() {
        for (int i = 0; i < enemyAi.Count; i++) {
            //reset enemies
            enemyAi[i].Respawn();
        }
        levelTimeSpent = 0f;
        currentKills = 0;
        player.ResetPosition(activeLevel.spawnPoint);
        player.GetComponent<ShootingController>().ResetAmmo();
        if (BulletLogic.instance != null)
            Destroy(BulletLogic.instance.gameObject);
        youDiedText.SetActive(false);
        UpdateKillsRemaining();
        if(levelsCompleted > 0) {
            doorOpened = false;
        }
        levelScore = 0;
        scoreText.text = "Score :" + totalScore;
        numOfDeaths++;
    }

    public bool OnLastKill() {
        if (currentKills == activeLevel.killsRequired - 1) {
            return true;
        }
        else {
            return false;
        }
    }

    public void SetTimeScaleBasedOnBullet(float distanceBetweenBulletAndEnemy) {
        if (distanceBetweenBulletAndEnemy < 0.5f) {
            Time.timeScale = 0.01f;
        }
        else if (distanceBetweenBulletAndEnemy < 1f) {
            Time.timeScale = 0.1f;
        }
        else if (distanceBetweenBulletAndEnemy < 1.5f) {
            Time.timeScale = 0.2f;
        }
        else {
            Time.timeScale = 0.3f;
        }
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        print(Time.timeScale);
    }

    private IEnumerator BulletTime(float timer) {
        while (timer > 0) {
            timer -= Time.deltaTime;
            if (currentKills >= activeLevel.killsRequired) {
                timer = -1f;
            }
            yield return null;
        }
        //Camera.main.transform.position = new Vector3(0, 10, -2);
        //Time.timeScale = 1f;
    }

    private void OpenDoor() {
    }

    public void ResetTimeAndCamera() {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //Camera.main.transform.position = new Vector3(0, 10, -2);
    }

    public void PlayerKilled() {
        player.transform.position = new Vector3(1000, player.transform.position.y, 1000);
        youDiedText.SetActive(true);
        foreach (EnemyAi ai in enemyAi) {
            ai.StopSearching();
        }
        CameraShake(0.1f, 0.5f);
    }

    public void CameraShake(float shakeAmount) {
        CameraShake(shakeAmount, 0.3f);
    }

    public void CameraShake(float shakeAmount, float shakeTime) {
        shakeAmt = shakeAmount;
        InvokeRepeating("Shake", 0, 0.01f);
        Invoke("StopShake", shakeTime);
    }

    private void Shake() {
        if (shakeAmt > 0f) {
            Vector3 camPos = maincam.transform.position;
            float shakeX = Random.value * shakeAmt * 2 - shakeAmt;
            float shakeY = Random.value * shakeAmt * 2 - shakeAmt;
            camPos.x += shakeX;
            camPos.z += shakeY;

            maincam.transform.position = camPos;
        }
    }

    private void StopShake() {
        CancelInvoke("Shake");
        maincam.transform.position = cameraOrigin;
    }
}

[System.Serializable]
public class LevelObject {
    public GameObject levelPrefab;
    public int killsRequired;
    public Vector2 spawnPoint;
}