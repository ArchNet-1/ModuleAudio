using UnityEngine;
using UnityEngine.UI;

namespace ArchNet.Module.Audio
{
    /// <summary>
    /// [MODULE] - [ARCH NET] - [AUDIO]
    /// author : LOUIS PAKEL
    /// </summary>
    public class Audio : MonoBehaviour
    {
        #region Serialized Fields

        // Audio System
        [SerializeField, Tooltip("Audio System")]
        private GameObject _audioSystem = null;

        [Header("Main Audio Source")]
        // Main Music Audio Source
        [SerializeField, Tooltip("Music Audio Source")]
        private AudioSource _musicAudioSource = null;

        // Main Game Sound Audio Source
        [SerializeField, Tooltip("Game Sound Audio Source")]
        private AudioSource _gameSoundAudioSource = null;

        [Header("Sliders")]
        // Music Slider
        [SerializeField, Tooltip("Music Slider")]
        private Slider _musicSlider = null;

        // Game Sound Slider"
        [SerializeField, Tooltip("Game Sound Slider")]
        private Slider _gameSoundSlider = null;


        [Header("Toggles")]
        // Music Toggle
        [SerializeField, Tooltip("Music Toggle")]
        private Toggle _musicToggle = null;

        // Game Sound Toggle"
        [SerializeField, Tooltip("Game Sound Toggle")]
        private Toggle _gameSoundToggle = null;

        #endregion

        #region Private Fields

        // Music value 
        private float _musicValue = 0;

        // Music is Muted 
        private bool _musicIsMute = false;

        // Game Sound Value
        private float _gameSoundValue = 0;
        
        // Game Sound is Muted
        private bool _gameSoundIsMute = false;

        // Lock the update methode
        private bool _lock = true;

        #endregion

        #region Init Methods

        /// <summary>
        /// Description : Initiate all sound values
        /// </summary>
        private void InitSoundsValues()
        {
            // Set Music value and Music is muted
            _musicValue = _musicAudioSource.volume;
            _musicIsMute = _musicAudioSource.mute;

            // Set Game Sound value and Music is muted
            _gameSoundValue = _gameSoundAudioSource.volume;
            _gameSoundIsMute = _gameSoundAudioSource.mute;
        }

        /// <summary>
        /// Description : Initiate all audio sources
        /// </summary>
        private void InitAudioSources()
        {
            // Force Set Music Audio Source
            if (null == _musicAudioSource)
            {
                // Set Music Audio Source
                _musicAudioSource = _audioSystem.transform.GetChild(0).GetComponent<AudioSource>();
            }

            // Force Set Game Sound Audio Source
            if (null == _gameSoundAudioSource)
            {
                // Set Music Audio Source
                _gameSoundAudioSource = _audioSystem.transform.GetChild(1).GetComponent<AudioSource>();
            }
        }

        /// <summary>
        /// Description : Initiate Audio System
        /// </summary>
        private void InitAudioSystem()
        {
            // Set Audio System
            if(null == _audioSystem)
            {
                GameObject _newAudioSystem = GameObject.Find("AudioSystem");

                if (null  == _newAudioSystem)
                {
                    _newAudioSystem = Instantiate(Resources.Load(Constants.AudioSystem), transform) as GameObject;
                }
                
                _audioSystem = _newAudioSystem;
            }
        }
        /// <summary>
        /// Description : Initiate all sliders
        /// </summary>
        private void InitSliders()
        {
            // Set Music Slider
            if (null != _musicSlider)
            {
                _musicSlider.value = _musicValue;
            }

            // Set Game Sound Slider
            if (null != _gameSoundSlider)
            {
                _gameSoundSlider.value = _musicValue;
            }
        }

        /// <summary>
        /// Description : Initiate all toggles
        /// </summary>
        private void InitToggles()
        {
            // Set Music Slider
            if(null != _musicSlider)
            {
                _musicToggle.isOn = !_musicIsMute;
            }

            // Set Game Sound Slider
            if (null != _gameSoundSlider)
            {
                _gameSoundToggle.isOn = !_musicIsMute;
            }
        }
        #endregion

        /// <summary>
        /// Description : Check if the module is available
        /// </summary>
        private void ModuleAvailable()
        {
            if(null == _musicSlider
                || null == _musicToggle
                || null == _gameSoundSlider
                || null == _gameSoundToggle)
            {
                Debug.LogError(Constants.ERROR_411);
            }
            else
            {
                _lock = false;
            }
        }

        public void Start()
        {
            // Check if the module is available
            ModuleAvailable();
            
            // Init Audio System
            InitAudioSystem();

            // Init audio sources
            InitAudioSources();

            // Init musics values
            InitSoundsValues();

            // Init sliders
            InitSliders();

            // Init toggles
            InitToggles();
        }


        #region Update Methods

        /// <summary>
        /// Description : Update Game sound value from slider and toggle
        /// </summary>
        private void UpdateGameSound()
        {
            // Update Game sound Slider and Game sound Value
            if (_gameSoundSlider.value != _gameSoundValue)
            {
                // Game Sound Value
                _gameSoundValue = _gameSoundSlider.value;

                // Game Sound Slider
                _gameSoundAudioSource.volume = _gameSoundValue;
            }

            // Update Game sound Toggle and Game sound Value
            if (_gameSoundToggle.isOn == _gameSoundIsMute)
            {
                // Game Sound Value
                _gameSoundIsMute = !_gameSoundToggle.isOn;

                // Game Sound Toggle
                _gameSoundAudioSource.mute = _gameSoundIsMute;
            }
        }

        /// <summary>
        /// Description : Update Music value from slider and toggle
        /// </summary>
        private void UpdateMusic()
        {
            // Update Music Slider and Music Value
            if (_musicSlider.value != _musicValue)
            {
                // Music Value
                _musicValue = _musicSlider.value;

                // Music Toggle
                _musicAudioSource.volume = _musicValue;
            }

            // Update Music Toggle and Music Value
            if (_musicToggle.isOn == _musicIsMute)
            {
                // Music Value
                _musicIsMute = !_musicToggle.isOn;

                // Music Toggle
                _musicAudioSource.mute = _musicIsMute;
            }
        }

        public void Update()
        {
            if(false == _lock)
            {
                // Update Music
                UpdateMusic();

                // Update Game SOund
                UpdateGameSound();
            }
        }

        #endregion
    }
}
