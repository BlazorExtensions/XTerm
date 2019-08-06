using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Extensions.XTerm
{
    public class XTermComponent : ComponentBase, IDisposable
    {
        private const string NAMESPACE_PREFIX = "BlazorExtensions.XTerm";
        private const string CREATE_ACTION = "CreateTerminal";
        private const string DISPOSE_ACTION = "DisposeTerminal";
        private const string WRITE_ACTION = "Write";
        private const string WRITE_LINE_ACTION = "WriteLine";
        private const string SCROLL_LINES_ACTION = "ScrollLines";

        [Parameter(CaptureUnmatchedValues = true)]
        protected Dictionary<string, object> InputAttributes { get; set; }

        [Parameter]
        internal EventCallback<UIKeyboardEventArgs> OnKey { get; set; }

        [Parameter]
        internal EventCallback OnLineFeed { get; set; }

        [Parameter]
        protected string TerminalId { get; set; }

        [Parameter]
        protected string HelloMessage { get; set; }

        [Parameter]
        public TerminalOptions Options { get; set; }

        protected string DivId { get; set; }
        protected ElementRef _divReference { get; set; }

        [Inject]
        internal IJSRuntime JSRuntime { get; set; }

        private bool _isFirstRender = true;

        protected override void OnParametersSet()
        {
            this.DivId = this.TerminalId;
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (this._isFirstRender)
            {
                if (this.Options == null) this.Options = new TerminalOptions();

                await JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{CREATE_ACTION}", this._divReference, this.Options);
                TerminalManager.RegisterTerminal(this.TerminalId, this);

                if (!string.IsNullOrWhiteSpace(this.HelloMessage))
                {
                    await JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{WRITE_LINE_ACTION}", this._divReference, this.HelloMessage);
                }
                this._isFirstRender = false;
            }
        }

        public async Task Write(string content)
        {
            await JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{WRITE_ACTION}", this._divReference, content);
        }

        public async Task WriteLine(string line = "")
        {
            await JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{WRITE_LINE_ACTION}", this._divReference, line);
        }

        public async Task ScrollLines(int lines = 1)
        {
            await JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{SCROLL_LINES_ACTION}", this._divReference, lines);
        }

        public void Dispose()
        {
            TerminalManager.UnregisterTerminal(this.TerminalId);
            //JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{DISPOSE_ACTION}", this._divReference).GetAwaiter().GetResult();
        }
    }
}