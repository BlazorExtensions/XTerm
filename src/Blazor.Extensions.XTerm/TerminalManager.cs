using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Extensions.XTerm
{
    public class TerminalManager
    {
        private static readonly Dictionary<string, XTerm> Terminals = new Dictionary<string, XTerm>();

        public static void RegisterTerminal(string id, XTerm terminal)
        {
            Terminals[id] = terminal;
        }

        public static void UnregisterTerminal(string id)
        {
            if (Terminals.ContainsKey(id))
            {
                Terminals.Remove(id);
            }
        }

        [JSInvokable]
        public static async Task OnKey(string id, KeyboardEventArgs @event)
        {
            if (Terminals.ContainsKey(id))
            {
                await Terminals[id]?.OnKey.InvokeAsync(@event);
            }
        }

        [JSInvokable]
        public static async Task OnLineFeed(string id)
        {
            if (Terminals.ContainsKey(id))
            {
                await Terminals[id]?.OnLineFeed.InvokeAsync(string.Empty);
            }
        }
    }
}