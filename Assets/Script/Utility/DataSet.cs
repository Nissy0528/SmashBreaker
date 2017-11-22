using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSet : MonoBehaviour
{
    public CharaDataSet[] charaData;
    public Player player;
    public Smash smash;
    //public GameObject boss;

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
            smash.SetParam(charaData[0].player_list[i].param, i);
        }
    }
}
