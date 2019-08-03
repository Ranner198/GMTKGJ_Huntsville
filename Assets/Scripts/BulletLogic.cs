using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public Rigidbody rb;
    public LayerMask lm;
    public int maxBounces;
    private int totalBounces = 0;
    private Collider[] overlapResults = new Collider[10];

    private void Update() {
        PredictTrajectory();
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red, Time.deltaTime, false);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.5f, lm)) {
            if (hit.transform.tag == "Wall") {
                Ricochet(hit.normal);
            }
        }

        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<EnemyAi>()) {
            other.GetComponent<EnemyAi>().Kill();
        }
    }

    void Ricochet(Vector3 normal) {
        totalBounces++;
        if (totalBounces >= maxBounces) {
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject);
            return;
        }
        float speed = rb.velocity.magnitude;
        var dir = Vector3.Reflect(rb.velocity.normalized, normal);
        rb.velocity = dir * speed;
    }

    void PredictTrajectory() {
        Vector3 movingPosition = transform.position;
        Vector3 startingPosition = movingPosition;
        Vector3 direction = transform.forward;

        //LayerMask wall = LayerMask.GetMask("Wall");

        float maxRayDistance = 1f;

        float totalDistance = 0f;
        float maxViewDistance = 10f;

        while(totalDistance < maxViewDistance){
            Ray ray = new Ray(movingPosition, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRayDistance, lm)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                    Camera.main.transform.position = (transform.position + hit.point) / 2;
                    movingPosition += direction * maxRayDistance;
                    totalDistance += maxRayDistance;
                    if (totalDistance >= maxViewDistance) {
                        break;
                    }
                    Debug.DrawLine(startingPosition, movingPosition, Color.blue, 0f, false);
                    GameManager.instance.StartBulletTime();
                }
                else {
                    if (totalBounces == maxBounces - 1) {
                        Debug.DrawLine(startingPosition, hit.point, Color.blue, 0f, false);
                        break;
                    }
                    direction = Vector3.Reflect(direction, hit.normal);
                    movingPosition = hit.point;
                    totalDistance += Vector3.Distance(startingPosition, movingPosition);
                    if (totalDistance >= maxViewDistance) {
                        break;
                    }
                    Debug.DrawLine(startingPosition, movingPosition, Color.blue, 0f, false);
                    startingPosition = movingPosition;
                }
            }
            else {
                movingPosition += direction * maxRayDistance;
                totalDistance += maxRayDistance;
                if (totalDistance >= maxViewDistance) {
                    break;
                }
                Debug.DrawLine(startingPosition, movingPosition, Color.blue, 0f, false);
            }
        }
    }
}