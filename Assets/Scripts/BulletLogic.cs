using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public Rigidbody rb;
    void OnCollisionEnter(Collision coll)
    {        
        if (coll.gameObject.tag == "Wall")
        {            
            var dir = Vector3.Reflect(rb.velocity, coll.contacts[0].normal);
            rb.velocity = dir * 1.5f;            
            transform.rotation = Quaternion.Euler(rb.velocity);
            rb.angularVelocity = Vector3.zero;
        }
    }
}
