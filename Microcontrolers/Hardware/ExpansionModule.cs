using EMP.Microcontrolers.Communication;
using System.Diagnostics;

namespace EMP.Microcontrolers.Hardware
{
    public abstract class ExpansionModule
    {
        protected Connection _connection;
        protected string _moduleName;

        protected ExpansionModule(Connection connection)
        {
            _connection = connection;
        }

        public string Name => _moduleName;

        protected string GetModuleName()
        {
            return _moduleName.ToUpper();
        }
        protected string GetMethodName()
        {
            return (new StackTrace()).GetFrame(1).GetMethod().Name.ToUpper();
        }

        public static class Names
        {
            public const string System = "System";
            public const string Lcd = "LCD";
        }
    }
}
