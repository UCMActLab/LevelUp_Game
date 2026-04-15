using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "ScriptableObjects/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public Test test;

    [Range(1, 10)]
    public int numArticles;
    public int numTrueArticles;

    public bool articleIsSharedWithGroups;

    public List<LocalizedString> avatarMessagesOnStart = new List<LocalizedString>();

    [Range(1, 3)]
    public int numGroupsToShareWith = 1;

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelInfo))]
    public class LevelInfoEditor : Editor
    {
        private Dictionary<string, SerializedProperty> _properties;

        private void OnEnable()
        {
            string[] propertyNames = {
                    "test", "numArticles", "numTrueArticles", "articleIsSharedWithGroups", "numGroupsToShareWith", "avatarMessagesOnStart"
                };

            _properties = new Dictionary<string, SerializedProperty>();
            for (int i = 0; i < propertyNames.Length; i++)
            {
                _properties.Add(propertyNames[i], serializedObject.FindProperty(propertyNames[i]));
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("On Level Start");
            DrawProperty(_properties["avatarMessagesOnStart"], "Avatar Messages on Start");

            GUILayout.Label("Test Settings");
            DrawProperty(_properties["test"], "Test");

            GUILayout.Label("Article Settings");
            DrawProperty(_properties["numArticles"], "Nº Articles");
            DrawProperty(_properties["numTrueArticles"], "Nº True Articles");

            GUILayout.Label("Sharing Settings");
            DrawProperty(_properties["articleIsSharedWithGroups"], "Article is Shared With Groups");

            if (_properties["articleIsSharedWithGroups"].boolValue)
            {
                DrawProperty(_properties["numGroupsToShareWith"], "Number of Groups to Share With");
            }

            //DrawEnumProperty(_properties[4], "Structure", typeof(StructureMode));
            //DrawStructureProperties((StructureMode)_properties[4].enumValueIndex);

            //DrawEnumProperty(_properties[5], "Mode", typeof(GenerateMode));
            //DrawModeProperties((GenerateMode)_properties[5].enumValueIndex);

            EditorGUI.EndChangeCheck();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawProperty(SerializedProperty prop, string label)
        {
            EditorGUILayout.PropertyField(prop, new GUIContent(label));
        }
    }
#endif
}