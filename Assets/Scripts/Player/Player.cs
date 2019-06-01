using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    /*---------------------------------------------------------------------------------------------------------------
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　〇変数宣言　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     ---------------------------------------------------------------------------------------------------------------*/
    // HE弾
    public GameObject BulletH;
    // AP弾
    public GameObject BulletA;
    // 水しぶき
    public GameObject Sprashes;

    // ジョイスティック
    private Joystick _joystick = null;
    private GameObject joystickob;

    RaycastHit2D hit;
    [HideInInspector]
    public Rigidbody2D rigid2d;
    readonly float jumpforce = 55f;

    Animator anim;

    // ジャンプ回数制限
    int JumpNum = 0;
    readonly int JumpNumLim = 2;

    // 素早さ
    float speed;
    float speedwardvalue = 5;

    // ショットできる回数
    int ShotCanNum = 6;
    // ショットやった回数
    [HideInInspector]
    public int ShotNum = 0;

    // Playerのレベル
    [HideInInspector]
    public int Level;

    // 弾の速さ
    [HideInInspector]
    public float BulletSpeed = 1f;

    // 接地判定
    [HideInInspector]
    public LayerMask groundLayer;

    // ボタン操作判定
    // ↓ボタンかどうか
    [HideInInspector]
    public bool isDownbutton = false;
    // しゃがんでるかどうか
    public bool isCDownbutton = false;
    [HideInInspector]
    public bool isUpbutton = false;
    bool isMovebutton = false;
    [HideInInspector]
    public bool isJumpbutton=false;
    bool isShotbutton=false;
    bool isRun = false;
    [HideInInspector]
    public bool ShotSwitch = false;
    bool isFall = false;
    [HideInInspector]
    public bool isRide = false;
    bool isDrive = false;

    bool ShotPause;
    // プレイヤーが向いてる方向
    int p_direction=1;

    // 水中かどうか
    bool Underwater;
    // 風で浮かんでるか
    bool Floating;

    // 死んだとき
    [HideInInspector]
    public bool Dead;

    // 接地判定
    bool isGround=false;
    // 地面にタッチしたか
    bool isGroundTouch = false;

    // 動く床用
    private BoxCollider2D FlyGround;

    public LayerMask layerMask;

    private Renderer renderer_p;

    /*---------------------------------------------------------------------------------------------------------------
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　〇初期設定関連　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     ---------------------------------------------------------------------------------------------------------------*/
    void Start () {
        // プレイヤーは死んでいない
        GameManager.Instance.playerdead = false;
        //Bullet_o = (GameObject)Resources.Load("Prefab/PlayerBullet");
        // プレイヤーのレイヤー
        gameObject.layer = LayerMask.NameToLayer("player");
        // スクリプト取得関連
        this.rigid2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer_p = GetComponent<Renderer>();
        joystickob = GameObject.Find("Joystick");
        _joystick = joystickob.GetComponent<Joystick>();
        // 初期値
        ShotNum = ShotCanNum;
        Time.timeScale = 1f;

        //中間ポイントを取ってるなら中間地点からスタート
        if (GameManager.Instance.isHalfFlag) this.transform.position = GameManager.Instance.HalfPosition;
    }
    /*---------------------------------------------------------------------------------------------------------------
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　〇常時実行　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     ---------------------------------------------------------------------------------------------------------------*/
    void FixedUpdate()
    {
        if (Dead == false)
        {

            // 落ちているかどうかの判定
            if (rigid2d.velocity.y < -2 && isRide == false|| rigid2d.velocity.y > 2 && isRide == false) isFall = true;
            else isFall = false;

            // キー操作用
            if (_joystick.Position.x > 0 || Input.GetAxis("Horizontal") > 0) // 右に動かすとき
            {
                isMovebutton = true;
                // プレイヤーの向きは→
                if(p_direction != 1)
                {
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                p_direction = 1;
            }
            else if (_joystick.Position.x < 0 || Input.GetAxis("Horizontal") < 0) // 左に動かすとき
            {
                isMovebutton = true;
                // プレイヤーの向きは←
                if(p_direction != -1)
                {
                    transform.Rotate(new Vector3(0f, 180f, 0f));
                }
                p_direction = -1;
            }
            else // 方向キーを動かさないとき
            {
                isMovebutton = false;
            }

            /*----------------------------
              ボタンを実行したときの操作
            -----------------------------*/

            // 
            // 運転してないなら
            if (!isDrive)
            {
                // 方向キーを動かすなら
                if (isMovebutton)
                {
                    // 走るときのスピード
                    if (isRun && speed <= speedwardvalue * 2)
                    {
                        speed += 0.4f;
                    }
                    // 歩くときのスピード
                    else if (isRun == false && speed <= speedwardvalue)
                    {
                        speed += 0.2f;
                    }
                    else
                    {
                        speed -= 0.5f;
                    }
                }
                else if(speed > 0)
                {
                    speed -= 0.5f;
                }
                else if(speed < 0)
                {
                    speed = 0;
                }

                if (Input.GetAxis("Horizontal") == 0)
                {
                    transform.Translate(speed * p_direction * _joystick.Position.x * Time.deltaTime, 0, 0);
                }
                else
                {
                    transform.Translate(speed * p_direction * Input.GetAxis("Horizontal") * Time.deltaTime, 0, 0);
                }
                
            }
            // 回数を超えていないならジャンプボタンでジャンプ
            if (isJumpbutton && JumpNum <= JumpNumLim)
            {
                //anim.SetTrigger("Jump");
                this.rigid2d.AddForce(transform.up * jumpforce);
            }
            // 接地判定
            if (Physics2D.Linecast(transform.position, transform.position - transform.up * 1.05f, groundLayer) == true && isGroundTouch)
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
            
            // ジャンプ回数リセット
            if (isGround)
            {
                JumpNum = 0;
            }
            // 発射ボタン
            if (isShotbutton)
            {
                anim.SetBool("Shoot", isShotbutton);
            }
            // ジャンプ攻撃
            if (Level > 1 )
            {
                hit = Physics2D.Raycast(transform.position, Vector2.down, 15.5f, layerMask);
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, layerMask);
            }
            if (hit.collider != null)
            {
                // 踏むことが可能ならば
                if (hit.collider.gameObject.GetComponent<Enemy>().CanDepressed == true)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().Attacked(500,-1);
                    JumpPress();
                }
            }
            // アニメーションメソッド実行
            Anim();
        }
    }
    // 入力関連用
    void Update ()
    {
        if (Dead == false)
        {
            //デバッグ用
            if (Input.GetKeyDown(KeyCode.O))
            {
                rigid2d.velocity = Vector3.zero;
                transform.Translate(-5, 5, 0);
            }
            //デバッグ用
            if (Input.GetKeyDown(KeyCode.P))
            {
                rigid2d.velocity = Vector3.zero;
                transform.Translate(5, 5, 0);
            }
            if (Input.GetKey(KeyCode.Z)) PlayerShoot();
            if (Input.GetKey(KeyCode.C)) RunButton();
            if (Input.GetKeyUp(KeyCode.C)) noRunButton();
            if (Input.GetKeyDown(KeyCode.UpArrow)) JumpMove();
            if (Input.GetKeyUp(KeyCode.UpArrow)) noJump();
            //パソコン用ここまで
            if (transform.position.y < -10)
            {
                StartCoroutine("DeadPlayer");
            }
        }

    }
    /*---------------------------------------------------------------------------------------------------------------
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　〇入力関連　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     ---------------------------------------------------------------------------------------------------------------*/

    // ジャンプボタン押してるとき
    public void JumpMove()
    {
        if (Dead == false && isShotbutton == false && Time.timeScale != 0)
        { 
            if (Underwater || Floating)
            {
                // ジャンプ音
                AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
                // yの加速度をリセットする
                rigid2d.velocity = new Vector2(0, 0);//rigid2d.velocity.x, 0);
                // ジャンプボタンはONの状態
                isJumpbutton = true;
                // コルーチンを呼び出す
                StartCoroutine("JumpUpLimit");
            }
            else
            {
                // 回数を増やす
                JumpNum++;
                if (JumpNum <= JumpNumLim)
                {
                    if (isGroundTouch)
                    {
                        // 地面には着いてない
                        isGroundTouch = false;
                    }
                    // ジャンプ音
                    AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
                    // yの加速度をリセットする
                    rigid2d.velocity = new Vector2(rigid2d.velocity.x, 0);
                    // ジャンプボタンはONの状態
                    isJumpbutton = true;
                    // コルーチンを呼び出す
                    StartCoroutine("JumpUpLimit");

                }
            }
        }
    }
    // 発射ボタンを押したとき
    public void PlayerShoot()
    {
        if (Dead == false && isGround == true &&  ShotNum> 0 && Time.timeScale != 0 && isShotbutton == false && ShotPause == false)
        {
            ShotNum--;
            isShotbutton = true;
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOT);
            GUN();
            ShotPause = true;
            StartCoroutine("ShotPauseTime");
        }
    }
    // 弾種切り替え
    public void GunModeExchange()
    {
        if (Dead == false && Time.timeScale != 0)
        {
            ShotSwitch = !ShotSwitch;
            AudioManager.Instance.PlaySE(AUDIO.SE_APLOAD);
        }
    }
    // リロード
    public void Reload()
    {
        StartCoroutine("ShotReloadTime");
    }
    // Runボタンを押してるとき
    public void RunButton()
    {
        isRun = true;
    }
    // Runボタン離したとき
    public void noRunButton()
    {
        isRun = false;
    }
    // ジャンプボタン離したとき
    public void noJump()
    {
        isJumpbutton = false;
    }
    // 弾の生成
    void GUN()
    {
        Vector3 BulletPosition;
        if (p_direction == -1)
        {
            BulletPosition = new Vector3(transform.position.x - 0.5f, transform.position.y - 0.25f, transform.position.z);
        }
        else
        {
            BulletPosition = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.25f, transform.position.z);
        }
        if (ShotSwitch)
        {
            // 弾を生成する
            Instantiate(BulletA, BulletPosition, transform.rotation);
        }
        else
        {
            // 弾を生成する
            Instantiate(BulletH, BulletPosition, transform.rotation);
        }
    }
    // 連射間隔
    IEnumerator ShotPauseTime()
    {
        // 待機中
        yield return new WaitForSeconds(1f);
        // 待機終了
        isShotbutton = false;
        ShotPause = false;
    }
    // リロード
    IEnumerator ShotReloadTime()
    {
        // リロード中
        yield return new WaitForSeconds(1f);
        // リロード音
        AudioManager.Instance.PlaySE(AUDIO.SE_RELOAD);
        yield return new WaitForSeconds(2f);
        // リロード完了
        isShotbutton = false;
        ShotNum = ShotCanNum;
    }
    // ジャンプの押してる限界
    IEnumerator JumpUpLimit()
    {
        yield return new WaitForSeconds(0.2f);
        isJumpbutton = false;
    }
    void Anim()
    {
        //Animatorへパラメーターを送る
        //anim.SetFloat
        anim.SetBool("isGround", isGround);
        anim.SetBool("Walk", isMovebutton && isDrive==false);
        anim.SetBool("Jump", isJumpbutton || isFall);
        anim.SetBool("Shoot", isShotbutton);
        anim.SetBool("Run", isRun && isMovebutton && isRide==false);
        anim.SetBool("CDown", isCDownbutton);
    }
    void LevelUP()
    {
        AudioManager.Instance.PlaySE(AUDIO.SE_LEVELUP);
        this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }
    // 水中の処理
    void Sprashe()
    {
        Quaternion Sprashnormal;
        Quaternion Sprashmirror;
        Sprashnormal = Quaternion.Euler(0, 0, 0);
        Sprashmirror = Quaternion.Euler(0, 180, 0);
        if (p_direction == 1)
        {

            // 水しぶきを生成
            Instantiate(Sprashes, transform.position - new Vector3(-0.5f, 0), Sprashnormal);
            // 水しぶきを生成
            Instantiate(Sprashes, transform.position - new Vector3(0.5f, 0), Sprashmirror);
        }
        else
        {
            // 水しぶきを生成
            Instantiate(Sprashes, transform.position - new Vector3(-0.5f, 0), Sprashnormal);
            // 水しぶきを生成
            Instantiate(Sprashes, transform.position - new Vector3(0.5f, 0), Sprashmirror);
        }
        // ザブーン
        AudioManager.Instance.PlaySE(AUDIO.SE_DIVING);
    }
