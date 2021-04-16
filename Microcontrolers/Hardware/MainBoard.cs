using EMP.Microcontrolers.Communication;
using System;
using System.Collections.Generic;

namespace EMP.Microcontrolers.Hardware
{
    public abstract class MainBoard
    {
        protected Connection _connection;
        protected string _mainBoardName;
        protected Dictionary<string, ExpansionModule> _modules;

        protected MainBoard()
        {
            _connection = new Connection();
            _modules = new Dictionary<string, ExpansionModule>();
            InitializeBoard();
        }

        public event DataPackageReceivedEventHandler DataPackageReceieved;

        public bool IsConnected => _connection.Port.IsOpen;
        public string Name => _mainBoardName;

        public abstract bool AttachModule(string moduleName);
        public abstract bool DetachModule(string moduleName);
        public abstract void InitializeBoard();
        public bool Connect()
        {
            if (!IsConnected)
            {
                _connection.Port.Open();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Dissconect()
        {
            if (IsConnected)
            {
                _connection.Port.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
        public ExpansionModule GetModule(string moduleName)
        {
            try
            {
                return _modules[moduleName];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        public bool SetPort(string portName)
        {
            if (!IsConnected)
            {
                _connection.Port.PortName = portName;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDataPackageReceived(Object sender, DataPackageReceivedEventArgs e)
        {
            DataPackageReceieved?.Invoke(this, e);
        }

        public static class Names
        {
            public const string Arduino = "Arduino";
            public const string RaspberryPi = "Raspberry Pi";
        }
    }
}
