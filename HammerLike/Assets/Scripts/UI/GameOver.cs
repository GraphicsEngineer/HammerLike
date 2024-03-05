using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ ����� ���� �߰�
using UnityEngine.SceneManagement; // �� ������ ���� �߰�

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel; // �ν����Ϳ��� �Ҵ��� ���� ���� �г�
    public Button restartButton; // �ν����Ϳ��� �Ҵ��� ����� ��ư
    public Button quitButton; // �ν����Ϳ��� �Ҵ��� ���� ��ư

    void Start()
    {
        // ��ư�� ������ �߰�
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        // ���� ���� �г��� ��Ȱ��ȭ
        gameOverPanel.SetActive(false);
    }

    // ���� ���� �� ȣ��� �޼���
    public void OnGameOver()
    {
        // ��� ������Ʈ�� �������� ����
        Time.timeScale = 0;

        // ���� ���� �г� Ȱ��ȭ
        gameOverPanel.SetActive(true);
    }

    void RestartGame()
    {
        // ���� ����� �� ��� ������Ʈ�� ������ �簳
        Time.timeScale = 1;

        // ���� ������ �ε�
        SceneManager.LoadScene("UI");
    }

    void QuitGame()
    {
        // ���� ����
        Application.Quit();

        // ����Ƽ �����Ϳ��� �۵��ϴ� ���
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
