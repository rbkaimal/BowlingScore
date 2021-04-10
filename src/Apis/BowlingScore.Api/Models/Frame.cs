using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Models
{
    public class Frame
    {
        public int FirstThrowPinsDowned { get; set; }
        public int SecondThrowPinsDowned { get; set; }
        public bool Strike { get; set; }
        public bool Spare { get; set; }
        public int FrameScore { get; set; }
        public bool CanDetermineCorrectScore { get; set; }

    }
}
