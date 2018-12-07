public abstract class QuestObjective 
{
	public bool		Completed { get; set; }
	public int		CurrentAmmount { get; set; }
	public int		RequiredAmmount { get; set; }

	public void CheckProgress()
	{
		if (CurrentAmmount == RequiredAmmount)
			Complete();
	}

	public void Complete()
	{
		Completed = true;
	}
}
