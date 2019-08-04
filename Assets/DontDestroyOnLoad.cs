using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	public static DontDestroyOnLoad instance;

    private void Awake() {
    	if (instance != null && instance != this) {
            Debug.LogError("Ya fucked up A-A-Ron");
            DestroyImmediate(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

}