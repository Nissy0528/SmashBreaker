using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

public class PlayerParameter
{
    private ParameterMenu menu;
    private Vector2 scrollPos = Vector2.zero;
    private Rect windowRect;
    private string[] names = new string[9];//パラメータ名の配列
    private float[] parameters = new float[9];//パラメータの配列
    private const string file = "Assets/Data/Excel/PlayerParameter.xls";//読み込むExcelファイルのパス

    /// <summary>
    /// シーン上に表示
    /// </summary>
    public PlayerParameter()
    {
        ReadExcel();//エクセルのデータを読み込む

        //シーン上にGUIを配置する
        SceneView.onSceneGUIDelegate += PlayerOnSceneGUI;
    }

    /// <summary>
    /// ウィンドウの作成
    /// </summary>
    /// <param name="sceneView"></param>
    private void PlayerOnSceneGUI(SceneView sceneView)
    {
        //ウィンドウの初期位置
        windowRect = new Rect(10, 30, 250, 200);

        Handles.BeginGUI();
        windowRect = GUILayout.Window(1, windowRect, PlayerGUI, "Playerのパラメーター");
        Handles.EndGUI();
    }

    /// <summary>
    /// ウィンドウにGUIを配置
    /// </summary>
    /// <param name="id"></param>
    private void PlayerGUI(int id)
    {
        int width = 130;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(windowRect.height));

        //各パラメータを設定
        for (int i = 0; i < names.Length; i++)
        {
            GUILayout.BeginHorizontal();
            names[i] = EditorGUILayout.TextField(names[i], GUILayout.Width(width));
            //if (i <= 2)
            //{
            //    parameters[i] = EditorGUILayout.IntField((int)parameters[i]);
            //}
            //else
            //{
            parameters[i] = EditorGUILayout.FloatField(parameters[i]);
            //}

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        //パラメータを保存
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("設定"))
        {
            ExcelExport();

            //SP取得量配列の設定
            //SpRateSettings.ChangeMaxHp((int)parameters[1]);
        }
        GUILayout.EndHorizontal();

        //ウィンドウを閉じる
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("閉じる"))
        {
            menu = new ParameterMenu();
            SceneView.onSceneGUIDelegate -= PlayerOnSceneGUI;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Excelを作成
    /// </summary>
    private void ExcelExport()
    {
        IWorkbook book = new HSSFWorkbook();//Excelを作成
        ISheet sheet = book.CreateSheet("PlayerParameter");//シートを作成
        CreateHeaderRow(book, sheet);//ヘッダー行を作成

        AddToExcel(book, sheet);//エクセルに保存

        //エクセルファイルを生成
        using (var fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
        {
            book.Write(fs);
        }

        ExcelImport.Import(file, 0);
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

        ICell[] cells = new ICell[names.Length * 2];//項目の配列

        //各項目にパラメータ設定
        for (int i = 0; i < names.Length; i++)
        {
            IRow row = sheet.CreateRow(rowIdx++);
            cells[i] = row.CreateCell(cellIdx++);
            cells[i].SetCellValue(names[i]);
            cells[i + names.Length] = row.CreateCell(cellIdx++);
            cells[i + names.Length].SetCellValue(parameters[i]);

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

    /// <summary>
    /// エクセルデータ読み込み
    /// </summary>
    private void ReadExcel()
    {
        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            IWorkbook book = new HSSFWorkbook(fs);
            ISheet sheet = book.GetSheetAt(0);
            int firstRow = sheet.FirstRowNum;
            int lastRow = sheet.LastRowNum;

            for (int rowIdx = firstRow + 1; rowIdx <= lastRow; ++rowIdx)
            {
                IRow row = sheet.GetRow(rowIdx);
                if (row == null) continue;

                int cellIdx = 1;
                int num = rowIdx - 2;

                if (num > names.Length - 1) continue;

                ICell nameCell = row.GetCell(cellIdx);
                names[num] = nameCell.StringCellValue;

                cellIdx++;

                ICell valueCell = row.GetCell(cellIdx);
                parameters[num] = (float)valueCell.NumericCellValue;
            }
        }
    }
}
