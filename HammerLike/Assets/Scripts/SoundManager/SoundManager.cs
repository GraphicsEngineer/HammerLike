using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<AudioSource> sfxSources = new List<AudioSource>();
    public AudioClip[] audioClip;
    public TMP_InputField masterVolumeTextInput;
    public TMP_InputField bgmVolumeTextInput;
    public TMP_InputField sfxVolumeTextInput;

    public Slider masterVolumeSlider; 
    public Slider bgmVolumeSlider; 
    public Slider sfxVolumeSlider; 


    private float masterVolume = 1f;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;
    private bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolumeSettings();
    }

    // ���ڿ� �Է��� �޾� ������ ���� ����
    public void SetMasterValue(string value)
    {
        if (float.TryParse(value, out float volume))
        {
            SetMasterVolume(volume / 100f); // 0���� 100 ������ ������ ��ȯ
            masterVolumeSlider.value = volume / 100f; // �����̴� �� ������Ʈ
        }
    }

    // ���ڿ� �Է��� �޾� ������� ���� ����
    public void SetBGMValue(string value)
    {
        if (float.TryParse(value, out float volume))
        {
            SetBGMVolume(volume / 100f); // 0���� 100 ������ ������ ��ȯ
            bgmVolumeSlider.value = volume / 100f; // �����̴� �� ������Ʈ
        }
    }

    // ���ڿ� �Է��� �޾� ȿ���� ���� ����
    public void SetSFXValue(string value)
    {
        if (float.TryParse(value, out float volume))
        {
            SetSFXVolume(volume / 100f); // 0���� 100 ������ ������ ��ȯ
            sfxVolumeSlider.value = volume / 100f; // �����̴� �� ������Ʈ
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource freeSource = sfxSources.Find(source => !source.isPlaying);
        if (freeSource == null)
        {
            freeSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(freeSource);
        }
        freeSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        string textValue = (volume * 100f).ToString("0");
        masterVolumeTextInput.text = textValue;
        UpdateAllVolumes();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        string textValue = (volume * 100f).ToString("0");
        bgmVolumeTextInput.text = textValue;
        UpdateAllVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        string textValue = (volume * 100f).ToString("0");
        sfxVolumeTextInput.text = textValue;
        
        UpdateAllVolumes();
    }

    private void UpdateAllVolumes()
    {
        bgmSource.volume = isMuted ? 0 : bgmVolume * masterVolume;
        sfxSources.ForEach(source => source.volume = isMuted ? 0 : sfxVolume * masterVolume);
    }

    public void ToggleMute(bool mute)
    {
        isMuted = mute;
        UpdateAllVolumes();
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChangeComplete()
    {
        // ���� ������ �������� �ʰ�, ����ڰ� ���� ������ �Ϸ����� �� �׽�Ʈ ���带 ���
        // ���÷�, sfxSources ����Ʈ�� ù ��° AudioSource�� ����Ͽ� �׽�Ʈ ���� ���
        // ���� ��� �ÿ��� ������ �׽�Ʈ ���� Ŭ���� �����ϰų�, Ư�� ���ǿ� �´� ���带 ����� �� �ֽ��ϴ�.
        if (sfxSources.Count > 0 && audioClip.Length > 0)
        {
            sfxSources[0].PlayOneShot(audioClip[0], sfxVolume * masterVolume); // ù ��° ����� Ŭ���� ���÷� ���
        }
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        UpdateAllVolumes();
    }

    public void ResetToDefaultSettings()
    {
        SetMasterVolume(1f);
        SetBGMVolume(1f);
        SetSFXVolume(1f);
        ToggleMute(false);
        SaveVolumeSettings();
    }
}
