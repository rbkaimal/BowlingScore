using BowlingScore.Api.Services;
using System;
using System.Linq;
using Xunit;

namespace BowlingScore.Api.UnitTest
{
    public class ScoreCalculatorUnitTest
    {
        private readonly ScoreCalculator _scoreCalculator;
        public ScoreCalculatorUnitTest()
        {
            _scoreCalculator = new ScoreCalculator();

        }
        [Theory]
        [InlineData(new int[] {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}, new int[] { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300 }, true)]
        [InlineData(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, new int[] { 2,4,6,8,10,12 }, false)]
        [InlineData(new int[] { 1, 1, 1, 1, 9, 1, 2, 8, 9, 1, 10, 10 }, new int[] { 2, 4, 16, 35, 55, -1, -1 }, false)]
        public void GetScores(int[] pinsdowned, int[] expectedProgressScores, bool gameCompleted)
        {
            //Act
            var result = _scoreCalculator.CalculateScore(pinsdowned.ToArray());
            //Assert
            Assert.Equal(result.IsGameCompleted, gameCompleted);
            Assert.Equal(result.FrameProgresScores, expectedProgressScores);
        }
    }
}
