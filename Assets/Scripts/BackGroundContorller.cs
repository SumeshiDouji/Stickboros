using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundContorller : MonoBehaviour {
    [SerializeField]
    float speed,last_position=0, warp_position=0;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(speed, 0, 0);
        if (transform.position.x > last_position)
        {
            transform.position = new Vector3(warp_position, transform.position.y, transform.position.z);
        }
    }
}
