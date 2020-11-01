#region Snippet Information and Use
/* Creator Information
 *
 * Script Name: ManagerAudio
 * Author: Joseph CF Rothwell
*/

/* Steps for use
 *
 * 1) Do...
 */
#endregion

using System.Collections;
using UnityEngine;

namespace Managers
{
    public class ManagerAudio : MonoBehaviour
    {
        #region Class Variables
        [Range(0.0f, 1.0f)] public float maxVolumeMaster, savedVolumeMaster, realMaxVolumeMusic, realMaxVolumeSFX, realMaxVolumeVoice;
        [HideInInspector] public bool audioIsMuted, audioIsPaused;

        private AudioSource _musicSourceA, _musicSourceB, _sfxSource, _voiceSourceA, _voiceSourceB, _voiceSourceC;
        [SerializeField] private float outputMaxVolumeMusic, outputMaxVolumeSFX, outputMaxVolumeVoice;
        private bool _musicSourceAIsPlaying;
        #endregion

        /// <DEBUG>
        /// DEBUG CODE BELOW REMOVE ON RELEASE
        /// </DEBUG>
        [SerializeField] [TextArea] private string audioMuteOrPause;
        /// <DEBUG>
        /// DEBUG CODE ABOVE REMOVE ON RELEASE
        /// </DEBUG>

        private static GameObject _audioManagerObject;
        private static ManagerAudio _audioManagerInstance;
        public static ManagerAudio AMI
        {
            get
            {
                #region Existence check
                #region Code explanation
                /* 
             * Find the Manager_Audio gameobject and set the variable. 
             * If there is no such game object, create one with this script and set variables.
             * If that object does exist but has no Manager_Audio component, create that component on it and set variables.
             */
                #endregion
                #region Existence check code
                if (_audioManagerInstance == null)
                {
                    _audioManagerInstance = FindObjectOfType<ManagerAudio>();
                    if (_audioManagerInstance == null)
                    {
                        _audioManagerInstance = new GameObject("Manager_Audio", typeof(ManagerAudio)).GetComponent<ManagerAudio>();
                    }
                }
                #endregion
                #endregion
                return _audioManagerInstance;
            }
/*
            private set
            {
                _audioManagerInstance = value;
            }
*/
        }
        public static void DontDestroyMeOnLoad(GameObject thisObject)
        {
            // This protects this, and objects above it (eg Managers gameobject), from being destroyed on load
            // This also means don't need to protect other manager classes?
            Transform parentTransform = thisObject.transform;

            // If this object doesn't have a parent then its the root transform.
            while (parentTransform.parent != null)
            {
                // Keep going up the chain.
                parentTransform = parentTransform.parent;
            }
            GameObject.DontDestroyOnLoad(parentTransform.gameObject);
        }

        private void Awake()
        {
            #region Instance Protection and Component Setup
            #region Code explanation
            /*
         * Protect this instance from destruction
         * Add components to the audio sources
         * Set musicSources to loop by default, but not the soundeffectsource or voice sources
         */
            #endregion
            #region Instance Protection and Component Setup Code
            _audioManagerObject = this.gameObject;
            DontDestroyMeOnLoad(_audioManagerObject);

            _musicSourceA = _audioManagerObject.AddComponent<AudioSource>();
            _musicSourceA.loop = true;
            _musicSourceB = _audioManagerObject.AddComponent<AudioSource>();
            _musicSourceB.loop = true;
            _sfxSource = _audioManagerObject.AddComponent<AudioSource>();
            _voiceSourceA = _audioManagerObject.AddComponent<AudioSource>();
            _voiceSourceB = _audioManagerObject.AddComponent<AudioSource>();
            _voiceSourceC = _audioManagerObject.AddComponent<AudioSource>();
            maxVolumeMaster = 1;
            realMaxVolumeMusic = 1;
            realMaxVolumeSFX = 1;
            realMaxVolumeVoice = 1;
            #endregion
            #endregion


            audioMuteOrPause = "Audio: ";
            Debug.Log("Audio Mute or Pause variable ready: " + audioMuteOrPause);


            ManagerIO.IOMI.IO_ReadWriteConfigFile("audio");  // Read audio config on awake (sets variables)
        }

