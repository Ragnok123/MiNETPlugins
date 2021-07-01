using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NovaPlay.Object;
using MiNET.Utils;
using MiNET.Blocks;
using MiNET.Worlds;

namespace NovaPlay.MiniGamesAPI
{
    public abstract class GameAPI
    {

        public Dictionary<string, GamePlayer> arenaplayers = new Dictionary<string, GamePlayer>();
        public Dictionary<string, PlayerLocation> arenadata;
        public Dictionary<string, BlockCoordinates> arenacoordsdata;
        public Team[] teams = new Team[5];
        public Level _world;

        public string arenaname;
        public string worldname;
        public string gameType;
        public string gamePrefix;

        public int waitTime;
        public int godTime;
        public int gameTime;
        public int endTime;
        public int lastTime;
        public int gameStatus;
        public int maxPlayers;
        

        public string GetGameType()
        {
            return this.gameType;
        }

        public int GetPlayerCount()
        {
            return arenaplayers.Count();
        }

        public int GetMaxPlayerCount()
        {
            return this.maxPlayers;
        }

        public bool IsInArena(PlayerData data)
        {
            return arenaplayers.ContainsKey(data.GetPlayer().Username);
        }

        public void AddPlayer(PlayerData data)
        {
            NovaPlayer player = data.GetPlayer();
            data.gData = new GamePlayer(data, this);
            GamePlayer gData = data.gData;
            arenaplayers.Add(data.GetPlayer().Username, gData);
            Level toLevel = NovaCore.GetInstance().GetLevelByName(this.worldname);
            data.GetPlayer().SpawnLevel(toLevel, arenadata["spawnPos"]);
            player.SendMessage(this.gamePrefix + " §l§aConnecting to §b" + this.arenaname);
        }

        public void RemovePlayer(PlayerData data)
        {
            arenaplayers.Remove(data.GetPlayer().Username);
            data.gData = null;
            Level toLevel = NovaCore.GetInstance().GetLevelByName("Overworld");
            data.GetPlayer().SpawnLevel(toLevel, new PlayerLocation()
            {
                X = toLevel.SpawnPoint.X,
                Y = toLevel.SpawnPoint.Y,
                Z = toLevel.SpawnPoint.Z,
                Yaw = toLevel.SpawnPoint.Yaw,
                Pitch = toLevel.SpawnPoint.Pitch
            });
        }
        

        public Team GetPlayerTeam(GamePlayer data)
        {
            return data.GetTeam();
        }

        public void JoinTeam(int team, GamePlayer data)
        {
            teams[team].AddPlayer(data);
            data.team = teams[team];
        }

        public void LeaveTeam(int team, GamePlayer data)
        {
            teams[team].RemovePlayer(data);
            data.team = null;
        }



    }
}
