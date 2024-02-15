using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    public GameObject save1FileExistsUI; // ���̺� ������ �����ϴ� ��� Ȱ��ȭ�� UI ������Ʈ
    public GameObject noSave1FileUI; // ���̺� ������ �������� �ʴ� ��� Ȱ��ȭ�� UI ������Ʈ
    public TextMeshProUGUI save1Text;


    public GameObject save2FileExistsUI; // ���̺� ������ �����ϴ� ��� Ȱ��ȭ�� UI ������Ʈ
    public GameObject noSave2FileUI; // ���̺� ������ �������� �ʴ� ��� Ȱ��ȭ�� UI ������Ʈ
    public TextMeshProUGUI save2Text;


    public GameObject save3FileExistsUI; // ���̺� ������ �����ϴ� ��� Ȱ��ȭ�� UI ������Ʈ
    public GameObject noSave3FileUI; // ���̺� ������ �������� �ʴ� ��� Ȱ��ȭ�� UI ������Ʈ
    public TextMeshProUGUI save3Text;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckSaveFiles();
    }

    public void SavePlayerData(PlayerStat stat, int saveFileIndex)
    {
        DateTime now = DateTime.Now;
        string timestamp = now.ToString("yyyyMMddHHmm");
        string fileName = "save" + saveFileIndex + "_" + timestamp + ".es3";
        string path = "E:/HammerLike_Git/HammerLike/HammerLike/" + fileName;

        ES3.Save("playerStat", stat, path);
        Debug.Log(fileName + "�� ���������� �����");
        CheckSaveFiles();
    }
    /*public void OverwriteSaveData(PlayerStat stat, int saveFileIndex)
    {
        string directoryPath = "D:/HammerLike_Git/HammerLike/HammerLike/";
        string searchPattern = "save" + saveFileIndex + "_*.es3";
        string[] files = Directory.GetFiles(directoryPath, searchPattern);

        // ���� ������ ������ �����մϴ�.
        foreach (string file in files)
        {
            File.Delete(file);
        }

        // ���ο� ���� �̸��� �����ϰ� �����մϴ�.
        DateTime now = DateTime.Now;
        string timestamp = now.ToString("yyyyMMddHHmm");
        string newFileName = "save" + saveFileIndex + "_" + timestamp + ".es3";
        string newPath = directoryPath + newFileName;

        ES3.Save("playerStat", stat, newPath);
        Debug.Log(newFileName + "���� �����Ͱ� ����� ��");

        // ���̺� ���� ���¸� �ٽ� üũ�մϴ�.
        CheckSaveFiles();
    }*/

    public void DeleteSaveFile(int saveFileIndex)
    {
        string directoryPath = "E:/HammerLike_Git/HammerLike/HammerLike/";
        string searchPattern = "save" + saveFileIndex + "_*.es3";
        string[] files = Directory.GetFiles(directoryPath, searchPattern);

        foreach (string file in files)
        {
            File.Delete(file);
            Debug.Log(file + " ������ ������");
        }

        // ���̺� ���� ���¸� �ٽ� üũ�մϴ�.
        CheckSaveFiles();
    }
    private void CheckSaveFiles()
    {
        string directoryPath = "E:/HammerLike_Git/HammerLike/HammerLike/";

        // �� ���̺� ���Ͽ� ���� �˻��ϰ� �ش��ϴ� �Է� �ʵ忡 ������ ������Ʈ
        CheckSaveFile(1, directoryPath, save1FileExistsUI, noSave1FileUI, save1Text);
        CheckSaveFile(2, directoryPath, save2FileExistsUI, noSave2FileUI, save2Text);
        CheckSaveFile(3, directoryPath, save3FileExistsUI, noSave3FileUI, save3Text);
    }

    private void CheckSaveFile(int saveFileIndex, string directoryPath, GameObject fileExistsUI, GameObject noFileUI, TextMeshProUGUI textComponent)
    {
        string searchPattern = "save" + saveFileIndex + "_*.es3";
        string[] files = System.IO.Directory.GetFiles(directoryPath, searchPattern);

        if (files.Length > 0)
        {
            // ������ �����ϸ�, ��¥�� �ð��� �����մϴ�.
            string fileName = System.IO.Path.GetFileNameWithoutExtension(files[0]);
            string dateTimePart = fileName.Split('_')[1];

            // ���ڿ��� DateTime ��ü�� �Ľ��մϴ�.
            if (DateTime.TryParseExact(dateTimePart, "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                // ���ϴ� �������� ���ڿ��� ��ȯ�մϴ�.
                string formattedDate = dateTime.ToString("yyyy/MM/dd HH:mm");
                textComponent.text = formattedDate; // �ؽ�Ʈ ������Ʈ�� ��¥�� �ð��� �����մϴ�.
            }

            fileExistsUI.SetActive(true);
            noFileUI.SetActive(false);
        }
        else
        {
            fileExistsUI.SetActive(false);
            noFileUI.SetActive(true);
            textComponent.text = ""; // �ؽ�Ʈ ������Ʈ�� ���ϴ�.
        }
    }
}
