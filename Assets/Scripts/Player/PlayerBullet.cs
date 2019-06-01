using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBullet : MonoBehaviour {
    Rigidbody2D rigid2D;
    Vector2 speed;
    [HideInInspector]
    public int damage=100;
    [SerializeField]
    int pass;
    int hit = 0;//AP弾のヒット数
    readonly int hit_max=3;//AP弾のヒット上限
	// Use this for initialization
	void Start () {
        rigid2D = GetComponent<Rigidbody2D>();
        rigid2D.velocity = transform.right.normalized * 45;
        Destroy(gameObject, 1f);
    }
    void Update()
    {
        damage--;
    }
    // 当たった時
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たった時のダメージの値を乱数にする
        damage += Random.Range(0, 25) - Random.Range(0, 20);
        if (collision.gameObject.tag == "Enemy")
        {
            hit++;//ヒット回数を増やす
            collision.gameObject.GetComponent<Enemy>().Attacked(this.damage,pass);
            if(hit >= hit_max || pass == 0)
            {
                Destroy(gameObject);
            }
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
        if (layerName == "ground")
        {
            Destroy(gameObject);
        }
    }
}
