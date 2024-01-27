using GXPEngine;
using System;
using System.Runtime.CompilerServices;
using TiledMapParser;

namespace gxpengine_template
{
    public class BeatManager : AnimationSprite
    {
        public event Action Beat;

        public static BeatManager Instance { get; private set; }
        public float BeatDuration => _beatDuration;
        public int BPM { get; }
        public bool OnBeat { get; private set; }

        float _lastBeatTime;
        readonly Sound _song;
        SoundChannel _playingSong;
        SoundData _songData;
        readonly float _beatErrorMargin;
        readonly float _beatDuration;
        public BeatManager(TiledObject data) : base("circle.png",1,1,-1,false,false)
        {
            BPM = data.GetIntProperty("BPM", 60);
            _beatErrorMargin = data.GetFloatProperty("ErrorMargin", 0.2f);
            _song = new Sound(data.GetStringProperty("MusicName" ));
            _songData = new SoundData(data.GetFloatProperty("Volume",1));
            _beatDuration = 1f / (BPM / 60000f);

            _playingSong = _song.Play(volume: _songData.volume);
            
            _lastBeatTime = Time.time;
            visible = false;
            if (Instance != null && Instance != this)
            {
                LateDestroy();
                return;
            }
            else
                Instance = this;
            
        }
        protected override void OnDestroy()
        {
            Instance = null;
            _playingSong?.Stop();
        }
        public void Update()
        {
            var progress = (Time.time - _lastBeatTime) / _beatDuration;

            OnBeat = 1f - progress < _beatErrorMargin || progress < _beatErrorMargin;

            if (progress >= 1)
            {
                _lastBeatTime = Time.time;
                Beat?.Invoke();
            }
            if(!_playingSong.IsPlaying)
            {
                _lastBeatTime = Time.time;
                _playingSong = _song.Play(volume: _songData.volume);
            }
        }

       
    }
  
}
