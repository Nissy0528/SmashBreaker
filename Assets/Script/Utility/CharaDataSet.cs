using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaDataSet : ScriptableObject
{
    public List<PlayerData> player_list = null;

    void OnEnable()
    {
        hideFlags = HideFlags.NotEditable;

        if (player_list == null)
        {
            player_list = new List<PlayerData>();
        }
    }

    /// <summary>
    /// リストにデータを追加
    /// </summary>
    /// <param name="data"></param>
    public void PlayerAdd(PlayerData data)
    {
        if (player_list == null) return;

        player_list.Add(data);
    }

    /// <summary>
    /// リストをクリア
    /// </summary>
    public void Clear()
    {
        if (player_list == null) return;

        player_list.Clear();
    }
}

[Serializable]
public class PlayerData
{
    /// <summary>
    /// プレイヤーのパラメータ
    /// </summary>
    public float param;
}

[Serializable]
public class BossData
{
    public float param;//ボスのパラメータ
}
