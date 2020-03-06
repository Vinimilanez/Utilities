using System;
using System.Diagnostics;


namespace Utilities.Log
{
    public class SystemLog
    {
        /// <summary>
        ///     System log record method;
        ///     Método de registro de log de sistema;
        /// </summary>
        /// <param name="Message">
        ///     Log message;
        ///     Mensagem do log;
        ///     (String)
        /// </param>
        /// <param name="type">
        ///     Log type;
        ///     Tipo do log;
        ///     (System.Diagnostics.EventLogEntryType)
        /// </param>
        public static void WriteLog(String Message, EventLogEntryType type)
        {
            String sSource = Process.GetCurrentProcess().ProcessName;
            String sLog = "Application";
            if (!EventLog.SourceExists(sSource))
            {
                EventLog.CreateEventSource(sSource, sLog);
            }
            EventLog.WriteEntry(sSource, String.Format("{0} >>" +
                " {1}{2}", System.DateTime.Now.ToString(), Message, Environment.NewLine), type);
        }

        /// <summary>
        ///     System log record method (Standard type ERROR);
        ///     Método de registro de log de sistema (Tipo padrão ERRO);
        /// </summary>
        /// <param name="Message">
        ///     Log message;
        ///     Mensagem do log;
        ///     (String)
        /// </param>
        public static void WriteLog(String Message)
        {

            String sSource = Process.GetCurrentProcess().ProcessName;
            String sLog = "Application";
            if (!EventLog.SourceExists(sSource))
            {
                EventLog.CreateEventSource(sSource, sLog);
            }
            EventLog.WriteEntry(sSource, String.Format("{0} >>" +
                " {1}{2}", System.DateTime.Now.ToString(), Message, Environment.NewLine), EventLogEntryType.Error);
        }
    }
}
