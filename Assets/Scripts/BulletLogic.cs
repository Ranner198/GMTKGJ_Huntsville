using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public static BulletLogic instance;

    private void Awake() {
        if(instance != null && instance != this) {
            Debug.LogError("Ya fucked up A-A-Ron");
            DestroyImmediate(gameObject);
        }
        else {
            instance = this;
        }
    }

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
            /*if (GameManager.instance.OnLastKill()) {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }*/
            other.GetComponent<EnemyAi>().Kill();
        }
    }

    void Ricochet(Vector3 normal) {
        totalBounces++;
        if (totalBounces >= maxBounces) {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(DestroyBullet());
            return;
        }
        float speed = rb.velocity.magnitude;
        var dir = Vector3.Reflect(rb.velocity.normalized, normal);
        rb.velocity = dir * speed;
    }

    private IEnumerator DestroyBullet(){
        rb.velocity = Vector3.zero;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<TrailRenderer>().emitting = false;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    List<Transform> enemiesInLine = new List<Transform>();

    void PredictTrajectory() {
        Vector3 movingPosition = transform.position;
        Vector3 startingPosition = movingPosition;
        Vector3 direction = transform.forward;

        float maxRayDistance = 2f;

        float totalDistance = 0f;
        float maxViewDistance = 10f;

        while(totalDistance < maxViewDistance){
            Ray ray = new Ray(movingPosition, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRayDistance, lm)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                    if (!enemiesInLine.Contains(hit.transform)) {
                        enemiesInLine.Add(hit.transform);
                    }
                    movingPosition += direction * maxRayDistance;
                    totalDistance += maxRayDistance;
                    if (totalDistance >= maxViewDistance) {
                        break;
                    }
                    Debug.DrawLine(startingPosition, movingPosition, Color.blue, 0f, false);
                    /*if(GameManager.instance.OnLastKill()) {
                        GameManager.instance.SetTimeScaleBasedOnBullet(Vector3.Distance(transform.position, hit.point));
                        Camera.main.transform.position = (transform.position + hit.point) / 2 + new Vector3(0, 5, 0);
                        GameManager.instance.StartBulletTime();
                    }*/
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
                enemiesInLine.Clear();
                GameManager.instance.ResetTimeAndCamera();
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