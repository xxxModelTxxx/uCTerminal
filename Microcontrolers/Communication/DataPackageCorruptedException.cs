using System;

namespace EMP.Microcontrolers.Communication
{
    [Serializable()]
    public class DataPackageCorruptedException : Exception
    {
        public DataPackageCorruptedException()
            : base()
        { }
        public DataPackageCorruptedException(string message)
            : base(message)
        { }
    }
}
