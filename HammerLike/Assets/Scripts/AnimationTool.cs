using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class AnimationTool : EditorWindow
{
    private List<AnimationClip> animationClips = new List<AnimationClip>();
    private AnimationClip selectedClip;
    private GameObject selectedObject;

    [MenuItem("Tools/Animation Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AnimationTool));
    }

    void OnGUI()
    {
        GUILayout.Label("Select a GameObject", EditorStyles.boldLabel);

        selectedObject = EditorGUILayout.ObjectField("GameObject", selectedObject, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Load Animations"))
        {
            LoadAnimations();
        }

        if (animationClips.Count > 0)
        {
            foreach (var clip in animationClips)
            {
                if (GUILayout.Button(clip.name))
                {
                    selectedClip = clip;
                    PlayAnimation();
                   
                }
            }
        }
        
        
    }

    private void LoadAnimations()
    {
        animationClips.Clear();
        LoadAnimationsFromFolder("Monster");
        LoadAnimationsFromFolder("Player");
    }

    private void LoadAnimationsFromFolder(string folderName)
    {
        // ���� ��� Ȯ��
        string folderPath = Path.Combine(Application.dataPath, "Resources", folderName);
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("Folder does not exist: " + folderPath);
            return;
        }

        var objects = Resources.LoadAll(folderName, typeof(GameObject));
        foreach (var obj in objects)
        {
            var go = obj as GameObject;
            if (go == null) continue;

            Debug.Log("Found FBX file: " + go.name);
            var animator = go.GetComponent<Animator>();
            if (animator == null) continue;

            var clips = AnimationUtility.GetAnimationClips(go);
            
            foreach (var clip in clips)
            {
                if (!animationClips.Contains(clip))
                {
                    animationClips.Add(clip);
                    Debug.Log("clip name :"+clip.name);
                }
                
            }
        }
        
    }


    private void PlayAnimation()
    {
        if (selectedObject != null && selectedClip != null)
        {
            var animator = selectedObject.GetComponent<Animator>();
            if (animator == null)
            {
                animator = selectedObject.AddComponent<Animator>();
            }

            // AnimatorController ����
            AnimatorController controller = new AnimatorController();
            controller.AddLayer("Default");

            // AnimationClip�� AnimatorController�� �߰�
            var stateMachine = controller.layers[0].stateMachine;
            var state = stateMachine.AddState("PlayState");
            state.motion = selectedClip;

            // RuntimeAnimatorController�� ����
            animator.runtimeAnimatorController = controller;

            // �ִϸ��̼� ���
            animator.Play("PlayState");
        }
    }
}
