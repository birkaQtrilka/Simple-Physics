using GXPEngine;

namespace gxpengine_template
{
    class WaitForSeconds : ICoroutineStepper
    {
        readonly float _endWaitTimeMs;

        public WaitForSeconds(float pTimeSeconds)
        {
            _endWaitTimeMs = Time.time + pTimeSeconds * 1000;
        }

        public bool IsDone()
        {
            return Time.time >= _endWaitTimeMs;
        }
    }
}
