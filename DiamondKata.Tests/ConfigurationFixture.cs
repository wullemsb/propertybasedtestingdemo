using DiamondKata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
[assembly: AssemblyFixture(typeof(ConfigurationFixture))]

namespace DiamondKata.Tests;
public class ConfigurationFixture : IDisposable
{
    public ConfigurationFixture()
    {
        FsCheck.Config.Quick.WithArbitrary<>();
    }

    public void Dispose()
    {
    }

}
