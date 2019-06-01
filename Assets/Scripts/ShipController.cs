using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
    // ジョイスティック
    private Joystick _joystick = null;
    private GameObject joystickob;
    // スピード
    float speed=2f;
    // プレイヤーが中にいるか
    bool playerin;
    // Use this for initialization
    void Start () {
        joystickob = GameObject.Find("Joystick");
        _joystick = joystickob.GetComponent<Joystick>();
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Mathf.Cos(Time.time * 5) /50
        if (playerin)
        {
            transform.Translate(speed*_joystick.Position.x*Time.deltaTime, speed * _joystick.Position.y *Time.deltaTime + (Mathf.Cos(Time.time * 5) / 50), 0);
        }
        else
        {
            transform.Translate(0, Mathf.Cos(Time.time * 5) / 50, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            playerin = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            playerin = false;
        }
    }
}
