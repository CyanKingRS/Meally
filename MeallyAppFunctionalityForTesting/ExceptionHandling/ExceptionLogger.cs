﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeallyApp.Resources.ExceptionHandling
{
    public delegate void MyEventHandler(string value);

    public class ExceptionLogger : IExceptionLogger
    {
        private readonly string logFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Log.txt");

        // 5. Custom event 
        public event MyEventHandler? exceptionAddedToFile;

        public void ClearLog()
        {
            File.WriteAllText(logFileName, string.Empty);
        }

        public void WriteToLog(string message)
        {
            File.AppendAllText(logFileName, message);
        }

        public string? ReadFromLog()
        {
            if (File.Exists(logFileName))
            {
                System.Diagnostics.Debug.WriteLine("All exceptions during current session: ");

                string buffer = File.ReadAllText(logFileName);
                return buffer;
            }

            return null;
        }
    }
}
