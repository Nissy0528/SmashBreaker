using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParameterMenu
{
    private PlayerParameter player_param;
    private BossParameter boss_param;

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
        if (GUILayout.Button("プレイヤーの設定を開く"))
        {
            player_param = new PlayerParameter();
            SceneView.onSceneGUIDelegate -= MenuOnSceneGUI;
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("ボスの設定を開く"))
        {
            boss_param = new BossParameter(null);
            SceneView.onSceneGUIDelegate -= MenuOnSceneGUI;
        }
        EditorGUILayout.Space();
    }
}
