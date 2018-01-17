using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParameterMenu
{
    private PlayerParameter player_param;//プレイヤー設定ウィンドウ
    private BossParameter boss_param;//ボス設定ウィンドウ
    private List<GameObject> stages = new List<GameObject>();//ステージの配列
    private Object stageFile;//ステージオブジェクトが入ってるファイル

    //シーン上に表示
    public ParameterMenu()
    {
        SceneView.onSceneGUIDelegate += MenuOnSceneGUI;
    }

    //ウィンドウを作成
    private void MenuOnSceneGUI(SceneView sceneView)
    {
        //ウィンドウの初期位置
        Rect windowRect = new Rect(10, 30, 120, 30);

        Handles.BeginGUI();
        windowRect = GUILayout.Window(1, windowRect, GUI, "設定メニュー");
        Handles.EndGUI();
    }

    /// <summary>
    /// ウィンドウにGUIを配置
    /// </summary>
    /// <param name="id"></param>
    private void GUI(int id)
    {
        //ボタンが押されたらプレイヤーの設定を開く
        if (GUILayout.Button("プレイヤーの設定を開く"))
        {
            player_param = new PlayerParameter();
            SceneView.onSceneGUIDelegate -= MenuOnSceneGUI;
        }
        EditorGUILayout.Space();

        //ボタンが押されたらボスの設定を開く
        if (GUILayout.Button("ボスの設定を開く"))
        {
            boss_param = new BossParameter();
            SceneView.onSceneGUIDelegate -= MenuOnSceneGUI;
        }
        EditorGUILayout.Space();

        //ステージファイル読み込み
        GUILayout.BeginHorizontal();
        GUILayout.Label("ステージファイル : ");
        stageFile = EditorGUILayout.ObjectField(stageFile, typeof(object), true);
        GUILayout.EndHorizontal();

        //ボタンが押されたらステージの大きさ変更
        GUILayout.BeginHorizontal();
        for (int i = 0; i < stages.Count; i++)
        {
            if (GUILayout.Button("ステージ" + i))
            {

            }
            EditorGUILayout.Space();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// ステージ読み込み
    /// </summary>
    private void ReadStages()
    {
        if (stageFile == null) return;

        var so = new SerializedObject(stageFile);
        var sp = so.GetIterator();
        //while(sp.Next(true))
        //{
        //    stages.Add(new GameObject(sp.pro))
        //}
    }
}
