using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // �� public ������ �ν����Ϳ��� ������ �� �ֽ��ϴ�.
    // ���⿡ �̵��ϰ� ���� ���� �̸��� �Է��ϼ���.
    public string sceneToLoad;

    // �� public �Լ��� ��ư Ŭ���� ���� �̺�Ʈ�� ���� ȣ��� �� �ֽ��ϴ�.
    public void ChangeScene()
    {
        // sceneToLoad�� ������ ������ �̵��մϴ�.
        LoadingSceneController.LoadScene(sceneToLoad);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
