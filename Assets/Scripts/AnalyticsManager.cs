using Unity.Services.Analytics;
using Unity.Services.Core;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
	protected override async void Awake()
	{
		base.Awake();
        await UnityServices.InitializeAsync();
        // TODO: ask for consent of collecting data if needed (maybe they already signed a waiver at the beginning of the app and its not needed)
        AnalyticsService.Instance.StartDataCollection();
    }

  //  private void OnDisable()
  //  {
		//FlushEvents();
  //  }

  //  private void OnDestroy()
  //  {
		//FlushEvents();
  //  }

    public void SubmitEvent(CustomEvent newEvent, bool isUrgent = false)
	{
		//DateTime now = DateTime.Now;
		//string time = now.ToString("yyyy-MM-dd HH:mm:ss.SSS ±[hh]:[mm]");
  //      newEvent["eventTimestamp"] = time;
		AnalyticsService.Instance.RecordEvent(newEvent);
		if (isUrgent) FlushEvents();
	}

	public void SubmitEvent(string eventName, bool isUrgent = false)
	{
		SubmitEvent(new CustomEvent(eventName), isUrgent);
	}

	public void FlushEvents()
	{
        AnalyticsService.Instance.Flush();
    }

 //   public void SubmitTestResults(Test test, EvaluationResult[] responses)
	//{
	//	//if(!_isInitialized)
	//	//{
	//	//	Debug.LogWarning("AnalyticsManager: Analytics service is not initialized yet.");
	//	//	return;
	//	//}
	//	// TODO: unity analytics hates data oriented implementations, must implement manually adding each response
	//	CustomEvent testResults = new CustomEvent("debug_test")
	//	{
	//		{ "test_name", test.testName },
	//		{ "number_of_questions", test.questions.Length },
	//		{ "debug_test_response0", responses[0].resultScore },
	//		{ "debug_test_response1", (int)responses[1].bitmaskScore },
	//		{ "debug_test_response2", responses[2].resultText }
	//	};

	//	// TODO: implement dynamic adding of responses -> MUST have standardized keys already added in Unity Analytics
	//	//for (int i = 0; i < test.questions.Length; i++)
	//	//{
	//	//	string responseKey = $"response_{i + 1}";
	//	//	testResults.Add(responseKey, responses[i]);
	//	//}

	//	AnalyticsService.Instance.RecordEvent(testResults);
	//	AnalyticsService.Instance.Flush();
	//	Debug.Log("test_results submited to Analytics");
	//}
}
