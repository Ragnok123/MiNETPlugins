using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Net;
using MiNET.Utils;

namespace NovaPlay.Object
{
    class MinecraftPackets
    {
    }

    public partial class McpeSetDefaultGameType : Packet<McpeSetDefaultGameType>
    {

        public int gamemode; // = null;

        public McpeSetDefaultGameType()
        {
            Id = 0x69;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            WriteVarInt(gamemode);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            gamemode = ReadVarInt();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            gamemode = default(int);
        }

    }

    public partial class McpeRemoveObjective : Packet<McpeRemoveObjective>
    {

        public string objectiveName; // = null;

        public McpeRemoveObjective()
        {
            Id = 0x6a;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            Write(objectiveName);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            objectiveName = ReadString();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            objectiveName = default(string);
        }

    }

    public partial class McpeSetDisplayObjective : Packet<McpeSetDisplayObjective>
    {

        public string displaySlot; // = null;
        public string objectiveName; // = null;
        public string displayName; // = null;
        public string criteriaName; // = null;
        public int sortOrder; // = null;

        public McpeSetDisplayObjective()
        {
            Id = 0x6b;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            Write(displaySlot);
            Write(objectiveName);
            Write(displayName);
            Write(criteriaName);
            WriteSignedVarInt(sortOrder);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            displaySlot = ReadString();
            objectiveName = ReadString();
            displayName = ReadString();
            criteriaName = ReadString();
            sortOrder = ReadSignedVarInt();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            displaySlot = default(string);
            objectiveName = default(string);
            displayName = default(string);
            criteriaName = default(string);
            sortOrder = default(int);
        }

    }

    public partial class McpeSetScore : Packet<McpeSetScore>
    {
        public enum Types
        {
            ModifyScore = 0,
            RemoveScore = 1,
        }

        public byte type; // = null;
        public Object.Scoreboard.ScorePacketInfos scorePacketInfos; // = null;

        public McpeSetScore()
        {
            Id = 0x6c;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            Write(type);
            Write(scorePacketInfos);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            type = ReadByte();
            scorePacketInfos = ReadScorePacketInfos();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            type = default(byte);
            scorePacketInfos = default(Object.Scoreboard.ScorePacketInfos);
        }

        public void Write(Object.Scoreboard.ScorePacketInfos list)
        {
            WriteUnsignedVarInt((uint)list.Count);
            foreach (var entry in list)
            {
                WriteVarLong(entry.scoreboardId);
                Write(entry.objectiveName);
                Write((uint)entry.score);
                Write(entry.addType);
                switch (entry.addType)
                {
                    case 1:
                    case 2:
                        WriteVarLong(entry.entityId);
                        break;
                    case 3:
                        Write(entry.fakePlayer);
                        break;
                }
            }
        }

        public Object.Scoreboard.ScorePacketInfos ReadScorePacketInfos()
        {
            var list = new Object.Scoreboard.ScorePacketInfos();

            var length = ReadUnsignedVarInt();
            for (var i = 0; i < length; ++i)
            {
                var entry = new Object.Scoreboard.ScorePacketInfo();
                entry.scoreboardId = ReadVarLong();
                entry.objectiveName = ReadString();
                entry.score = (int)ReadUint();
                entry.addType = ReadByte();
                switch (entry.addType)
                {
                    case 1:
                    case 2:
                        entry.entityId = ReadVarLong();
                        break;
                    case 3:
                        entry.fakePlayer = ReadString();
                        break;
                }
                list.Add(entry);
            }

            return list;
        }

    }

    public partial class McpeLabTable : Packet<McpeLabTable>
    {

        public byte uselessByte; // = null;
        public int labTableX; // = null;
        public int labTableY; // = null;
        public int labTableZ; // = null;
        public byte reactionType; // = null;

        public McpeLabTable()
        {
            Id = 0x6d;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            Write(uselessByte);
            WriteVarInt(labTableX);
            WriteVarInt(labTableY);
            WriteVarInt(labTableZ);
            Write(reactionType);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            uselessByte = ReadByte();
            labTableX = ReadVarInt();
            labTableY = ReadVarInt();
            labTableZ = ReadVarInt();
            reactionType = ReadByte();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            uselessByte = default(byte);
            labTableX = default(int);
            labTableY = default(int);
            labTableZ = default(int);
            reactionType = default(byte);
        }

    }

    public partial class McpeUpdateBlockSynced : Packet<McpeUpdateBlockSynced>
    {

        public BlockCoordinates coordinates; // = null;
        public uint blockRuntimeId; // = null;
        public uint blockPriority; // = null;
        public uint dataLayerId; // = null;
        public long unknown0; // = null;
        public long unknown1; // = null;

