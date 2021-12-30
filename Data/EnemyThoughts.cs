using System;
using System.Collections.Generic;
using Mother4.Data.Enemies;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;

namespace Mother4.Data
{
	// Token: 0x02000079 RID: 121
	internal class EnemyThoughts
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000FF60 File Offset: 0x0000E160
		public static string GetLike(EnemyType enemy)
		{
			string result = string.Empty;
			EnemyData data = EnemyFile.Instance.GetData(enemy);
			string qualifiedName;
			if (data.TryGetStringQualifiedName("thoughts", out qualifiedName))
			{
				RufiniString rufiniString = StringFile.Instance.Get(qualifiedName);
				if (rufiniString.Value != null)
				{
					result = rufiniString.Value;
				}
			}
			return result;
		}

		// Token: 0x040003F1 RID: 1009
		private static Dictionary<EnemyType, string> likes = new Dictionary<EnemyType, string>
		{
			{
				EnemyType.Dummy,
				"Commander Keen"
			},
			{
				EnemyType.MagicSnail,
				"shell ettiquite"
			},
			{
				EnemyType.Stickat,
				"furballs"
			},
			{
				EnemyType.Rat,
				"Red Leicester"
			},
			{
				EnemyType.HermitCan,
				"soda"
			},
			{
				EnemyType.Flamingo,
				"krill"
			},
			{
				EnemyType.AtomicPowerRobo,
				"isotopes and gamma particles"
			},
			{
				EnemyType.CarbonPup,
				"crystalline structures"
			},
			{
				EnemyType.MeltyRobot,
				"Back to the Future II"
			},
			{
				EnemyType.ModernMind,
				"the interconnectedness of all things"
			},
			{
				EnemyType.PunkAssassin,
				"being a bad enough dude"
			},
			{
				EnemyType.PunkEnforcer,
				"improvised weaponry"
			},
			{
				EnemyType.RatDispenser,
				"rodents of unusual size"
			}
		};
	}
}
