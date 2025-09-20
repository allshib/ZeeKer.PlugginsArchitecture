using ZeeKer.Orion.Services;

namespace ZeeKer.Orion.Infrastructure.Services;

internal class WriteTextService : IWriteTextService
{
    public string WriteText()
    => "Hello from WriteTextService in Infrastructure layer";
}

