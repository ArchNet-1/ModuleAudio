// ArchNet · Audio Module
// Audio settings for an options menu: a volume slider and a mute toggle for the music and
// for the game sounds. Changes apply at once and are saved between sessions.
// Drop the ModuleAudio prefab (Runtime/Resources/Prefabs) into your menu canvas.
// Docs: https://archnet-plateform.fr/#ModuleAudio

using UnityEngine;
using UnityEngine.UI;

namespace ArchNet.Module
{
    [AddComponentMenu("ArchNet/Audio Module")]
    public class AudioModule : MonoBehaviour
    {
        const string MusicVolumeKey = "ArchNet.Audio.MusicVolume";
        const string MusicMutedKey = "ArchNet.Audio.MusicMuted";
        const string SoundVolumeKey = "ArchNet.Audio.SoundVolume";
        const string SoundMutedKey = "ArchNet.Audio.SoundMuted";

        [Tooltip("Object holding the two AudioSources (music first, sounds second). Empty = the scene's \"AudioSystem\", or the package's prefab.")]
        [SerializeField] GameObject _audioSystem;

        [Header("Audio Sources")]
        [SerializeField] AudioSource _musicAudioSource;
        [SerializeField] AudioSource _gameSoundAudioSource;

        [Header("UI (optional)")]
        [SerializeField] Slider _musicSlider;
        [SerializeField] Slider _gameSoundSlider;
        [SerializeField] Toggle _musicToggle;
        [SerializeField] Toggle _gameSoundToggle;

        public AudioSource MusicSource => _musicAudioSource;
        public AudioSource SoundSource => _gameSoundAudioSource;

        void Start()
        {
            FindSources();
            Bind(_musicAudioSource, _musicSlider, _musicToggle, MusicVolumeKey, MusicMutedKey);
            Bind(_gameSoundAudioSource, _gameSoundSlider, _gameSoundToggle, SoundVolumeKey, SoundMutedKey);
        }

        void OnDisable() => PlayerPrefs.Save();

        public void SetMusicVolume(float volume) => SetVolume(_musicAudioSource, _musicSlider, MusicVolumeKey, volume);
        public void SetSoundVolume(float volume) => SetVolume(_gameSoundAudioSource, _gameSoundSlider, SoundVolumeKey, volume);
        public void SetMusicMuted(bool muted) => SetMuted(_musicAudioSource, _musicToggle, MusicMutedKey, muted);
        public void SetSoundMuted(bool muted) => SetMuted(_gameSoundAudioSource, _gameSoundToggle, SoundMutedKey, muted);

        void FindSources()
        {
            if (_audioSystem == null && (_musicAudioSource == null || _gameSoundAudioSource == null))
            {
                _audioSystem = GameObject.Find("AudioSystem");
                if (_audioSystem == null) _audioSystem = Instantiate(Resources.Load<GameObject>("Prefabs/AudioSystem"), transform);
            }
            if (_musicAudioSource == null) _musicAudioSource = _audioSystem.transform.GetChild(0).GetComponent<AudioSource>();
            if (_gameSoundAudioSource == null) _gameSoundAudioSource = _audioSystem.transform.GetChild(1).GetComponent<AudioSource>();
        }

        // Applies the saved values to the source, shows them on the UI, and listens to the UI.
        void Bind(AudioSource source, Slider slider, Toggle toggle, string volumeKey, string mutedKey)
        {
            if (source == null)
            {
                Debug.LogError("[ArchNet] Audio Module: an AudioSource is missing.", this);
                return;
            }
            source.volume = PlayerPrefs.GetFloat(volumeKey, source.volume);
            source.mute = PlayerPrefs.GetInt(mutedKey, source.mute ? 1 : 0) == 1;

            if (slider != null)
            {
                slider.SetValueWithoutNotify(source.volume);
                slider.onValueChanged.AddListener(volume => SetVolume(source, null, volumeKey, volume));
            }
            if (toggle != null)
            {
                toggle.SetIsOnWithoutNotify(!source.mute); // toggle on = sound on
                toggle.onValueChanged.AddListener(on => SetMuted(source, null, mutedKey, !on));
            }
        }

        static void SetVolume(AudioSource source, Slider slider, string key, float volume)
        {
            volume = Mathf.Clamp01(volume);
            source.volume = volume;
            if (slider != null) slider.SetValueWithoutNotify(volume);
            PlayerPrefs.SetFloat(key, volume);
        }

        static void SetMuted(AudioSource source, Toggle toggle, string key, bool muted)
        {
            source.mute = muted;
            if (toggle != null) toggle.SetIsOnWithoutNotify(!muted);
            PlayerPrefs.SetInt(key, muted ? 1 : 0);
        }
    }
}
