using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Models
{
    public class Frame
    {
        public int FrameNo { get; set; }
        public int FirstThrowPinsDowned { get; set; }
        public int SecondThrowPinsDowned { get; set; }
        public int BonusThrowPinsDowned { get; set; }
        public bool Strike { get; set; }
        public bool Spare { get; set; }
        public bool CanDetermineCorrectScore { get; set; }
        public bool LastFrame { get; set; }

        public int FrameScore { get; set; }
        public int TotalPinsDowned {
            get 
            {
                return FirstThrowPinsDowned + SecondThrowPinsDowned + BonusThrowPinsDowned;
            }
            private set
            { 
            }
        }

        


    }
}
