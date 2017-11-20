using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaDataSet : ScriptableObject
{
    public List<PlayerData> player_list = null;
    public List<BossData> boss_list = null;

    void OnEnable()
    {
        hideFlags = HideFlags.NotEditable;

        if (player_list == null)
        {
            player_list = new List<PlayerData>();
        }
        if (boss_list == null)
        {
            boss_list = new List<BossData>();
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
    /// リストにデータを追加
    /// </summary>
    /// <param name="data"></param>
    public void BossAdd(BossData data)
    {
        if (boss_list == null) return;

        boss_list.Add(data);
    }

    /// <summary>
    /// リストをクリア
    /// </summary>
    public void Clear()
    {
        if (player_list == null || boss_list == null) return;

        player_list.Clear();
        boss_list.Clear();
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
    public string posCsv;//座標の数字文字列
    public List<Vector2> translate_positions = new List<Vector2>();//移動時の座標リスト
}
