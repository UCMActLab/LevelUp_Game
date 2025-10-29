using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
	public static AnalyticsManager Instance { get; private set; }
	private bool _isInitialized = false;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			//DontDestroyOnLoad(gameObject);
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private async void Start()
	{
		await UnityServices.InitializeAsync();
		// TODO: ask for consent of collecting data if needed (maybe they already signed a waiver at the beginning of the app and its not needed)
		AnalyticsService.Instance.StartDataCollection();
		_isInitialized = true;
	}

	public void SubmitTestResults(Test test, string[] responses)
	{
		if(!_isInitialized)
		{
			Debug.LogWarning("AnalyticsManager: Analytics service is not initialized yet.");
			return;
		}
		// TODO: unity analytics hates data oriented implementations, must implement manually adding each response
		// TODO: change string array, maybe use a dictionary or a custom object to hold different types of responses to avoid parsing
		CustomEvent testResults = new CustomEvent("debug_test")
		{
			{ "test_name", test.testName },
			{ "number_of_questions", test.questions.Length },
			{ "debug_test_response0", int.Parse(responses[0]) },
			{ "debug_test_response1", int.Parse(responses[1]) },
			{ "debug_test_response2", responses[2] }
		};

		//for (int i = 0; i < test.questions.Length; i++)
		//{
		//	string responseKey = $"response_{i + 1}";
		//	testResults.Add(responseKey, responses[i]);
		//}

		AnalyticsService.Instance.RecordEvent(testResults);
		AnalyticsService.Instance.Flush();
		Debug.Log("Stest_resultsubmited test results to Analytics");
	}
}
