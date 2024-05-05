using QuantframeLib.LogParser.Events.Blessing;
using QuantframeLib.LogParser.Events.Conversation;
using QuantframeLib.LogParser.Events.Trade;
using QuantframeLib.Model;
using QuantframeLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuantframeLib.LogParser
{
    public class LogParserClient
    {
        #region Const/Static Values
        #endregion
        #region Private Values
        private static long lastPosition = 0;
        private static bool _coldStart = true;
        private static string _lastReadMethod = "";
        #endregion
        #region New
        public static void Initializer()
        {
            LoopReadEELog();
        }
        #endregion
        #region Method

        private static void ProcessLine(string line)
        {
            if (OnTradeEvent.ProcessLine(line))
                return;
            if (OnConversationEvent.ProcessLine(line))
                return;
            if (OnBlessingEvent.ProcessLine(line))
                return;
        }
        #endregion
        #region Loops
        private static void LoopReadEELog()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        string mode = "";
                        //if (StaticData.WFProcessID == -1)
                        //    continue;
                        if (StaticData.IsAlecaFrameOpen)
                        {
                            mode = "file";
                            string eeLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Warframe\EE.log");
                            DateTime lastCheckedTime = DateTime.MinValue; // Initialize to a default value
                            if (!File.Exists(eeLogPath))
                                continue;
                            DateTime lastModifiedTime = File.GetLastWriteTime(eeLogPath);
                            if (lastModifiedTime > lastCheckedTime)
                            {
                                lastCheckedTime = lastModifiedTime;

                                // Get the length of the file
                                long fileLength = new FileInfo(eeLogPath).Length;
                                if (fileLength <= 0)
                                    lastPosition = 0;
                                // Open the file and move to the last position read
                                using (FileStream fs = new FileStream(eeLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    // Set the file pointer to the last position
                                    fs.Seek(lastPosition, SeekOrigin.Begin);

                                    // Read new lines
                                    if (!_coldStart)
                                        using (StreamReader reader = new StreamReader(fs))
                                        {
                                            string line;
                                            while ((line = reader.ReadLine()) != null)
                                                ProcessLine(line);
                                        }
                                    _coldStart = false;
                                    lastPosition = fileLength;
                                }
                            }
                        }
                        else
                        {
                            mode = "memory";
                            using (MemoryMappedFile orOpen = MemoryMappedFile.CreateOrOpen("DBWIN_BUFFER", 4096L))
                            using (EventWaitHandle eventWaitHandle1 = new EventWaitHandle(false, EventResetMode.AutoReset, "DBWIN_BUFFER_READY", out bool createdNew1))
                                try
                                {
                                    using (EventWaitHandle eventWaitHandle2 = new EventWaitHandle(false, EventResetMode.AutoReset, "DBWIN_DATA_READY", out bool createdNew2))
                                    {
                                        char[] chArray = new char[5000];
                                        while (StaticData.WFProcessID != -1 && !StaticData.IsAlecaFrameOpen)
                                        {
                                            eventWaitHandle1.Set();
                                            if (eventWaitHandle2.WaitOne(TimeSpan.FromSeconds(3.0)))
                                            {
                                                using (MemoryMappedViewStream viewStream = orOpen.CreateViewStream())
                                                {
                                                    using (BinaryReader binaryReader = new BinaryReader((Stream)viewStream, Encoding.UTF8))
                                                    {
                                                        if (binaryReader.ReadUInt32() == StaticData.WFProcessID)
                                                        {
                                                            char[] array = binaryReader.ReadChars(4092);
                                                            ProcessLine(new string(array, 0, Array.IndexOf<char>(array, char.MinValue)));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Thread.Sleep(10000);
                                }
                                finally
                                {
                                    eventWaitHandle1.Set();
                                }

                        }
                        if (_lastReadMethod != mode)
                        {
                            _lastReadMethod = mode;
                            Logger.Info("Log:ReadMethod", "Reading EE.log using " + mode);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        Thread.Sleep(10000);
                    }
                }

            })
            {
                IsBackground = true
            }.Start();
        }
        #endregion
        #region Override Method
        /// <summary>
        /// Override ToString for <see cref="LogParserClient"/>
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

        #endregion
        #region Events

        #endregion
    }
}
