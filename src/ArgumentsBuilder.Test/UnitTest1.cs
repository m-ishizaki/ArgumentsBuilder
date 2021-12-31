using NUnit.Framework;
using System.Collections.Generic;

namespace ArgumentsBuilder.Test;

public class Tests
{
    class Arguments : Rksoftware.ArgumentsBuilder.Models.IArguments
    {
        public IEnumerable<string> ParameterNames => new[] { nameof(Aparam), nameof(Bparam) };

        public string? Aparam { get; set; }
        public string? Bparam { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public bool D { get; set; }
        public bool E { get; set; }
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var args = new[] {
            "aparam",
            "-a",
            "aopt",
            "-d",
            "-b",
            "bopt",
            "bparam",
            "/c",
            "copt",
            };
        var parsed = Rksoftware.ArgumentsBuilder.ArgumentsBuilder.Parse<Arguments>(args);
        Assert.AreEqual(parsed.Aparam, "aparam");
        Assert.AreEqual(parsed.Bparam, "bparam");
        Assert.AreEqual(parsed.A, "aopt");
        Assert.AreEqual(parsed.B, "bopt");
        Assert.AreEqual(parsed.C, "copt");
        Assert.AreEqual(parsed.D, true);
        Assert.AreEqual(parsed.E, false);
    }
}