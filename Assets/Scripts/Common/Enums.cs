namespace Assets.Scripts.Common
{
    public enum LevelResult
    {
        None = 0,
        Win = 1,
        Lose = 2
    }
	public enum EnemySeaType {
		Enemy_Sea_1,
		Enemy_Sea_2,
		Enemy_Sea_3
	}

	public enum EnemyLandType {
		Enemy_Land_1,
		Enemy_Land_2,
		Enemy_Land_3
	}

	public enum ButtonType {
		None,
		Play,
		Pause
	}

	public enum DragState
    {
        Continue,
        Canceled,
        Wait
    }

	public enum GridUnitLandType {
		Land_1,
		Land_2,
		Land_3,
		Land_4,
		Land_5,
		Land_6,
		Land_7,
		Land_8,
		Land_9
	}

	public enum GridUnitSeaType {
		Sea_1,
		Sea_2,
		Sea_3,
		Sea_4,
		Sea_5,
		Sea_6
	}

	public enum TypeSwipe {
		None,
		Click,
		LeftSwipe,
		RightSwipe,
		UpSwipe,
		DownSwipe
	}
}
