using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject shootingPoint;
    public GameObject bulletCasing;
    public Text shotsLeft;
    public float bulletSpeed;
    public LayerMask lm;
    public float YOffset = .5f;
    public GameObject player;
    private int ammo = 1;
    public Camera cam;

    void Start()
    {
        lm = ~lm;
        shotsLeft.text = "Shots Remaining: " + ammo;
    }

    public void ResetAmmo() {
        if(BulletLogic.instance)
            Destroy(BulletLogic.instance.gameObject);
        ammo = 1;
        shotsLeft.text = "Shots Remaining: " + ammo;
    }

    void Update()
    {
           
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 pos = hit.point;
            pos.y = transform.position.y;
            transform.LookAt(pos);            
            if (Input.GetMouseButtonDown(0) && ammo > 0)
            {
                Shoot(hit.point);   
                ammo--;
                shotsLeft.text = "Shots Remaining: " + ammo;
            }            
        }
        else
            transform.rotation = transform.rotation;
    }

    void Shoot(Vector3 pos)
    {
        pos.y += YOffset;
        GameObject Bullet = Instantiate(bullet, shootingPoint.transform.position, transform.rotation);
        Bullet.name = "Bullet";
        Bullet.GetComponent<Rigidbody>().velocity = bulletSpeed * transform.forward;
        //GameObject BulletCasing = Instantiate(bulletCasing, shootingPoint.transform.position, Random.rotation);
        //BulletCasing.GetComponent<Rigidbody>().velocity = transform.TransformPoint(new Vector3(1, 1, 0)) * Time.deltaTime * bulletSpeed/3; 
        GameManager.instance.CameraShake(0.05f);
    }
}
