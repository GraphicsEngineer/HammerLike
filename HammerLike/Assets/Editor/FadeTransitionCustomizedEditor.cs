using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FadeTransitionExtended))]
public class FadeTransitionCustomizedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FadeTransitionExtended script = (FadeTransitionExtended)target;

        if (GUILayout.Button("Move Up"))
        {
            MoveElement(script, -1);
        }

        if (GUILayout.Button("Move Down"))
        {
            MoveElement(script, 1);
        }

        base.OnInspectorGUI();
    }

    private void MoveElement(FadeTransitionExtended script, int direction)
    {
        int newSelected = script.selectedElement + direction;

        if (newSelected < 0 || newSelected >= script.imagesSettings.Length)
            return;

        // ���õ� ��ҿ� ��ȯ�� ����� ��ġ�� �ٲߴϴ�.
        var temp = script.imagesSettings[newSelected];
        script.imagesSettings[newSelected] = script.imagesSettings[script.selectedElement];
        script.imagesSettings[script.selectedElement] = temp;

        // ���õ� ����� �ε����� ������Ʈ�մϴ�.
        script.selectedElement = newSelected;
    }

}
