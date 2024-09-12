using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace Tankz
{
    class SoundEmitter : Component
    {
        protected AudioSource source;
        protected AudioClip clip;

        public float Volume { get { return source.Volume; } set { source.Volume = value; } }
        public float Pitch { get { return source.Pitch; } set { source.Pitch = value; } }

        public SoundEmitter(GameObject owner, string clipName) : base(owner)
        {
            source = new AudioSource();
            clip = SoundMngr.GetClip(clipName);
        }

        public void Play()
        {
            source.Play(clip);
        }

        public bool IsPlaying()
        {
            return source.IsPlaying;
        }

        public void RandomizePitch()
        {
            Pitch = RandomGenerator.GetRandomFloat() * 0.4f + 0.8f; //0.8f => 1.2f
        }

        public void Play(float volume, float pitch = 1f, AudioClip clipToPlay = null)
        {
            source.Volume = volume;
            source.Pitch = pitch;
            source.Play(clipToPlay != null ? clipToPlay : clip);
        }
    }
}
