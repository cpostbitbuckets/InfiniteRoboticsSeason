using Godot;
using System;

public class AllianceSummary : Node
{
	[Export]
	Alliance Alliance { get; set; } = Alliance.Blue;

	Label scoreTotalLabel;
	Label highScoreLabel;
	Label winLabel;

	public override void _Ready()
	{
		scoreTotalLabel = GetNode<Label>("AlliancePanel/MarginContainer/VBoxContainer/TeamAndScore/ScoreTotal/Score");
		highScoreLabel = GetNode<Label>("AlliancePanel/MarginContainer/VBoxContainer/TeamAndScore/ScoreTotal/HighScoreLabel");
		winLabel = GetNode<Label>("AlliancePanel/MarginContainer/VBoxContainer/TeamAndScore/ScoreTotal/WinLabel");

		Signals.ScoreUpdatedEvent += OnScoreUpdated;
	}

	private void OnScoreUpdated(ScoreKeeper scoreKeeper)
	{
		switch (Alliance)
		{
			case Alliance.Blue:
				scoreTotalLabel.Text = scoreKeeper.BlueScore.ToString();
				if (scoreKeeper.BlueScore > scoreKeeper.RedScore)
				{
					highScoreLabel.Visible = true;
					winLabel.Visible = true;
				}
				break;
			case Alliance.Red:
				scoreTotalLabel.Text = scoreKeeper.RedScore.ToString();
				if (scoreKeeper.RedScore > scoreKeeper.BlueScore)
				{
					highScoreLabel.Visible = true;
					winLabel.Visible = true;
				}
				break;
		}

	}
}
