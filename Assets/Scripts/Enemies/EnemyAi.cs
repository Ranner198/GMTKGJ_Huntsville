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

    private Transform startTransform;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
        playerLastPos = player.position;

        startTransform = transform;
    }

    private void Update() {
        if (player.position != playerLastPos) {
            agent.destination = player.position;
            playerLastPos = player.position;
        }
    }

    public void Respawn() {
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }

    public void Kill() {
        transform.position = new Vector3(-1000, -1000, -1000);
    }

    public void OnDestroy() {
        if (instances.Contains(this)) {
            instances.Remove(this);
        }
    }
}