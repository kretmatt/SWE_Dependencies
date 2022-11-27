using Microsoft.AspNetCore;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using Xunit;
using Xunit.Abstractions;

namespace ProviderTests;

public class ProviderPactTests
{
    private string _providerUri;
    private string _pactServiceUri;
    private IWebHost _webHost;
    private IOutput output;

    public ProviderPactTests(ITestOutputHelper helper)
    {
        _providerUri = "http://localhost:8180";
        _pactServiceUri = "http://localhost:9003";
        output = new XUnitOutput(helper);
    }

    [Fact]
    public void EnsureProviderHonoursPactWithConsumer1()
    {
        var config = new PactVerifierConfig()
        {
            Outputters = new List<IOutput>()
            {
                output
            }
        };

        IPactVerifier pactVerifier = new PactVerifier(config);
        var pactFile =
            new FileInfo(
                $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName}{Path.DirectorySeparatorChar}pacts/Consumer1-CustomerAPI.json");
        using (var webhost = WebHost.CreateDefaultBuilder()
                   .UseUrls(_pactServiceUri)
                   .UseStartup<TestStartup>()
                   .Build())
        {
            webhost.Start();
            pactVerifier.ServiceProvider("CustomerAPI", new Uri(_providerUri)).WithFileSource(pactFile)
                .WithProviderStateUrl(new Uri($"{_pactServiceUri}/provider-states"))
                .Verify();
        }
        

    }
    
    [Fact]
    public void EnsureProviderHonoursPactWithConsumer2()
    {
        var config = new PactVerifierConfig()
        {
            Outputters = new List<IOutput>()
            {
                output
            }
        };

        IPactVerifier pactVerifier = new PactVerifier(config);
        var pactFile =
            new FileInfo(
                $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName}{Path.DirectorySeparatorChar}pacts/Consumer2-CustomerAPI.json");
        
        using (var webhost = WebHost.CreateDefaultBuilder()
                   .UseUrls(_pactServiceUri)
                   .UseStartup<TestStartup>()
                   .Build())
        {
            webhost.Start();
            pactVerifier.ServiceProvider("CustomerAPI", new Uri(_providerUri)).WithFileSource(pactFile)
                .WithProviderStateUrl(new Uri($"{_pactServiceUri}/provider-states"))
                .Verify();
        }
    }
}

public class XUnitOutput : IOutput
{
    private readonly ITestOutputHelper _output;

    public XUnitOutput(ITestOutputHelper output)
    {
        _output = output;
    }

    public void WriteLine(string line)
    {
        _output.WriteLine(line);
    }
}