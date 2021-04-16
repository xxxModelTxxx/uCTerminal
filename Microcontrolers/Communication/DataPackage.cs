using System;
using System.Collections.Generic;
using System.Linq;

namespace EMP.Microcontrolers.Communication
{
    /// <summary>
    /// DataPackage class represents single data frame of transmission protocol used for communication between PC and Arduino.
    /// DataPackage class consists of several main data carring fields.
    /// Main fields include:
    ///     Instance   
    ///         Unique command id. Used for identifing data packages refering to single command which has to be transmitted with more than one data package.
    ///         Used in cases when data is transmitted in two or more data packages which need to be processed toogather after trasmission is complete.
    ///         In case if data is transmitted in single data package it is recommended to set this property to 0 during construction of data package, otherwise use of random number shall be considered.
    ///     Module 
    ///         Name of data package target or source module.
    ///     Command   
    ///         Name of command or type of data transffered in data block.
    ///     Attribute
    ///         Description or additional identification information of data transferred in data block (i.e. register name, memmory adress, multi-package transmission information)
    ///     Data
    ///         Data block. Data block may consist sub-blocks.
    /// Complete frame consists of additional control codes which mark beginning end end of data package frame, and separate main fields within frame. 
    /// All fields are transferred in ASCII format. This includes text as well as numeric values (both fixed and floating point). It shall be noted that DataPackage class utilizes UNICODE encoding while coding and decoding data. It operates exclusevly at C0 Controls and Basic Latin subset of BMP.
    /// Structure of data package frame as transmitted is specified below. 
    /// [001] SOH           -   Start Of Heading
    /// [002] STX           -   Start of TeXt
    /// [003] Instance      -   Command instance No. field
    /// [004] IS1           -   Information Separator
    /// [005] Module        -   Module field
    /// [006] IS1           -   Information Separator
    /// [007] Command       -   Command field
    /// [008] IS1           -   Information Separator
    /// [009] Attribute     -   Attribute field (NUL if not used)
    /// [010] IS1           -   Information Separator
    /// [011] Data          -   Data block (if data shall be divided into separate sub-block, all sub-blocks shall be separated by IS2 control character)
    /// [012] ETX           -   End of TeXt
    /// [013] EOT           -   End Of Transmission    
    /// </summary>
    public class DataPackage
    {
        private Int16 _instance;
        private string _module;
        private string _command;
        private string _attribute;
        private string[] _data;

        public Int16 Instance => _instance;
        public string Module => _module;
        public string Command => _command;
        public string Attribute => _attribute;
        public string[] Data => _data;

        public DataPackage(string dataFrame)
        {
            CheckDataFrameHeader(dataFrame);
            CheckDataFrameFooter(dataFrame);
            CheckDataFrameFields(dataFrame);
            string[] fields = GetDataFields(dataFrame);
            ValidateDataFields(fields);
            SetDataPackageFields(fields);
        }
        public DataPackage(Int16 instance, string module, string command, string attribute, string[] data)
            : this(instance, module, command, attribute)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data Package null data argument.");
            }
            _data = data;
        }
        public DataPackage(Int16 instance, string module, string command, string attribute, string data)
            : this(instance, module, command, attribute)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data Package null data argument.");
            }
            _data = new string[] { data };
        }
        public DataPackage(Int16 instance, string module, string command, string attribute, char data)
            : this(instance, module, command, attribute)
        {
            _data = new string[] { data.ToString() };
        }
        private DataPackage(Int16 instance, string module, string command, string attribute)
        {
            _instance = instance;
            _module = module ?? throw new ArgumentNullException("Data Package null module argument.");
            _command = command ?? throw new ArgumentNullException("Data Package null command argument.");
            _attribute = attribute ?? throw new ArgumentNullException("Data Package null attribute argument");
        }

        public override string ToString()
        {
            List<string> frame = new List<string>
            {
                Constants.DataPackage.ControlCharacter.Soh,
                Constants.DataPackage.ControlCharacter.Stx,
                _instance.ToString(),
                Constants.DataPackage.ControlCharacter.Is1,
                _module,
                Constants.DataPackage.ControlCharacter.Is1,
                _command,
                Constants.DataPackage.ControlCharacter.Is1,
                _attribute,
                Constants.DataPackage.ControlCharacter.Is1,
                string.Join(Constants.DataPackage.ControlCharacter.Is2, _data),
                Constants.DataPackage.ControlCharacter.Etx,
                Constants.DataPackage.ControlCharacter.Eot
            };
            return string.Join(string.Empty, frame.ToArray());
        }
        private void CheckDataFrameFields(string dataFrame)
        {
            if (dataFrame.ToCharArray().Count(c => c == Constants.DataPackage.ControlCharacter.Is1.ToCharArray()[0]) != Constants.DataPackage.Index.DataPackageFields - 1)
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Control code error --- Inccorect no. of data package fields (IS1 control code count).");
            }
        }
        private void CheckDataFrameFooter(string dataFrame)
        {
            if (!dataFrame.Contains(Constants.DataPackage.ControlCharacter.Etx))
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Control code error --- ETX control code missing in data frame.");
            }
            else if (!dataFrame.Contains(Constants.DataPackage.ControlCharacter.Eot))
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Control code error --- EOT control code missing in data frame.");
            }
        }
        private void CheckDataFrameHeader(string dataFrame)
        {
            if (!dataFrame.Contains(Constants.DataPackage.ControlCharacter.Soh))
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Control code error --- SOH control code missing in data frame.");
            }
            else if (!dataFrame.Contains(Constants.DataPackage.ControlCharacter.Stx))
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Control code error --- STX control code missing in data frame.");
            }
        }
        private string[] GetDataFields(string dataFrame)
        {
            string[] spl = new string[] {
                Constants.DataPackage.ControlCharacter.Soh,
                Constants.DataPackage.ControlCharacter.Stx,
                Constants.DataPackage.ControlCharacter.Is1,
                Constants.DataPackage.ControlCharacter.Etx,
                Constants.DataPackage.ControlCharacter.Eot };
            return dataFrame.Split(spl, StringSplitOptions.RemoveEmptyEntries);
        }
        private void SetDataPackageFields(string[] fields)
        {
            if (!Int16.TryParse(fields[Constants.DataPackage.Index.InstanceIndex], out _instance))
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Instance field corrupted. --- Failed to parse instance field to integer.");
            }
            _module = fields[Constants.DataPackage.Index.ModuleIndex];
            _command = fields[Constants.DataPackage.Index.CommandIndex];
            _attribute = fields[Constants.DataPackage.Index.AttributeIndex];
            _data = fields[Constants.DataPackage.Index.DataIndex].Split(new string[] { Constants.DataPackage.ControlCharacter.Is2 }, StringSplitOptions.None);
        }
        private void ValidateDataFields(string[] fields)
        {
            if (fields.Length != Constants.DataPackage.Index.DataPackageFields)
            {
                throw new DataPackageCorruptedException("Data package corrupted. --- Data corrupted. --- Data field missing.");
            }
        }
    }
}
