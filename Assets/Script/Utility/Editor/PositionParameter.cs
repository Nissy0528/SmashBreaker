using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PositionParameter
{
    private BossParameter boss_param;
    private GameObject positionObj;

    /// <summary>
    /// シーン上に表示
    /// </summary>
    public PositionParameter()
    {
        SceneView.onSceneGUIDelegate += PositionOnSceneGUI;
        positionObj = GameObject.Instantiate(Resources.Load("Prefab/System/PositionSet")) as GameObject;
    }

    /// <summary>
    /// ウィンドウを作成
    /// </summary>
    /// <param name="sceneView"></param>
    private void PositionOnSceneGUI(SceneView sceneView)
    {
        //ウィンドウの初期位置
        Rect windowRect = new Rect(10, 30, 120, 30);

        Handles.BeginGUI();
        windowRect = GUILayout.Window(1, windowRect, PositionGUI, "移動先の座標設定");
        Handles.EndGUI();
    }

    /// <summary>
    /// ウィンドウにGUIを配置
    /// </summary>
    /// <param name="id"></param>
    private void PositionGUI(int id)
    {
        //ウィンドウを閉じる
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("設定"))
        {
            boss_param = new BossParameter();
            GameObject.DestroyImmediate(positionObj);
            SceneView.onSceneGUIDelegate -= PositionOnSceneGUI;
        }
        EditorGUILayout.EndVertical();
    }
}
