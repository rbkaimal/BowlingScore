using BowlingScore.Api.Models;
using BowlingScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Services.Interface
{
    public interface IScoreCalculator
    {
        public Score CalculateScore(IEnumerable<int> pinsDowned);
    }
}
