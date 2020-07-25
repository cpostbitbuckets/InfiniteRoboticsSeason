using Godot;
using System;

/// <summary>
/// This component is for the 3 stages of a match. It updates the icons 
/// from lock icons, to numbers, to color wheel spinners
/// </summary>
public class Stages : VBoxContainer
{
	[Export]
	Alliance Alliance { get; set; } = Alliance.Blue;

	private StyleBox redStyle;

	private Label stage1Label;
	private Label stage2Label;
	private Label stage3Label;

	private Sprite stage1Lock;
	private Sprite stage2Lock;
	private Sprite stage3Lock;

	private Sprite stage1Complete;
	private Sprite stage2Complete;
	private Sprite stage3Complete;

	private Sprite stage1SpinColorWheel;
	private Sprite stage2SpinColorWheel;
	private Sprite stage3PositionColorWheel;

	public override void _Ready()
	{
		// load the red style so we can switch it based on this component's alliance
		redStyle = (StyleBox)ResourceLoader.Load("res://themes/red_box_round.stylebox");


		stage1Label = GetNode<Label>("HBoxContainer/Stage1/Label");
		stage2Label = GetNode<Label>("HBoxContainer/Stage2/Label");
		stage3Label = GetNode<Label>("HBoxContainer/Stage3/Label");

		stage1Lock = GetNode<Sprite>("HBoxContainer/Stage1/Lock");
		stage2Lock = GetNode<Sprite>("HBoxContainer/Stage2/Lock");
		stage3Lock = GetNode<Sprite>("HBoxContainer/Stage3/Lock");

		stage1Complete = GetNode<Sprite>("HBoxContainer/Stage1/Complete");
		stage2Complete = GetNode<Sprite>("HBoxContainer/Stage2/Complete");
		stage3Complete = GetNode<Sprite>("HBoxContainer/Stage3/Complete");

		stage1SpinColorWheel = GetNode<Sprite>("HBoxContainer/Stage1/RotateColorWheel");
		stage2SpinColorWheel = GetNode<Sprite>("HBoxContainer/Stage2/RotateColorWheel");
		stage3PositionColorWheel = GetNode<Sprite>("HBoxContainer/Stage3/RotateColorWheel");

		// start with all stages locked
		stage1Lock.Visible = true;
		stage2Lock.Visible = true;
		stage3Lock.Visible = true;

		stage1Complete.Visible = false;
		stage2Complete.Visible = false;
		stage3Complete.Visible = false;

		stage1SpinColorWheel.Visible = false;
		stage2SpinColorWheel.Visible = false;
		stage3PositionColorWheel.Visible = false;

		if (Alliance == Alliance.Red)
		{
			// turn our various panels red
			GetNode("Level").Set("custom_styles/panel", redStyle);
			GetNode("HBoxContainer/Stage1").Set("custom_styles/panel", redStyle);
			GetNode("HBoxContainer/Stage2").Set("custom_styles/panel", redStyle);
			GetNode("HBoxContainer/Stage3").Set("custom_styles/panel", redStyle);
		}
		Signals.StageChangedEvent += OnStageChanged;
	}

	private void OnStageChanged(AllianceStage stage)
	{
		if (stage.Alliance == Alliance)
		{
			switch (stage.Stage)
			{
				case Stage.One:
					stage1Label.Visible = true;
					stage1Label.Text = stage.StageBallsRemaining.ToString();
					stage1Label.Show();
					stage1Lock.Visible = false;
					break;
				case Stage.Two:
					stage1Label.Visible = false;
					stage1Complete.Visible = true;
					stage2Lock.Visible = false;

					if (stage.StageState == StageState.Balls)
					{
						stage2Label.Visible = true;
						stage2Label.Text = stage.StageBallsRemaining.ToString();
					}
					else if (stage.StageState == StageState.SpinColorWheel)
					{
						stage2Label.Visible = false;
						stage2SpinColorWheel.Visible = true;
					}
					break;
				case Stage.Three:
					stage2Label.Visible = false;
					stage2SpinColorWheel.Visible = false;
					stage2Complete.Visible = true;
					stage3Lock.Visible = false;

					if (stage.StageState == StageState.Balls)
					{
						stage3Label.Visible = true;
						stage3Label.Text = stage.StageBallsRemaining.ToString();
					}
					else if (stage.StageState == StageState.PositionColorWheel)
					{
						stage3Label.Visible = false;
						stage3PositionColorWheel.Visible = true;
					}
					else if (stage.StageState == StageState.Complete)
					{
						stage3PositionColorWheel.Visible = false;
						stage3Complete.Visible = true;
					}

					break;
			}
		}
	}
}
