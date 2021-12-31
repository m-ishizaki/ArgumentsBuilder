using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rksoftware.ArgumentsBuilder.Models;

public interface IArguments
{
    IEnumerable<string> ParameterNames { get; }
}
