using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour {

    public bool BAIn;

    // エリア領域に入ったなら
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == ("Player"))
        {
            BAIn = true;
        }
    }
    // エリア領域を出たなら
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == ("Player"))
        {
            BAIn = false;
        }
    }
}
