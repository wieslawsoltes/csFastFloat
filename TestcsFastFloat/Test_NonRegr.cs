﻿using csFastFloat;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace TestcsFastFloat.Tests
{
  public class ParseException : Exception
  {
    public string Value;
    public string Reason;
    private double _x;
    private double _d;

    public ParseException(string v, string reason, double x, double d)
    {
      Value = v;
      Reason = reason;
      _x = x;
      _d = d;
    }
  }

  public class TestCharge : BaseTestClass
  {
    // Bit mixer
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong rng(ulong h)
    {
      h ^= h >> 33;
      h *= (ulong)(0xff51afd7ed558ccd);
      h ^= h >> 33;
      h *= (ulong)(0xc4ceb9fe1a85ec53);
      h ^= h >> 33;
      return h;
    }

    private static void check(double d)
    {
      //std::string s(64, '\0');
      string s = new string('\0', 64);

      //auto written = std::snprintf(&s[0], s.size(), "%.*e", DBL_DIG + 1, d);
      //s.resize(written);
      s = d.ToString().Replace(",", ".");

      double? x = FastParser.ParseDouble(s);
      if (!x.HasValue)
      {
        throw new ParseException(s, "refused", 0, 0);
      }
      // if (isok != s.data() + s.size()) throw std::runtime_error("does not point at the end");
      if (d != x)
      {
        throw new ParseException(s, "disagree", x.Value, d);
      }
    }

    [Fact]
    private unsafe void Test_Charge()
    {
      var did = 0;
      var refused = 0;
      var disagree = 0;

      ulong offset = 1190;
      var howmany = 10000000;

      for (var i = 1; i <= howmany; i++)
      {
        // mix bits
        ulong x = rng((ulong)i + offset);
        double d;
        Buffer.MemoryCopy(&x, &d, sizeof(double), sizeof(double));
        // paranoid
        while ((!double.IsNormal(d)) || double.IsNaN(d) || double.IsInfinity(d))
        {
          offset++;
          x = rng((ulong)i + offset);
          Buffer.MemoryCopy(&x, &d, sizeof(double), sizeof(double));
        }
        try
        {
          did += 1;
          check(d);
        }
        catch (ParseException ex)
        {
          if (ex.Reason == "refused") refused += 1;
          if (ex.Reason == "disagree") disagree += 1;
        }
        catch
        {
          throw;
        }
      }
      ApprovalTests.Approvals.Verify($"Did :{did} Refused: {refused} Disagree{disagree}");
    }
  }

  public class Test_NonRegr : BaseTestClass
  {
    [Fact]
    public void cas_ProfLemire()
    {
      Dictionary<string, double> sut = new Dictionary<string, double>();

      // sut.Add("1090544144181609348835077142190", 0x1.b8779f2474dfbp + 99); // TODO
      sut.Add("4503599627370496.5", 4503599627370496.5);
      sut.Add("4503599627370497.5", 4503599627370497.5);
      sut.Add("0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000044501477170144022721148195934182639518696390927032912960468522194496444440421538910330590478162701758282983178260792422137401728773891892910553144148156412434867599762821265346585071045737627442980259622449029037796981144446145705102663115100318287949527959668236039986479250965780342141637013812613333119898765515451440315261253813266652951306000184917766328660755595837392240989947807556594098101021612198814605258742579179000071675999344145086087205681577915435923018910334964869420614052182892431445797605163650903606514140377217442262561590244668525767372446430075513332450079650686719491377688478005309963967709758965844137894433796621993967316936280457084866613206797017728916080020698679408551343728867675409720757232455434770912461317493580281734466552734375", 0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000044501477170144022721148195934182639518696390927032912960468522194496444440421538910330590478162701758282983178260792422137401728773891892910553144148156412434867599762821265346585071045737627442980259622449029037796981144446145705102663115100318287949527959668236039986479250965780342141637013812613333119898765515451440315261253813266652951306000184917766328660755595837392240989947807556594098101021612198814605258742579179000071675999344145086087205681577915435923018910334964869420614052182892431445797605163650903606514140377217442262561590244668525767372446430075513332450079650686719491377688478005309963967709758965844137894433796621993967316936280457084866613206797017728916080020698679408551343728867675409720757232455434770912461317493580281734466552734375);
      sut.Add("0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000022250738585072008890245868760858598876504231122409594654935248025624400092282356951787758888037591552642309780950434312085877387158357291821993020294379224223559819827501242041788969571311791082261043971979604000454897391938079198936081525613113376149842043271751033627391549782731594143828136275113838604094249464942286316695429105080201815926642134996606517803095075913058719846423906068637102005108723282784678843631944515866135041223479014792369585208321597621066375401613736583044193603714778355306682834535634005074073040135602968046375918583163124224521599262546494300836851861719422417646455137135420132217031370496583210154654068035397417906022589503023501937519773030945763173210852507299305089761582519159720757232455434770912461317493580281734466552734375", 0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000022250738585072008890245868760858598876504231122409594654935248025624400092282356951787758888037591552642309780950434312085877387158357291821993020294379224223559819827501242041788969571311791082261043971979604000454897391938079198936081525613113376149842043271751033627391549782731594143828136275113838604094249464942286316695429105080201815926642134996606517803095075913058719846423906068637102005108723282784678843631944515866135041223479014792369585208321597621066375401613736583044193603714778355306682834535634005074073040135602968046375918583163124224521599262546494300836851861719422417646455137135420132217031370496583210154654068035397417906022589503023501937519773030945763173210852507299305089761582519159720757232455434770912461317493580281734466552734375);

      StringBuilder sb = new StringBuilder();

      foreach (KeyValuePair<string, double> kv in sut)
      {
        sb.AppendLine($"Valeur   : {kv.Key} ");
        sb.AppendLine($"Expected : {kv.Value} ");
        sb.AppendLine($"Resultat : { FastParser.ParseDouble(kv.Key)}");
      }

      ApprovalTests.Approvals.Verify(sb.ToString());
    }

    [Fact]
    private void issue13()
    {
      double? x = FastParser.ParseDouble("0");
      Assert.True(x.HasValue, "Parsed");
      Assert.True(x == 0, "Maps to 0");
    }

    [Fact]
    private void issue40()
    {
      //https://tools.ietf.org/html/rfc7159
      // A fraction part is a decimal point followed by one or more digits.
      Assert.Throws<System.FormatException>(() => Double.Parse("0."));
    }

    [Fact]
    private void issue32()
    {
      double? x = FastParser.ParseDouble("-0");
      Assert.True(x.HasValue, "could not parse -zero.");
      Assert.True(x == 0, "-zero does not map to zero.");
    }

    [Fact]
    private void issue23()
    {
      double? x = FastParser.ParseDouble("0e+42949672970");

      Assert.True(x.HasValue, "could not parse zero.");
      Assert.True(x == 0, "zero does not map to zero.");
    }

    [Fact]
    private void issue23_2()
    {
      double? x = FastParser.ParseDouble("5e0012");

      Assert.True(x.HasValue, "could not parse 5e0012.");
      Assert.True(x == 5e12, "does not map to 5e0012.");
    }
  }
}