using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ParticleEmiter : MonoBehaviour
{
    public GameObject particle;
    public float spawnRate = 2;
    public float size = 2.5f;
    public float sizeRate = 0.08f;
    public float fadeRate = 0.01f;
    private List<GameObject> objects = new List<GameObject>();
    void Start()
    {
        InvokeRepeating("wrapper", spawnRate, spawnRate);
    }

    private void wrapper()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()    
    {
        var spawned = Instantiate(particle, transform.position, transform.rotation);
        objects.Add(spawned);
        var imageRef = spawned.transform.GetChild(0).GetComponent<SpriteRenderer>();
        float counter = 0;
        float slowdown = 0.98f;
        float sizeChangeRate = sizeRate;
        while(counter < size)
        {            
            counter += Time.deltaTime;
            Vector3 tempSize = spawned.transform.localScale;
            tempSize = new Vector3(tempSize.x + sizeChangeRate * (Time.deltaTime * 60), tempSize.y + sizeChangeRate * (Time.deltaTime * 60), tempSize.z + sizeChangeRate * (Time.deltaTime * 60));
            spawned.transform.localScale = tempSize;
            var temp = imageRef.color.a;
            imageRef.color = new Color(imageRef.color.r, imageRef.color.g, imageRef.color.b, temp-fadeRate);
            sizeChangeRate *= slowdown;
            yield return new WaitForEndOfFrame();
        }

        Destroy(spawned);
    }

    void OnDestroy()
    {
        foreach(GameObject go in objects)
            Destroy(go);
    }
}
