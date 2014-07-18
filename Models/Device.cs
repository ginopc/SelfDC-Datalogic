#define DEVICE

using System;
using System.Collections.Generic;
using System.Text;
using SelfDC.Utils;
using datalogic.datacapture;


namespace SelfDC.Models
{
    class Device
    {
        #region Variables
        public string Version
        {
            get { return "0.0.1.a"; }
        }
        private Laser bcReader; /* riferimento alla classe del device hardware */
        public bool Enabled
        {
            get { return this.bcReader.ScannerEnabled; }
            set { if (value) this.bcReader.ScannerEnabled = value; }
        }
        private string value;
        public string Value
        {
            get 
            {
                if (this.Enabled)
                {
                    this.value = bcReader.BarcodeDataAsText;
                    return this.value;
                }
                else
                    return string.Empty;
             }
        }
        #endregion

        /*********** Mermbi della classe ***********************/
        #region Members
        public Device()
        {
            ScsUtils.WriteLog("Creazione della classe " + this.GetType().ToString());
            bcReader = new Laser();
            bcReader.GoodReadEvent += this.DLScan;
            bcReader.EngineStartedEvent += this.EngineStarted;
            bcReader.EngineStoppedEvent += this.EngineStopped;
        } 

        /** abilita lo scanner barcode */ 
        private void enableScanner()
        {
            ScsUtils.WriteLog("Abilita scanner su " + this.GetType());
            bcReader.ScannerEnabled = true;
        }

        /** disabilita lo scanner barcode */
        private void disableScanner()
        {
            bcReader.ScannerEnabled = false;
        }
        #endregion

        #region Events
        public event EventHandler OnScan;
        public virtual void ScanEvent(EventArgs ev)
        {
            ScsUtils.WriteLog("ScanEvent su " + this.GetType().ToString());
            if (this.OnScan != null)
                OnScan(this, ev);
        }

        public event EventHandler OnStatus;
        public virtual void StatusEvent(EventArgs ev)
        {
            ScsUtils.WriteLog("StatusEvent su " + this.GetType().ToString());
            if (OnStatus != null)
                OnStatus(this, ev);
        }

        public virtual void DLScan(datalogic.datacapture.ScannerEngine device)
        {
            ScsUtils.WriteLog("DLScan su " + this.GetType().ToString());
            this.value = device.BarcodeDataAsText;
            ScanEvent(EventArgs.Empty);
        }

        public event EventHandler OnEngineStart;
        public virtual void EngineStarted(datalogic.datacapture.ScannerEngine sender)
        {
            if (OnEngineStart != null)
                OnEngineStart(sender, EventArgs.Empty);
        }

        public event EventHandler OnEngineStop;
        public virtual void EngineStopped(datalogic.datacapture.ScannerEngine sender)
        {
            if (OnEngineStop != null)
                OnEngineStop(sender, EventArgs.Empty);
        }
        #endregion
    }
}
