using Mono.Debugging.Client;
using Mono.Debugging.Soft;

namespace Mono.Debugging.VisualStudio
{
    public class SuspendDebuggerSession : SoftDebuggerSession
    {
        protected override BreakEventInfo OnInsertBreakEvent(BreakEvent breakEvent)
        {
            this.VirtualMachine?.Suspend();

            return base.OnInsertBreakEvent(breakEvent);
        }
    }
}
