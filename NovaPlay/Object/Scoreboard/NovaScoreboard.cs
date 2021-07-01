using System;
using System.Collections.Generic;
using System.Text;

namespace NovaPlay.Object.Scoreboard
{
    public class NovaScoreboard
    {
        public NovaScoreboardObjective objective { get; set; }
        public long id { get; set; }
        public bool AlreadyCreated = false;

        public NovaScoreboard()
        {
            id = -RandomId();
        }

        public NovaScoreboardObjective RegisterNewObjective(string name, ScoreboardCriteria criteria)
        {
            var obj = new NovaScoreboardObjective()
            {
                Name = name,
                Criteria = criteria
            };
            objective = obj;
            return objective;
        }

        public NovaScoreboardObjective GetObjective()
        {
            return objective;
        }

        public static long RandomId()
        {
            Random rnd = new Random();

            byte[] buf = new byte[8];
            rnd.NextBytes(buf);
            long intRand = BitConverter.ToInt64(buf, 0);

            long result = (Math.Abs(intRand % (20000000000 - 10000000000)) + 10000000000);

            long random_seed = (long)rnd.Next(1000, 5000);
            random_seed = random_seed * result + rnd.Next(1000, 5000);
            long randomlong = ((long)(random_seed / 655) % 10000000001);
            return randomlong;
        }


    }
}
