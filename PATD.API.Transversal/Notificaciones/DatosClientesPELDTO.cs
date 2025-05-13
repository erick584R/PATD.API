using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Notificaciones
{
    public class DatosClientesPELDTO
    {
        public string vDIRECCIONPERSONAL { get; set; }
        public string vBLOQUESECTORPERSONAL { get; set; }
        public string vPUNTOREFPERSONAL { get; set; }
        public int vIDDIRECPERSONAL { get; set; }
        public int vTIPODIRPERSONAL { get; set; }
        public string vDIRECCIONLABORAL { get; set; }
        public string vBLOQUESECTORLABORAL { get; set; }
        public string vPUNTOREFLABORAL { get; set; }
        public int vIDDIRECLABORAL { get; set; }
        public int vTIPODIRLABORAL { get; set; }
        public string vCELULAR { get; set; }
        public string VFIJOPER { get; set; }
        public string VFIJOLAB { get; set; }
        public string vCORREO { get; set; }
        public string vESTADO { get; set; }
    }
}
