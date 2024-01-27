using GXPEngine;
using System;
using TiledMapParser;

namespace gxpengine_template
{
    public enum State
    {
        Walk,
        Jump,
        Fall,
        Idle
    }
    public sealed class Player : JumpableObject, IHealthHolder
    {
        public event Action<int> HealthUpdate;
        public event Action RecievedDamage;
        public event Action<int> MoneyUpdate;
        public event Action Killed;
        public int Health
        {
            get => _health;
            set
            {
                if (value < _health) RecievedDamage?.Invoke();
                if (value <= 0)
                {
                    Killed?.Invoke();
                    HandleDeath();
                    LateDestroy();
                }

                _health = (int)Mathf.Clamp(value, 0, _maxHealth);


                HealthUpdate?.Invoke(_health);
            }
        }

        public int MaxHealth => _maxHealth;

        public Attacker Attacker => _attacker;
        public SpellMaker SpellMaker => _spellMaker;
        public SpellSlot Slot => _slot;
        public PlayerData PlayerGameData { get; }
        
        public int CoinAmount { get => PlayerGameData.Money; set { PlayerGameData.Money = value; MoneyUpdate?.Invoke(value); } }

        readonly int _startingMoney;
        //readonly List<Spell> _startingSpells = new List<Spell>();
        readonly Attacker _attacker;
        readonly SpellMaker _spellMaker;
        readonly Sound _attackSound;
        readonly SoundData _attackSoundData;
        readonly int _maxHealth;
        int _health;
        SpellSlot _slot;
        
        public Player(TiledObject serializedData) : base("barry.png",7,1, serializedData)
        {
            _health = serializedData.GetIntProperty("Health");
            _maxHealth = serializedData.GetIntProperty("MaxHealth");
            if(_maxHealth < _health)
            {
                _maxHealth = _health;
                Console.WriteLine("WARNING ! Max health can't be smaller value than Health");
            }

            var damage = serializedData.GetIntProperty("Damage");
            var critMult = serializedData.GetFloatProperty("CritMult");
            var visualStayTime = serializedData.GetFloatProperty("VisualStayTime");
            var coolDown = serializedData.GetIntProperty("CoolDown");
            _attackSound = new Sound(serializedData.GetStringProperty("AttackSound","transmit.wav"));
            _attackSoundData = new SoundData(
                serializedData.GetFloatProperty("AttackVolume",1), 
                serializedData.GetFloatProperty("AttackPan",0), 
                serializedData.GetFloatProperty("AttackFrequency", 44100)
            );
            var (normalSprite, critSprite) = InitAttackerSprites();
            _attacker = new Attacker(damage, critMult, this, normalSprite, critSprite, visualStayTime, coolDown);

            AddChild( _attacker );
            AddChild( new PickUper(this) );
            AddChild( new PlayerAnimationManager(this) );
            var myGame = MyUtils.MyGame;
            PlayerGameData = myGame.Data["PlayerConfig1"] as PlayerData;
            _spellMaker = PlayerGameData.SpellMaker;
            _startingMoney = PlayerGameData.Money;
            _spellMaker.CacheCurrentSpells();
            alpha = 0;

            _spellMaker.CreatedSpell += OnSpellCreate;
            Attacker.Attacked += PlayAttackSound;
            myGame.CurrentLevel.LevelStarted += Init;
            myGame.BeforeLevelReload += ResetLocalData;
            //checking collider bounds
            //var extents = GetExtents();
            //EasyDraw colDebug = new EasyDraw(Mathf.Floor(extents[1].x - extents[0].x), Mathf.Floor(extents[2].x - extents[0].x), false);
            //colDebug.SetCenterOrigin();
            //AddChild(colDebug);
            //colDebug.Clear(255);
            
        }
        protected override void EnterIdle()
        {
            base.EnterIdle();
        }
        private void ResetLocalData()
        {
            PlayerGameData.Money = _startingMoney;
            _spellMaker.ResetToCachedSpells();
        }

        void OnSpellCreate(Spell spell)
        {
            _slot.EquipedSpell = spell;
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
        void Init(Level level)
        {
            _slot = ((MyGame)MyGame.main).FindObjectOfType<SpellSlot>();
            SpellMaker.Init();
        }
        protected override void OnUpdate()
        {
            if(Input.GetKeyDown(Key.SPACE)) 
            {
                _attacker.Perform();
                
            }
            SpellMaker.Update();
        }
        void PlayAttackSound()
        {
            _attackSound.Play(volume: _attackSoundData.volume);
        }
        void ShakeCamera()//find a way so designers can change values
        {

        }
        protected override void OnDestroy()
        {
            MyUtils.MyGame.CurrentLevel.LevelStarted -= Init;
            Attacker.Attacked -= PlayAttackSound;
            MyUtils.MyGame.BeforeLevelReload -= ResetLocalData;

        }
        private void HandleDeath()
        {
            MyUtils.MyGame.ReloadLevel();
        }
        #region Old Movement Code
        //public Player(TiledObject obj = null) : base("barry.png",7,1, walkStep:20,beatsForCompletion: 1,jumpLag: 0.01f, jumpHeight: 3f, maxFallSpeed: .1f, fallAccelerationBuildUpSpeed: 1  )
        //{

        //    States = new Dictionary<Type, MovementState>()
        //    {
        //        { typeof(Idle), new Idle(this) },
        //        { typeof(Walk), new Walk(this) },
        //        { typeof(Jump), new Jump(this) },
        //        { typeof(Fall), new Fall(this) }
        //    };
        //    _transitioning = true;
        //    CurrentState = States[typeof(Idle)];
        //    CurrentState.OnEnter();
        //    _transitioning = false;
        //}

        //protected override void OnUpdate()
        //{
        //    if (_transitioning) return;
        //    float x = 0, y = 0;

        //    if (Input.GetKey(Key.D))
        //        x = 1;
        //    else if (Input.GetKey(Key.A))
        //        x = -1;
        //    if (Input.GetKey(Key.W))
        //        y = 1;
        //    else if (Input.GetKey(Key.S))
        //        y = -1;
        //    DirectionInput = new Vector2(x, y);

        //}
        #endregion
    }


}
