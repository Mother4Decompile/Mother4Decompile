using System;
using System.Collections.Generic;

namespace Mother4.Actors.Animation
{
	// Token: 0x02000004 RID: 4
	internal static class AnimationNames
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string GetString(AnimationType type)
		{
			string result;
			if (!AnimationNames.ANIMTYPE_TO_STRING.TryGetValue(type, out result))
			{
				result = "stand south";
			}
			return result;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
		public static AnimationType GetAnimationType(string name)
		{
			AnimationType result;
			if (!AnimationNames.STRING_TO_ANIMTYPE.TryGetValue(name, out result))
			{
				result = AnimationType.INVALID;
			}
			return result;
		}

		// Token: 0x04000020 RID: 32
		public const string EAST = "east";

		// Token: 0x04000021 RID: 33
		public const string NORTHEAST = "northeast";

		// Token: 0x04000022 RID: 34
		public const string NORTH = "north";

		// Token: 0x04000023 RID: 35
		public const string NORTHWEST = "northwest";

		// Token: 0x04000024 RID: 36
		public const string WEST = "west";

		// Token: 0x04000025 RID: 37
		public const string SOUTHWEST = "southwest";

		// Token: 0x04000026 RID: 38
		public const string SOUTH = "south";

		// Token: 0x04000027 RID: 39
		public const string SOUTHEAST = "southeast";

		// Token: 0x04000028 RID: 40
		public const string STAND = "stand";

		// Token: 0x04000029 RID: 41
		public const string WALK = "walk";

		// Token: 0x0400002A RID: 42
		public const string RUN = "run";

		// Token: 0x0400002B RID: 43
		public const string CROUCH = "crouch";

		// Token: 0x0400002C RID: 44
		public const string DEAD = "dead";

		// Token: 0x0400002D RID: 45
		public const string IDLE = "idle";

		// Token: 0x0400002E RID: 46
		public const string TALK = "talk";

		// Token: 0x0400002F RID: 47
		public const string BLINK = "blink";

		// Token: 0x04000030 RID: 48
		public const string CLIMB = "climb";

		// Token: 0x04000031 RID: 49
		public const string SWIM = "swim";

		// Token: 0x04000032 RID: 50
		public const string FLOAT = "float";

		// Token: 0x04000033 RID: 51
		public const string STAND_EAST = "stand east";

		// Token: 0x04000034 RID: 52
		public const string STAND_NORTHEAST = "stand northeast";

		// Token: 0x04000035 RID: 53
		public const string STAND_NORTH = "stand north";

		// Token: 0x04000036 RID: 54
		public const string STAND_NORTHWEST = "stand northwest";

		// Token: 0x04000037 RID: 55
		public const string STAND_WEST = "stand west";

		// Token: 0x04000038 RID: 56
		public const string STAND_SOUTHWEST = "stand southwest";

		// Token: 0x04000039 RID: 57
		public const string STAND_SOUTH = "stand south";

		// Token: 0x0400003A RID: 58
		public const string STAND_SOUTHEAST = "stand southeast";

		// Token: 0x0400003B RID: 59
		public const string WALK_EAST = "walk east";

		// Token: 0x0400003C RID: 60
		public const string WALK_NORTHEAST = "walk northeast";

		// Token: 0x0400003D RID: 61
		public const string WALK_NORTH = "walk north";

		// Token: 0x0400003E RID: 62
		public const string WALK_NORTHWEST = "walk northwest";

		// Token: 0x0400003F RID: 63
		public const string WALK_WEST = "walk west";

		// Token: 0x04000040 RID: 64
		public const string WALK_SOUTHWEST = "walk southwest";

		// Token: 0x04000041 RID: 65
		public const string WALK_SOUTH = "walk south";

		// Token: 0x04000042 RID: 66
		public const string WALK_SOUTHEAST = "walk southeast";

		// Token: 0x04000043 RID: 67
		public const string RUN_EAST = "run east";

		// Token: 0x04000044 RID: 68
		public const string RUN_NORTHEAST = "run northeast";

		// Token: 0x04000045 RID: 69
		public const string RUN_NORTH = "run north";

		// Token: 0x04000046 RID: 70
		public const string RUN_NORTHWEST = "run northwest";

		// Token: 0x04000047 RID: 71
		public const string RUN_WEST = "run west";

		// Token: 0x04000048 RID: 72
		public const string RUN_SOUTHWEST = "run southwest";

		// Token: 0x04000049 RID: 73
		public const string RUN_SOUTH = "run south";

		// Token: 0x0400004A RID: 74
		public const string RUN_SOUTHEAST = "run southeast";

		// Token: 0x0400004B RID: 75
		public const string CROUCH_EAST = "crouch east";

		// Token: 0x0400004C RID: 76
		public const string CROUCH_NORTHEAST = "crouch northeast";

		// Token: 0x0400004D RID: 77
		public const string CROUCH_NORTH = "crouch north";

		// Token: 0x0400004E RID: 78
		public const string CROUCH_NORTHWEST = "crouch northwest";

		// Token: 0x0400004F RID: 79
		public const string CROUCH_WEST = "crouch west";

		// Token: 0x04000050 RID: 80
		public const string CROUCH_SOUTHWEST = "crouch southwest";

		// Token: 0x04000051 RID: 81
		public const string CROUCH_SOUTH = "crouch south";

		// Token: 0x04000052 RID: 82
		public const string CROUCH_SOUTHEAST = "crouch southeast";

		// Token: 0x04000053 RID: 83
		public const string DEAD_EAST = "dead east";

		// Token: 0x04000054 RID: 84
		public const string DEAD_NORTHEAST = "dead northeast";

		// Token: 0x04000055 RID: 85
		public const string DEAD_NORTH = "dead north";

		// Token: 0x04000056 RID: 86
		public const string DEAD_NORTHWEST = "dead northwest";

		// Token: 0x04000057 RID: 87
		public const string DEAD_WEST = "dead west";

		// Token: 0x04000058 RID: 88
		public const string DEAD_SOUTHWEST = "dead southwest";

		// Token: 0x04000059 RID: 89
		public const string DEAD_SOUTH = "dead south";

		// Token: 0x0400005A RID: 90
		public const string DEAD_SOUTHEAST = "dead southeast";

		// Token: 0x0400005B RID: 91
		public const string IDLE_EAST = "idle east";

		// Token: 0x0400005C RID: 92
		public const string IDLE_NORTHEAST = "idle northeast";

		// Token: 0x0400005D RID: 93
		public const string IDLE_NORTH = "idle north";

		// Token: 0x0400005E RID: 94
		public const string IDLE_NORTHWEST = "idle northwest";

		// Token: 0x0400005F RID: 95
		public const string IDLE_WEST = "idle west";

		// Token: 0x04000060 RID: 96
		public const string IDLE_SOUTHWEST = "idle southwest";

		// Token: 0x04000061 RID: 97
		public const string IDLE_SOUTH = "idle south";

		// Token: 0x04000062 RID: 98
		public const string IDLE_SOUTHEAST = "idle southeast";

		// Token: 0x04000063 RID: 99
		public const string TALK_EAST = "talk east";

		// Token: 0x04000064 RID: 100
		public const string TALK_NORTHEAST = "talk northeast";

		// Token: 0x04000065 RID: 101
		public const string TALK_NORTH = "talk north";

		// Token: 0x04000066 RID: 102
		public const string TALK_NORTHWEST = "talk northwest";

		// Token: 0x04000067 RID: 103
		public const string TALK_WEST = "talk west";

		// Token: 0x04000068 RID: 104
		public const string TALK_SOUTHWEST = "talk southwest";

		// Token: 0x04000069 RID: 105
		public const string TALK_SOUTH = "talk south";

		// Token: 0x0400006A RID: 106
		public const string TALK_SOUTHEAST = "talk southeast";

		// Token: 0x0400006B RID: 107
		public const string BLINK_EAST = "blink east";

		// Token: 0x0400006C RID: 108
		public const string BLINK_NORTHEAST = "blink northeast";

		// Token: 0x0400006D RID: 109
		public const string BLINK_NORTH = "blink north";

		// Token: 0x0400006E RID: 110
		public const string BLINK_NORTHWEST = "blink northwest";

		// Token: 0x0400006F RID: 111
		public const string BLINK_WEST = "blink west";

		// Token: 0x04000070 RID: 112
		public const string BLINK_SOUTHWEST = "blink southwest";

		// Token: 0x04000071 RID: 113
		public const string BLINK_SOUTH = "blink south";

		// Token: 0x04000072 RID: 114
		public const string BLINK_SOUTHEAST = "blink southeast";

		// Token: 0x04000073 RID: 115
		public const string CLIMB_EAST = "climb east";

		// Token: 0x04000074 RID: 116
		public const string CLIMB_NORTHEAST = "climb northeast";

		// Token: 0x04000075 RID: 117
		public const string CLIMB_NORTH = "climb north";

		// Token: 0x04000076 RID: 118
		public const string CLIMB_NORTHWEST = "climb northwest";

		// Token: 0x04000077 RID: 119
		public const string CLIMB_WEST = "climb west";

		// Token: 0x04000078 RID: 120
		public const string CLIMB_SOUTHWEST = "climb southwest";

		// Token: 0x04000079 RID: 121
		public const string CLIMB_SOUTH = "climb south";

		// Token: 0x0400007A RID: 122
		public const string CLIMB_SOUTHEAST = "climb southeast";

		// Token: 0x0400007B RID: 123
		public const string SWIM_EAST = "swim east";

		// Token: 0x0400007C RID: 124
		public const string SWIM_NORTHEAST = "swim northeast";

		// Token: 0x0400007D RID: 125
		public const string SWIM_NORTH = "swim north";

		// Token: 0x0400007E RID: 126
		public const string SWIM_NORTHWEST = "swim northwest";

		// Token: 0x0400007F RID: 127
		public const string SWIM_WEST = "swim west";

		// Token: 0x04000080 RID: 128
		public const string SWIM_SOUTHWEST = "swim southwest";

		// Token: 0x04000081 RID: 129
		public const string SWIM_SOUTH = "swim south";

		// Token: 0x04000082 RID: 130
		public const string SWIM_SOUTHEAST = "swim southeast";

		// Token: 0x04000083 RID: 131
		public const string FLOAT_EAST = "float east";

		// Token: 0x04000084 RID: 132
		public const string FLOAT_NORTHEAST = "float northeast";

		// Token: 0x04000085 RID: 133
		public const string FLOAT_NORTH = "float north";

		// Token: 0x04000086 RID: 134
		public const string FLOAT_NORTHWEST = "float northwest";

		// Token: 0x04000087 RID: 135
		public const string FLOAT_WEST = "float west";

		// Token: 0x04000088 RID: 136
		public const string FLOAT_SOUTHWEST = "float southwest";

		// Token: 0x04000089 RID: 137
		public const string FLOAT_SOUTH = "float south";

		// Token: 0x0400008A RID: 138
		public const string FLOAT_SOUTHEAST = "float southeast";

		// Token: 0x0400008B RID: 139
		public const string NAUSEA_EAST = "nausea east";

		// Token: 0x0400008C RID: 140
		public const string NAUSEA_NORTHEAST = "nausea northeast";

		// Token: 0x0400008D RID: 141
		public const string NAUSEA_NORTH = "nausea north";

		// Token: 0x0400008E RID: 142
		public const string NAUSEA_NORTHWEST = "nausea northwest";

		// Token: 0x0400008F RID: 143
		public const string NAUSEA_WEST = "nausea west";

		// Token: 0x04000090 RID: 144
		public const string NAUSEA_SOUTHWEST = "nausea southwest";

		// Token: 0x04000091 RID: 145
		public const string NAUSEA_SOUTH = "nausea south";

		// Token: 0x04000092 RID: 146
		public const string NAUSEA_SOUTHEAST = "nausea southeast";

		// Token: 0x04000093 RID: 147
		private static readonly Dictionary<AnimationType, string> ANIMTYPE_TO_STRING = new Dictionary<AnimationType, string>
		{
			{
				(AnimationType)257,
				"stand east"
			},
			{
				(AnimationType)258,
				"stand northeast"
			},
			{
				(AnimationType)259,
				"stand north"
			},
			{
				(AnimationType)260,
				"stand northwest"
			},
			{
				(AnimationType)261,
				"stand west"
			},
			{
				(AnimationType)262,
				"stand southwest"
			},
			{
				(AnimationType)263,
				"stand south"
			},
			{
				(AnimationType)264,
				"stand southeast"
			},
			{
				(AnimationType)513,
				"walk east"
			},
			{
				(AnimationType)514,
				"walk northeast"
			},
			{
				(AnimationType)515,
				"walk north"
			},
			{
				(AnimationType)516,
				"walk northwest"
			},
			{
				(AnimationType)517,
				"walk west"
			},
			{
				(AnimationType)518,
				"walk southwest"
			},
			{
				(AnimationType)519,
				"walk south"
			},
			{
				(AnimationType)520,
				"walk southeast"
			},
			{
				(AnimationType)769,
				"run east"
			},
			{
				(AnimationType)770,
				"run northeast"
			},
			{
				(AnimationType)771,
				"run north"
			},
			{
				(AnimationType)772,
				"run northwest"
			},
			{
				(AnimationType)773,
				"run west"
			},
			{
				(AnimationType)774,
				"run southwest"
			},
			{
				(AnimationType)775,
				"run south"
			},
			{
				(AnimationType)776,
				"run southeast"
			},
			{
				(AnimationType)1025,
				"crouch east"
			},
			{
				(AnimationType)1026,
				"crouch northeast"
			},
			{
				(AnimationType)1027,
				"crouch north"
			},
			{
				(AnimationType)1028,
				"crouch northwest"
			},
			{
				(AnimationType)1029,
				"crouch west"
			},
			{
				(AnimationType)1030,
				"crouch southwest"
			},
			{
				(AnimationType)1031,
				"crouch south"
			},
			{
				(AnimationType)1032,
				"crouch southeast"
			},
			{
				(AnimationType)1281,
				"dead east"
			},
			{
				(AnimationType)1282,
				"dead northeast"
			},
			{
				(AnimationType)1283,
				"dead north"
			},
			{
				(AnimationType)1284,
				"dead northwest"
			},
			{
				(AnimationType)1285,
				"dead west"
			},
			{
				(AnimationType)1286,
				"dead southwest"
			},
			{
				(AnimationType)1287,
				"dead south"
			},
			{
				(AnimationType)1288,
				"dead southeast"
			},
			{
				(AnimationType)1537,
				"idle east"
			},
			{
				(AnimationType)1538,
				"idle northeast"
			},
			{
				(AnimationType)1539,
				"idle north"
			},
			{
				(AnimationType)1540,
				"idle northwest"
			},
			{
				(AnimationType)1541,
				"idle west"
			},
			{
				(AnimationType)1542,
				"idle southwest"
			},
			{
				(AnimationType)1543,
				"idle south"
			},
			{
				(AnimationType)1544,
				"idle southeast"
			},
			{
				(AnimationType)1793,
				"talk east"
			},
			{
				(AnimationType)1794,
				"talk northeast"
			},
			{
				(AnimationType)1795,
				"talk north"
			},
			{
				(AnimationType)1796,
				"talk northwest"
			},
			{
				(AnimationType)1797,
				"talk west"
			},
			{
				(AnimationType)1798,
				"talk southwest"
			},
			{
				(AnimationType)1799,
				"talk south"
			},
			{
				(AnimationType)1800,
				"talk southeast"
			},
			{
				(AnimationType)2049,
				"blink east"
			},
			{
				(AnimationType)2050,
				"blink northeast"
			},
			{
				(AnimationType)2051,
				"blink north"
			},
			{
				(AnimationType)2052,
				"blink northwest"
			},
			{
				(AnimationType)2053,
				"blink west"
			},
			{
				(AnimationType)2054,
				"blink southwest"
			},
			{
				(AnimationType)2055,
				"blink south"
			},
			{
				(AnimationType)2056,
				"blink southeast"
			},
			{
				(AnimationType)2305,
				"climb east"
			},
			{
				(AnimationType)2306,
				"climb northeast"
			},
			{
				(AnimationType)2307,
				"climb north"
			},
			{
				(AnimationType)2308,
				"climb northwest"
			},
			{
				(AnimationType)2309,
				"climb west"
			},
			{
				(AnimationType)2310,
				"climb southwest"
			},
			{
				(AnimationType)2311,
				"climb south"
			},
			{
				(AnimationType)2312,
				"climb southeast"
			},
			{
				(AnimationType)2561,
				"swim east"
			},
			{
				(AnimationType)2562,
				"swim northeast"
			},
			{
				(AnimationType)2563,
				"swim north"
			},
			{
				(AnimationType)2564,
				"swim northwest"
			},
			{
				(AnimationType)2565,
				"swim west"
			},
			{
				(AnimationType)2566,
				"swim southwest"
			},
			{
				(AnimationType)2567,
				"swim south"
			},
			{
				(AnimationType)2568,
				"swim southeast"
			},
			{
				(AnimationType)2817,
				"float east"
			},
			{
				(AnimationType)2818,
				"float northeast"
			},
			{
				(AnimationType)2819,
				"float north"
			},
			{
				(AnimationType)2820,
				"float northwest"
			},
			{
				(AnimationType)2821,
				"float west"
			},
			{
				(AnimationType)2822,
				"float southwest"
			},
			{
				(AnimationType)2823,
				"float south"
			},
			{
				(AnimationType)2824,
				"float southeast"
			},
			{
				(AnimationType)3073,
				"nausea east"
			},
			{
				(AnimationType)3074,
				"nausea northeast"
			},
			{
				(AnimationType)3075,
				"nausea north"
			},
			{
				(AnimationType)3076,
				"nausea northwest"
			},
			{
				(AnimationType)3077,
				"nausea west"
			},
			{
				(AnimationType)3078,
				"nausea southwest"
			},
			{
				(AnimationType)3079,
				"nausea south"
			},
			{
				(AnimationType)3080,
				"nausea southeast"
			}
		};

		// Token: 0x04000094 RID: 148
		private static readonly Dictionary<string, AnimationType> STRING_TO_ANIMTYPE = new Dictionary<string, AnimationType>
		{
			{
				"stand east",
				(AnimationType)257
			},
			{
				"stand northeast",
				(AnimationType)258
			},
			{
				"stand north",
				(AnimationType)259
			},
			{
				"stand northwest",
				(AnimationType)260
			},
			{
				"stand west",
				(AnimationType)261
			},
			{
				"stand southwest",
				(AnimationType)262
			},
			{
				"stand south",
				(AnimationType)263
			},
			{
				"stand southeast",
				(AnimationType)264
			},
			{
				"walk east",
				(AnimationType)513
			},
			{
				"walk northeast",
				(AnimationType)514
			},
			{
				"walk north",
				(AnimationType)515
			},
			{
				"walk northwest",
				(AnimationType)516
			},
			{
				"walk west",
				(AnimationType)517
			},
			{
				"walk southwest",
				(AnimationType)518
			},
			{
				"walk south",
				(AnimationType)519
			},
			{
				"walk southeast",
				(AnimationType)520
			},
			{
				"run east",
				(AnimationType)769
			},
			{
				"run northeast",
				(AnimationType)770
			},
			{
				"run north",
				(AnimationType)771
			},
			{
				"run northwest",
				(AnimationType)772
			},
			{
				"run west",
				(AnimationType)773
			},
			{
				"run southwest",
				(AnimationType)774
			},
			{
				"run south",
				(AnimationType)775
			},
			{
				"run southeast",
				(AnimationType)776
			},
			{
				"crouch east",
				(AnimationType)1025
			},
			{
				"crouch northeast",
				(AnimationType)1026
			},
			{
				"crouch north",
				(AnimationType)1027
			},
			{
				"crouch northwest",
				(AnimationType)1028
			},
			{
				"crouch west",
				(AnimationType)1029
			},
			{
				"crouch southwest",
				(AnimationType)1030
			},
			{
				"crouch south",
				(AnimationType)1031
			},
			{
				"crouch southeast",
				(AnimationType)1032
			},
			{
				"dead east",
				(AnimationType)1281
			},
			{
				"dead northeast",
				(AnimationType)1282
			},
			{
				"dead north",
				(AnimationType)1283
			},
			{
				"dead northwest",
				(AnimationType)1284
			},
			{
				"dead west",
				(AnimationType)1285
			},
			{
				"dead southwest",
				(AnimationType)1286
			},
			{
				"dead south",
				(AnimationType)1287
			},
			{
				"dead southeast",
				(AnimationType)1288
			},
			{
				"idle east",
				(AnimationType)1537
			},
			{
				"idle northeast",
				(AnimationType)1538
			},
			{
				"idle north",
				(AnimationType)1539
			},
			{
				"idle northwest",
				(AnimationType)1540
			},
			{
				"idle west",
				(AnimationType)1541
			},
			{
				"idle southwest",
				(AnimationType)1542
			},
			{
				"idle south",
				(AnimationType)1543
			},
			{
				"idle southeast",
				(AnimationType)1544
			},
			{
				"talk east",
				(AnimationType)1793
			},
			{
				"talk northeast",
				(AnimationType)1794
			},
			{
				"talk north",
				(AnimationType)1795
			},
			{
				"talk northwest",
				(AnimationType)1796
			},
			{
				"talk west",
				(AnimationType)1797
			},
			{
				"talk southwest",
				(AnimationType)1798
			},
			{
				"talk south",
				(AnimationType)1799
			},
			{
				"talk southeast",
				(AnimationType)1800
			},
			{
				"blink east",
				(AnimationType)2049
			},
			{
				"blink northeast",
				(AnimationType)2050
			},
			{
				"blink north",
				(AnimationType)2051
			},
			{
				"blink northwest",
				(AnimationType)2052
			},
			{
				"blink west",
				(AnimationType)2053
			},
			{
				"blink southwest",
				(AnimationType)2054
			},
			{
				"blink south",
				(AnimationType)2055
			},
			{
				"blink southeast",
				(AnimationType)2056
			},
			{
				"climb east",
				(AnimationType)2305
			},
			{
				"climb northeast",
				(AnimationType)2306
			},
			{
				"climb north",
				(AnimationType)2307
			},
			{
				"climb northwest",
				(AnimationType)2308
			},
			{
				"climb west",
				(AnimationType)2309
			},
			{
				"climb southwest",
				(AnimationType)2310
			},
			{
				"climb south",
				(AnimationType)2311
			},
			{
				"climb southeast",
				(AnimationType)2312
			},
			{
				"swim east",
				(AnimationType)2561
			},
			{
				"swim northeast",
				(AnimationType)2562
			},
			{
				"swim north",
				(AnimationType)2563
			},
			{
				"swim northwest",
				(AnimationType)2564
			},
			{
				"swim west",
				(AnimationType)2565
			},
			{
				"swim southwest",
				(AnimationType)2566
			},
			{
				"swim south",
				(AnimationType)2567
			},
			{
				"swim southeast",
				(AnimationType)2568
			},
			{
				"float east",
				(AnimationType)2817
			},
			{
				"float northeast",
				(AnimationType)2818
			},
			{
				"float north",
				(AnimationType)2819
			},
			{
				"float northwest",
				(AnimationType)2820
			},
			{
				"float west",
				(AnimationType)2821
			},
			{
				"float southwest",
				(AnimationType)2822
			},
			{
				"float south",
				(AnimationType)2823
			},
			{
				"float southeast",
				(AnimationType)2824
			},
			{
				"nausea east",
				(AnimationType)3073
			},
			{
				"nausea northeast",
				(AnimationType)3074
			},
			{
				"nausea north",
				(AnimationType)3075
			},
			{
				"nausea northwest",
				(AnimationType)3076
			},
			{
				"nausea west",
				(AnimationType)3077
			},
			{
				"nausea southwest",
				(AnimationType)3078
			},
			{
				"nausea south",
				(AnimationType)3079
			},
			{
				"nausea southeast",
				(AnimationType)3080
			}
		};
	}
}
