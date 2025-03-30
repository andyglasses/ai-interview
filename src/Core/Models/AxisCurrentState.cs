namespace AI.Interview.Core.Models;

public record AxisCurrentState
  (bool HasError,
  string? ErrorMessage,
  double? PositionInCounts,
  double? VelocityInCounts);
