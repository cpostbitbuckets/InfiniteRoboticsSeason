using System;
using Godot;

public class Constants
{
    public static Color RedAllianceColor { get; } = new Color("c02418");
    public static Color BlueAllianceColor { get; } = new Color("00378c");

    // matches are 2.5 minutes long with a 15 second auto period and 30 second
    // endgame
    public const int MatchTime = 5;
    public const int AutoTime = 15;
    public const int EndGameTime = 30;

    // Scoring constants
    public const int LowBallPoints = 1;
    public const int HighBallPoints = 2;
    public const int InnerBallPoints = 3;

    public const int AutoFactor = 2;
    public const int AutoLeaveLine = 5;

    public const int SpinColorWheel = 10;
    public const int PositionColorWheel = 20;

    public const int Climb = 25;
    public const int Level = 15;
    public const int Park = 5;

    // Stage Constants;
    public const int StageOneBalls = 9;
    public const int StageTwoBalls = 20;
    public const int StageThreeBalls = 20;
    public const int StageTwoRotationsMin = 3;
    public const int StageTwoRotationsMax = 5;
}