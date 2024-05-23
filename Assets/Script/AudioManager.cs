using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("#BGM")]
    [SerializeField] private AudioClip[] Dt_StageBgmClips;
    Dictionary<int, AudioClip> Dt_StageBgms; // ����!! AudioClip�� ������ ����Ǵ� ���� ����, ������ ���ƾ� �� (buildIndex�� ����)
    [SerializeField] private float bgmVolume;
    AudioSource bgmPlayer;
    
    [Header("#SFX")]
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private float sfxVolume;
    [SerializeField] private int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum StageBgm { stage1, stage2, stage3 };
    public enum Sfx {  } // ȿ������ ������ ���⿡ �߰�

    private static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("bgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        Dt_StageBgms = new Dictionary<int, AudioClip>();
        for (int i = 0; i < Dt_StageBgmClips.Length; i++)
        {
            Dt_StageBgms.Add(i, Dt_StageBgmClips[i]);
        }
        bgmPlayer.clip = Dt_StageBgms[0];

        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];
        for (int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    // ���� �ε� �ɶ� ���� ��������Ʈ ü������ �ɾ���� �Լ����� ����ȴ�.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgmPlayer.Play();
    }

    public static AudioManager Instance
    {
        get { return instance; }
    }

    public bool IsBgmPlaying { get { return bgmPlayer.isPlaying; } }

    public void LoadStageBgmClip(int StageBgmIndex)
    {
        bgmPlayer.clip = Dt_StageBgms[StageBgmIndex];
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void PauseBgm(bool isPause)
    {
        if (isPause)
        {
            bgmPlayer.Pause();
        }
        else
        {
            bgmPlayer.Play();
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < channels; i++)
        {
            int loopIndex = (i + channelIndex) % channels;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
