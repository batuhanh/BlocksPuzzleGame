using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.EditorTools;
using Utils;

[CustomEditor(typeof(LevelCreatorObject))]

public class LevelCreatorEditor : Editor
{
    private LevelCreatorObject myTarget;
    private bool isExec = false;
    private int blockCount = 5;
    private GridSize gridSizeType = GridSize.FourXFour;
    private Color[] blockColors = new Color[5];
    private int windowHeight;
    private bool triangleToggle = false;
    void OnSceneGUI()
    {
        if (!Application.isPlaying)
        {
            Handles.BeginGUI();
            {
                GUILayout.BeginArea(new Rect(10, 10, 400, 100), new GUIStyle("window"));
                {
                    EditorGUILayout.HelpBox("You must enter the playmode to use this editor", MessageType.Warning);
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }
        else
        {
            myTarget = (LevelCreatorObject)target;
            if (!isExec)
            {
                isExec = true;
                windowHeight = 300 + (blockCount * 20);
                for (int i = 0; i < blockColors.Length; i++)
                {
                    blockColors[i] = new Color(255, 255, 255, 255);
                }
            }

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Handles.BeginGUI();
            {
                GUILayout.BeginArea(new Rect(10, 10, 400, windowHeight), new GUIStyle("window"));
                {
                    GUIStyle guiStyle = new GUIStyle("BoldLabel");
                    guiStyle.fontSize = 15;

                    GUI.Label(new Rect(150, -50, 200, 150), "Level Creator", guiStyle);
                    GUILayout.Space(35);

                    GUILayout.Label("Current Level Difficulty is : " + CalculateDifficulty(), GUILayout.Width(300), GUILayout.Height(30));

                    GUILayout.Space(20);

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Grid Size: ", GUILayout.Width(150), GUILayout.Height(15));
                    gridSizeType = (GridSize)EditorGUILayout.EnumPopup(gridSizeType, GUILayout.Width(150));
                    GUILayout.Space(35);
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(20);

                    triangleToggle = GUILayout.Toggle(triangleToggle, "Can be Triangular?");

                    GUILayout.Space(20);

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Block Count: ", GUILayout.Width(150), GUILayout.Height(15));
                    blockCount = EditorGUILayout.IntSlider(blockCount, 5, 12);
                    if (GUILayout.Button("UPDATE", GUILayout.Width(75)))
                    {
                        blockColors = new Color[blockCount];
                        windowHeight = 300 + (blockCount * 20);
                        for (int i = 0; i < blockColors.Length; i++)
                        {
                            blockColors[i] = new Color(255, 255, 255, 255);
                        }
                    }
                    GUILayout.Space(35);
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(20);

                    for (int i = 0; i < blockColors.Length; i++)
                    {
                        blockColors[i] = EditorGUILayout.ColorField("Block " + (i + 1).ToString() + " Color: ", blockColors[i], GUILayout.Width(300));

                    }

                    GUILayout.Space(20);
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Create New Level"))
                    {
                        myTarget.OnCreateLevelButtonClicked(blockCount, (int)gridSizeType, CalculateDifficulty(), triangleToggle, blockColors);
                    }
                    if (GUILayout.Button("Save Level Data"))
                    {
                        myTarget.OnSaveLevelDataButtonClicked();
                    }
                    EditorGUILayout.EndHorizontal();

                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }

    }

    private string CalculateDifficulty()
    {
        int diffScore = 0;
        if (gridSizeType == GridSize.FourXFour)
            diffScore += 3;
        else if (gridSizeType == GridSize.FiveXFive)
            diffScore += 6;
        else if (gridSizeType == GridSize.SixXSix)
            diffScore += 9;
        diffScore += blockCount;

        string diffStr = "";
        if (diffScore < 12)
            diffStr = "EASY";
        else if (diffScore < 16)
            diffStr = "MEDIUM";
        else
            diffStr = "HARD";

        return diffStr;
    }
}
public enum GridSize
{
    FourXFour = 4,
    FiveXFive = 5,
    SixXSix = 6
}