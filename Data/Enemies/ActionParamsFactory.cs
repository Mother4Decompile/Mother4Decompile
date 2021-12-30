using System;
using System.Collections.Generic;
using Mother4.Battle.Actions;

namespace Mother4.Data.Enemies
{
	// Token: 0x0200001C RID: 28
	internal static class ActionParamsFactory
	{
		// Token: 0x0600005F RID: 95 RVA: 0x0000468C File Offset: 0x0000288C
		public static ActionParams Build(int actionType, int actionWeight, object[] paramData)
		{
			Type actionType2;
			if (ActionParamsFactory.ACTION_TYPE_MAP.TryGetValue(actionType, out actionType2))
			{
				return new ActionParams
				{
					actionType = actionType2,
					data = paramData,
					weight = actionWeight
				};
			}
			string message = string.Format("{0} is not a valid action type.", actionType);
			throw new ArgumentException(message);
		}

		// Token: 0x04000117 RID: 279
		public const int ENEMY_TURN_WASTE = 0;

		// Token: 0x04000118 RID: 280
		public const int ENEMY_BASH = 1;

		// Token: 0x04000119 RID: 281
		public const int ENEMY_PROJECTILE = 2;

		// Token: 0x0400011A RID: 282
		private static readonly Dictionary<int, Type> ACTION_TYPE_MAP = new Dictionary<int, Type>
		{
			{
				0,
				typeof(EnemyTurnWasteAction)
			},
			{
				1,
				typeof(EnemyBashAction)
			},
			{
				2,
				typeof(EnemyProjectileAction)
			}
		};
	}
}
