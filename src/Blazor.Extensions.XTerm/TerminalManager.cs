using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Extensions.XTerm
{
    public class TerminalManager
    {
        private static Dictionary<string, XTermComponent> _terminals = new Dictionary<string, XTermComponent>();

        public static void RegisterTerminal(string id, XTermComponent terminal)
        {
            _terminals[id] = terminal;
        }

        public static void UnregisterTerminal(string id)
        {
            if (_terminals.ContainsKey(id))
            {
                _terminals.Remove(id);
            }
        }

        [JSInvokable]
        public static async Task OnKey(string id, UIKeyboardEventArgs @event)
        {
            if (_terminals.ContainsKey(id))
            {
                await _terminals[id]?.OnKey.InvokeAsync(@event);
            }
        }

        [JSInvokable]
        public static async Task OnLineFeed(string id)
        {
            if (_terminals.ContainsKey(id))
            {
                await _terminals[id]?.OnLineFeed.InvokeAsync(string.Empty);
            }
        }
    }
}