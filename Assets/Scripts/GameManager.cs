using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [HideInInspector]
    public bool playerdead;
    [HideInInspector]
    public int playerHP=3;
    [HideInInspector]
    public int coin = 0;
    [HideInInspector]
    public int EnemyDamage;
    [HideInInspector]
    public bool isHalfFlag = false;
    [HideInInspector]
    public int HalfFlagNum;
    [HideInInspector]
    public Vector3 HalfPosition;
    [HideInInspector]
    // 前のシーン格納、とりあえず初期値はタイトル画面
    public string PreviousScene="Scene/Title";
    private GameObject damageUI;
    // Use this for initialization
    void Start () {

        damageUI = Resources.Load("Prefab/EnemyDamage") as GameObject;
        //　シーンが遷移しても消えないように
        DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () {
        // UIManagerを見つけたとき、それが自分以外なら
        if (GameObject.Find("GameManager") != this.gameObject)
        {
            // 二つ以上常駐させないため、自分自身を削除
            Destroy(gameObject);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            // エスケープキー取得
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // アプリケーション終了
                Application.Quit();
                return;
            }
        }
    }
    public void PlayerHPDown()
    {
        playerHP--;
        if (playerHP > 0)
        {
            ChangeScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            ChangeScene("Scene/GameOver");
            playerHP = 3;
        }
    }
    public void PlayerHPUp(int i)
    {
        AudioManager.Instance.PlaySE(AUDIO.SE_KAIFUKU);
        // iの分だけ増やす
        playerHP+=i;
    }
    public void CoinPlus()
    {
        coin++;
        // コインが100の倍数ならばHPを回復する
        if (coin % 100 == 0)
        {
            PlayerHPUp(1);
        }
        else
        {
            //コインが100の倍数でないなら、コイン獲得の音を鳴らす
            AudioManager.Instance.PlaySE(AUDIO.SE_COIN);
        }
    }
    public void ChangeScene(string sceneName)
    {
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            PreviousScene = SceneManager.GetActiveScene().name;
        }
        SceneManager.LoadScene(sceneName);
    }
    public void AmountOfDamage(int Damage,Vector3 DamagedPosition)
    {
        // 取得したダメージを共有用へ
        EnemyDamage = Damage;
        GameObject.Instantiate(damageUI, DamagedPosition, transform.rotation);
    }
}
