using NUnit.Framework;
using Rksoftware.ArgumentsBuilder.Attributes;
using System.Collections.Generic;

namespace ArgumentsBuilder.Test;

public class Tests
{
    class ArgumentsC : Rksoftware.ArgumentsBuilder.Models.IArguments
    {
        public static IEnumerable<string> ParameterNames => new[] { nameof(Aparam), nameof(Bparam) };

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
        var parsed = Rksoftware.ArgumentsBuilder.ArgumentsBuilder.Parse<ArgumentsC>(args);
        Assert.AreEqual(parsed.Aparam, "aparam");
        Assert.AreEqual(parsed.Bparam, "bparam");
        Assert.AreEqual(parsed.A, "aopt");
        Assert.AreEqual(parsed.B, "bopt");
        Assert.AreEqual(parsed.C, "copt");
        Assert.AreEqual(parsed.D, true);
        Assert.AreEqual(parsed.E, false);
    }

    [Test]
    public void Test2()
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

    record struct Arguments(string? Aparam, string? Bparam, string? A, string? B, string? C, bool D, bool E) : Rksoftware.ArgumentsBuilder.Models.IArguments
    {
        public static IEnumerable<string> ParameterNames => new[] { nameof(Aparam), nameof(Bparam) };
    }

    [Test]
    public void Test3()
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
        var parsed = Rksoftware.ArgumentsBuilder.ArgumentsBuilder.Parse<Arguments2>(args);
        Assert.AreEqual(parsed.Aparam, "aparam");
        Assert.AreEqual(parsed.Bparam, "bparam");
        Assert.AreEqual(parsed.A, "aopt");
        Assert.AreEqual(parsed.B, "bopt");
        Assert.AreEqual(parsed.C, "copt");
        Assert.AreEqual(parsed.D, true);
        Assert.AreEqual(parsed.E, false);
    }

    record struct Arguments2([Parameter] string? Aparam, [Parameter] string? Bparam, string? A, string? B, string? C, bool D, bool E)
    {
    }

    [Test]
    public void Test4()
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
            "dparam",
            "cparam",
            };
        var parsed = Rksoftware.ArgumentsBuilder.ArgumentsBuilder.Parse<Arguments3>(args);
        Assert.AreEqual(parsed.Aparam, "aparam");
        Assert.AreEqual(parsed.Bparam, "bparam");
        Assert.AreEqual(parsed.Cparam, "cparam");
        Assert.AreEqual(parsed.Dparam, "dparam");
        Assert.AreEqual(parsed.Eparam, null);
        Assert.AreEqual(parsed.A, "aopt");
        Assert.AreEqual(parsed.B, "bopt");
        Assert.AreEqual(parsed.C, "copt");
        Assert.AreEqual(parsed.D, true);
        Assert.AreEqual(parsed.E, false);
    }

    struct Arguments3
    {
        public Arguments3([Parameter] string? Aparam, [Parameter] string? Bparam, string? A, string? B, string? C, bool D, bool E)
            => (this.Aparam, this.Bparam, this.A, this.B, this.C, this.D, this.E) = (Aparam, Bparam, A, B, C, D, E);

        public string? Aparam { get; set; }
        public string? Bparam { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public bool D { get; set; }
        public bool E { get; set; }

        string? _cparam = default;
        [Parameter(No = 2)]
        public string? Cparam { get => _cparam; set=>_cparam = value; }
        [Parameter(No = 1)]
        public string? Dparam { get; set; } = default;
        public string? Eparam { get; set; } = default;
    }

}