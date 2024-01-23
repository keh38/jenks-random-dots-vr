using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRD_Controller
{
    public class VRDControllerSettings
    {
        public string lastConfigFile;
        public string defaultConfigFile;
        public int X;
        public int Y;
        public string vrIPAddress = "127.0.0.1";

        public VRDControllerSettings()
        {
            X = -1;
            Y = -1;
        }

    }
}
