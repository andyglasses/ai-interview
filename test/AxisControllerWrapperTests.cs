using AI.Interview.Core.Services;
using Moq;
using SolentimHardwareAccessLayer.Interface;

namespace AI.Interview.Tests;

[TestFixture]
public class AxisControllerWrapperTests
{
  [Test]
  public void AxisControllerWrapper_PositionAndVelocityUnitsOnHardware_ShouldBeCalledOnce()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Millimeters);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CentimetersPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var _ = sut.PositionUnits;
    _ = sut.PositionUnits;
    var __ = sut.VelocityUnits;
    var ___ = sut.GetCapabilities();

    // Assert
    axisController.Verify(x => x.VelocityUnit, Times.Once);
    axisController.Verify(x => x.PositionUnit, Times.Once);
  }

  [Test]
  public void AxisControllerWrapper_GetCapabilities_ShouldReturnCorrectValues()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.Setup(ac => ac.GetMinPosition()).Returns(-123);
    axisController.Setup(ac => ac.GetMaxPosition()).Returns(234);
    axisController.Setup(ac => ac.GetMinVelocity()).Returns(-456);
    axisController.Setup(ac => ac.GetMaxVelocity()).Returns(789);
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Counts);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CountsPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var actual = sut.GetCapabilities();

    // Assert
    Assert.That(actual.Position.MinInCounts, Is.EqualTo(-123));
    Assert.That(actual.Position.MaxInCounts, Is.EqualTo(234));
    Assert.That(actual.Velocity.MinInCounts, Is.EqualTo(-456));
    Assert.That(actual.Velocity.MaxInCounts, Is.EqualTo(789));
    Assert.That(actual.SourcePositionUnits, Is.EqualTo(SolentimHardwareAccessLayer.Enums.ePositionUnits.Counts));
    Assert.That(actual.SourceVelocityUnits, Is.EqualTo(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CountsPerSecond));
  }

  [Test]
  public void AxisControllerWrapper_GetCapabilities_ShouldReturnCorrectConvertedValues()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.Setup(ac => ac.GetMinPosition()).Returns(-123);
    axisController.Setup(ac => ac.GetMaxPosition()).Returns(234);
    axisController.Setup(ac => ac.GetMinVelocity()).Returns(-456);
    axisController.Setup(ac => ac.GetMaxVelocity()).Returns(789);
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Millimeters);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CentimetersPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var actual = sut.GetCapabilities();

    // Assert
    Assert.That(actual.Position.MinInCounts, Is.EqualTo(-123 * 100));
    Assert.That(actual.Position.MaxInCounts, Is.EqualTo(234 * 100));
    Assert.That(actual.Velocity.MinInCounts, Is.EqualTo(-456 * 1000));
    Assert.That(actual.Velocity.MaxInCounts, Is.EqualTo(789 * 1000));
    Assert.That(actual.SourcePositionUnits, Is.EqualTo(SolentimHardwareAccessLayer.Enums.ePositionUnits.Millimeters));
    Assert.That(actual.SourceVelocityUnits, Is.EqualTo(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CentimetersPerSecond));
  }

  [Test]
  public void AxisControllerWrapper_GetCurrent_ShouldReturnCorrectValues()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.Setup(ac => ac.GetCurrentPosition()).Returns(123);
    axisController.Setup(ac => ac.GetCurrentVelocity()).Returns(456);
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Counts);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CountsPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var actual = sut.GetCurrentState();

    // Assert
    Assert.That(actual.PositionInCounts, Is.EqualTo(123));
    Assert.That(actual.VelocityInCounts, Is.EqualTo(456));
  }

  [Test]
  public void AxisControllerWrapper_GetCurrent_ShouldReturnCorrectConvertedValues()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.Setup(ac => ac.GetCurrentPosition()).Returns(123);
    axisController.Setup(ac => ac.GetCurrentVelocity()).Returns(456);
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Millimeters);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CentimetersPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var actual = sut.GetCurrentState();

    // Assert
    Assert.That(actual.PositionInCounts, Is.EqualTo(123 * 100));
    Assert.That(actual.VelocityInCounts, Is.EqualTo(456 * 1000));
  }

  [Test]
  public void AxisControllerWrapper_GetCurrent_ShouldHandleErrorInPosition()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.Setup(ac => ac.GetCurrentPosition()).Throws(new Exception("Error"));
    axisController.Setup(ac => ac.GetCurrentVelocity()).Returns(456);
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Counts);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CountsPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var actual = sut.GetCurrentState();

    // Assert
    Assert.That(actual.HasError, Is.True);
    Assert.That(actual.ErrorMessage, Is.EqualTo("Error"));
  }

  [Test]
  public void AxisControllerWrapper_GetCurrent_ShouldHandleErrorInVelocity()
  {
    // Arrange
    var axisController = new Mock<IAxisController>();
    axisController.Setup(ac => ac.GetCurrentPosition()).Returns(123);
    axisController.Setup(ac => ac.GetCurrentVelocity()).Throws(new Exception("Error"));
    axisController.SetupGet(ac => ac.PositionUnit).Returns(SolentimHardwareAccessLayer.Enums.ePositionUnits.Counts);
    axisController.SetupGet(ac => ac.VelocityUnit).Returns(SolentimHardwareAccessLayer.Enums.eVelocityUnits.CountsPerSecond);
    var sut = new AxisControllerWrapper(axisController.Object);

    // Act
    var actual = sut.GetCurrentState();

    // Assert
    Assert.That(actual.HasError, Is.True);
    Assert.That(actual.ErrorMessage, Is.EqualTo("Error"));
  }
}
