using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyGround : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            //乗った状態
            collision.gameObject.GetComponent<Player>().isRide = true;
            collision.transform.parent = gameObject.transform;
            Debug.Log(collision.gameObject.tag);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        //降りた状態
        collision.gameObject.GetComponent<Player>().isRide = false;
        collision.transform.parent = null;
    }
}
