using Unity.Services.Analytics;
using Unity.Services.Core;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
	bool _canSendData = false;

	public static async void StartGatheringData()
	{
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();

		Instance._canSendData = true;
    }

    public void SubmitEvent(CustomEvent newEvent, bool isUrgent = false)
	{
		if (!_canSendData) return;

		AnalyticsService.Instance.RecordEvent(newEvent);
		if (isUrgent) FlushEvents();
	}

	public void SubmitEvent(string eventName, bool isUrgent = false)
	{
        if (!_canSendData) return;

        SubmitEvent(new CustomEvent(eventName), isUrgent);
	}

	public void FlushEvents()
	{
        if (!_canSendData) return;

        AnalyticsService.Instance.Flush();
    }
}
