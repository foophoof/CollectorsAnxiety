using System;

namespace CollectorsAnxiety.UI; 

public class CommandDispatcher : IDisposable {
    public CommandDispatcher() {
        
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class DalamudCommandAttribute : Attribute {
    public readonly string CommandName;

    public DalamudCommandAttribute(string commandName) {
        this.CommandName = commandName;
    }
}