using AI.Interview.Core.Converters;
using SolentimHardwareAccessLayer.Enums;

namespace AI.Interview.Tests;

[TestFixture]
public class ConverterTests
{

  [Test]
  public void Converter_PositionalMillimetersToCounts_ShouldReturnCorrectValue()
  {
    // Arrange
    var value = 1;
    // Act
    var actual = Converter.ToPositionalCounts(value, ePositionUnits.Millimeters);
    // Assert
    Assert.That(actual, Is.EqualTo(100));
  }

  [Test]
  public void Converter_VelocityCentimetersToCounts_ShouldReturnCorrectValue()
  {
    // Arrange
    var value = 1;
    // Act
    var actual = Converter.ToVelocityCounts(value, eVelocityUnits.CentimetersPerSecond);
    // Assert
    Assert.That(actual, Is.EqualTo(1000));
  }

  [Test]
  public void Converter_PositionalInchesToCounts_ShouldReturnCorrectValue()
  {
    // Arrange
    var value = 1;
    // Act
    var actual = Converter.ToPositionalCounts(value, ePositionUnits.Inches);
    // Assert
    Assert.That(actual, Is.EqualTo(100 * 25.4));
  }

  [Test]
  public void Converter_VelocityInchesToCounts_ShouldReturnCorrectValue()
  {
    // Arrange
    var value = 1;
    // Act
    var actual = Converter.ToVelocityCounts(value, eVelocityUnits.InchesPerSecond);
    // Assert
    Assert.That(actual, Is.EqualTo(100 * 25.4));
  }

  [Test]
  [TestCase(ePositionUnits.Inches, TestName = "Converter_ToAndFromPositionalCounts_ShouldReturnOriginal_Inches")]
  [TestCase(ePositionUnits.Counts, TestName = "Converter_ToAndFromPositionalCounts_ShouldReturnOriginal_Counts")]
  [TestCase(ePositionUnits.Millimeters, TestName = "Converter_ToAndFromPositionalCounts_ShouldReturnOriginal_Millimeters")]
  public void Converter_ToAndFromPositionalCounts_ShouldReturnOriginal(ePositionUnits units)
  {
    // Arrange
    var original = 123.456;

    // Act
    var counts = Converter.ToPositionalCounts(original, units);
    var actual = Converter.FromPositionalCounts(counts, units);
    var actualTo3dp = Math.Round(actual, 3);

    // Assert
    Assert.That(actualTo3dp, Is.EqualTo(original));
  }

  [Test]
  [TestCase(eVelocityUnits.InchesPerSecond, TestName = "Converter_ToAndFromVelocityCounts_ShouldReturnOriginal_Inches")]
  [TestCase(eVelocityUnits.CountsPerSecond, TestName = "Converter_ToAndFromVelocityCounts_ShouldReturnOriginal_Counts")]
  [TestCase(eVelocityUnits.CentimetersPerSecond, TestName = "Converter_ToAndFromVelocityCounts_ShouldReturnOriginal_Centimeters")]
  public void Converter_ToAndFromVelocityCounts_ShouldReturnOriginal(eVelocityUnits units)
  {
    // Arrange
    var original = 123.456;

    // Act
    var counts = Converter.ToVelocityCounts(original, units);
    var actual = Converter.FromVelocityCounts(counts, units);
    var actualTo3dp = Math.Round(actual, 3);

    // Assert
    Assert.That(actualTo3dp, Is.EqualTo(original));
  }
}