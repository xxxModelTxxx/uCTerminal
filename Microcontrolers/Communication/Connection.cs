using System;
using System.IO.Ports;

namespace EMP.Microcontrolers.Communication
{
    public class Connection
    {
        private SerialPort _serialPort;
        private string _readBuffer;

        public Connection()
        {
            _serialPort = new SerialPort();
            ConfigurePort();
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
            _readBuffer = string.Empty;
        }

        public event DataPackageReceivedEventHandler DataPackageReveived;

        public SerialPort Port => _serialPort;

        public void SendDataPackage(DataPackage dataPackage)
        {
            try
            {
                _serialPort.Write(dataPackage.ToString());
            }
            catch
            {
                throw;
            }
        }
        private bool CheckBufferForDataFrameBoundaries()
        {
            if (_readBuffer.Contains(Constants.DataPackage.ControlCharacter.Soh) &&
                _readBuffer.Contains(Constants.DataPackage.ControlCharacter.Eot) &&
                _readBuffer.IndexOf(Constants.DataPackage.ControlCharacter.Soh) < _readBuffer.IndexOf(Constants.DataPackage.ControlCharacter.Eot))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ConfigurePort()
        {
            _serialPort.PortName = GetDefaultPortName();
            _serialPort.BaudRate = Constants.SerialPort.DefBaudRate;
            _serialPort.Parity = Constants.SerialPort.DefParity;
            _serialPort.DataBits = Constants.SerialPort.DefDataBits;
            _serialPort.StopBits = Constants.SerialPort.DefStopBits;
            _serialPort.ReadBufferSize = Constants.SerialPort.DefReadBufferSize;
            _serialPort.WriteBufferSize = Constants.SerialPort.DefWriteBufferSize;
        }
        private void CutBufferToDataFrameHeader()
        {
            _readBuffer = _readBuffer.Remove(0, _readBuffer.IndexOf(Constants.DataPackage.ControlCharacter.Soh));
        }
        private string GetDefaultPortName()
        {
            if (SerialPort.GetPortNames().Length > 0)
            {
                return SerialPort.GetPortNames()[0];
            }
            else
            {
                return null;
            }
        }
        private void OnDataReceived(Object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            _readBuffer += sp.ReadExisting();

            while (CheckBufferForDataFrameBoundaries())
            {
                CutBufferToDataFrameHeader();
                string fr = RemoveDataFrame();
                DataPackageReveived?.Invoke(null, new DataPackageReceivedEventArgs(new DataPackage(fr)));
            }
        }
        private string RemoveDataFrame()
        {
            string s = _readBuffer.Substring(0, _readBuffer.IndexOf(Constants.DataPackage.ControlCharacter.Eot) + 1);
            _readBuffer = _readBuffer.Remove(0, _readBuffer.IndexOf(Constants.DataPackage.ControlCharacter.Eot) + 1);
            return s;
        }
    }
}
