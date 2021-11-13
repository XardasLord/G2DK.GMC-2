using System.Runtime.InteropServices;

namespace GothicModComposer.Core.Utils.GothicSpyProcess
{
    public class GothicSpyProcess : Form
    {
        public delegate void ZSpyMessageNotify(string message);

        private const string TargetProcessName = "[zspy]";
        private const int WM_COPYDATA = 0x4A;
        public ZSpyMessageNotify notifyEvent = null;

        public GothicSpyProcess()
        {
            Text = TargetProcessName;
            Opacity = 0.0f;
            ShowInTaskbar = false;
        }
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                var data = (CopyData) m.GetLParam(typeof(CopyData));
                var msg = Marshal.PtrToStringAnsi(data.LpData);
                notifyEvent?.Invoke(msg);
            }

            base.WndProc(ref m);
        }
    }
}