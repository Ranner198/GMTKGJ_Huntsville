using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour {

    #region Multiton
    public static List<EnemyAi> instances = new List<EnemyAi>();
    private void Awake() {
        if (instances.Contains(this)) {
            return;
        }

        instances.Add(this);
    }
    #endregion

    private PlayerMovement player;
    private Vector3 playerLastPos;
    private NavMeshAgent agent;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool dead = false;

    private void Start() {
        player = PlayerMovement.playerInstance;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        playerLastPos = player.transform.position;

        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void Update() {
        if (dead) {
            return;
        }
        if (player.transform.position != playerLastPos) {
            agent.destination = player.transform.position;
            playerLastPos = player.transform.position;
        }
    }

    public void Respawn() {
        transform.position = startPos;
        transform.rotation = startRot;
        agent.enabled = true;
        agent.destination = player.transform.position;
        dead = false;
    }

    public void Kill() {
        agent.enabled = false;
        transform.position = new Vector3(-1000, -1000, -1000);
        dead = true;
        GameManager.instance.Kill();
    }

    public void OnDestroy() {
        if (instances.Contains(this)) {
            instances.Remove(this);
        }
    }
}