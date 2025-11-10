public struct EvaluationResult
{
	public enum ResultType
	{
		NONE,
		STRING,
		BIT_MASK,
		INT
	}

	public ResultType resultType;

	public string resultText;

	public int resultScore;

	public uint bitmaskScore;
}