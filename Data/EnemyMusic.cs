using System;
using System.Collections.Generic;
using Mother4.Data.Enemies;

namespace Mother4.Data
{
	// Token: 0x02000083 RID: 131
	internal class EnemyMusic
	{
		// Token: 0x060002AF RID: 687 RVA: 0x00011168 File Offset: 0x0000F368
		public static string GetMusic(EnemyType enemy)
		{
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			return Paths.AUDIO + data.MusicName + ".wav";
		}

		// Token: 0x0400041D RID: 1053
		private const string RESOURCE_EXT = ".wav";

		// Token: 0x0400041E RID: 1054
		private static Dictionary<EnemyType, string> musics = new Dictionary<EnemyType, string>
		{
			{
				EnemyType.Dummy,
				"Battle Against a Clueless Foe"
			},
			{
				EnemyType.MagicSnail,
				"Battle Against a Clueless Foe"
			},
			{
				EnemyType.Stickat,
				"Battle Against a Clueless Foe"
			},
			{
				EnemyType.Rat,
				"Battle Against a Clueless Foe"
			},
			{
				EnemyType.HermitCan,
				"Battle Against an Intense Opponent"
			},
			{
				EnemyType.Flamingo,
				"Battle Against an Intense Opponent"
			},
			{
				EnemyType.AtomicPowerRobo,
				"Battle Against a Familiar Foe"
			},
			{
				EnemyType.CarbonPup,
				"Battle Against a Hot Opponent"
			},
			{
				EnemyType.MeltyRobot,
				"Battle Against a Hot Opponent"
			},
			{
				EnemyType.ModernMind,
				"Battle Against an Ultra-Dimensional Foe"
			},
			{
				EnemyType.NotSoDeer,
				"Battle Against an Intense Opponent"
			},
			{
				EnemyType.PunkAssassin,
				"Battle Against an Intense Opponent"
			},
			{
				EnemyType.PunkEnforcer,
				"Battle Against an Intense Opponent"
			},
			{
				EnemyType.RatDispenser,
				"Battle Against a Clueless Foe"
			}
		};
	}
}
