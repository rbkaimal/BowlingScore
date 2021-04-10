using BowlingScore.Api.Infrastructure.Exceptions;
using BowlingScore.Api.Models;
using BowlingScore.Api.Services.Interface;
using BowlingScore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BowlingScore.Controllers
{
    [ApiController]
    [Route("/scores")]
    public class ScoresController : ControllerBase
    {
        private readonly ILogger<ScoresController> _logger;
        private readonly IScoreCalculator _scoreCalculator;
        public ScoresController(ILogger<ScoresController> logger, IScoreCalculator scoreCalculator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scoreCalculator = scoreCalculator ?? throw new ArgumentNullException(nameof(scoreCalculator)); 
        }
        [HttpGet]
        public ActionResult Scores()
        {
            return Ok("Welcome!!");
        }
        [HttpPost]
        public async Task<ActionResult<ScoreDto>> Scores(PinsDownedInfo pinsDowned)
        {
            if (pinsDowned == null || pinsDowned.PinsDowned == null)
                throw new BowlingScoreApiExceptions($"Invalid input, pinsDowned cannot be null.");
            var score = await Task.Run(()=>_scoreCalculator.CalculateScore(pinsDowned.PinsDowned));
            var result = new ScoreDto
            {
                GameCompleted = score.IsGameCompleted,
                FrameProgresScores = score.FrameProgresScores.Select(x => x == -1 ? "*" : x.ToString())
            };
            return Ok(result);
        }
    }
}
