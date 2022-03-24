namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Match3Test/ScoringRule")]
	
	public sealed class ScoringRule : ScriptableObject
	{
		[SerializeField] private int _minMatchNum = 3;
		[SerializeField] private int _minMatchScore = 10;
		[SerializeField] private int _bonusMatchScore = 5;

		public int GetScoreForSolution(List<Cell> solution)
		{
			var solutionLength = solution.Count;
			var score = 0;
			if (solutionLength >= _minMatchNum)
			{
				score += _minMatchScore;
				solutionLength -= _minMatchNum;
			}

			while (solutionLength > 0)
			{
				score += _bonusMatchScore;
				solutionLength--;
			}

			return score;
		}
	}
}