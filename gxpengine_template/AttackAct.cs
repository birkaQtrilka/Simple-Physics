namespace gxpengine_template
{
    public class AttackAct : IAct
    {
        public bool IsInstant { get; }
        readonly bool _oppositeWay;
        readonly Attacker _attacker;
        public AttackAct(Attacker attacker, bool oppositeWay = false, bool instant = false) 
        {
            _attacker = attacker;
            IsInstant = instant;
            _oppositeWay = oppositeWay;
        }

        public void Perform()
        {
            _attacker.Perform(_oppositeWay);

        }
    }
}
