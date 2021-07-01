using System;
using System.Collections.Generic;
using System.Text;

namespace NovaPlay.Object.Scoreboard
{
    public enum ScoreboardSort
    {
        Ascending = 0,
        Descending = 1
    }

    public class NovaScoreboardObjective
    {

        public String Name { get; set; }
        public ScoreboardDisplaySlot DisplaySlot { get; set; }
        public ScoreboardCriteria Criteria { get; set; }
        public String DisplayName { get; set; }
        public Dictionary<string, NovaScore> Scores = new Dictionary<string, NovaScore>();

        public byte Sort = (int)ScoreboardSort.Descending;

        public void SetDisplayName(String displayName)
        {
            this.DisplayName = displayName;
        }

        public string GetDisplayName()
        {
            return this.DisplayName;
        }

        public void SetDisplaySlot(ScoreboardDisplaySlot displaySlot)
        {
            this.DisplaySlot = displaySlot;
        }

        public ScoreboardDisplaySlot getDisplaySlot()
        {
            return this.DisplaySlot;
        }

        public void RegisterScore(string id, string text, int value)
        {
            RegisterScore(id, text, value, 0);
        }

        private void RegisterScore(string id, string text, int value, int type)
        {
            NovaScore score = new NovaScore(this, text);
            score.SetScore(value);
            score.FakeId = id;
            score.AddOrRemove = (byte)type;
            if (!Scores.ContainsKey(id))
            {
                Scores.Add(id, score);
            }
            else
            {
            }
        }

        public void SetScore(string id, int value)
        {
            if (Scores.ContainsKey(id))
            {
                NovaScore modified = Scores[id];
                modified.SetScore(value);
                Scores.Remove(id);
                Scores.Add(id, modified);
            }
        }

        public void SetScoreText(string id, string text)
        {
            if (Scores.ContainsKey(id))
            {
                NovaScore old = Scores[id];
                old.AddOrRemove = 1;
                old.FakeId = id + "_old_changed";

                NovaScore nju = new NovaScore(this, text);
                nju.SetScore(old.GetScore());
                nju.FakeId = id;
                Scores.Remove(id);
                Scores.Add(id, nju);
                Scores.Add(id + "_old_changed", old);
            }
        }

        public int GetScore(string id)
        {
            int i = 0;
            if (Scores.ContainsKey(id))
            {
                NovaScore score = Scores[id];
                i = score.GetScore();
            }
            return i;
        }

        public void ResetScore(String id)
        {
            if (Scores.ContainsKey(id))
            {
                NovaScore modified = Scores[id];
                modified.AddOrRemove = 1;
                Scores.Remove(id);
                Scores.Add(id, modified);
            }
        }

        public string CriteriaToString()
        {
            string s = "";
            switch (Criteria)
            {
                case ScoreboardCriteria.Dummy:
                    s = "dummy";
                    break;
            }
            return s;
        }

        public string SlotToString()
        {
            string slot = "";
            switch (DisplaySlot)
            {
                case ScoreboardDisplaySlot.Sidebar:
                    slot = "sidebar";
                    break;
                case ScoreboardDisplaySlot.List:
                    slot = "list";
                    break;
                case ScoreboardDisplaySlot.BelowName:
                    slot = "belowname";
                    break;
            }
            return slot;
        }

    }
}
