using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugExit : MonoBehaviour
{

	public GameManager gm;

    void OnTriggerEnter(Collider other){
    	if(other.tag == "Player"){
    		GameManager.instance.CompleteLevel();
    	}
    }
}
