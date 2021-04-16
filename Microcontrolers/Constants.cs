using System.IO.Ports;

namespace EMP.Microcontrolers
{
    public static class Constants
    {
        public static class DataPackage
        {
            public static class ControlCharacter
            {
                public const string Nul = "\u0000";
                public const string Soh = "\u0001";
                public const string Stx = "\u0002";
                public const string Etx = "\u0003";
                public const string Eot = "\u0004";
                public const string Is1 = "\u001F";
                public const string Is2 = "\u001E";
            }
            public static class Index
            {
                // DataPackage class constants
                public const int DataPackageFields = 5;
                public const int InstanceIndex = 0;
                public const int ModuleIndex = 1;
                public const int CommandIndex = 2;
                public const int AttributeIndex = 3;
                public const int DataIndex = 4;
            }
        }
        public static class SerialPort
        {
            // SerialPort class constants
            public const int DefReadBufferSize = 1024;
            public const int DefWriteBufferSize = 1024;
            public const int DefBaudRate = 9600;
            public const Parity DefParity = Parity.None;
            public const int DefDataBits = 8;
            public const StopBits DefStopBits = StopBits.One;
        }
    }
}
