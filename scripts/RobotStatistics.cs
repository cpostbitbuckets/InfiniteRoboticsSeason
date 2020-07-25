using Godot;
using System;

/// <summary>
/// Robot specific statistics
/// </summary>
public class RobotStatistics : Statistics
{
    /// <summary>
    /// The optional robot for this statistics
    /// </summary>
    /// <value></value>
    public Robot Robot { get; set; }
}
