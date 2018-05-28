using MarkLight.Views.UI;

namespace ARM
{
	public class RaceView : UIView
	{
        public Label PositionText;
        public Label CenterText;
        public Label TimeText;
        public bool _enableLeaderboard;

		void OnEnable()
        {
            EventsManager.UpdatePositionTextEvent += UpdatePositionTextEvent;
            EventsManager.UpdateCenterTextEvent += UpdateCenterTextEvent;
            EventsManager.UpdateTimeTextEvent += UpdateTimeTextEvent;
            EventsManager.StartRaceEvent += StartRaceEvent;
        }

        void OnDisable()
        {
            EventsManager.UpdatePositionTextEvent -= UpdatePositionTextEvent;
            EventsManager.UpdateCenterTextEvent -= UpdateCenterTextEvent;
            EventsManager.UpdateTimeTextEvent -= UpdateTimeTextEvent;
            EventsManager.StartRaceEvent -= StartRaceEvent;
        }

		public override void Initialize()
        {
            base.Initialize();
            SetValue(() => _enableLeaderboard, true);
        }

		public void Restart() {

			EventsManager.SwitchUiContext("restart");
			SetValue(() => _enableLeaderboard, true);
		}

		public void StartRaceEvent()
        {
            SetValue(() => _enableLeaderboard, false);
        }

        void UpdatePositionTextEvent(string text)
        {
            if (PositionText != null)
            {
                SetValue(() => PositionText.Text, text);
            }
        }

        void UpdateCenterTextEvent(string text)
        {
            if (CenterText != null)
            {
                SetValue(() => CenterText.Text, text);
            }
        }

        void UpdateTimeTextEvent(string text)
        {
            if (TimeText != null)
            {
                SetValue(() => TimeText.Text, text);
            }
        }
	}
}
