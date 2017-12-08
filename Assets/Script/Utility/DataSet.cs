using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSet : MonoBehaviour
{
    public CharaDataSet[] charaData;
    public GameObject boss;
    public Player player;
    //public Smash smash;

    // Use this for initialization
    void Awake()
    {
        if (charaData == null) return;
        PlayerSet();
    }

    /// <summary>
    /// プレイヤーパラメータ設定
    /// </summary>
    private void PlayerSet()
    {
        for (int i = 0; i < charaData[0].player_list.Count; i++)
        {
            player.SetParam(charaData[0].player_list[i].param, i);
            //smash.SetParam(charaData[0].player_list[i].param, i);
        }
    }

    /// <summary>
    /// ボスパラメータ設定
    /// </summary>
    private void BossSet()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        Boss boss_class = boss.GetComponent<Boss>();
        RazerShooter razer = boss.GetComponent<RazerShooter>();
        Rotation rotation = boss.GetComponent<Rotation>();


        for (int i = 0; i < charaData[1].boss_list.Count; i++)
        {

        }
    }
}
