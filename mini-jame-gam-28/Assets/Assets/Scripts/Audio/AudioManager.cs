using System;
using System.Linq;
using Assets.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {

        [Serializable]
        public class Sound
        {
            public string name;
            public AudioClip Clip;
            public float pitch;
            public float Volume;
            public AudioSource audioSource;
        }
    
    
        [SerializeField] private Sound[] sounds;
    
        private void Start()
        {
            TileManager.OnCollectedSun += TileManagerOnOnCollectedSun;
            PlayerController.OnPlayerMove += OnPlayerMove;
            TileManager.OnInvalidMove += TileManagerOnOnInvalidMove;
            PlayerController.OnPlayerReset += OnPlayerReset;
            InitSounds();
        }

        private void OnPlayerReset()
        {
            PlaySound("Fail");
        }

        private void TileManagerOnOnInvalidMove()
        {
            PlaySound("hitWall");
        }

        private void OnPlayerMove(int remainingmoves,Vector2 _)
        {
            PlaySound("Grow");
            if (sounds[1].pitch < 1.9f)
            {
                sounds[1].pitch += 0.1f;
            }
            else
            {
                sounds[1].pitch = 1;
            }
            
        }

        private void TileManagerOnOnCollectedSun()
        {
            PlaySound("pickUpSun");
        }

        private void InitSounds()
        {
            foreach (var sound in sounds)
            {
                sound.audioSource = gameObject.AddComponent<AudioSource>();
                sound.audioSource.clip = sound.Clip;
                sound.audioSource.pitch = sound.pitch;
                sound.audioSource.volume = sound.Volume;
            }
        }

        private void Update()
        {
            UpdateSounds();
        }

        private void UpdateSounds()
        {
            foreach (var sound in sounds)
            {
                sound.audioSource.pitch = sound.pitch;
                sound.audioSource.volume = sound.Volume;
            }
        }

        private void PlaySound(string soundName)
        {
            var matchingSounds = (from sound in sounds where sound.name == soundName select sound).ToList();
            if (matchingSounds.Count > 0) 
            {
                matchingSounds[0].audioSource.Play();
            }
        }
    
    }
}
