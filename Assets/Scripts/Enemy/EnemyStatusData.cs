using System.Collections.Generic;
using UnityEngine;

//ScriptableObjectを継承したクラス
[CreateAssetMenu(
  fileName = "EnemyStatusData",
  menuName = "ScriptableObject/EnemyStatusData",
  order = 0)
]
public class EnemyStatusData : ScriptableObject
{
    //MyScriptableObjectが保存してある場所のパス
    public const string PATH = "Data/EnemyStatusData";

    //MyScriptableObjectの実体
    private static EnemyStatusData _entity;
    public static EnemyStatusData Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<EnemyStatusData>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _entity;
        }
    }
    //ListステータスのList
    public List<EnemyStatus> EnemyStatusList = new List<EnemyStatus>();

}

//System.Serializableを設定しないと、データを保持できない(シリアライズできない)ので注意
[System.Serializable]
public class EnemyStatus
{

    //設定したいデータの変数
    public string Name = "なまえ";
    public int HP = 100, ATK = 20, DEF = 10, SP = 10, EXP = 10;
    public bool IsBoss = false,CanEat = false, CanDepressed = false;

}