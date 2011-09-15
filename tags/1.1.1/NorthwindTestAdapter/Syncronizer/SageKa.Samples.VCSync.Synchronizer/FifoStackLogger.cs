#region Usings

using System;
using System.Collections.Generic;
using SageKa.Samples.VcSync.SyncEngine;
using SageKa.Samples.VcSync.SyncEngine.Syndications;

#endregion

namespace SageKa.Samples.VCSync.Synchronizer
{
    public class FifoStackLogger : ILogger
    {
        private SyncState _syncState;

        #region Ctor.

        public FifoStackLogger()
        {
            _syncState = new SyncState();
            this.SeverityFilter = Severity.Info;
        }

        #endregion

        #region ILogger Members

        public void Write(string msg, Severity severity)
        {
            if (severity <= this.SeverityFilter)
            {
                lock (_syncState)
                {
                    _syncState.Messages.Add(msg);
                }
            }
        }
        public void Write(Exception exception)
        {
            if (Severity.Error <= this.SeverityFilter)
            {
                lock (_syncState)
                {
                    _syncState.ErrorMsg = exception.Message;
                    if (this.SeverityFilter >= Severity.Trace)
                        _syncState.ErrorMsg += Environment.NewLine + exception.StackTrace;
                }
            }
        }
        public void Write(Diagnosis diagnosis)
        {
            if (Severity.Error <= this.SeverityFilter)
            {
                lock (_syncState)
                {
                    string diagnosisErrorMsg = "";
                    diagnosisErrorMsg += "Application code: " + diagnosis.ApplicationCode;
                    diagnosisErrorMsg += Environment.NewLine;
                    diagnosisErrorMsg += "Message: " + diagnosis.Message;
                    diagnosisErrorMsg += Environment.NewLine;
                    diagnosisErrorMsg += "PayloadPath: " + diagnosis.PayloadPath;
                    diagnosisErrorMsg += Environment.NewLine;
                    diagnosisErrorMsg += "SdataCode: " + diagnosis.SdataCode.ToString();
                    diagnosisErrorMsg += Environment.NewLine;
                    diagnosisErrorMsg += "Severity: " + diagnosis.Severity.ToString();
                    diagnosisErrorMsg += Environment.NewLine;

                    if (this.SeverityFilter >= Severity.Trace)
                    {
                        diagnosisErrorMsg += "StackTrace: " + diagnosis.StackTrace;
                        diagnosisErrorMsg += Environment.NewLine;
                    }
                    _syncState.ErrorMsg = diagnosisErrorMsg;
                }
            }
        }

        #endregion

        public SyncState SyncState
        {
            get
            {
                lock (_syncState)
                    return _syncState.Clone();
            }
        }

        public Severity SeverityFilter { get; set; }

        
    }

    #region CLASS: SyncState

    public class SyncState : ICloneable
    {
        public SyncState()
        {
            this.Messages = new List<string>();
        }

        public string ErrorMsg { get; set; }
        //public int CurrentStep { get; set; }
        //public int StepsToRun { get; set; }
        public List<string> Messages { get; set; }

        public SyncState Clone()
        {
            SyncState syncState = new SyncState();
            //syncState.CurrentStep = this.CurrentStep;
            syncState.ErrorMsg = this.ErrorMsg;
            syncState.Messages = this.Messages;
            //syncState.StepsToRun = this.StepsToRun;
            return syncState;
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }

    #endregion
}
