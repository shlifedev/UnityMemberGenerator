﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace Hamster.Unity.Utility
{
    [Serializable]
    public class UIScriptGeneratorEditor : UnityEditor.EditorWindow
    {

        public enum AccessPermissionLevel { None, PUBLIC, PRIVATE };
        public enum Attribute { None, SerializeField };
        private static bool writeRegion = false;
        private static string region = "SetRegionName";

        private static AccessPermissionLevel apl;
        private static Attribute attr;
        private static UIScriptGeneratorEditor m_window = null;
        public static AccessPermissionLevel Apl { get => apl; }
         
        [SerializeField] public List<string> reflectionFields = new List<string>();
        public static string AplStr
        {
            get
            {
                if (apl == AccessPermissionLevel.None) return null;
                if (apl == AccessPermissionLevel.PRIVATE) return "private";
                if (apl == AccessPermissionLevel.PUBLIC) return "public";

                return null;
            }
        }
        public static Attribute Attr { get => attr; }
        public static string AttrStr
        {
            get
            {
                if (attr == Attribute.None) return null;
                var ret = "[" + attr.ToString() + "] ";
                return ret;
            }
        }

        public static bool WriteRegion { get => writeRegion; set => writeRegion = value; }
        public static string Region { get => region; set => region = value; }
        public static UIScriptGeneratorEditor Window { get => m_window; set => m_window = value; }

        [MenuItem("UIScriptGeneratorEditor/Code")]
        public static void OpenWindow()
        {
            if (Window == null)
                Window = EditorWindow.GetWindow(typeof(UIScriptGeneratorEditor)) as UIScriptGeneratorEditor;
            Window.titleContent = new GUIContent("github => shlifedev", "Make by.CheeseAllergyHamster ");
            Window.Show();

        }

        private void DrawSourceCodeWindow()
        {
            GUI.contentColor = Color.white;
            UIScriptGenerator.reflectionFields = this.reflectionFields; 
            SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
            SerializedProperty b = serializedObject.FindProperty("reflectionFields"); 
            GUILayout.Label("Generated Code :)");
            UIScriptGenerator.CompleteCode = EditorGUILayout.TextArea(UIScriptGenerator.CompleteCode);
             
            GUILayout.Label("Option");

            Region = EditorGUILayout.TextField(Region);
            WriteRegion = GUILayout.Toggle(WriteRegion, "Use Region");
            apl = (AccessPermissionLevel)EditorGUILayout.EnumPopup("AccessPermissionLevel", Apl);
            attr = (Attribute)EditorGUILayout.EnumPopup("Attribute", Attr);
            GUI.contentColor = Color.red;
            GUILayout.Label("Member List (Please Namespace Include!)");
            GUI.contentColor = Color.green;
            EditorGUILayout.PropertyField(b, new GUIContent("TypeList"), true);
  

            /// Custom Array Draw~~
            if (b.arraySize != reflectionFields.Count)
            {
                reflectionFields.Clear();
                for (int i = 0; i < b.arraySize; i++)
                {
                    reflectionFields.Add(b.GetArrayElementAtIndex(i).stringValue);
                }
            }
            else
            {
                for (int i = 0; i < b.arraySize; i++)
                {
                    reflectionFields[i] = (b.GetArrayElementAtIndex(i).stringValue);
                 
                }
            }
            if (GUILayout.Button("Clear")) { reflectionFields.Clear(); }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("UI.Image")) { string type = "UnityEngine.UI.Image"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); }
            if (GUILayout.Button("UI.Text")) { string type = "UnityEngine.UI.Text"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); }
            if (GUILayout.Button("UI.ScrollRect")) { string type = "UnityEngine.UI.ScrollRect"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); }
            if (GUILayout.Button("UI.Button")) { string type = "UnityEngine.UI.Button"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); }
            GUILayout.EndHorizontal(); 
            GUILayout.BeginHorizontal(); 
            if (GUILayout.Button("Transform")) { string type = "UnityEngine.Transform"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); }
            if (GUILayout.Button("RectTransform")) { string type = "UnityEngine.RectTransform"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); } 
            if (GUILayout.Button("SpriteRenderer")) { string type = "UnityEngine.SpriteRenderer"; if (reflectionFields.Contains(type) == false) reflectionFields.Add(type); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

        
            if (GUILayout.Button("UISet")) { reflectionFields.Clear(); reflectionFields.Add("UnityEngine.UI.Image"); reflectionFields.Add("UnityEngine.UI.Text"); };
            if (GUILayout.Button("UISet2")) { reflectionFields.Clear(); reflectionFields.Add("UnityEngine.UI.Image"); reflectionFields.Add("UnityEngine.UI.Text"); reflectionFields.Add("UnityEngine.UI.Button"); reflectionFields.Add("UnityEngine.UI.GridLayoutGroup"); };
            if (GUILayout.Button("UISet3")) { reflectionFields.Clear(); reflectionFields.Add("UnityEngine.UI.Image"); reflectionFields.Add("UnityEngine.UI.Text"); reflectionFields.Add("UnityEngine.UI.Button"); reflectionFields.Add("UnityEngine.UI.GridLayoutGroup"); reflectionFields.Add("UnityEngine.UI.ScrollRect"); }; 
            if (GUILayout.Button("Ebichu")) { reflectionFields.Clear(); reflectionFields.Add("https://github.com/shlifedev/UnityUGUIMemberGenerator"); };

            GUILayout.EndHorizontal();

        }
        private void OnGUI()
        {
            DrawSourceCodeWindow();
        }
    }
}