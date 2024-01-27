
namespace gxpengine_template
{
    public class MoveAct : IAct
    {
        public bool IsInstant { get; }
        readonly int _xDir;
        readonly MovableObject _context;
        public MoveAct(MovableObject context, int xDir, bool instant = false) 
        {
            _xDir = xDir;
            _context = context;
            IsInstant = instant;
        }


        public void Perform()
        {
            _context.DirectionInput.x = _xDir;
        }
    }
}
