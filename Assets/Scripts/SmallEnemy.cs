using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour {
    Rigidbody2D rigid2d;
    public float speed;
    public float turnsecond;
    // レイヤーマスク
    public LayerMask layerMask;
    // 踏むことが可能か
    public bool CanDepressed;
    // 死んでるかどうか
    public bool EnemyDead;
    // 体力
    [SerializeField]
    private int Life;
    // 敵のレベル
    int Level;
    // 敵のレベルアップ
    int LevelUp;
    [SerializeField]
    bool honoo;
    [SerializeField]
    public GameObject BulletH;
    // アイテムを食べるどうか
    [SerializeField]
    bool CanEat;

    // Use this for initialization
    void Start () {
        this.rigid2d = GetComponent<Rigidbody2D>();
        StartCoroutine("Turn");
        //Life = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (honoo)
        {
            //Debug.Log(EnemyLife);
        }
        if (EnemyDead == false)
        {
            // 移動
            transform.Translate(-Time.deltaTime * speed, 0, 0);
        }
        // 敵の大きさ関係
        if(Life <= 100 && Level != 1)
        {
            Level = 1;
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(Life > 100 && Life <= 200 && Level != 2)
        {
            Level = 2;
            this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        
    }
    IEnumerator Turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnsecond);
            transform.Rotate(new Vector3(0f, 180f, 0f));
            if (honoo)
            {
                bress();
            }
        }
    }
    void EnemyAlive()
    {
        rigid2d.velocity = Vector3.zero;
        if (Life <= 0)
        {
            StartCoroutine("DeadEnemy");
        }
    }
    IEnumerator DeadEnemy()
    {
        EnemyDead = true;
        rigid2d.velocity = Vector3.zero;
        //レイヤーをすり抜けるレイヤーに変更
        gameObject.layer = LayerMask.NameToLayer("dead");
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(new Vector3(0, 0, 180));
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (CanEat)
        {
            if (collision.gameObject.tag == ("TastyMush"))
            {
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == ("NormalMush") || collision.gameObject.tag == ("1UPMush"))
            {
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == ("PoisonMush"))
            {
                Attacked(100);
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == ("KillMush"))
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_NODEADPRESS);
            StartCoroutine("DeadEnemy");
            Destroy(collision.gameObject);
        }
    }
    public void Attacked(int AmountofDamage) // 喰らったときの判定
    {
        // 敵がレベルアップしたかどうか
        if (LevelUp > Level)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_LEVELUP);
            LevelUp = Level;
        }
        // 敵がレベルダウンしたかどうか
        if (LevelUp < Level)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_NODEADPRESS);
            LevelUp = Level;
        }
        if (Level <= 0)
        {
            Life -= AmountofDamage;
        }
        GameManager.Instance.AttackedDamage(AmountofDamage);
        EnemyAlive();
    }
    void bress()
    {
        // 弾を生成する
        Instantiate(BulletH, transform.position-new Vector3(5.5f,0,0), transform.rotation);
    }
}
