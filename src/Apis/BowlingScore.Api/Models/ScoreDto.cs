using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Models
{
    public class ScoreDto
    {
        
        public IEnumerable<string> FrameProgresScores { get; set; }
        public bool GameCompleted { get; set; }
    }
}
