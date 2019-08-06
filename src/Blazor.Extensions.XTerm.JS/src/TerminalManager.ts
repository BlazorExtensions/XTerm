import { Terminal, ITerminalOptions } from 'xterm';
import { WebLinksAddon } from 'xterm-addon-web-links';
import { FitAddon } from 'xterm-addon-fit';
import { SearchAddon } from 'xterm-addon-search';

declare var DotNet: any;

export class TerminalManager {
  private readonly _terminals = new Map<string, Terminal>();

  public CreateTerminal = (ref: HTMLElement, options?: ITerminalOptions) => {
    if (!this._terminals.has(ref.id)) {
      const terminal = new Terminal(options);
      terminal.loadAddon(new WebLinksAddon());
      let fitAddon = new FitAddon();
      terminal.loadAddon(fitAddon);
      terminal.loadAddon(new SearchAddon());
      terminal.onKey(async (e: { key: string, domEvent: KeyboardEvent }) => {
        console.log(e.domEvent);
        await DotNet.invokeMethodAsync("Blazor.Extensions.XTerm", "OnKey", ref.id,
          {
            key: e.domEvent.key,
            code: e.domEvent.keyCode.toString(),
            location: e.domEvent.location,
            repeat: e.domEvent.repeat,
            ctrlKey: e.domEvent.ctrlKey,
            shiftKey: e.domEvent.shiftKey,
            altKey: e.domEvent.altKey,
            metaKey: e.domEvent.metaKey
          });
      });
      terminal.onLineFeed(async () => {
        await DotNet.invokeMethodAsync("Blazor.Extensions.XTerm", "OnLineFeed", ref.id);
      });
      terminal.open(ref);
      terminal.focus();

      this._terminals.set(ref.id, terminal);
    }
  }

  public DisposeTerminal = (ref: HTMLElement) => {
    const terminal = this._terminals.get(ref.id);
    if (terminal) {
      terminal.dispose();
      this._terminals.delete(ref.id);
    }
  }

  public Write = (ref: HTMLElement, content: string) => {
    const terminal = this._terminals.get(ref.id);
    if (terminal) {
      terminal.write(content);
    } else {
      throw new Error("Invalid terminal id.");
    }
  }

  public WriteLine = (ref: HTMLElement, line: string) => {
    const terminal = this._terminals.get(ref.id);
    if (terminal) {
      terminal.writeln(line);
    } else {
      throw new Error("Invalid terminal id.");
    }
  }

  public ScrollLines = (ref: HTMLElement, lines: number) => {
    const terminal = this._terminals.get(ref.id);
    if (terminal) {
      terminal.scrollLines(lines);
    } else {
      throw new Error("Invalid terminal id.");
    }
  }
}
