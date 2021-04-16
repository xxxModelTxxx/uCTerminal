using EMP.Microcontrolers.Communication;
using System;

namespace EMP.Microcontrolers.Hardware.Arduino.ExpansionModules
{
    public class Lcd : ExpansionModule
    {
        public Lcd(Connection connection) :
            base(connection)
        {
            _moduleName = Names.Lcd;
        }

        public void AutoScroll()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void NoAutoScroll()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Begin(int cols, int rows)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                cols.ToString() + Constants.DataPackage.ControlCharacter.Is2 + rows.ToString());
            _connection.SendDataPackage(dp);
        }
        public void Blink()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void NoBlink()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Clear()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Cursor()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void NoCursor()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Display()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void NoDisplay()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Home()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void Print(string data)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                data);
            _connection.SendDataPackage(dp);
        }
        public void ScrollDisplayLeft()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void ScrollDisplayRight()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void LeftToRight()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void RightToLeft()
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                Constants.DataPackage.ControlCharacter.Nul);
            _connection.SendDataPackage(dp);
        }
        public void SetCursor(int col, int row)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                col.ToString() + Constants.DataPackage.ControlCharacter.Is2 + row.ToString());
            _connection.SendDataPackage(dp);
        }
        public void Write(byte data)
        {
            DataPackage dp = new DataPackage(
                0,
                GetModuleName(),
                GetMethodName(),
                Constants.DataPackage.ControlCharacter.Nul,
                data.ToString());
            _connection.SendDataPackage(dp);
        }
    }
}