        public McpeUpdateBlockSynced()
        {
            Id = 0x6e;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            Write(coordinates);
            WriteUnsignedVarInt(blockRuntimeId);
            WriteUnsignedVarInt(blockPriority);
            WriteUnsignedVarInt(dataLayerId);
            WriteUnsignedVarLong(unknown0);
            WriteUnsignedVarLong(unknown1);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            coordinates = ReadBlockCoordinates();
            blockRuntimeId = ReadUnsignedVarInt();
            blockPriority = ReadUnsignedVarInt();
            dataLayerId = ReadUnsignedVarInt();
            unknown0 = ReadUnsignedVarLong();
            unknown1 = ReadUnsignedVarLong();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            coordinates = default(BlockCoordinates);
            blockRuntimeId = default(uint);
            blockPriority = default(uint);
            dataLayerId = default(uint);
            unknown0 = default(long);
            unknown1 = default(long);
        }

    }

    public partial class McpeMoveEntityDelta : Packet<McpeMoveEntityDelta>
    {

        public long runtimeEntityId; // = null;
        public byte flags; // = null;

        public McpeMoveEntityDelta()
        {
            Id = 0x6f;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();

            WriteUnsignedVarLong(runtimeEntityId);
            Write(flags);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();

            runtimeEntityId = ReadUnsignedVarLong();
            flags = ReadByte();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();

            runtimeEntityId = default(long);
            flags = default(byte);
        }

    }

    public partial class McpeSetScoreboardIdentity : Packet<McpeSetScoreboardIdentity>
    {
        public enum Types
        {
            RegisterIdentity = 0,
            ClearIdentity = 1
        }

        public byte type; // = null;
        public ScoreboardIdentityPackets scoreboardIdentityPackets; // = null;

        public McpeSetScoreboardIdentity()
        {
            Id = 0x70;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();
            Write(type);
            Write(scoreboardIdentityPackets, type);
            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();
            type = ReadByte();
            scoreboardIdentityPackets = ReadScoreboardIdentityPackets(type);
            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();
            type = default(byte);
            scoreboardIdentityPackets = default(ScoreboardIdentityPackets);
        }

        public void Write(ScoreboardIdentityPackets sip, byte type)
        {
            WriteUnsignedVarInt((uint)sip.Count);
            foreach (var list in sip)
            {
                Write(list.ScoreboardId);
                if (type == 0)
                {
                    WriteVarLong(list.EntityId);
                }
            }
        }

        public ScoreboardIdentityPackets ReadScoreboardIdentityPackets(byte type)
        {
            var list = new ScoreboardIdentityPackets();

            var length = ReadUnsignedVarInt();
            for (var i = 0; i < length; ++i)
            {
                var entry = new ScoreboardIdentityPacket();
                entry.ScoreboardId = ReadVarLong();
                if (type == 0)
                {
                    entry.EntityId = ReadVarLong();
                }
                list.Add(entry);
            }

            return list;
        }

    }

    public class ScoreboardIdentityPackets : List<ScoreboardIdentityPacket> { }

    public class ScoreboardIdentityPacket
    {
        public long ScoreboardId { get; set; }
        public long EntityId { get; set; }


    }

    public partial class McpeSetLocalPlayerAsInitialized : Packet<McpeSetLocalPlayerAsInitialized>
    {

        public long runtimeEntityId; // = null;

        public McpeSetLocalPlayerAsInitialized()
        {
            Id = 0x71;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();
            WriteUnsignedVarLong(runtimeEntityId);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();
            runtimeEntityId = ReadUnsignedVarLong();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();
            runtimeEntityId = default(long);
        }

    }

    public class EnumValues : List<string> { }

    public partial class McpeUpdateSoftEnum : Packet<McpeUpdateSoftEnum>
    {
        public string enumName; // = null;
        public EnumValues values; // = null;
        public byte type; // = null;

        public McpeUpdateSoftEnum()
        {
            Id = 0x72;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();
            Write(enumName);
            Write(values);
            Write(type);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();
            enumName = ReadString();
            values = ReadEnumValues();
            type = ReadByte();
            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();
            enumName = default(string);
            values = default(EnumValues);
            type = default(byte);
        }

        public void Write(EnumValues values)
        {
            WriteUnsignedVarInt((uint)values.Count);
            foreach (var value in values)
            {
                Write(value);
            }
        }

        public EnumValues ReadEnumValues()
        {
            var list = new EnumValues();
            var length = ReadUnsignedVarInt();
            for (int i = 0; i <= length; i++)
            {
                list.Add(ReadString());
            }
            return list;
        }

    }

    public partial class McpeNetworkStackLatency : Packet<McpeNetworkStackLatency>
    {

        public ulong timestamp;

        public McpeNetworkStackLatency()
        {
            Id = 0x73;
            IsMcpe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();

            BeforeEncode();
            Write(timestamp);

            AfterEncode();
        }

        partial void BeforeEncode();
        partial void AfterEncode();

        protected override void DecodePacket()
        {
            base.DecodePacket();

            BeforeDecode();
            timestamp = ReadUlong();

            AfterDecode();
        }

        partial void BeforeDecode();
        partial void AfterDecode();

        protected override void ResetPacket()
        {
            base.ResetPacket();
            timestamp = default(ulong);
        }

    }

}