        private void Update()
        {
            Audio_VolumeMax_Update();           //We call an update for volumes every frame


            if (audioIsMuted && !audioIsPaused)
            { audioMuteOrPause = "Audio: Muted"; }
            if (!audioIsMuted && audioIsPaused)
            { audioMuteOrPause = "Audio: Paused"; }
            if (audioIsMuted && audioIsPaused)
            { audioMuteOrPause = "Audio: Muted, Paused"; }
            if (!audioIsMuted && !audioIsPaused)
            { audioMuteOrPause = "Audio: "; }

        }

        #region General and Master Methods
        public float Audio_Normalise0To100(float input)
        {
            if (input > 100) { input = 100; }
            if (input < 0) { input = 0; }
            return input;
        }
        #region Methods: Mute - public AudioMaster_Mute() and private AudioMaster_MuteSet()
        #region Mute methods Explanation
        /*  AudioMaster_Mute() can be called from other scripts.
     *  AudioMaster_MuteSet() can only be called from this class.
     *  
     *  Call in other scripts with:
     *  >        ManagerAudio.AMI.AudioMaster_Mute();
     *  Can force it to be muted with:
     *  >        ManagerAudio.AMI.AudioMaster_Mute(true, true);
     *  or force unmute with
     *  >        ManagerAudio.AMI.AudioMaster_Mute(true, false);
     *  
     *  AudioMaster_Mute() by default TOGGLES whether the audio is muted.
     *  However, it can take two optional arguments, overrideToggle and trueIsMuteFalseIsUnmute
     *  If overrideToggle is true, then trueIsMuteFalseIsUnmute is used to SET mute state
     *  In either case, AudioMaster_Mute() calls AudioMaster_MuteSet() which actually handles muting
     *  
     *  AudioMaster_MuteSet() by default mutes the audio.
     *  However, it takes optional argument trueIsMuteFalseIsUnmute.
     *  If true, it mutes 6 audiosources, musics 1 and 2 and the sfx source, and three voice sources.
     *  It checks whether the muting call is the same as its input, and if not then it takes the action
     *  ^ this step protects against multiple inputs of setting mute state to the same state again
     * 
     */
        #endregion
        public void AudioMaster_Mute(bool overrideToggle = false, bool trueIsMuteFalseIsUnmute = true)
        {
            switch (overrideToggle)
            {
                case true:
                    switch (trueIsMuteFalseIsUnmute)
                    {
                        case true:
                            AudioMaster_MuteSet();
                            break;
                        case false:
                            AudioMaster_MuteSet(false);
                            break;
                    }
                    break;
                case false:
                    switch (audioIsMuted)
                    {
                        case true:
                            AudioMaster_MuteSet(false);
                            break;
                        case false:
                            AudioMaster_MuteSet();
                            break;
                    }
                    break;
            }
        }
        private void AudioMaster_MuteSet(bool trueIsMuteFalseIsUnmute = true)
        {
            if (audioIsMuted != trueIsMuteFalseIsUnmute)
            {
                audioIsMuted = trueIsMuteFalseIsUnmute;
                switch (trueIsMuteFalseIsUnmute)
                {
                    case true:
                        savedVolumeMaster = maxVolumeMaster;
                        AudioMaster_Volume_SetMaximum(0f);

                        break;
                    case false:
                        AudioMaster_Volume_SetMaximum(savedVolumeMaster * 100);
                        break;
                }
            }
        }
        #endregion
        #region Methods: Pause - public AudioMaster_Pause() and private AudioMaster_PauseSet()
        #region Pause methods Explanation
        /*  AudioMaster_Pause() can be called from other scripts.
     *  AudioMaster_PauseSet() can only be called from this class.
     *  
     *  Call in other scripts with:
     *  >        ManagerAudio.AMI.AudioMaster_Pause();
     *  Can force it to be paused with:
     *  >        ManagerAudio.AMI.AudioMaster_Pause(true, true);
     *  or force unpause with
     *  >        ManagerAudio.AMI.AudioMaster_Pause(true, false);
     *  
     *  AudioMaster_Pause() by default TOGGLES whether the audio is paused.
     *  However, it can take two optional arguments, overrideToggle and trueIsPauseFalseIsUnpause
     *  If overrideToggle is true, then trueIsPauseFalseIsUnpause is used to SET pause state
     *  In either case, AudioMaster_Pause() calls AudioMaster_PauseSet() which actually handles pausing audio
     *  
     *  AudioMaster_PauseSet() by default pauses the audio.
     *  However, it takes optional argument trueIsPauseFalseIsUnpause.
     *  If true, it pauses 6 audiosources, musics 1 and 2 and the sfx source, and three voice sources.
     *  It checks whether the pausing call is the same as its input, and if not then it takes the action
     *  ^ this step protects against multiple inputs of setting pause state to the same state again
     * 
     */
        #endregion
        public void AudioMaster_Pause(bool overrideToggle = false, bool trueIsPauseFalseIsUnpause = true)
        {
            switch (overrideToggle)
            {
                case true:
                    switch (trueIsPauseFalseIsUnpause)
                    {
                        case true:
                            AudioMaster_PauseSet();
                            break;
                        case false:
                            AudioMaster_PauseSet(false);
                            break;
                    }
                    break;
                case false:
                    switch (audioIsPaused)
                    {
                        case true:
                            AudioMaster_PauseSet(false);
                            break;
                        case false:
                            AudioMaster_PauseSet();
                            break;
                    }
                    break;
            }
        }
        private void AudioMaster_PauseSet(bool trueIsPauseFalseIsUnpause = true)
        {
            if (audioIsPaused != trueIsPauseFalseIsUnpause)
            {
                audioIsPaused = trueIsPauseFalseIsUnpause;
                switch (trueIsPauseFalseIsUnpause)
                {
                    case true:
                        _musicSourceA.Pause();
                        _musicSourceB.Pause();
                        _sfxSource.Pause();
                        _voiceSourceA.Pause();
                        _voiceSourceB.Pause();
                        _voiceSourceC.Pause();
                        break;
                    case false:
                        _musicSourceA.UnPause();
                        _musicSourceB.UnPause();
                        _sfxSource.UnPause();
                        _voiceSourceA.UnPause();
                        _voiceSourceB.UnPause();
                        _voiceSourceC.UnPause();
                        break;
                }
            }
        }
        #endregion
        public void AudioMaster_Stop()
        {
            AudioMusic_Stop();
            AudioSFX_Stop();
            AudioVoice_Stop();
        }
        #region Methods: Master Volume and Audio_VolumeMax_Update()
        public void AudioMaster_Volume_SetMaximum(float newMaxBetween0And100 = 100)
        {
            //We take in input between 0 and 100, and make sure it stays within 0 and 100. Then
            // we divide by 100, to get a float between 0 and 1
            newMaxBetween0And100 = Audio_Normalise0To100(newMaxBetween0And100);
            maxVolumeMaster = newMaxBetween0And100 / 100;
        }
        public void AudioMaster_Volume_IncrementMaximum(float incrementAmount = 1)
        {
            float newMax = Audio_Normalise0To100((maxVolumeMaster * 100) + incrementAmount);
            AudioMaster_Volume_SetMaximum(newMax);
        }
        public void Audio_VolumeMax_Update()
        {
            // We need to turn the realMaximums into output maximums THIS MEANS WE DON'T SET outputMax elsewhere
            outputMaxVolumeMusic = realMaxVolumeMusic * maxVolumeMaster;
            outputMaxVolumeSFX = realMaxVolumeSFX * maxVolumeMaster;
            outputMaxVolumeVoice = realMaxVolumeVoice * maxVolumeMaster;
            // So we set the volume to the output maximums. 
            _musicSourceA.volume = _musicSourceB.volume = outputMaxVolumeMusic;
            _sfxSource.volume = outputMaxVolumeSFX;
            _voiceSourceA.volume = _voiceSourceB.volume = _voiceSourceC.volume = outputMaxVolumeVoice;
        }
        #endregion
        #endregion

