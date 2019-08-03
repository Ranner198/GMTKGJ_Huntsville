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

    public Transform player;
    private Vector3 playerLastPos;
    private NavMeshAgent agent;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
        playerLastPos = player.position;

        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void Update() {
        if (player.position != playerLastPos) {
            agent.destination = player.position;
            playerLastPos = player.position;
        }
    }

    public void Respawn() {
        transform.position = startPos;
        transform.rotation = startRot;
        agent.enabled = true;
        agent.destination = player.position;
    }

    public void Kill() {
        agent.enabled = false;
        transform.position = new Vector3(-1000, -1000, -1000);
    }

    public void OnDestroy() {
        if (instances.Contains(this)) {
            instances.Remove(this);
        }
    }
}