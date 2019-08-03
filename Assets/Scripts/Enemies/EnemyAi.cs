using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour {

    public Transform player;
    private Vector3 playerLastPos;
    NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
        playerLastPos = player.position;
    }

    private void Update() {
        if (player.position != playerLastPos) {
            agent.destination = player.position;
            playerLastPos = player.position;
        }
    }
}