/*---------------------------------------------------------------------------------------------------------------
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　〇コライダー（当たり判定・喰らい判定など）　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 ---------------------------------------------------------------------------------------------------------------*/
    void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        Debug.Log(layerName);
        if (layerName == ("Enemy") && !Dead)
        {
            DeadorAlive();
        }
        if(layerName == ("ground"))
        {
            isGroundTouch = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        // アイテム取得時
        if (collision.gameObject.tag == ("TastyMush"))
        {
            if(Level == 0)
            {
                Level = 1;
                LevelUP();
            }
            else
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_STUPID);
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == ("NormalMush"))
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_STUPID);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == ("PoisonMush"))
        {
            DeadorAlive();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == ("KillMush"))
        {
            StartCoroutine("DeadPlayer");
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == ("1UPMush"))
        {
            GameManager.Instance.PlayerHPUp(1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == ("Coin"))
        {
            GameManager.Instance.CoinPlus();
            Destroy(collision.gameObject);
        }
        // 羽ブロックや乗り物
        if (collision.gameObject.tag == ("Ship"))
        {
            isRide = true;
            isDrive = true;
            transform.parent = collision.gameObject.transform;
        }
        // 水に入る
        if (layerName == ("Water") && Dead == false)
        {
            // ジャンプ回数リセット
            JumpNum = 0;
            Underwater = true;
            Sprashe();
            rigid2d.drag = 6.0f;
        }
        // 風ポイントで浮かぶ
        if(layerName == "Wind")
        {
            // ジャンプ回数リセット
            JumpNum = 0;
            Floating = true;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        // 羽ブロックや乗り物
        if (collision.gameObject.tag == ("Ship") && !isDrive)
        {
            isRide = true;
            isDrive = true;
            transform.parent = collision.gameObject.transform;
        }
        // 土管ワープ処理
        if (collision.gameObject.tag == ("PIPE") && isCDownbutton)
        {
            transform.position = new Vector3(250, 5, 0);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent = null;
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (collision.gameObject.tag == ("Ship"))
        {
            isRide = false;
            isDrive = false;
        }
        // 水から出る
        if (layerName == ("Water") && Dead == false)
        {
            Underwater = false;
            Sprashe();
            rigid2d.drag = 0;
        }
        // 風ポイントから離れる
        if (layerName == "Wind")
        {
            Floating = false;
        }
    }
    /*---------------------------------------------------------------------------------------------------------------
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　〇その他　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
     ---------------------------------------------------------------------------------------------------------------*/
    // 踏んだ時のジャンプ
    void JumpPress()
    {
        if (Dead == false && isShotbutton == false && Time.timeScale != 0)
        {
            // ジャンプ音
            AudioManager.Instance.PlaySE(AUDIO.SE_PRESS);
            // yの加速度をリセットする
            rigid2d.velocity = new Vector2(rigid2d.velocity.x, 0);

            // 加速度を加える
            this.rigid2d.AddForce(transform.up * jumpforce * 5);
            // コルーチンを呼び出す
            StartCoroutine("JumpUpLimit");
        }
    }
    // ダメージ時の無敵時間
    IEnumerator NotHit()
    {
        //レイヤーをPlayerDamageに変更
        gameObject.layer = LayerMask.NameToLayer("NotHit");
        //while文を10回ループ
        int count = 10;
        while (count > 0)
        {
            //透明にする
            renderer_p.material.color = new Color(1, 1, 1, 0);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            //元に戻す
            renderer_p.material.color = new Color(1, 1, 1, 1);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            count--;
        }
        //レイヤーをPlayerに戻す
        gameObject.layer = LayerMask.NameToLayer("player");
    }
/*---------------------------------------------------------------------------------------------------------------
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　〇死亡判定と死亡演出　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 -　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
 ---------------------------------------------------------------------------------------------------------------*/
    void DeadorAlive()
    {
        if (Level > 0)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_LEVELDOWN);
            this.transform.localScale = new Vector3(1, 1, 1);
            Level = 0;
            StartCoroutine("NotHit");
        }
        else
        {
            StartCoroutine("DeadPlayer");
        }
    }

    // Playerが死んだときの挙動
    IEnumerator DeadPlayer()
    {
        if (!GameManager.Instance.playerdead)
        {
            //anim.SetTrigger("Dead");
            GameManager.Instance.playerdead = true;
            //Playerは死んだ状態
            Dead = true;
            // BGMを停める
            AudioManager.Instance.StopBGM();
            // 死亡時SEを鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_DEAD);
            //物理挙動を一時的になくす
            rigid2d.velocity = Vector3.zero;
            rigid2d.isKinematic = true;
            //Playerのレイヤーをすり抜けるレイヤーに変更
            gameObject.layer = LayerMask.NameToLayer("dead");
            //死んだときのアニメーションに設定
            anim.SetBool("Dead", Dead);
            yield return new WaitForSeconds(0.1f);
            //物理挙動を許可する
            rigid2d.isKinematic = false;
            //ジャンプ
            this.rigid2d.AddForce(transform.up * 1000);
            rigid2d.gravityScale = 6.0f;
            yield return new WaitForSeconds(1.5f);
            GameManager.Instance.PlayerHPDown();
        }
    }
}