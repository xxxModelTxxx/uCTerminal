using EMP.Microcontrolers.Hardware.Arduino.ExpansionModules;

namespace EMP.Microcontrolers.Hardware.Arduino
{
    public class ArduinoMainBoard : MainBoard
    {
        public ArduinoMainBoard()
            : base()
        {
            _mainBoardName = Names.Arduino;
        }

        public override bool AttachModule(string moduleName)
        {
            if (_modules.ContainsKey(moduleName))
            {
                return false;
            }
            else
            {
                switch (moduleName)
                {
                    case ExpansionModule.Names.System:
                        _modules.Add(moduleName, new Basic(_connection));
                        return true;
                    case ExpansionModule.Names.Lcd:
                        _modules.Add(moduleName, new Lcd(_connection));
                        return true;
                }
                return true;
            }
        }
        public override bool DetachModule(string moduleName)
        {
            return _modules.Remove(moduleName);
        }
        public override void InitializeBoard()
        { }
    }
}
