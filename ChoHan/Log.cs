using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace ChoHan
{
    class Log
    {
        //The struct for a log entry. Using a struct here because an entry (once made) will never change
        public struct LogEntry
        {
            public string TimeStamp;
            public string PlayerName;
            public string PlayerAction;
        }

        private readonly List<LogEntry> _dataLog = new List<LogEntry>();
        private readonly string _logName;

        public Log(string logName)
        {
            _logName = logName;
        }

        //Adds a log entry to the log.
        public void AddLogEntry(string playerName, string playerAction)
        {
            _dataLog.Add(new LogEntry()
            {
                TimeStamp = DateTime.Today + "/" + DateTime.Now,
                PlayerName = playerName,
                PlayerAction = playerAction
            });
        }

        //Prints the log to a file. Uses a filename dependent on the given name of the log. The if-else automaticly decides to append or create a newe file.
        public void PrintLog()
        {
            string filepath = ($@"..\..\LogMap/LogFile{_logName}");

            if (!File.Exists(filepath))
            {
                FileStream logFile = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(logFile);

                foreach (var logEntry in _dataLog)
                {
                    writer.Write(JsonConvert.SerializeObject(logEntry));
                }

                writer.Close();
                logFile.Close();
            }
            else
            {
                FileStream logFile = new FileStream(filepath, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(logFile);

                foreach (var logEntry in _dataLog)
                {
                    writer.Write(JsonConvert.SerializeObject(logEntry));
                }

                writer.Close();
                logFile.Close();
            }
        }
    }
}
