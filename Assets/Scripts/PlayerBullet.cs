using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBullet : MonoBehaviour {
    Rigidbody2D rigid2D;
    Vector2 speed;
    [HideInInspector]
    public int damage=100;
    // 貫通するかどうか
    [SerializeField]
    bool pass;
	// Use this for initialization
	void Start () {
        rigid2D = GetComponent<Rigidbody2D>();
        rigid2D.velocity = transform.right.normalized * 45;
        damage--;
        Destroy(gameObject, 1f);
    }
    // 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<SmallEnemy>().Attacked(this.damage);
        }
        if (collision.gameObject.tag == "BossEnemy")
        {
            SceneManager.LoadScene("Scene/Clear");
        }
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        // 水に入る
        if (layerName == ("Water"))
        {
            rigid2D.drag = 12.0f;
        }
        if (layerName == "ground" || pass != false)
        {
            Destroy(gameObject);
        }
    }
}
