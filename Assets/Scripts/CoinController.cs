using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {
    public bool boxin;
    float Destroytime=0;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (boxin)
        {
            if (Destroytime == 0)
            {
                GameManager.Instance.CoinPlus();
            }
            else if (Destroytime <= 0.3)
            {
                transform.Translate(0, Time.deltaTime * 10, 0);
            }
            else
            {
                Destroy(gameObject);
            }
            Destroytime += Time.deltaTime;
        }
        else
        {
            transform.Rotate(new Vector3(0, Time.deltaTime * 200, 0));
        }
    }
}
