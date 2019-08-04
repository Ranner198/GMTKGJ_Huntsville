using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAudioEnd : MonoBehaviour {
    IEnumerator Start() {
        AudioSource source = GetComponent<AudioSource>();
        while(source.isPlaying == false) {
            yield return null;
        }
        while (source.isPlaying) {
            yield return null;
        }
        Destroy(gameObject);
    }
}