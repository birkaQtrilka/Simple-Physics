using GXPEngine;
using GXPEngine.Core;
using System;
using System.Linq;
using TiledMapParser;

namespace gxpengine_template
{
    public class Enemy : MovableObject, IHealthHolder
    {
        public event Action<int> HealthUpdate;
        public event Action RecievedDamage;
        public event Action Killed;
        public int Health
        {
            get => _health;
            set
            {
                if (value < _health) RecievedDamage?.Invoke();
                if (value <= 0)
                {
                    ClearMovePattern();
                    Killed?.Invoke();
                }

                _health = (int)Mathf.Clamp(value, 0, _maxHealth);
                HealthUpdate?.Invoke(_health);
            }
        }
        public int MaxHealth => _maxHealth;
        public Attacker Attacker { get; }

        int _currPatternIndex;
        bool _performedPattern;
        int _health;

        readonly IAct[] _movePattern;
        readonly EnemyAnimationManager _animationManager;
        readonly Sound _attackSound;
        readonly SoundData _attackSoundData;
        readonly int _maxHealth;
        readonly int _holdingCoins;

        public Enemy(TiledObject data) : base("barry.png", 7, 1, data)
        {
            collider.isTrigger = true;
            _health = data.GetIntProperty("Health");
            _maxHealth = data.GetIntProperty("MaxHealth");
            _holdingCoins = data.GetIntProperty("Coins");
            var damage = data.GetIntProperty("Damage");
            var critMult = data.GetFloatProperty("CritMult");
            var visualStayTime = data.GetFloatProperty("VisualStayTime");

            _attackSound = new Sound(data.GetStringProperty("AttackSound", "transmit.wav"));
            _attackSoundData = new SoundData
            (
                data.GetFloatProperty("AttackVolume", .5f),
                data.GetFloatProperty("AttackPan", 0),
                data.GetFloatProperty("AttackFrequency", 44100)
            );

            if (_maxHealth < _health)
            {
                _maxHealth = _health;
                Console.WriteLine("WARNING ! Max health can't be smaller value than Health");
            }

            var (normalSprite, critSprite) = InitAttackerSprites();
            Attacker = new Attacker(damage, critMult, this, normalSprite, critSprite, visualStayTime, 0);

            AddChild(Attacker);

            _movePattern = MovePatternFactory(data);

            _animationManager = new EnemyAnimationManager(this);
            _animationManager.Animations["Death"].AnimationExit += Die;
            Attacker.Attacked += PlayAttackSound;
            BeatManager.Instance.Beat += OnBeat;
            alpha = 0;

        }
        
        void ClearMovePattern()
        {
            for (int i = 0; i < _movePattern.Length; i++)
                _movePattern[i] = null;

        }

        (Sprite normal, Sprite crit) InitAttackerSprites()
        {
            var attackSprite = new Sprite("slash.png");
            var critSprite = new Sprite("slash_crit.png");
            attackSprite.SetCenterOrigin();
            critSprite.SetCenterOrigin();
            attackSprite.SetScaleXY(.125f);
            critSprite.SetScaleXY(.125f);
            return (attackSprite, critSprite);
        }

        protected override bool IgnoreCollision(GameObject go)
        {
            return go is Player;
        }

        IAct[] MovePatternFactory(TiledObject data)
        {
            var moveLeft = new MoveAct(this, -1);
            var moveRight = new MoveAct(this, 1);
            var attack = new AttackAct(Attacker);
            var attackInst = new AttackAct(Attacker, true);
            var sitStill = new MoveAct(this, 0);
            var attackBehind = new AttackAct(Attacker, oppositeWay: true);

            IAct GetActByName(string name)
            {
                switch (name)
                {
                    case "moveLeft":
                        return moveLeft;
                    case "moveRight":
                        return moveRight;
                    case "attack":
                        return attack;
                    case "attackBehind":
                        return attackBehind;
                    case "attackInst":
                        return attackInst;
                    case "stay":
                        return sitStill;
                    default:
                        Console.WriteLine("name from property isn't supported to return an IAct instance");
                    return null;
                }
            }

            return data.GetStringProperty("MovePattern").Split(',').Select(x => GetActByName(x)).ToArray();
        }

        void PlayAttackSound()
        {
            var persentage = Mathf.Clamp(VolumePosition() - 1,0,1);
            _attackSound.Play(volume: _attackSoundData.volume * persentage) ;
        }

        float VolumePosition()
        {
            var screenPos = TransformPoint(0,0);

            float dx = screenPos.x - game.width / 2;
            float dy = screenPos.y - game.height / 2;

            float L1distance = Mathf.Abs(dx) + Mathf.Abs(dy);
            float volume = 1;
            //inverted comparison operator
            if (L1distance < game.width / 2 + game.height / 2 ) 
                volume = (game.width + game.height) / (2 * L1distance);
            return volume;
        }

        void Die()
        {
            var currLevel = MyUtils.MyGame.CurrentLevel;
            for(int i = 0;  i < _holdingCoins; i++)
            {

                var c = MyUtils.MyGame.Prefabs["BouncingCoin"].Clone() as BouncingCoin;
                c.SetOrigin(c.width/2,c.height/2);
                c.width = 32;
                c.height = 32;
                
                c.Config(Utils.Random(7, 12));
                
                currLevel.AddChild(c);
                c.SetXY(x, y);
            }
            LateDestroy();
        }

        protected override void OnDestroy()
        {
            _animationManager.Animations["Death"].AnimationExit -= Die;
            BeatManager.Instance.Beat -= OnBeat;
            Attacker.Attacked -= PlayAttackSound;

        }

        void OnBeat()
        {
            //Health -= 2;
            if (_movePattern.Length == 0 || _movePattern[0] == null) return;
            _movePattern[_currPatternIndex].Perform();

            if (++_currPatternIndex == _movePattern.Length)
                _currPatternIndex = 0;
            var safetyCheck = 0;
            while (_movePattern[_currPatternIndex].IsInstant)
            {
                _movePattern[_currPatternIndex++].Perform();
                if (_currPatternIndex == _movePattern.Length)
                    _currPatternIndex = 0;
                if (safetyCheck++ > 50)
                    throw new Exception($"you can't have all move patterns of {name} be instant");
            }
            
            _performedPattern = true;
        }

        protected override void HandleInput()
        {
            if (_performedPattern)
            {
                _performedPattern = false;
                return;
            }

            DirectionInput = Vector2.zero;

        }

        
    }
}
