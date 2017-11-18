using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ShowMenu : EditorWindow
{
    private static ParameterMenu menu;

    static ShowMenu()
    {
        menu = new ParameterMenu();
    }
}
