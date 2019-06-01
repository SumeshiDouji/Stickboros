using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagContoller : MonoBehaviour {

    //フラグナンバー（フラグナンバーが高いほど、中間地点としての優先度は高い）
    [SerializeField]
    private int FlagNumber;
    [SerializeField]
    bool isGoalFlag;

    void Start()
    {
        // フラグを取ってるなら
        if(!isGoalFlag && GameManager.Instance.HalfFlagNum >= FlagNumber)
        {
            Debug.Log(GameManager.Instance.HalfFlagNum);
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 中間地点やゴールの旗取得時
        if (collision.gameObject.tag == ("Player"))
        {
            //中間フラグ
            if (!isGoalFlag)
            {
                GameManager.Instance.isHalfFlag = true;
                // フラグナンバーが高ければフラグナンバーを代入
                if(GameManager.Instance.HalfFlagNum < FlagNumber)
                {
                    GameManager.Instance.HalfFlagNum = FlagNumber;
                    GameManager.Instance.HalfPosition = transform.position;
                }
                AudioManager.Instance.PlaySE(AUDIO.SE_HALFFLAG);
                Destroy(gameObject);
            }
            //ゴールフラグ
            else
            {
                GameManager.Instance.ChangeScene("Scene/Clear");
            }
        }
    }
}
