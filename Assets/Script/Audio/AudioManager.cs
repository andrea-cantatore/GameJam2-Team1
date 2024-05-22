using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioMixer Mixer;

    public const string MASTER_KEY = "MasterVolume";
    public const string MUSIC_KEY = "MusicVolume";
    public const string SFX_KEY = "SFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolumes();
    }

    void LoadVolumes() // Volumes saved in VolumeSettings.cs
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 0.5f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

        Mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20); ;
        Mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) *20);
        Mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) *20);
    }

    public void PlaySFX( AudioClip clip, Transform playPosition )
    { 
        AudioSource audioSource = ObjectPooler.SharedInstance.GetPooledObject().GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.gameObject.transform.position = playPosition.position;
        audioSource.gameObject.SetActive(true);
        audioSource.Play();
        StartCoroutine(DeactivationTimer(audioSource.gameObject, audioSource.clip.length));
    }

 
    IEnumerator DeactivationTimer(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}
