using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class FillAmount : MonoBehaviour
{
    public Panel[] panels;
    float targetAmt;

    public void ChangeTo(int index)
    {
        StartCoroutine(TurnOff(index, done => {
        }));
    }

    IEnumerator TurnOff (int index, System.Action<bool> done)
    {
        // Turn all of the images off
        foreach (var panel in panels)
        {
            foreach (var image in panel.images)
            {
                if (image.fillAmount > .1f)
                {
                    StartCoroutine(IFillAmount(false, image, threadSafe => {
                        if (threadSafe)
                        {      
                            panel.panel.SetActive(false);              
                        }
                    }));
                }
            }                      
        }

        done(true);
        yield return new WaitForSeconds(1.25f);
        StartCoroutine(TurnOn(index, threadSafe => {

        }));
    }

    IEnumerator TurnOn(int index, System.Action<bool> done)
    {
        // Turn on the one we want
        for (int i = 0; i < panels[index].images.Length; i++)
        {
            //Set the panel active
            panels[index].panel.SetActive(true);  
            if (panels[index].images[i].fillAmount < .9f)
            {
                try {
                    panels[index].images[i].GetComponent<Button>().interactable = true;
                }
                catch(SystemException e) {
                    Debug.Log(e);
                }   
                StartCoroutine(IFillAmount(true, panels[index].images[i], threadSafe => {
                    if (threadSafe)
                    {                   
                    }
                }));
            }
        }

        done(true);
        yield return new WaitForEndOfFrame();
    }

    IEnumerator IFillAmount(bool full, Image image, System.Action<bool> done)
    {
        float minAmt = full? 1: 0;
        float amt = full? 0: 1;        
        if (minAmt > .8f)
        {            
            while(amt < minAmt)
            {
                amt += Time.deltaTime;
                image.fillAmount = amt;
                yield return new WaitForEndOfFrame();
            }        
        }
        else
        {
            while(amt > minAmt)
            {
                amt -= Time.deltaTime;
                image.fillAmount = amt;
                yield return new WaitForEndOfFrame();
            }        
        }
        done(true);
        yield return done;
    }
}
[System.Serializable]
public class Panel
{
    public GameObject panel;
    public Image[] images;
    public bool enabled = false;
}