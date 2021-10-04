using BH.Adapter.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.OpenStreetMap
{
    public partial class OpenStreetMapAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public OpenStreetMapAdapter()
        {
            m_HTTPAdapter = new HTTPAdapter();
        }

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        private HTTPAdapter m_HTTPAdapter = null;

        /***************************************************/


    }
}