        #region Music-specific methods
        private AudioSource ActiveMusicSource(AudioSource musicSourceSide1, AudioSource musicSourceSide2)
        {
            #region ActiveMusicSource method explained
            /* This checks whether side1 source is playing. If yes, return it as the active source
         * and if not, then return side2 source.
         * Eg ActiveSource(musicSourceA, musicSourceB) or
         * ActiveSource(musicSourceB, musicSourceA)
         * Call this method with 'AudioSource activeSource = ActiveSource(side1, side2);'
         */
            #endregion
            AudioSource currentSource;
            if (_musicSourceAIsPlaying) { currentSource = musicSourceSide1; }
            else { currentSource = musicSourceSide2; }
            return currentSource;
        }
        public void AudioMusic_Play(AudioClip musicClip, float volumeBetween0And100 = 100)
        {
            volumeBetween0And100 = Audio_Normalise0To100(volumeBetween0And100);
            AudioSource activeSource = ActiveMusicSource(_musicSourceA, _musicSourceB);
            activeSource.clip = musicClip;

            activeSource.volume = (volumeBetween0And100 / 100) * maxVolumeMaster;
            if (activeSource.volume > outputMaxVolumeMusic)
            {
                activeSource.volume = outputMaxVolumeMusic;
            }
            activeSource.Play();
        }
        public void AudioMusic_Stop()
        {
            _musicSourceA.Stop();
            _musicSourceB.Stop();
        }
        #region Music-specific volume Methods
        public void AudioMusic_Volume_SetMaximum(float newMaxBetween0And100 = 100)
        {
            // we normalise input to 0-100, then we set realMax and flag to the update function that we changed
            newMaxBetween0And100 = Audio_Normalise0To100(newMaxBetween0And100);
            realMaxVolumeMusic = newMaxBetween0And100 / 100;
        }
        public void AudioMusic_Volume_IncrementMaximum(float incrementAmount = 1)
        {
            /* float newMax = Audio_Normalise0To100((maxVolumeMusic * 100) + incrementAmount);
         AudioMusic_Volume_SetMaximum(newMax);*/
            float newMax = Audio_Normalise0To100((realMaxVolumeMusic * 100) + incrementAmount);
            AudioMusic_Volume_SetMaximum(newMax);
        }
        #endregion
        #region Fading and Crossfading Methods and Coroutines
        #region Fade Out
        public void AudioMusic_FadeOut(float transitionTime = 1.0f)
        {

            AudioSource activeSource = ActiveMusicSource(_musicSourceA, _musicSourceB);
            StartCoroutine(AudioMusic_FadeOut_Coroutine(activeSource, transitionTime));
        }
        private IEnumerator AudioMusic_FadeOut_Coroutine(AudioSource activeSource, float transitionTime)
        {
            if (activeSource.isPlaying == false) { activeSource.Play(); }

            for (float time = 0; time < transitionTime; time += Time.deltaTime)
            {
                activeSource.volume = (1 * outputMaxVolumeMusic) - (time / transitionTime);
                yield return null;
            }

            activeSource.Stop();
        }
        #endregion
        #region Fade to New
        public void AudioMusic_FadeToNew(AudioClip musicClip, float transitionTime = 1.0f)
        {

            AudioSource activeSource = ActiveMusicSource(_musicSourceA, _musicSourceB);
            StartCoroutine(AudioMusic_FadeToNew_Coroutine(musicClip, activeSource, transitionTime));
        }
        private IEnumerator AudioMusic_FadeToNew_Coroutine(AudioClip musicClip, AudioSource activeSource, float transitionTime)
        {
            if (activeSource.isPlaying == false) { activeSource.Play(); }

            for (float time = 0; time < transitionTime; time += Time.deltaTime)
            {
                activeSource.volume = (1 * outputMaxVolumeMusic) - (time / transitionTime);
                yield return null;
            }

            activeSource.Stop();
            activeSource.clip = musicClip;
            activeSource.Play();

            for (float time = 0; time < transitionTime; time += Time.deltaTime)
            {
                activeSource.volume = (time / transitionTime) * outputMaxVolumeMusic;
                yield return null;
            }
        }
        #endregion
        #region Crossfade to New
        public void AudioMusic_CrossfadeToNew(AudioClip musicClip, float transitionTime = 1.0f)
        {
            AudioSource activeSource = ActiveMusicSource(_musicSourceA, _musicSourceB);
            AudioSource nextSource = ActiveMusicSource(_musicSourceB, _musicSourceA);
            _musicSourceAIsPlaying = !_musicSourceAIsPlaying;

            nextSource.clip = musicClip;
            nextSource.Play();

            StartCoroutine(AudioMusic_CrossfadeToNew_Coroutine(nextSource, activeSource, transitionTime));
        }
        private IEnumerator AudioMusic_CrossfadeToNew_Coroutine(AudioSource nextSource, AudioSource activeSource, float transitionTime)
        {
            for (float time = 0; time < transitionTime; time += Time.deltaTime)
            {
                nextSource.volume = (time / transitionTime) * outputMaxVolumeMusic;
                activeSource.volume = (1 * outputMaxVolumeMusic) - (time / transitionTime);
                yield return null;
            }

            activeSource.Stop();
        }
        #endregion
        #endregion
        #endregion

