# csFastFloat : a fast and accurate float parser

C# version of Daniel Lemire's [fast_float](https://github.com/fastfloat/fast_float)  fully ported from C to C#. It is up to three times faster than the standard library.

# Benchmarks

Using content of file in /data/Canada.txt

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1316 (1909/November2018Update/19H2)
Intel Xeon CPU E3-1285 v6 4.10GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.200-preview.20614.14
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  Job-FWAYQO : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT

Runtime=.NET Core 5.0  IterationCount=100  

```
|         Method |      Mean |     Error |    StdDev |    Median |       Min | Ratio | RatioSD |
|--------------- |----------:|----------:|----------:|----------:|----------:|------:|--------:|
|      FastFloat |  9.627 ms | 0.1471 ms | 0.4314 ms |  9.419 ms |  9.211 ms |  0.35 |    0.02 |
| Double.Parse() | 27.259 ms | 0.0609 ms | 0.1676 ms | 27.203 ms | 27.060 ms |  1.00 |    0.00 |



# Requirements

.NET 5 framework

# Usage

```C#
using csFastFloat;


double x;
float y;
double answer = 0;
foreach (string l in lines)
{
        x = FastDoubleParser.ParseDouble(l);
        y = FastFloatParser.ParseFloat(l);
}
```

# Reference

- Daniel Lemire, [Number Parsing at a Gigabyte per Second](https://arxiv.org/abs/2101.11408), arXiv:2101.11408
