using GXPEngine;
using System;
using System.Collections.Generic;

namespace gxpengine_template
{
    public class SpellMaker 
    {
        public SpellMaker(List<Spell> availableSpells)
        {
            _availableSpells = availableSpells;

        }

        readonly List<Spell> _availableSpells = new List<Spell>();
        List<Spell> _cachedSpells = new List<Spell>();
        BeatManager _songManager;
        readonly int[] _myCombo = new int[4];

        bool _wasBeatWithCode;
        bool _wasBeat;
        int _comboIndex = 0;
        int _beatNum = 0;
        int _codeInputBeatNum;

        public event Action<Spell> CreatedSpell;
        public event Action FailCreatedSpell;
        public event Action<int> ComboStep;
        public event Action FailedCombo;
        public void Init()
        {
            _songManager = BeatManager.Instance;
            //_availableSpells.Add( Utils.MyGame.Data["PortalSpell"] as Spell);
            _availableSpells.Add( MyUtils.MyGame.Data["UnlockSpell"] as Spell);
        }
        public bool AddAvailableIfNone(Spell spell)
        {
            //notify player about new combination
            if (_availableSpells.Contains(spell)) return false;

            SpellNotifier.Instance.Notify(spell);
            _availableSpells.Add(spell);
            return true;

        }
        public void CacheCurrentSpells()
        {
            _cachedSpells.Clear();
            foreach(var spell in _availableSpells) 
                _cachedSpells.Add(spell);
        }
        public void ResetToCachedSpells()
        {
            _availableSpells.Clear();
            foreach (var spell in _cachedSpells)
                _availableSpells.Add(spell);
        }
        public void Update()
        {
            int code = 0;
            if (Input.GetKeyUp(Key.ONE))
            {
                code = 1;
            }
            else if (Input.GetKeyUp(Key.TWO))
            {
                code = 2;
            }
            else if (Input.GetKeyUp(Key.THREE))
            {
                code = 3;
            }
            else if (Input.GetKeyUp(Key.FOUR))
            {
                code = 4;
            }
            var onBeat = _songManager.OnBeat;
            if (onBeat && !_wasBeat)
            {
                _wasBeat = true;
                _beatNum++;
            }
            else if (!onBeat)
            {
                _wasBeatWithCode = false;
                _wasBeat = false;
            }

            if (code == 0)
            {
                if (_codeInputBeatNum < _beatNum && !onBeat)
                {
                    ClearCombo();
                    FailedCombo?.Invoke();
                }

                return;
            }

            if (onBeat && !_wasBeatWithCode)
            {
                _wasBeatWithCode = true;
                _myCombo[_comboIndex++] = code;
                _codeInputBeatNum = _beatNum;
                ComboStep?.Invoke(code);

                if (_comboIndex == _myCombo.Length)
                {
                    var spell = CheckCastedSpell();
                    if (spell != null)
                        CreatedSpell?.Invoke(spell);
                    else 
                        FailCreatedSpell?.Invoke();
                    ClearCombo();
                }

            }
            else if (!onBeat)
            {
                ClearCombo();
                FailedCombo?.Invoke();
            }
        }
        void ClearCombo()
        {
            if (_comboIndex == 0) return;
            _comboIndex = 0;
        }
        Spell CheckCastedSpell()
        {
            foreach (var spell in _availableSpells)
            {
                var combination = spell.Combination;
                int i = 0;
                for (; i < combination.Length; i++)
                    if (combination[i] != _myCombo[i])
                        break;

                if (i == 4)
                    return spell;
            }
            return null;
        }
    }
}
