using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomTileMapping))]
public class RandomTileMappingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomTileMapping script = (RandomTileMapping)target;

        // �⺻ ��ũ��Ʈ �������� ǥ��
        DrawDefaultInspector();

        // ���� ������Ʈ�� �������� ����� ���͸���鸸 ǥ��
        Renderer renderer = script.gameObject.GetComponent<Renderer>();
        if (renderer)
        {
            int selectedIndex = -1;
            string[] options = new string[renderer.sharedMaterials.Length];
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                options[i] = renderer.sharedMaterials[i].name;
                if (renderer.sharedMaterials[i] == script.targetMaterial)
                {
                    selectedIndex = i;
                }
            }
            selectedIndex = EditorGUILayout.Popup("Target Material", selectedIndex, options);
            if (selectedIndex >= 0 && selectedIndex < renderer.sharedMaterials.Length)
            {
                script.targetMaterial = renderer.sharedMaterials[selectedIndex];
            }
        }
    }
}
#endif
