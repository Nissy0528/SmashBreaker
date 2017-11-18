using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSet : MonoBehaviour
{
    public CharaDataSet charaData;
    public Player player;
    public Smash smash;

    // Use this for initialization
    void Awake()
    {
        if (charaData == null) return;

        for (int i = 0; i < charaData.player_list.Count; i++)
        {
            player.SetParam(charaData.player_list[i].param, i);
            smash.SetParam(charaData.player_list[i].param, i);
        }
    }
}
