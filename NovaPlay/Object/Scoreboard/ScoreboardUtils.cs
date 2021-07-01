using System;
using System.Collections.Generic;
using System.Text;

namespace NovaPlay.Object.Scoreboard
{
    public enum ScoreboardCriteria
    {
        Dummy
    }

    public enum ScoreboardDisplaySlot
    {
        List,
        Sidebar,
        BelowName
    }

    public class ScorePacketInfos : List<ScorePacketInfo> { }

    public class ScorePacketInfo
    {
        public long scoreboardId { get; set; }
        public string objectiveName { get; set; }
        public int score { get; set; }
        public long entityId { get; set; }
        public string fakePlayer { get; set; }
        public byte addType;
    }

}
