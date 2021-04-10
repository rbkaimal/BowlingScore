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
        [Fact]
        public void CalculateScore_Should_Throw_Error_For_InvalidPinsDowned()
        {
            //arrange
            var pinsdowned = new int[] { -1, 22, 7, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
            
            //Act &Assert
            Assert.Throws<ArgumentException>(() => _scoreCalculator.CalculateScore(pinsdowned));
        }
        [Fact]
        public void CalculateScore_Should_Throw_Error_When_PinsDowned_Size_Morethan_21()
        {
            //arrange
            var pinsdowned = new int[] { -1, 22, 7, 10, 10, 10, 10, 10, 10, 10, 10, 10 - 1, 22, 7, 10, 10, 10, 10, 10, 10, 10, 10, 10 };

            //Act &Assert
            Assert.Throws<ArgumentException>(() => _scoreCalculator.CalculateScore(pinsdowned));
        }
        [Theory]
        [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, new int[] { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300 }, true)]
        [InlineData(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, new int[] { 2, 4, 6, 8, 10, 12 }, false)]
        [InlineData(new int[] { 1, 1, 1, 1, 9, 1, 2, 8, 9, 1, 10, 10 }, new int[] { 2, 4, 16, 35, 55, -1, -1 }, false)]
        [InlineData(new int[] { 2, 2 }, new int[] { 4 }, false)]
        [InlineData(new int[] { 3, 7 }, new int[] { -1 }, false)]
        [InlineData(new int[] { 3, 7, 4, 5, 10, 6, 4, 2, 5, 7, 1, 10, 4, 3, 10, 10, 10, 10 }, new int[] { 14, 23, 43, 55, 62, 70, 87, 94, 124, 154 }, true)]
        [InlineData(new int[] { 3, 4, 5, 1, 2, 6, 10, 5, 4, 5 }, new int[] { 7, 13, 21, 40, 49, -1 }, false)]
        [InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, false)]
        [InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, true)]
        [InlineData(new int[] {  }, new int[] {}, false)]
        [InlineData(new int[] { 3, 7, 4, 4, 7, 1, 10, 6, 3, 2, 7, 2, 7, 9, 1, 0, 2, 7, 3, 0 }, new int[] { 14, 22, 30, 49, 58, 67, 76, 86, 88, 98 }, true)]
        [InlineData(new int[] { 3, 7, 4, 4, 7, 1, 10, 6, 3, 2, 7, 2, 7, 9, 1, 0, 2, 7, 2 }, new int[] { 14, 22, 30, 49, 58, 67, 76, 86, 88, 97 }, true)]
        public void GetScores(int[] pinsdowned, int[] expectedProgressScores, bool gameCompleted)
        {
            //Act
            var result = _scoreCalculator.CalculateScore(pinsdowned);
            //Assert
            Assert.Equal(result.IsGameCompleted, gameCompleted);
            Assert.Equal(result.FrameProgresScores, expectedProgressScores);
        }
    }
}