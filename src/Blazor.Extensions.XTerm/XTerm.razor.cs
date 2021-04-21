using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Extensions.XTerm
{
    public partial class XTerm : ComponentBase, IDisposable
    {
        private const string NAMESPACE_PREFIX = "BlazorExtensions.XTerm";
        private const string CREATE_ACTION = "CreateTerminal";
        private const string DISPOSE_ACTION = "DisposeTerminal";
        private const string WRITE_ACTION = "Write";
        private const string WRITE_LINE_ACTION = "WriteLine";
        private const string SCROLL_LINES_ACTION = "ScrollLines";

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; }

        [Parameter] public EventCallback<KeyboardEventArgs> OnKey { get; set; }

        [Parameter] public EventCallback OnLineFeed { get; set; }

        [Parameter] public string TerminalId { get; set; }

        [Parameter] public string HelloMessage { get; set; }

        [Parameter] public TerminalOptions Options { get; set; }

        protected string DivId { get; set; }
        public ElementReference DivReference { get; set; }

        [Inject] internal IJSRuntime JSRuntime { get; set; }

        protected override void OnParametersSet()
        {
            this.DivId = this.TerminalId;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (this.Options == null) this.Options = new TerminalOptions();

                await this.JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{CREATE_ACTION}", this.DivReference,
                    this.Options);
                TerminalManager.RegisterTerminal(this.TerminalId, this);

                if (!string.IsNullOrWhiteSpace(this.HelloMessage))
                {
                    await this.JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{WRITE_LINE_ACTION}",
                        this.DivReference, this.HelloMessage);
                }
            }
        }

        public async Task Write(string content)
        {
            await this.JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{WRITE_ACTION}", this.DivReference, content);
        }

        public async Task WriteLine(string line = "")
        {
            await this.JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{WRITE_LINE_ACTION}", this.DivReference,
                line);
        }

        public async Task ScrollLines(int lines = 1)
        {
            await this.JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{SCROLL_LINES_ACTION}", this.DivReference,
                lines);
        }

        public void Dispose()
        {
            TerminalManager.UnregisterTerminal(this.TerminalId);
            //JSRuntime.InvokeAsync<object>($"{NAMESPACE_PREFIX}.{DISPOSE_ACTION}", this._divReference).GetAwaiter().GetResult();
        }
    }
}