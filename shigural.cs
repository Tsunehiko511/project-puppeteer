using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class shigural : MonoBehaviour {
    GA_SHIGURAL_ORIGIN ga_shigural_origin;
    int flag = 0;
    int counter = 0;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (flag == 0)
        {
            ga_shigural_origin = new GA_SHIGURAL_ORIGIN();
            flag = 1;              
        }
        counter = (counter + 1) % 2;
        if(counter==0) ga_shigural_origin.Play();
	}
}
