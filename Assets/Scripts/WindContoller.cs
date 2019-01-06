using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindContoller : MonoBehaviour {
    Rigidbody2D rigid2d;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (rigid2d == null)
            {
                rigid2d = collision.gameObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                if (rigid2d.velocity.y < 5)
                {
                    rigid2d.AddForce(transform.up * 50);
                }
            }
        }
    }
}
