using System;
using Carbine;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle
{
	// Token: 0x02000066 RID: 102
	internal class BattleCalculator
	{
		// Token: 0x06000242 RID: 578 RVA: 0x0000E208 File Offset: 0x0000C408
		private static bool IsSmashAttack(Combatant attacker, Combatant target)
		{
			double num = (double)attacker.Stats.Guts / 500.0 * 100.0;
			return (double)Engine.Random.Next(100) <= num;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000E24C File Offset: 0x0000C44C
		public static int CalculatePhysicalDamage(float power, Combatant attacker, Combatant target, out bool smash)
		{
			smash = false;
			float num = power;
			if (BattleCalculator.IsSmashAttack(attacker, target))
			{
				num = 4f;
				smash = true;
			}
			double mean = (double)(num * (float)attacker.Stats.Offense - (float)attacker.Stats.Defense);
			double val = GaussianRandom.Next(mean, 2.0);
			return (int)Math.Max(0.0, val);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E2B0 File Offset: 0x0000C4B0
		public static int CalculateComboDamage(float power, Combatant attacker, Combatant target, int minimum, out bool smash)
		{
			smash = false;
			float num;
			if (BattleCalculator.IsSmashAttack(attacker, target))
			{
				num = power * 0.25f;
				smash = true;
			}
			else
			{
				num = power * 0.2f;
			}
			double mean = (double)(num * (float)attacker.Stats.Offense - (float)attacker.Stats.Defense);
			double val = GaussianRandom.Next(mean, 2.0);
			return (int)Math.Max((double)minimum, val);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000E318 File Offset: 0x0000C518
		public static int CalculatePsiDamage(int lowerDamage, int upperDamage, Combatant attacker, Combatant target)
		{
			int num = lowerDamage + (upperDamage - lowerDamage) / 2;
			double val = GaussianRandom.Next((double)num, 3.0);
			return (int)Math.Max((double)lowerDamage, Math.Min((double)upperDamage, val));
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000E350 File Offset: 0x0000C550
		public static int CalculateProjectileDamage(int targetDamage, Combatant attacker, Combatant target)
		{
			double val = GaussianRandom.Next((double)targetDamage, 2.0);
			return (int)Math.Max(0.0, val);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E380 File Offset: 0x0000C580
		public static bool RunSuccess(CombatantController combatantController, int turnNumber)
		{
			Combatant[] factionCombatants = combatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
			Combatant[] factionCombatants2 = combatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
			int num = int.MinValue;
			int num2 = int.MinValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.Stats.Speed > num)
				{
					num = combatant.Stats.Speed;
				}
			}
			foreach (Combatant combatant2 in factionCombatants2)
			{
				if (combatant2.Stats.Speed > num2)
				{
					num2 = combatant2.Stats.Speed;
				}
			}
			int num3 = num2 - num + 10 * turnNumber;
			return Engine.Random.Next(100) < num3;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000E43C File Offset: 0x0000C63C
		public static bool CalculateReflection(Combatant attacker, Combatant target)
		{
			bool flag = target is PlayerCombatant && ((PlayerCombatant)target).Character == CharacterType.Travis;
			double num = (double)attacker.Stats.Guts / 500.0 * 100.0;
			return (double)Engine.Random.Next(100) <= num && flag;
		}

		// Token: 0x04000356 RID: 854
		private const double PHYSICAL_STD_DEV = 2.0;

		// Token: 0x04000357 RID: 855
		private const double PSI_STD_DEV = 3.0;

		// Token: 0x04000358 RID: 856
		private const float SMAAASH_POWER = 4f;

		// Token: 0x04000359 RID: 857
		private const float COMBO_POWER_FACTOR = 0.2f;

		// Token: 0x0400035A RID: 858
		private const float COMBO_POWER_FACTOR_SMAAASH = 0.25f;

		// Token: 0x0400035B RID: 859
		private const double SMAAASH_DIVISOR = 500.0;

		// Token: 0x0400035C RID: 860
		private const double REFLECT_DIVISOR = 500.0;
	}
}
