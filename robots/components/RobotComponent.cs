using Godot;
using System;

public class RobotComponent : Node2D
{
    private Alliance _alliance = Alliance.Blue;

    public Alliance Alliance
    {
        get
        {
            return _alliance;
        }
        set
        {
            _alliance = value;
            UpdateAllianceColor();
        }
    }

    /// <summary>
    /// Every RobotComponent belongs to a robot
    /// </summary>
    /// <value></value>
    public Robot Robot { get; set; }

    protected Godot.Color color;

    public override void _Ready()
    {
    }

    virtual public void UpdateAllianceColor()
    {
        switch (Alliance)
        {
            case Alliance.Blue:
                color = Constants.BlueAllianceColor;
                break;
            case Alliance.Red:
                color = Constants.RedAllianceColor;
                break;
        }
    }
}
