using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public Rigidbody rb;
    public LayerMask lm;
    private Collider[] overlapResults = new Collider[10];
 
    private void Start()
    {
        lm = ~lm;
    }

    private void Update() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, .2f, lm))
        {
            if (hit.transform.tag == "Wall")
            {
                Ricochet(hit.normal);
            }
        }       
    }

    void Ricochet(Vector3 normal)
    {
        float speed = rb.velocity.magnitude;       
        var dir = Vector3.Reflect(rb.velocity.normalized, normal);
        transform.rotation = Quaternion.Euler(dir);
        rb.velocity = dir * speed;            
    }
}
