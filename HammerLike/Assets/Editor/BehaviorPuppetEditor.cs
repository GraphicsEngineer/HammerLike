using UnityEngine;
using UnityEditor;
using RootMotion.Dynamics;

[CustomEditor(typeof(BehaviourPuppet))]
public class BehaviourPuppetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BehaviourPuppet script = (BehaviourPuppet)target;

        // �⺻ Inspector �ɼ� �����ֱ�
        DrawDefaultInspector();

        EditorGUILayout.Space();

        // ���� ����
        script.masterProps.normalMode = (BehaviourPuppet.NormalMode)EditorGUILayout.EnumPopup("Normal Mode", script.masterProps.normalMode);
        script.masterProps.mappingBlendSpeed = EditorGUILayout.FloatField("Mapping Blend Speed", script.masterProps.mappingBlendSpeed);
        script.masterProps.activateOnStaticCollisions = EditorGUILayout.Toggle("Activate On Static Collisions", script.masterProps.activateOnStaticCollisions);
        script.masterProps.activateOnImpulse = EditorGUILayout.FloatField("Activate On Impulse", script.masterProps.activateOnImpulse);

        EditorGUILayout.Space();

        // Muscle Props ����
        SerializedProperty muscleProps = serializedObject.FindProperty("defaults");
        EditorGUILayout.PropertyField(muscleProps, true);

        EditorGUILayout.Space();

        // Muscle Props Group ����
        SerializedProperty musclePropsGroups = serializedObject.FindProperty("groupOverrides");
        EditorGUILayout.PropertyField(musclePropsGroups, true);

        EditorGUILayout.Space();

        // ��Ÿ ����
        script.groundLayers = EditorGUILayout.LayerField("Ground Layers", script.groundLayers);
        script.collisionLayers = EditorGUILayout.LayerField("Collision Layers", script.collisionLayers);
        script.collisionThreshold = EditorGUILayout.FloatField("Collision Threshold", script.collisionThreshold);

        EditorGUILayout.Space();

        // �̺�Ʈ
        SerializedProperty onGetUpProne = serializedObject.FindProperty("onGetUpProne");
        EditorGUILayout.PropertyField(onGetUpProne);

        SerializedProperty onGetUpSupine = serializedObject.FindProperty("onGetUpSupine");
        EditorGUILayout.PropertyField(onGetUpSupine);

        // ������� ����
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