        #region SFX-specific Methods
        public void AudioSFX_Play(AudioClip sfxClip, float volumeBetween0And100 = 100)
        {

            volumeBetween0And100 = Audio_Normalise0To100(volumeBetween0And100);
            float volume = (volumeBetween0And100 / 100) * maxVolumeMaster;
            if (volume > outputMaxVolumeSFX)
            {
                volume = outputMaxVolumeSFX;
            }
            _sfxSource.PlayOneShot(sfxClip, volume);
        }
        public void AudioSFX_Stop()
        {
            _sfxSource.Stop();
        }
        #region SFX-specific volume
        public void AudioSFX_Volume_SetMaximum(float newMaxBetween0And100 = 100)
        {
            // we normalise input to 0-100, then we set realMax and flag to the update function that we changed
            newMaxBetween0And100 = Audio_Normalise0To100(newMaxBetween0And100);
            realMaxVolumeSFX = newMaxBetween0And100 / 100;
        }
        public void AudioSFX_Volume_IncrementMaximum(float incrementAmount = 1)
        {
            /* float newMax = Audio_Normalise0To100((maxVolumeMusic * 100) + incrementAmount);
         AudioMusic_Volume_SetMaximum(newMax);*/
            float newMax = Audio_Normalise0To100((realMaxVolumeMusic * 100) + incrementAmount);
            AudioSFX_Volume_SetMaximum(newMax);
        }
        #endregion
        #endregion

