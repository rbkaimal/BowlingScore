using BowlingScore.Api.Models;
using BowlingScore.Api.Services.Interface;
using BowlingScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingScore.Api.Services
{
    public class ScoreCalculator : IScoreCalculator
    {
        private const int StrikePinsCount = 10;
        private const int MaxFrames = 10;
        private const int MaxSizePinsDowned = 21;
        
        public Score CalculateScore(IEnumerable<int> pinsDowned)
        {
            if(pinsDowned.Count() > MaxSizePinsDowned)
                throw new ArgumentException($"Invalid values in {nameof(pinsDowned)}. Pins down length cannot exceed {MaxSizePinsDowned}  ");
            if (pinsDowned.Any(x => x < 0 || x > 10))
                throw new ArgumentException($"Invalid values in {nameof(pinsDowned)}. Pins down in each throw should be between 0 and 10  ");
            //Create Frames
            var frames = CreateFrames(pinsDowned);
            //calcute framescores
            CalculateFrameScores(frames);
            //calculate score
            var score = CalculateScore(frames);
            return score;
        }

       

        //here we create frames from the input array of pinsDowned
        private IDictionary<int,Frame> CreateFrames(IEnumerable<int> pinsDowned)
        {
            var frames = new Dictionary<int,Frame>();
            var pinsDownedArray = pinsDowned.ToArray();
            var i = 0;
            var frameKey = 0;

            while (i < pinsDownedArray.Length)
            {
                frameKey = frameKey + 1;
                var currentFrame = new Frame { FrameNo = frameKey };                
                if (pinsDownedArray[i] == StrikePinsCount)
                {
                    //this is a strike
                    currentFrame.FirstThrowPinsDowned = StrikePinsCount;
                    currentFrame.Strike = true;
                    currentFrame.Spare = false;
                    currentFrame.CanDetermineCorrectScore = CanDetermineFrameScoreForStrike(pinsDownedArray, i);
                                        
                    i = i + 1;                    

                }                
                else
                {
                    //take value from next element
                    //this could be spare or open frame 
                    currentFrame.FirstThrowPinsDowned = pinsDownedArray[i];
                    i = i + 1; //move to next
                    if (i  < pinsDownedArray.Length)// make sure next value exists
                    {
                        currentFrame.SecondThrowPinsDowned = pinsDownedArray[i];
                        currentFrame.Spare = (currentFrame.FirstThrowPinsDowned + currentFrame.SecondThrowPinsDowned) == StrikePinsCount;
                        currentFrame.Strike = false;
                        currentFrame.CanDetermineCorrectScore = currentFrame.Spare ? CanDetermineFrameScoreForSpare(pinsDownedArray, i) : true;
                        i = i + 1;
                    }
                    else
                    {
                        currentFrame.CanDetermineCorrectScore = false;
                    }
                    

                }
                if (IsLastFrame(frameKey))
                {
                    currentFrame.LastFrame = true;
                    UpdateLastFrame(currentFrame, pinsDownedArray, i);
                    i = i + 2;
                }
                frames.Add(frameKey, currentFrame);
            }
            return frames;

        }
        private void UpdateLastFrame(Frame frame, int[] pinsDownedArray, int index)
        {
            
            if (frame.Strike && frame.CanDetermineCorrectScore)
            {
                frame.SecondThrowPinsDowned = pinsDownedArray[index ];
                frame.BonusThrowPinsDowned = pinsDownedArray[index + 1];
            }
            else if (frame.Spare && frame.CanDetermineCorrectScore)
            {
                frame.BonusThrowPinsDowned = pinsDownedArray[index ];
            }
            
        }
        private void CalculateFrameScores(IDictionary<int, Frame> frames)
        {
            for(int i=1; i<=10; i++)
            {
                if (frames.ContainsKey(i))
                    frames[i].FrameScore = GetFrameScore(frames, frames[i]);
                else
                    break;                  
               
            }
        }
        private Score CalculateScore(IDictionary<int, Frame> frames)
        {
            var gameCompleted = true;
            var progressScore = 0;
            var progressScores = new int[frames.Count];
            for (int i = 1; i <= MaxFrames; i++)
            {
                if (frames.ContainsKey(i))
                {
                    if (frames[i].CanDetermineCorrectScore)
                    {
                        progressScore = progressScore + frames[i].FrameScore;
                        progressScores[i - 1] = progressScore;
                    }
                    else
                    {
                        progressScores[i - 1] = -1;
                        gameCompleted = false;
                    }
                }
                else
                {
                    gameCompleted = false;
                    break;
                }
            }
            return new Score { FrameProgresScores = progressScores, IsGameCompleted = gameCompleted, TotalScore = progressScore };
        }
        private bool CanDetermineFrameScoreForStrike(int[] pinsDownedArray, int currentIndex)
        {
            //if we have score for next two rolls 
            return currentIndex + 2 < pinsDownedArray.Length;
        }
        private bool CanDetermineFrameScoreForSpare(int[] pinsDownedArray, int currentIndex)
        {
            //if we have score for next roll 
            return currentIndex + 1 < pinsDownedArray.Length;
        }
        private bool IsLastFrame(int frameKey)
        {
            return frameKey == MaxFrames;
        }
        public int GetFrameScore(IDictionary<int, Frame> frames, Frame frame)
        {
            var score = -1;
            if (!frame.CanDetermineCorrectScore)
                return score;
            if (frame.LastFrame)
                return frame.TotalPinsDowned;
            else
            {
                if (frame.Strike && frame.CanDetermineCorrectScore)
                {
                    //if we have a strike then we need to add pins from next frame
                    if (frames.TryGetValue(frame.FrameNo + 1, out var nextFrame))
                    {
                        if (!nextFrame.Strike)
                        {
                            return frame.TotalPinsDowned + nextFrame.FirstThrowPinsDowned + nextFrame.SecondThrowPinsDowned;

                        }
                        else //if next frame is strike then we need pins from following frame
                        {
                            if (nextFrame.LastFrame)
                            {
                                return frame.TotalPinsDowned + nextFrame.FirstThrowPinsDowned + nextFrame.SecondThrowPinsDowned;
                            }
                            else if (frames.TryGetValue(frame.FrameNo + 2, out var secondFrame))
                            {
                                return frame.TotalPinsDowned + nextFrame.FirstThrowPinsDowned + secondFrame.FirstThrowPinsDowned;
                            }
                            
                        }
                    }

                }
                else if (frame.Spare && frame.CanDetermineCorrectScore)
                {
                    //if we have spare we need to add pins from  first throw for next frame
                    if (frames.TryGetValue(frame.FrameNo + 1, out var nextFrame))
                        return frame.TotalPinsDowned + nextFrame.FirstThrowPinsDowned;

                }
                else
                {
                   return frame.TotalPinsDowned;
                }

            }
            //we should not reach here, in case the we could nto determine sorrect score
            frame.CanDetermineCorrectScore = false;
            return score;
        }

      
    }
}
