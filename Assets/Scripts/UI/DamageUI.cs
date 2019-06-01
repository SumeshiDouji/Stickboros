using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    private Text damageText;
    //　テキストの透明度
    private float alpha;
    //　フェードアウトするスピード
    private float fadeOutSpeed = 1f;
    private float red=0,green=0,blue=0;
    //　移動値
    [SerializeField]
    private float moveValue = 0.9f;

    void Start()
    {
        damageText = GetComponentInChildren<Text>();
        //　不透明度は最初は1.0f
        alpha = 1f;
        //　テキストのcolorを設定
        if (GameManager.Instance.EnemyDamage < 0)
        {
            GameManager.Instance.EnemyDamage *= -1;
            red = 1f;
        }
        else
        {
            green = 0.2f;
            blue = 1f;
        }
        damageText.text = GameManager.Instance.EnemyDamage.ToString("F0");
    }

    void LateUpdate()
    {
        damageText.color = new Color(red, green, blue, alpha);// 0.6f 0.1f 0.4fで毒っぽい色

        //　少しづつ透明にしていく
        alpha -= fadeOutSpeed * Time.deltaTime;

        transform.position += Vector3.up * moveValue * Time.deltaTime;

        moveValue -= 0.3f;

        if (alpha < 0f)
        {
            Destroy(gameObject);
        }
    }
}