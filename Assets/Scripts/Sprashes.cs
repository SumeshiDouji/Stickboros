using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprashes : MonoBehaviour {
    public float speedx;
    public float speedy;
    float destroytime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speedx*Time.deltaTime, speedy * Time.deltaTime, 0);
        destroytime += Time.deltaTime;
        if(destroytime > 0.4f)
        {
            Destroy(gameObject);
        }
	}
}
