// using System.Diagnostics;
// using System.Threading;
//
// namespace GothicModComposer.Core.Utils.GothicSpyProcess
// {
//     public class GothicSpyProcessRunner
//     {
//         private const string GothicSpyProcessName = "zSpy";
//         private readonly GothicSpyProcess _gothicSpyProcessInstance;
//         private Thread _thread;
//
//         public GothicSpyProcessRunner() => _gothicSpyProcessInstance = new GothicSpyProcess();
//
//         public void Run()
//         {
//             if (IsZSpyRunning())
//                 return;
//
//             _thread = CreateThread();
//             _thread.Start();
//         }
//
//         public void Subscribe(GothicSpyProcess.ZSpyMessageNotify notify)
//         {
//             _gothicSpyProcessInstance.notifyEvent += notify;
//         }
//
//         public void Unsubscribe(GothicSpyProcess.ZSpyMessageNotify notify)
//         {
//             // ReSharper disable once DelegateSubtraction
//             if (_gothicSpyProcessInstance.notifyEvent != null) _gothicSpyProcessInstance.notifyEvent -= notify;
//         }
//
//         public void Abort()
//         {
//             if (!_thread.IsAlive)
//                 return;
//
//             _gothicSpyProcessInstance.notifyEvent = null;
//             _thread.Interrupt();
//         }
//
//         private Thread CreateThread()
//         {
//             return new Thread(() => { Application.Run(_gothicSpyProcessInstance); })
//             {
//                 IsBackground = true
//             };
//         }
//
//         private static bool IsZSpyRunning() => Process.GetProcessesByName(GothicSpyProcessName).Length != 0;
//     }
// }