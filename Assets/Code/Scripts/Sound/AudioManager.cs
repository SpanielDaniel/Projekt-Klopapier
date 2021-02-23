using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts
{
    
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private Sound[] Sounds;

        protected override void AwakeFunction()
        {
            
            base.AwakeFunction();
            DontDestroyOnLoad(this);

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
        }

        public void Play(string _name)
        {
            if (Sounds == null) return; 
            Sound s = Array.Find(Sounds, sound => sound.GetName == _name);
            s.Source.Play();
        }

    }
}