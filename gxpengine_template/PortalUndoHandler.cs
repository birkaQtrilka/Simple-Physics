using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
namespace gxpengine_template
{
    public class PortalUndoHandler : GameObject
    {
        public List<Portal> PlacedPortals = new List<Portal>();

        void Update()
        {
            var portalsCount = PlacedPortals.Count;
            if (Input.GetKeyDown(Key.E) && portalsCount > 0)
            {
                PlacedPortals[portalsCount - 1].LateDestroy();
            }
        }
    }
}
