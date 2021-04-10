using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Models
{
    public class Score
    {
        public int TotalScore { get; set; }
        public int[] FrameProgresScores { get; set; }
        public bool IsGameCompleted { get; set; }
    }
}
