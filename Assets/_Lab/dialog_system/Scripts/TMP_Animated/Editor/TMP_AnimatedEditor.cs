using UnityEngine;
using UnityEditor;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(TMP_Animated), true)]
    [CanEditMultipleObjects]
    public class TMP_AnimatedEditor : TMP_BaseEditorPanel
    {
        SerializedProperty speedProp;
        SerializedProperty pauseProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            speedProp = serializedObject.FindProperty("speed");
            pauseProp = serializedObject.FindProperty("pauseTime");

        }
        protected override void OnUndoRedo()
        {
        }
        protected override void DrawExtraSettings()
        {
            EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(speedProp, new GUIContent("     Default Speed"));
            EditorGUILayout.PropertyField(pauseProp, new GUIContent("     Pause time before animation continue"));
        }

        protected override bool IsMixSelectionTypes()
        {
            return false;
        }
    }
}