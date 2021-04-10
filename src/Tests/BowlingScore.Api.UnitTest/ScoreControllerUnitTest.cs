using BowlingScore.Api.Infrastructure.Exceptions;
using BowlingScore.Api.Services.Interface;
using BowlingScore.Controllers;
using BowlingScore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BowlingScore.Api.UnitTest
{
    public class ScoreControllerUnitTest
    {
        private readonly ScoresController _scoreController;
        private readonly Mock<ILogger<ScoresController>> _loggerMock;
        private readonly Mock<IScoreCalculator> _scoreCalculatorMock;
        public ScoreControllerUnitTest()
        {
            _loggerMock = new Mock<ILogger<ScoresController>>();
            _scoreCalculatorMock = new Mock<IScoreCalculator>();
            _scoreController = new ScoresController(_loggerMock.Object, _scoreCalculatorMock.Object);
        }
        [Fact]
        public async Task Post_Scores_Should_Return_Success()
        {
            //arrange
            var pinsDowned = new PinsDownedInfo { PinsDowned = new int[] { 1, 1, 1, 1, 9, 1, 2, 8, 9, 1, 10, 10 } };
            _scoreCalculatorMock.Setup(x => x.CalculateScore(pinsDowned.PinsDowned))
                                .Returns(new Models.Score 
                                { 
                                    IsGameCompleted = false,
                                    FrameProgresScores = new int[] { 2, 4, 16, 35, 55, -1, -1 }
                                });
            //act
            var result = await _scoreController.Scores(pinsDowned);
            //Asset
            Assert.Equal((result.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }
        [Fact]
        public async Task Post_Scores_Throws_Error_WhenInput_IsNotvalid()
        {
            //arrange
            var pinsDowned = new PinsDownedInfo { PinsDowned = null };

            //act && Asset
            await Assert.ThrowsAsync<BowlingScoreApiExceptions>(async () => await _scoreController.Scores(pinsDowned));
            
            
        }
    }
}