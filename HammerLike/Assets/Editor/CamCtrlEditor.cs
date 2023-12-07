using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CamCtrl))]
public class CamCtrlEditor : Editor
{
    private SerializedObject serializedCamCtrl;
    private SerializedProperty boundaryPosToRayProperty;
    private SerializedProperty zoomOptionProperty;

    private bool showCamsSection = true;
    private bool showResolutionSection = true;
    private bool showFollowSection = true;
    private bool showZoomSection = true;
    private bool showEtcSection = true;

    private void OnEnable()
    {
        serializedCamCtrl = new SerializedObject(target);
        boundaryPosToRayProperty = serializedCamCtrl.FindProperty("boundaryPosToRay");
        zoomOptionProperty = serializedCamCtrl.FindProperty("zoomOption");
    }

    public override void OnInspectorGUI()
    {
        serializedCamCtrl.Update();

        // 'Cams' ����
        showCamsSection = EditorGUILayout.Foldout(showCamsSection, "Cams");
        if (showCamsSection)
        {
            SerializedProperty mainCam = serializedCamCtrl.FindProperty("mainCam");
            SerializedProperty subCam = serializedCamCtrl.FindProperty("subCam");

            if (mainCam != null)
            {
                EditorGUILayout.PropertyField(mainCam);
            }

            if (subCam != null)
            {
                EditorGUILayout.PropertyField(subCam);
            }
        }

        // 'Resolution' ����
        showResolutionSection = EditorGUILayout.Foldout(showResolutionSection, "Resolution");
        if (showResolutionSection)
        {
            SerializedProperty renderTex = serializedCamCtrl.FindProperty("renderTex");
            SerializedProperty resolutionRef = serializedCamCtrl.FindProperty("resolutionRef");
            EditorGUILayout.PropertyField(renderTex);
            EditorGUILayout.PropertyField(resolutionRef, true);
        }

        // 'Follow' ����
        showFollowSection = EditorGUILayout.Foldout(showFollowSection, "Follow");
        if (showFollowSection)
        {
            SerializedProperty followOption = serializedCamCtrl.FindProperty("followOption");
            SerializedProperty followObjTr = serializedCamCtrl.FindProperty("followObjTr");
            SerializedProperty followSpdCurve = serializedCamCtrl.FindProperty("followSpdCurve");
            SerializedProperty followSpd = serializedCamCtrl.FindProperty("followSpd");
            SerializedProperty followDistOffset = serializedCamCtrl.FindProperty("followDistOffset");
            SerializedProperty currentRoom = serializedCamCtrl.FindProperty("currentRoom");

            EditorGUILayout.PropertyField(followOption);
            EditorGUILayout.PropertyField(followObjTr);
            EditorGUILayout.PropertyField(followSpdCurve);
            EditorGUILayout.PropertyField(followSpd);
            EditorGUILayout.PropertyField(followDistOffset);
            EditorGUILayout.PropertyField(currentRoom);
        }

        // 'Zoom' ����
        showZoomSection = EditorGUILayout.Foldout(showZoomSection, "Zoom");
        if (showZoomSection)
        {
            EditorGUILayout.PropertyField(zoomOptionProperty);

            if (zoomOptionProperty.boolValue) // ZoomOption�� true�� ���� PixelPerfectCamera �ɼ� ǥ��
            {
                SerializedProperty zoomCam = serializedCamCtrl.FindProperty("zoomCam");
                SerializedProperty zoomMin = serializedCamCtrl.FindProperty("zoomMin");
                SerializedProperty zoomMax = serializedCamCtrl.FindProperty("zoomMax");
                SerializedProperty zoomSpd = serializedCamCtrl.FindProperty("zoomSpd");
                SerializedProperty zoomSpdCrv = serializedCamCtrl.FindProperty("zoomSpdCrv");

                EditorGUILayout.PropertyField(zoomCam);
                EditorGUILayout.PropertyField(zoomMin);
                EditorGUILayout.PropertyField(zoomMax);
                EditorGUILayout.PropertyField(zoomSpd);
                EditorGUILayout.PropertyField(zoomSpdCrv);
            }
        }

        // 'ETC' ����
        showEtcSection = EditorGUILayout.Foldout(showEtcSection, "ETC");
        if (showEtcSection)
        {
            SerializedProperty worldScaleCrt = serializedCamCtrl.FindProperty("worldScaleCrt");
            SerializedProperty distToGround = serializedCamCtrl.FindProperty("distToGround");

            EditorGUILayout.PropertyField(worldScaleCrt);
            EditorGUILayout.PropertyField(distToGround);

            EditorGUILayout.LabelField("Boundary Positions To Ray", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(boundaryPosToRayProperty, new GUIContent("Boundary Pos To Ray"), true);
        }

        serializedCamCtrl.ApplyModifiedProperties();
    }
}
