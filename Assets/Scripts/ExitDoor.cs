using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour{

    public GameObject particles;
    public GameObject portal;
    private bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        particles.SetActive(false);
        portal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.doorOpened)
        {
            particles.SetActive(true);
            portal.SetActive(true);
            open = true;
        }
        else
        {
            particles.SetActive(false);
            portal.SetActive(false);
            open = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!open) {
            return;
        }

        if (other.tag == "Player")
        {
            GameManager.instance.CompleteLevel();
        }
    }
}
