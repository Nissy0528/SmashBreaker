using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TranslationPositionSet : MonoBehaviour
{
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        //マウスのクリックがあったら処理
        if (Event.current == null || Event.current.type != EventType.mouseUp)
        {
            return;
        }

        //マウスの位置取得
        Vector3 mousePosition = Event.current.mousePosition;

        //シーン上の座標に変換
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        Debug.Log("座標 : " + mousePosition.x.ToString("F2") + ", " + mousePosition.y.ToString("F2"));
    }
#endif
}
