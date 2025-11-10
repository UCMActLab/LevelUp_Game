public interface IQuestionLogic
{
	public abstract void SetUp(Question question);

	public abstract EvaluationResult GetResults();

	public abstract void LockQuestion();

	public abstract bool IsCorrect();

	public abstract string GetCorrectResponse();
}