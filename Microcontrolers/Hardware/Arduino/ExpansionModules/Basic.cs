using EMP.Microcontrolers.Communication;
using System;

namespace EMP.Microcontrolers.Hardware.Arduino.ExpansionModules
{
    public class Basic : ExpansionModule
    {
        public Basic(Connection connection) :
            base(connection)
        {
            _moduleName = Names.System;
        }

        public void MemmoryRead(Int16 memoryAdress)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                memoryAdress.ToString(),
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void MemmoryWrite(Int16 memoryAdress, Int16 data)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                memoryAdress.ToString(),
                data.ToString());
            _connection.SendDataPackage(dp);
        }
        public void RegisterRead(string registerName)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                registerName,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void RegisterWrite(string registerName, Int16 data)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                registerName,
                data.ToString());
            _connection.SendDataPackage(dp);
        }
        public void ProgramControl(string controlWord, Int16 data)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                controlWord,
                data.ToString());
            _connection.SendDataPackage(dp);
        }
        public void Diagnostics(string controlWord)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                controlWord,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Reset()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Message(string message)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                message);
            _connection.SendDataPackage(dp);
        }
    }
}
