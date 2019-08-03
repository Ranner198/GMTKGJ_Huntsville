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
 
    private void Start()
    {
        lm = ~lm;
        //StartCoroutine(PredictTrajectory(100));
    }

    private void Update() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, .01f, lm))
        {
            if (hit.transform.tag == "Wall")
            {
                Ricochet(hit.normal);
            }
        }       
    }

    void Ricochet(Vector3 normal)
    {
        totalBounces++;
        if(totalBounces > maxBounces) {
            print("yup");
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject);
            return;
        }
        float speed = rb.velocity.magnitude;       
        var dir = Vector3.Reflect(rb.velocity.normalized, normal);
        transform.rotation = Quaternion.Euler(dir);
        rb.velocity = dir * speed;            
    }

    IEnumerator PredictTrajectory(int steps) {
        Vector3 movingPosition = transform.position;
        Vector3 startingPosition = movingPosition;
        Vector3 direction = transform.forward;

        float maxStepDistance = 100f;

        int layerMask =~ LayerMask.GetMask("Player");

        int samples = 100;
        int i = 0;
        int raycastBounces = 0;

        while(i < samples){
            Ray ray = new Ray(movingPosition, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxStepDistance, layerMask)) {
                raycastBounces++;
                if(raycastBounces > maxBounces) {
                    break;
                }
                direction = Vector3.Reflect(direction, hit.normal);
                movingPosition = hit.point;
                Debug.DrawLine(startingPosition, movingPosition, Color.blue, 10f, false);
                startingPosition = movingPosition;
            }
            else {
                movingPosition += direction * maxStepDistance;
                Debug.DrawLine(startingPosition, movingPosition, Color.blue, 10f, false);
            }

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawLine(startingPosition, position);

            i++;
        }
        yield return null;
        //return results;
    }
}
