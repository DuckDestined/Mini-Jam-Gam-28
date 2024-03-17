using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {

        [Serializable]
        public class Sound
        {
            public string Name;
            public AudioClip Clip;
            public float Pitch;
            public float Volume;
            public AudioSource audioSource;
        }
    
    
        [SerializeField] private Sound[] sounds;
    
        private void Start()
        {
            TileManager.OnCollectedSun += TileManagerOnOnCollectedSun;
            PlayerController.OnPlayerMove += OnPlayerMove;
            TileManager.OnInvalidMove += TileManagerOnOnInvalidMove;
            InitSounds();
        }

        private void TileManagerOnOnInvalidMove()
        {
            PlaySound("hitWall");
        }

        private void OnPlayerMove(int remainingmoves)
        {
            PlaySound("grow");
            sounds[1].Pitch += 0.2f;
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
                sound.audioSource.pitch = sound.Pitch;
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
                sound.audioSource.pitch = sound.Pitch;
                sound.audioSource.volume = sound.Volume;
            }
        }
    
        public void PlaySound(string soundName)
        {
            (from sound in sounds where sound.Name == soundName select sound).ToList()[0].audioSource.Play();
        }
    
    }
}
