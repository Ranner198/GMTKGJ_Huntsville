using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public Rigidbody rb;
    void OnCollisionEnter(Collision coll)
    {        
        float speed = rb.velocity.magnitude;
        if (coll.gameObject.tag == "Wall")
        {            
            var dir = Vector3.Reflect(rb.velocity.normalized, coll.contacts[0].normal);
            rb.velocity = dir * speed * 2;            
            transform.rotation = Quaternion.Euler(rb.velocity);
            rb.angularVelocity = Vector3.zero;
        }
    }
}
