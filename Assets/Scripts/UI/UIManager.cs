using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour {
    [SerializeField]
    GameObject HpTextob;
    [SerializeField]
    GameObject CoinTextob;
    [SerializeField]
    GameObject AmmoTextob;
    [SerializeField]
    GameObject EnemyDamageob;
    GameObject playerob;
    Player playersc;
    Text HPtextsc;
    Text CoinTextsc;
    Text AmmoTextsc;

    // Use this for initialization
    void Start () {
        playerob = GameObject.FindGameObjectWithTag("Player");
        playersc = playerob.GetComponent<Player>();
        // HP表示のTextのスクリプト取得
        HPtextsc = this.HpTextob.GetComponent<Text>();
        // コイン表示のTextのスクリプト取得
        CoinTextsc = this.CoinTextob.GetComponent<Text>();
        // 残弾数表示のTextのスクリプト取得
        AmmoTextsc = this.AmmoTextob.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        HPtextsc.text = "="+ GameManager.Instance.playerHP.ToString("F0");
        CoinTextsc.text = "=" + GameManager.Instance.coin.ToString("F0");
        AmmoTextsc.text = "=" + playersc.ShotNum.ToString("F0");
        //EDamageTextsc.text = ;
    }
    // 一時停止ボタンを押したとき
    public void ScreenPause()
    {
        // プレイヤーが死亡時はポーズ画面を押せない
        if (GameManager.Instance.playerdead == false)
        {
            if (Time.timeScale != 0)
            {
                // ポーズ音
                AudioManager.Instance.PlaySE(AUDIO.SE_PAUSESCREEN);
                Time.timeScale = 0f;
                SceneManager.LoadSceneAsync("Scene/Pause", LoadSceneMode.Additive);
            }
            else
            {
                // 再開時の音
                AudioManager.Instance.PlaySE(AUDIO.SE_PAUSEEXIT);
                SceneManager.UnloadSceneAsync("Scene/Pause");
                Time.timeScale = 1f;
            }
        }
    }
    // ジャンプボタン押してるとき
    public void JumpMove()
    {
        playersc.JumpMove();
    }
    // 発射ボタンを押したとき
    public void PlayerShoot()
    {
        playersc.PlayerShoot();
    }
    // リロード
    public void Reload()
    {
        playersc.Reload();
    }
    // 弾切り替え
    public void GunModeExchange()
    {
        playersc.GunModeExchange();
    }
    // Runボタンを押してるとき
    public void RunButton()
    {
        playersc.RunButton();
    }
    // Runボタン離したとき
    public void noRunButton()
    {
        playersc.noRunButton();
    }
    // ジャンプボタン離したとき
    public void noJump()
    {
        playersc.noJump();
    }
}
