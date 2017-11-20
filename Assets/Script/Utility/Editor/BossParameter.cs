using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

public class BossParameter
{
    private ParameterMenu menu;
    private PositionParameter positionsSet;
    private Vector2 scrollPos = Vector2.zero;
    private Rect windowRect;
    private string[] names = new string[8];//パラメータ名の配列
    private float[] parameters = new float[7];//パラメータの配列
    private List<Vector2> positions;//移動時の座標の配列
    private const string file = "Assets/Data/Excel/BossParameter.xls";//読み込むExcelファイルのパス

    /// <summary>
    /// シーン上に表示
    /// </summary>
    public BossParameter(List<Vector2> positions)
    {
        SetPositions(positions);
        SceneView.onSceneGUIDelegate += BossOnSceneGUI;
    }

    /// <summary>
    /// ウィンドウの作成
    /// </summary>
    /// <param name="sceneView"></param>
    private void BossOnSceneGUI(SceneView sceneView)
    {
        //ウィンドウの初期位置
        windowRect = new Rect(10, 30, 250, 200);

        Handles.BeginGUI();
        windowRect = GUILayout.Window(1, windowRect, BossGUI, "Bossのパラメーター");
        Handles.EndGUI();
    }

    /// <summary>
    /// ウィンドウにGUIを配置
    /// </summary>
    /// <param name="id"></param>
    private void BossGUI(int id)
    {
        int width = 110;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(windowRect.height));

        GUILayout.BeginHorizontal();
        names[0] = EditorGUILayout.TextField(names[0], GUILayout.Width(width));
        parameters[0] = EditorGUILayout.FloatField(parameters[0]);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        names[1] = EditorGUILayout.TextField(names[1], GUILayout.Width(width));
        GUILayout.EndHorizontal();

        for (int i = 0; i < positions.Count; i++)
        {
            GUILayout.BeginHorizontal();
            positions[i] = EditorGUILayout.Vector2Field(i + ":", positions[i]);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("座標の設定を開く"))
        {
            positionsSet = new PositionParameter();
            SceneView.onSceneGUIDelegate -= BossOnSceneGUI;
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //各パラメータを設定
        for (int i = 0; i < names.Length; i++)
        {
            if (i <= 1) continue;

            GUILayout.BeginHorizontal();
            names[i] = EditorGUILayout.TextField(names[i], GUILayout.Width(width));
            if (i == 5)
            {
                parameters[i - 1] = EditorGUILayout.IntField((int)parameters[i - 1]);
            }
            else
            {
                parameters[i - 1] = EditorGUILayout.FloatField(parameters[i - 1]);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        //パラメータを保存
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("設定"))
        {
            ExcelExport();
        }
        GUILayout.EndHorizontal();

        //ウィンドウを閉じる
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("閉じる"))
        {
            menu = new ParameterMenu();
            SceneView.onSceneGUIDelegate -= BossOnSceneGUI;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 座標リスト設定
    /// </summary>
    private void SetPositions(List<Vector2> positions)
    {
        if (positions != null)
        {
            this.positions = positions;
        }
        else
        {
            this.positions = new List<Vector2>();
            this.positions.Add(Vector2.zero);
        }
    }

    /// <summary>
    /// Excelを作成
    /// </summary>
    private void ExcelExport()
    {
        IWorkbook book = new HSSFWorkbook();//Excelを作成
        ISheet sheet = book.CreateSheet("BossParameter");//シートを作成
        CreateHeaderRow(book, sheet);//ヘッダー行を作成

        AddToExcel(book, sheet);//エクセルに保存

        //エクセルファイルを生成
        using (var fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
        {
            book.Write(fs);
        }

        ExcelImport.Import(file, 1);
    }

    /// <summary>
    /// ヘッダー行を作成
    /// </summary>
    /// <param name="sheet"></param>
    private void CreateHeaderRow(IWorkbook book, ISheet sheet)
    {
        IRow header = sheet.CreateRow(1);

        int cellIdx = 1;
        string name = "パラメータ名";
        string paramName = "パラメータ";

        //1列目はパラメータ名
        sheet.SetColumnWidth(cellIdx, 256 * 23);
        ICell paramNameCell = header.CreateCell(cellIdx++);
        paramNameCell.SetCellValue(name);

        //2列目はパラメータ
        sheet.SetColumnWidth(cellIdx, 256 * 15);
        ICell paramCell = header.CreateCell(cellIdx);
        paramCell.SetCellValue(paramName);

        //4方に罫線
        var style = book.CreateCellStyle();
        style.BorderTop = BorderStyle.Thin;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;

        //薄いグリーンで塗りつぶす
        style.FillForegroundColor = HSSFColor.LightGreen.Index;
        style.FillPattern = FillPattern.SolidForeground;

        //テキストはセンターに
        style.Alignment = HorizontalAlignment.Center;

        //太字
        var font = book.CreateFont();
        font.Boldweight = (short)FontBoldWeight.Bold;
        style.SetFont(font);

        //全てのヘッダーにスタイルを適用する
        foreach (var cell in new[] { paramNameCell, paramCell })
        {
            cell.CellStyle = style;
        }
    }

    /// <summary>
    /// Excelにデータを保存
    /// </summary>
    /// <param name="sheet"></param>
    private void AddToExcel(IWorkbook book, ISheet sheet)
    {
        int rowIdx = 2;
        int cellIdx = 1;
        int length = names.Length + positions.Count;

        ICell[] cells = new ICell[length * 2];//項目の配列

        IRow speedRow = sheet.CreateRow(rowIdx++);
        cells[0] = speedRow.CreateCell(cellIdx++);
        cells[length] = speedRow.CreateCell(cellIdx++);
        cells[0].SetCellValue(names[0]);
        cells[length].SetCellValue(parameters[0]);

        cellIdx = 1;

        IRow posNameRow = sheet.CreateRow(rowIdx++);
        cells[1] = posNameRow.CreateCell(cellIdx++);
        cells[1 + length] = posNameRow.CreateCell(cellIdx++);
        cells[1].SetCellValue(names[1]);
        cells[1 + length].SetCellValue("");

        cellIdx = 1;

        //各項目にパラメータ設定
        for (int i = 2; i < length; i++)
        {
            IRow row = sheet.CreateRow(rowIdx++);
            cells[i] = row.CreateCell(cellIdx++);
            cells[i + length] = row.CreateCell(cellIdx++);
            if (i >= 2 && i <= positions.Count + 1)
            {
                cells[i].SetCellValue("Pos : " + i);
                cells[i + length].SetCellValue(positions[i - 2].ToString("F2"));
            }
            else
            {
                cells[i].SetCellValue(names[i - positions.Count]);
                cells[i + length].SetCellValue(parameters[i - positions.Count - 1]);
            }

            cellIdx = 1;
        }

        //4方に罫線
        var style = book.CreateCellStyle();
        style.BorderTop = BorderStyle.Thin;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;

        //全てのヘッダーにスタイルを適用する
        foreach (var cell in cells)
        {
            cell.CellStyle = style;
        }
    }
}
