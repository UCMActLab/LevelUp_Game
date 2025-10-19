using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Question))]
[CanEditMultipleObjects]
public class EditorSOQuestion : Editor
{
	SerializedProperty questionType;

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		questionType = serializedObject.FindProperty("questionType");

		EditorGUILayout.PropertyField(questionType);

		switch ((QuestionType)questionType.enumValueIndex)
		{
			case QuestionType.NONE:
				EditorGUILayout.HelpBox("Please select a valid question type.", MessageType.Warning);
				break;
			case QuestionType.LIKERT:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("questionText"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("leftLablel"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rightLabel"));
				
				SerializedProperty lVal = serializedObject.FindProperty("leftValue");
				EditorGUILayout.PropertyField(lVal);

				SerializedProperty rVal = serializedObject.FindProperty("rightValue");
				EditorGUILayout.PropertyField(rVal);

				SerializedProperty val = serializedObject.FindProperty("defaultValue");
				val.intValue = Mathf.Clamp(val.intValue, lVal.intValue, rVal.intValue);
				EditorGUILayout.PropertyField(val);
				break;
			case QuestionType.MULTIPLE_CHOICE:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("questionText"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("answerOptions"), true);
				break;
			case QuestionType.OPEN_ENDED:
				EditorGUILayout.PropertyField(serializedObject.FindProperty("questionText"));
				break;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
