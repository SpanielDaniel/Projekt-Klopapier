using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts
{
    
    public class AudioManager : Singleton<AudioManager>
    {
        // -------------------------------------------------------------------------------------------------------------
        
        #region Init
        // Serialize Fields --------------------------------------------------------------------------------------------
        [SerializeField] private Sound[] Sounds;
        [SerializeField] private Sound[] Music;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Singleton awake function.
        /// </summary>
        /// 
        protected override void AwakeFunction()
        {
            
            
            base.AwakeFunction();
            DontDestroyOnLoad(this);

            // Initialize Sounds.
            if (Sounds != null)
            {
                foreach (Sound sound in Sounds)
                {
                    sound.Source = gameObject.AddComponent<AudioSource>();
                    sound.Source.clip = sound.GetClip;
                    sound.Source.volume = sound.GetVolume;
                    sound.Source.pitch = sound.GetPitch;
                }
            }
            
            // Initalize Music.
            if (Music != null)
            {
                foreach (Sound sound in Music)
                {
                    sound.Source = gameObject.AddComponent<AudioSource>();
                    sound.Source.clip = sound.GetClip;
                    sound.Source.volume = sound.GetVolume;
                    sound.Source.pitch = sound.GetPitch;
                }
            }
        }
        
        // -------------------------------------------------------------------------------------------------------------
        
        #region Functions

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="_name">Hands over a sound name, which starts to play.</param>
        public void PlaySound(string _name)
        {
            if (Sounds == null) return; 
            Sound s = Array.Find(Sounds, sound => sound.GetName == _name);
            s.Source.Play();
        }

        /// <summary>
        /// Plays music.
        /// </summary>
        /// <param name="_name">Hands over the music name.</param>
        public void PlayMusic(string _name)
        {
            if (Music == null) return; 
            Sound m = Array.Find(Music, music => music.GetName == _name);
            m.Source.Play();
            m.Source.loop = true;
        }


        public void OnSoundValueChanged(float _value)
        {
            foreach (var sound in Sounds)
            {
                sound.Source.volume = _value;
            }
            
            foreach (var music in Music)
            {
                music.Source.volume = _value;
            }
            
        }
        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
    }
}