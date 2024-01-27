using GXPEngine;

namespace gxpengine_template
{
    public readonly struct SoundData
    {
        public readonly float volume; 
        public readonly float pan; 
        public readonly float frequency;

        public SoundData(float volume, float pan = 0, float frequency = 44100)
        {
            this.volume = volume;
            this.pan = pan;
            this.frequency = frequency;

        }
        public void SetSoundChanel(SoundChannel channel)
        {
            channel.Pan = pan;
            channel.Frequency = frequency;
            channel.Volume = volume;
        }
    }


}
