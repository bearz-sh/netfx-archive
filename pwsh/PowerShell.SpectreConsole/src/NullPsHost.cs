using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;

namespace Ze.PowerShell.SpectreConsole;

public class NullPsHost : PSHost
{
    public static PSHost Instace { get; } = new NullPsHost();

    public override CultureInfo CurrentCulture => throw new NotImplementedException();

    public override CultureInfo CurrentUICulture => throw new NotImplementedException();

    public override Guid InstanceId { get; } = Guid.Empty;

    public override string Name => "Null";

    public override PSHostUserInterface UI { get; } = new NullPSHostUserInterface();

    public override Version Version { get; } = new Version(0, 0, 0);

    public override void EnterNestedPrompt()
    {
        throw new NotImplementedException();
    }

    public override void ExitNestedPrompt()
    {
        throw new NotImplementedException();
    }

    public override void NotifyBeginApplication()
    {
        throw new NotImplementedException();
    }

    public override void NotifyEndApplication()
    {
        throw new NotImplementedException();
    }

    public override void SetShouldExit(int exitCode)
    {
        throw new NotImplementedException();
    }

    internal class NullPSHostUserInterface : PSHostUserInterface
    {
        public override PSHostRawUserInterface RawUI { get; } = new NullPSHostRawUserInterface();

        public override Dictionary<string, PSObject> Prompt(
            string caption,
            string message,
            Collection<FieldDescription> descriptions)
        {
            throw new NotImplementedException();
        }

        public override int PromptForChoice(
            string caption,
            string message,
            Collection<ChoiceDescription> choices,
            int defaultChoice)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(
            string caption,
            string message,
            string userName,
            string targetName,
            PSCredentialTypes allowedCredentialTypes,
            PSCredentialUIOptions options)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(
            string caption,
            string message,
            string userName,
            string targetName)
        {
            throw new NotImplementedException();
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }

        public override SecureString ReadLineAsSecureString()
        {
            throw new NotImplementedException();
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            throw new NotImplementedException();
        }

        public override void Write(string value)
        {
            throw new NotImplementedException();
        }

        public override void WriteDebugLine(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteErrorLine(string value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string value)
        {
            throw new NotImplementedException();
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            throw new NotImplementedException();
        }

        public override void WriteVerboseLine(string message)
        {
            throw new NotImplementedException();
        }

        public override void WriteWarningLine(string message)
        {
            throw new NotImplementedException();
        }
    }

    internal class NullPSHostRawUserInterface : PSHostRawUserInterface
    {
        public override ConsoleColor BackgroundColor { get; set; }

        public override Size BufferSize { get; set; }

        public override Coordinates CursorPosition { get; set; }

        public override int CursorSize { get; set; }

        public override ConsoleColor ForegroundColor { get; set; }

        public override bool KeyAvailable => false;

        public override Size MaxPhysicalWindowSize { get; } = new Size(80, 100);

        public override Size MaxWindowSize { get; } = new Size(80, 100);

        public override Coordinates WindowPosition { get; set; } = new Coordinates(0, 0);

        public override Size WindowSize { get; set; } = new Size(80, 100);

        public override string WindowTitle { get; set; } = "Null Window";

        public override void FlushInputBuffer()
        {
            throw new NotImplementedException();
        }

        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException();
        }

        public override void ScrollBufferContents(
            Rectangle source,
            Coordinates destination,
            Rectangle clip,
            BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(
            Coordinates origin,
            BufferCell[,] contents)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            throw new NotImplementedException();
        }
    }
}