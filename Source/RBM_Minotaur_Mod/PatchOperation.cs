using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Verse;

namespace RBM_Minotaur
{
    public class PatchOperationRockChunk : PatchOperation
    {
        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (this.match != null)
            {
                if (MinotaurSettings.disableChunks == false)
                {
                    Log.Message("RBM: Applied patch");
                    return this.match.Apply(xml);
                }
                Log.Message("RBM: Did not apply patch");
                return true;
            }
            Log.Message("RBM: match was null.");
            return false;
        }
        private PatchOperation match;
    }

}
