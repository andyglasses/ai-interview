using SolentimHardwareAccessLayer.Enums;

namespace AI.Interview.Core.Models;

public record AxisCapabilities(MinMaxCounts Position, MinMaxCounts Velocity, ePositionUnits SourcePositionUnits, eVelocityUnits SourceVelocityUnits);