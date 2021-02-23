// File     : Sound.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

namespace Code.Scripts
{
    [Serializable]
    public class Sound
    {
        [SerializeField] private string Name;
        [SerializeField] private AudioClip Clip;
        [SerializeField] [Range(0f,1f)] private float Volume;
        [SerializeField] [Range(0.1f,3f)] private float Pitch;

        public string GetName => Name;
        public AudioClip GetClip => Clip;
        public float GetVolume => Volume; 
        public float GetPitch => Pitch; 
        
        [HideInInspector] public AudioSource Source;
    }
}