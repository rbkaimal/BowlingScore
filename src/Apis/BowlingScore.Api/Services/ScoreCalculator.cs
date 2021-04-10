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
        public Score CalculateScore(IEnumerable<int> pinsDowned)
        {
            //Create Frames
            var frames = CreateFrames(pinsDowned);
            //calcute framescores
            CalculateFrameScores(frames);
            //calculate score
            var score = CalculateScore(frames);
            return score;
        }
        private IDictionary<int,Frame> CreateFrames(IEnumerable<int> pinsDowned)
        {
            //todo:
            // validate input 

            var frames = new Dictionary<int,Frame>();
            var pinsDownedArray = pinsDowned.ToArray();
            var i = 0;
            var frameKey = 0;

            while (i < pinsDownedArray.Length)
            {
                var currentFrame = new Frame();
                frameKey = frameKey + 1;
                if (pinsDownedArray[i] == 10)
                {
                    //this is a strike
                    currentFrame.FirstThrowPinsDowned = 10;
                    currentFrame.SecondThrowPinsDowned = 0;
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
                    if (i + 1 < pinsDownedArray.Length)// make sure next value exists
                    {
                        currentFrame.SecondThrowPinsDowned = pinsDownedArray[i + 1];
                        currentFrame.Spare = (currentFrame.FirstThrowPinsDowned + currentFrame.SecondThrowPinsDowned) == 10;
                        currentFrame.Strike = false;
                        currentFrame.CanDetermineCorrectScore = currentFrame.Spare ? CanDetermineFrameScoreForSpare(pinsDownedArray, i) : true;
                        i = i + 2;
                    }
                    else
                    {
                        currentFrame.CanDetermineCorrectScore = false;
                        i = i + 1;
                    }
                    

                }
                frames.Add(frameKey, currentFrame);
            }
            return frames;

        }
        private void CalculateFrameScores(IDictionary<int, Frame> frames)
        {
            for(int i=1; i<=10; i++)
            {
                if (frames.ContainsKey(i))
                {
                    var currentFrame = frames[i];
                    currentFrame.FrameScore = CalculateFrameScore(frames, i);
                    frames[i] = currentFrame;
                }
                else
                    break;                  
               
            }
        }
        private Score CalculateScore(IDictionary<int, Frame> frames)
        {
            var gameCompleted = true;
            var progressScore = 0;
            var size = frames.Count > 10 ? 10 : frames.Count;
            var progressScores = new int[size];
            for (int i = 1; i <= 10; i++)
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
        private int CalculateFrameScore(IDictionary<int, Frame> frames , int currentFrameKey)
        {
            var currentFrame = frames[currentFrameKey];
            var currentScore = -1;
            if (!currentFrame.CanDetermineCorrectScore )
                return currentScore;
            else
            {
                currentScore = 0;
                if (currentFrame.Strike)
                {
                    var nextFrame = frames[currentFrameKey + 1];
                    currentScore = 10;
                    
                    if (!nextFrame.Strike)
                        currentScore = currentScore + nextFrame.FirstThrowPinsDowned + nextFrame.SecondThrowPinsDowned;                    
                    else 
                        currentScore = currentScore + 10 + frames[currentFrameKey + 2].FirstThrowPinsDowned;
                }
                else if (currentFrame.Spare)
                {
                    var nextFrame = frames[currentFrameKey + 1];
                    currentScore = 10 + nextFrame.FirstThrowPinsDowned;

                }
                else
                {
                    currentScore = currentFrame.FirstThrowPinsDowned + currentFrame.SecondThrowPinsDowned;
                }
                
            }
            return currentScore;
        }
    }
}
