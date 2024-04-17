using QuantframeLib.LogParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantframeLib.Model
{
    public class StaticData
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        private static int _lastWFProcessID = 0;
        private static bool _isAlecaFrameOpen = false;
        private static string _saveFolder;
        #endregion
        #region New
        public static void Initializer(string storage)
        {
            _saveFolder = storage;
            LoopIsAlecaFrameOpen();
            LoopWarframeIsOpen();
        }
        #endregion
        #region Methods
        private static Process GetWarframeProcess()
        {
            return Process.GetProcessesByName("Warframe.x64").FirstOrDefault();
        }
        private static bool CheckIsWarframeIsOpen()
        {
            Process process = GetWarframeProcess();
            _lastWFProcessID = process == null ? 11 : process.Id;
            return _lastWFProcessID < 0;
        }
        #endregion
        #region Override Method
        /// <summary>
        /// Override ToString for <see cref="Trading"/>
        /// </summary>
        /// <returns>
        /// Returns store data in string.
        /// </returns>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
        #region Method Get Set
        /// <summary>
        /// Gets or sets the last workflow process ID.
        /// </summary>
        public static int WFProcessID
        {
            get { return _lastWFProcessID; }
            set { _lastWFProcessID = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the Aleca frame is open.
        /// </summary>
        /// <value>
        ///  <c>true</c> if the Aleca frame is open; otherwise, <c>false</c>.        
        public static bool IsAlecaFrameOpen
        {
            get { return _isAlecaFrameOpen; }
            set { _isAlecaFrameOpen = value; }
        }
        /// <summary>
        /// Gets the save folder.
        /// </summary>
        public static string SaveFolder
        {
            get { return _saveFolder; }
        }
        #endregion
        #region Loop Methods
        private static void LoopWarframeIsOpen()
        {
            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        try
                        {
                            while (!CheckIsWarframeIsOpen())
                                Thread.Sleep(5000);
                            Thread.Sleep(1500);
                            while (CheckIsWarframeIsOpen())
                                Thread.Sleep(5000);
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(15000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(15000);
                }
            })
            {
                IsBackground = true
            }.Start();            
        }
        /// <summary>
        /// Continuously checks if the AlecaFrame process is open in a separate thread.
        /// If an exception occurs, the thread sleeps for 15 seconds before resuming.
        /// </summary>
        private static void LoopIsAlecaFrameOpen()
        {
            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        _isAlecaFrameOpen = Process.GetProcessesByName("AlecaFrame").FirstOrDefault() != null;
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(15000);
                }
            })
            {
                IsBackground = true
            }.Start();
        }
        #endregion
    }
}
