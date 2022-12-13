using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    // Singletone pattern //
    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // ... //

    [SerializeField]
    EventReference e_refMusic;
    EventInstance e_instMusic;

    [SerializeField]
    EventReference e_refBoost;
    EventInstance e_instBoost;

    [SerializeField]
    EventReference e_refSlow;
    EventInstance e_instSlow;


    public float globalFadeOutDuration = 4f;
    float globalFadeOutValue = 1f;
    bool isGlobalFadeOutActive = false;


    PARAMETER_ID p_idHardmode;


    void Start()
    {
        // Init music event and fetch hardmode parameter
        e_instMusic = RuntimeManager.CreateInstance(e_refMusic);
        EventDescription e_descHardmode;
        e_instMusic.getDescription(out e_descHardmode);
        PARAMETER_DESCRIPTION p_descrHardmode;
        e_descHardmode.getParameterDescriptionByName("Hardmode", out p_descrHardmode);
    }

    public void ChangeMusicToHardmode()
    {
        e_instMusic.setParameterByID(p_idHardmode, 1);  
    }

    public void StartGlobalFadeOut(float duration)
    {
        isGlobalFadeOutActive = true;
        globalFadeOutDuration = duration;
    }

    public void StartMusic()
    {
        if (!IsPlaying(e_instMusic))
        {
            e_instMusic.start();
        }
    }

    public void StopMusic()
    {
        if (IsPlaying(e_instMusic))
        {
            e_instMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            e_instMusic.release();
        }
    }

    public void PlayBoostSound()
    {
        e_instBoost = RuntimeManager.CreateInstance(e_refBoost);

        if (!IsPlaying(e_instBoost))
        {
            e_instBoost.start();
        }
    }

    public void PlaySlowSound()
    {
        e_instSlow = RuntimeManager.CreateInstance(e_refSlow);

        if (!IsPlaying(e_instSlow))
        {
            e_instSlow.start();
        }
    }

    bool IsPlaying(EventInstance instance)
    {
        PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != PLAYBACK_STATE.STOPPED;
    }


    void Update()
    {
        if (isGlobalFadeOutActive)
        {
            if (globalFadeOutValue > 0)
            {
                globalFadeOutValue -= Time.deltaTime / globalFadeOutDuration;
                RuntimeManager.StudioSystem.setParameterByName("MasterVolume", globalFadeOutValue);
            }
            else
            {
                globalFadeOutValue = 1f;
                isGlobalFadeOutActive = false;
            }
        }
    }
}
