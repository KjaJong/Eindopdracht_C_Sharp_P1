using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ChoHan
{
    public class Log
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

        //Adds a log entry to the log. The second is for the server.
        public void AddLogEntry(string playerName, string playerAction)
        {
            _dataLog.Add(new LogEntry()
            {
                TimeStamp = DateTime.Today + "_" + DateTime.Now,
                PlayerName = playerName,
                PlayerAction = playerAction + "\n"
            });
        }

        public void AddLogEntry(string playerAction)
        {
            _dataLog.Add(new LogEntry()
            {
                TimeStamp = DateTime.Today + "_" + DateTime.Now,
                PlayerName = "SERVER",
                PlayerAction = playerAction + "\n"
            });
        }

        //Prints the log to a file. Uses a filename dependent on the given name of the log. The if-else automaticly decides to append or create a newe file.
        public void PrintLog()
        {
            //TODO something should start double logging here i think
            //TODO softcode the filepath (solved?)
            string filepath1 = Path.Combine(Environment.CurrentDirectory, @"LogFolder");
            string fileName = Path.Combine(_logName, getMagicNumber(filepath1).ToString(), ".txt");
            fileName = ToSafeFileName(fileName);
            string filepath2 = Path.Combine(filepath1, fileName);
            

            if (!File.Exists(filepath2))
            {
                FileStream logFile = new FileStream(filepath2, FileMode.Create, FileAccess.Write);
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
                FileStream logFile = new FileStream(filepath2, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(logFile);

                foreach (var logEntry in _dataLog)
                {
                    writer.Write(JsonConvert.SerializeObject(logEntry));
                }

                writer.Close();
                logFile.Close();
            }
        }

        private string ToSafeFileName(string s)
        {
            String temp = s;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                temp = temp.Replace(c, '_');
            }
            return temp;
        }

        private int getMagicNumber(string filepath)
        {
            return Directory.GetFiles(filepath, "*", SearchOption.TopDirectoryOnly).Length;
        }
    }
}
