using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPointController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name+"に入った");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name+"から出た");
    }
}
