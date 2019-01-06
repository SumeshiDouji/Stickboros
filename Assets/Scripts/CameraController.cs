using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    GameObject player;
    Player playersc;

    float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

    bool playerdead;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playersc = player.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        playerdead = playersc.Dead;
        if (playerdead==false)
        { 
            float x, y;
            // 制限範囲よりも外ならカメラを動かさない、逆ならばカメラを動かす
            if (player.transform.position.x < 0)
            {
                x = this.transform.position.x;
            }
            else
            {
                x = player.transform.position.x;
            }
            if(player.transform.position.y < -2.5f)
            {
                y = this.transform.position.y;
            }
            else
            {
                y = player.transform.position.y+1;
            }
            // ↑ボタン押したとき
            if (playersc.isUpbutton && player.transform.position.y+5 > transform.position.y && player.transform.position.y >= -2.5f)
            {
                y += Time.deltaTime*100;
            }
            // ↓ボタン押したとき
            if (playersc.isDownbutton && player.transform.position.y - 2 < transform.position.y && player.transform.position.y >= -2.5f)
            {
                y -= Time.deltaTime*100;
            }
            Vector3 targetPos = new Vector3(x, y, this.transform.position.z);
            //SmoothDampを使うことによって追従を遅延させることができる、詳しくはググろう
            transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref velocity, smoothTime/2);//new Vector3(player.transform.position.x, player.transform.position.y + 3, this.transform.position.z);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
