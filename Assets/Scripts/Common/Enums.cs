namespace Assets.Scripts.Common
{
    public enum LevelResult
    {
        None = 0,
        Win = 1,
        Lose = 2
    }
	public enum EnemyType {
		None,
		Land,
		Sea
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
}
