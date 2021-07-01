using MiNET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovaPlay.Object.Scoreboard
{
    public class NovaScore
    {
        public long ScoreId { get; set; }
        public NovaScoreboardObjective Objective { get; set; }
        public string FakePlayer { get; set; }
        public int Score { get; set; }
        public long Id { get; set; }
        public bool IsFake { get; set; }
        public string FakeId { get; set; }
        public byte AddOrRemove { get; set; }

        public NovaScore(NovaScoreboardObjective obj, string fakeplayer)
        {
            ScoreId = NovaScoreboard.RandomId();
            Objective = obj;
            FakePlayer = fakeplayer;
            IsFake = true;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public int GetScore()
        {
            return Score;
        }

    }
}
