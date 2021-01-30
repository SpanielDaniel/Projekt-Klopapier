using System;
using UnityEngine;

namespace Code.Scripts
{
    
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] Sounds;

        public void Awake()
        {
            foreach (Sound sound in Sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.GetClip;
                sound.Source.volume = sound.GetVolume;
                sound.Source.pitch = sound.GetPitch;
            }
        }

        public void Play(string _name)
        {
            Sound s = Array.Find(Sounds, sound => sound.GetName == _name);
            s.Source.Play();
        }
    }
}