using Godot;
using System;

/// <summary>
/// This component updates the score and match timer when there is a score event or a match time update event
/// </summary>
public class Score : HBoxContainer
{
	ProgressBar matchTimeProgressBar;
	Label matchTimeLabel;

	public override void _Ready()
	{
		matchTimeProgressBar = GetNode<ProgressBar>("TeamScores/MatchTime");
		matchTimeLabel = GetNode<Label>("TeamScores/MatchTime/Label");

		matchTimeProgressBar.MaxValue = Constants.MatchTime;
		matchTimeLabel.Text = "";

		Signals.ScoreUpdatedEvent += OnScoreUpdated;
		Signals.MatchTimeUpdatedEvent += OnMatchTimeUpdated;
	}

	private void OnMatchTimeUpdated(float timeElapsed)
	{
		matchTimeProgressBar.Value = timeElapsed;
		matchTimeLabel.Text = $"{(int)timeElapsed}";
	}

	private void OnScoreUpdated(ScoreKeeper scoreKeeper)
	{
		GetNode<Label>("TeamScores/TeamScores/BlueScore/Label").Text = $"{scoreKeeper.BlueScore}";
		GetNode<Label>("TeamScores/TeamScores/RedScore/Label").Text = $"{scoreKeeper.RedScore}";
	}
}
