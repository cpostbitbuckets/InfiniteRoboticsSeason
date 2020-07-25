using System;

public class ShooterStats
{
    public String Name { get; set; } = "Super Shooter";
    /// <summary>
    /// The force this shooter shoots balls at
    /// </summary>
    /// <value></value>
    public float Force { get; set; } = 500f;

    /// <summary>
    /// The amount of random wobbliness balls 
    /// </summary>
    /// <value></value>
    public float Wobbliness { get; set; } = 0f;

    /// <summary>
    /// Time to charge the shooter
    /// </summary>
    /// <value></value>
    public float ChargeTime { get; set; } = 1f;

    /// <summary>
    /// Time spent cooling down between shots
    /// </summary>
    /// <value></value>
    public float CooldownTime { get; set; } = .5f;
}
