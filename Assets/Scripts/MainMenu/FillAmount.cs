using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillAmount : MonoBehaviour
{
    public Image image;
    bool finished = true;
    float targetAmt;
    public float amt;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            finished = false;
            if (targetAmt == 0)
            {
                targetAmt = 1;
                amt = 0;
            }
            else
            {
                targetAmt = 0;    
                amt = 1;            
            }
        }

        if (!finished)
        {
            if (targetAmt == 1)
                amt += Time.deltaTime;
            else
                amt -= Time.deltaTime;

            image.fillAmount = amt;

            if (amt > 1 || amt < 0)
            {
                finished = true;
            }
        }
    }
}
