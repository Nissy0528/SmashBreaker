using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TranslationPositionSet : MonoBehaviour
{
    public GameObject marker;

    private List<Vector2> positions = new List<Vector2>();
    private GameObject markerObj;

#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        //マウスのクリックがあったら処理
        if (Event.current == null || Event.current.type != EventType.mouseUp)
        {
            return;
        }

        //マウスの位置取得
        Vector2 mousePosition = Event.current.mousePosition;

        //シーン上の座標に変換
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        positions.Add(mousePosition);
        CreateMarker(mousePosition);
    }
#endif

    /// <summary>
    /// マーカー設置
    /// </summary>
    /// <param name="position"></param>
    private void CreateMarker(Vector2 position)
    {
        markerObj = Instantiate(marker);
        markerObj.transform.position = position;
        markerObj.name = marker.name + positions.Count;
        markerObj.transform.parent = gameObject.transform;
    }

    /// <summary>
    /// 座標リスト取得
    /// </summary>
    public List<Vector2> GetPositions
    {
        get { return positions; }
    }
}
