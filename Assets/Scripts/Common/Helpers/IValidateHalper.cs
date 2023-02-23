namespace Assets.Scripts.Common.Helpers {
	public interface IValidateHalper {

#if UNITY_EDITOR
		bool IsValidate { get; set; }
		void OnValidate();
#endif
	}
}
