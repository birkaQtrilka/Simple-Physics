using GXPEngine;
using System.Xml.Linq;
using TiledMapParser;

namespace gxpengine_template
{
    public abstract class PickUp : AnimationSprite
    {
        protected Sound pickUpSound;
        readonly SoundData _pickUpSoundData;

        protected readonly string fileName;
        protected readonly int columns, rows;
        protected readonly TiledObject serializedData;

        public PickUp(string fileName, int c, int r, TiledObject data) : base(fileName, c, r,addCollider: true)
        {
            collider.isTrigger = true;

            if (data == null) return;

            pickUpSound = new Sound(data.GetStringProperty("PickUpSound"));
            _pickUpSoundData = new SoundData
            (
                data.GetFloatProperty("Volume", 0.3f)
            );
            this.fileName = fileName;
            columns = c;
            rows = r;
            serializedData = data;
            SetCycle(0, c, animationDelay: 6);

        }
        public void Take(Player player) 
        {
            Grab(player);
            pickUpSound.Play(volume: _pickUpSoundData.volume);
            LateDestroy();
        }
        protected abstract void Grab(Player player);
    }
}
