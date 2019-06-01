using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    Rigidbody2D rigid2d;
    public float speed;
    public float turnsecond;
    // レイヤーマスク
    public LayerMask layerMask;
    // 踏むことが可能か
    [HideInInspector]
    public bool CanDepressed;
    // 死んでるかどうか
    [HideInInspector]
    public bool EnemyDead;
    // 体力（体力の初期値はScriptableObjectのEnemyStatusDataを参照）
    [SerializeField]
    private int HP;
    private int PrevHP;
    // 敵のレベル
    [SerializeField]
    int Level;
    // レベルダウンアップの際の数
    int LevelUp_HP_Difference;
    [SerializeField]
    bool Not_move;
    // 敵のレベル上限
    //[SerializeField]
    int LevelLimit=5;
    [SerializeField]
    bool hasnot_hp;
    [SerializeField]
    bool honoo;
    [SerializeField]
    public GameObject BulletH;
    [SerializeField]
    int CharactorNumber;

    //ScriptableObjectのデータ
    EnemyStatus enemyStatus;
    // Use this for initialization
    void Start () {
        // データの読み込み
        //enemyStatusData = Resources.Load<EnemyStatusData>("EnemyStatusData");
        /// <summary>
        /// ステータス管理用のキャラクターの番号
        /// 0   クリ
        /// 1   ケイム
        /// 
        /// </summary>
        // 指定されたリストの番号に従って使う
        enemyStatus = EnemyStatusData.Entity.EnemyStatusList[CharactorNumber];
        CanDepressed = enemyStatus.CanDepressed;
        HP = enemyStatus.HP;
        // レベルに合ったHPの初期化
        HP += enemyStatus.HP  * (Level - 1) / 3;
        PrevHP = HP;
        // レベルダウンアップの際の数の差を計算
        LevelUp_HP_Difference = (enemyStatus.HP + enemyStatus.HP * (Level - 1) / 3) / Level;
        // レベルに合ったキャラのサイズの初期化
        this.transform.localScale = new Vector3(3 + (Level -1)* 2, 3 + (Level - 1) * 2, 1);
        // Rigidbody取得
        this.rigid2d = GetComponent<Rigidbody2D>();

        //計算で分母を０にしないための対策
        if(Level<=0) Level = 1;
        

        StartCoroutine("Turn");
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
    }
    IEnumerator Turn()
    {
        if (Not_move == false)//動くならば
        {
            while (true)//ループを開始
            {
                yield return new WaitForSeconds(turnsecond);
                transform.Rotate(new Vector3(0f, 180f, 0f));
                if (honoo)
                {
                    Bress();
                }
            }
        }
    }
    void EnemyAlive()
    {
        rigid2d.velocity = Vector3.zero;
        if (HP <= 0)
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
        // アイテムを食べれるかどうか
        if (enemyStatus.CanEat)
        {
            if (collision.gameObject.tag == ("TastyMush"))
            {
                Attacked(-LevelUp_HP_Difference, -1);
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == ("NormalMush") || collision.gameObject.tag == ("1UPMush"))
            {
                Attacked(-LevelUp_HP_Difference/2, -1);
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.tag == ("PoisonMush"))
            {
                Attacked(LevelUp_HP_Difference, -1);
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
    // 喰らったときのダメージ判定（APかHEでダメージ判定が変化）
    public void Attacked(int AmountofDamage,int Damage_dicision) // 喰らったときの判定
    {
        //　ダメージの値
        if (AmountofDamage > 0)
        {
            if (Damage_dicision == 1)//貫通すれば
            {
                AmountofDamage = AmountofDamage * Level / 2;//ダメージとレベルの乗算をして、二分の一にする
            }
            else if(Damage_dicision == 0)//貫通するものでなければ
            {
                AmountofDamage /= Level;//ダメージの数はレベルの分だけ割る
            }
            // それ以外（ジャンプ攻撃など）は固定ダメージとします
        }
        if(hasnot_hp==false) HP -= AmountofDamage;//　体力があれば体力との減算をする

        LevelDicision(AmountofDamage);
        
        GameManager.Instance.AmountOfDamage(AmountofDamage, transform.position);//ダメージ表示
        EnemyAlive();
    }
    void Bress()
    {
        // 弾を生成する
        Instantiate(BulletH, transform.position-new Vector3(5.5f,0,0), transform.rotation);
    }
    void LevelDicision(int AmountDamage)
    {
        Debug.Log(HP);
        Debug.Log(PrevHP - LevelUp_HP_Difference);
        // HPが前のHPの３３パー以下ならばレベルダウン
        if (HP <= PrevHP - LevelUp_HP_Difference && Level > 1)
        {
            PrevHP = PrevHP - LevelUp_HP_Difference;
            LevelUp(false);
        }
        else if(HP >= PrevHP - LevelUp_HP_Difference && AmountDamage >= 0)
        {
            // 喰らったときの音
            AudioManager.Instance.PlaySE(AUDIO.SE_NODEADPRESS);
        }
        // HPが前のHPの３３パー以上あればレベルアップ
        if (HP >= PrevHP + LevelUp_HP_Difference && Level < LevelLimit)
        {
            PrevHP = PrevHP + LevelUp_HP_Difference;
            LevelUp(true);
        }
        else if(HP < PrevHP + LevelUp_HP_Difference && AmountDamage < 0)
        {
            // 回復したときの音
            AudioManager.Instance.PlaySE(AUDIO.SE_KAIFUKU_ENEMY);
        }

    }
    void LevelUp(bool WardValue)
    {
        // 敵の大きさ関係
        if (WardValue == false)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_LEVELDOWN_ENEMY);
            Level--;
            this.transform.localScale -= new Vector3(2f, 2f, 0);
        }
        else if (WardValue)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_LEVELUP_ENEMY);
            Level++;
            this.transform.localScale += new Vector3(2f, 2f, 0);
        }
        
    }
}
