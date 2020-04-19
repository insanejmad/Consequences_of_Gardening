using UnityEngine;
using UnityEditor;

namespace TMPro.EditorUtilities
{
    [CustomEditor(typeof(TMP_Animated), true)]
    [CanEditMultipleObjects]
    public class TMP_AnimatedEditor : TMP_BaseEditorPanel
    {
        SerializedProperty SpeedProp;
        SerializedProperty PauseProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            SpeedProp = serializedObject.FindProperty("Speed");
            PauseProp = serializedObject.FindProperty("PauseTime");

        }
        protected override void OnUndoRedo()
        {
        }
        protected override void DrawExtraSettings()
        {
            EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(SpeedProp, new GUIContent("     Default Speed"));
            EditorGUILayout.PropertyField(PauseProp, new GUIContent("     Pause time"));
        }

        protected override bool IsMixSelectionTypes()
        {
            return false;
        }
    }
}