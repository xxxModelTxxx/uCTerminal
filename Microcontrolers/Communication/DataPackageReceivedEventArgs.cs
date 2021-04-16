using System;

namespace EMP.Microcontrolers.Communication
{
    public class DataPackageReceivedEventArgs : EventArgs
    {
        public DataPackage GetDataPackage { get; }

        public DataPackageReceivedEventArgs(DataPackage dp)
        {
            GetDataPackage = dp;
        }
    }
}