        #region voice-specific Methods
        private AudioSource ActiveVoiceSource(AudioSource voiceSourceSide1, AudioSource voiceSourceSide2, AudioSource voiceSourceSide3)
        {
            #region ActiveVoiceSource method explained
            /* This checks whether side1 source is playing. If yes, return it as the active source
         * and if not, if side2 source is playing, if yes return, otherwise return side2 source.
         * Eg ActiveVoiceSource(voiceSourceA, voiceSourceB, voiceSourceC) or
         * ActiveSource(voiceSourceB, VoiceSourceA, VoiceSourceC)
         * Call this method with 'AudioSource activeVoiceSource = ActiveVoiceSource(side1, side2, side3);'
         */
            #endregion
            AudioSource currentSource;
            if (voiceSourceSide1.isPlaying) { currentSource = voiceSourceSide1; }
            else if (voiceSourceSide2.isPlaying) { currentSource = voiceSourceSide2; }
            else { currentSource = voiceSourceSide3; }
            return currentSource;
        }
        public void AudioVoice_Play(AudioClip voiceClip, float volumeBetween0And100 = 100)
        {
            volumeBetween0And100 = Audio_Normalise0To100(volumeBetween0And100);
            AudioSource activeSource = ActiveVoiceSource(_voiceSourceA, _voiceSourceB, _voiceSourceC);

            activeSource.volume = (volumeBetween0And100 / 100) * maxVolumeMaster;
            if (activeSource.volume > outputMaxVolumeVoice)
            {
                activeSource.volume = outputMaxVolumeVoice;
            }
            activeSource.PlayOneShot(voiceClip);
        }
        public void AudioVoice_Stop()
        {
            _voiceSourceA.Stop();
            _voiceSourceB.Stop();
            _voiceSourceC.Stop();
        }

        #region voice-specific volume
        public void AudioVoice_Volume_SetMaximum(float newMaxBetween0And100 = 100)
        {
            // we normalise input to 0-100, then we set realMax and flag to the update function that we changed
            newMaxBetween0And100 = Audio_Normalise0To100(newMaxBetween0And100);
            realMaxVolumeVoice = newMaxBetween0And100 / 100;
        }
        public void AudioVoice_Volume_IncrementMaximum(float incrementAmount = 1)
        {
            /* float newMax = Audio_Normalise0To100((maxVolumeMusic * 100) + incrementAmount);
         AudioMusic_Volume_SetMaximum(newMax);*/
            float newMax = Audio_Normalise0To100((realMaxVolumeVoice * 100) + incrementAmount);
            AudioMusic_Volume_SetMaximum(newMax);
        }
        #endregion
        #endregion


    }
}
