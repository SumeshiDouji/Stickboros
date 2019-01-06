using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
    float speed = 1.5f;
    float turnsecond = 2f;

    [HideInInspector]
    public int PlayerLevelUp = 0;
    // Use this for initialization
    void Start () {
        StartCoroutine("Turn");
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Time.deltaTime * speed, 0, 0);
    }
    IEnumerator Turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnsecond);
            speed *= -1;
        }
    }
}
