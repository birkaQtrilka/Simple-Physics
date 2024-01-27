using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;

namespace gxpengine_template
{
    public static class EaseFunc
    {
        public static float EaseOutSin(float t)=> Mathf.Sin((t * Mathf.PI) / 2);

    }
}
