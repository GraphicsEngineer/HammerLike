using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RandomObjectSpawner))]
public class RandomObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // �⺻ �ν����� UI�� �׸��ϴ�.

        RandomObjectSpawner script = (RandomObjectSpawner)target;
        if (GUILayout.Button("Spawn Random Object"))
        {
            script.ReplaceObject(); // ��ư Ŭ�� �� �Լ� ȣ��
        }
    }
}
