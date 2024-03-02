using System;
using System.Collections;
using UnityEngine;

public class Capture : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(Rendering());
        }
    }

    IEnumerator Rendering()
    {
        yield return new WaitForEndOfFrame();

        // ���� �ð��� "�����_�ú���" �������� ��ȯ�Ͽ� ���� �̸��� ���
        string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = "ScreenShot_" + dateTime + ".png"; // ��: ScreenShot_20240303_123456.png

        // ������� ����ȭ�鿡 �����ϱ� ���� ��� ����
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string folderPath = System.IO.Path.Combine(desktopPath, "ScreenShot");

        // ScreenShot ������ �������� ������ ����
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        string path = System.IO.Path.Combine(folderPath, fileName);

        byte[] imgBytes;
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();
        imgBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, imgBytes);
    }
}
