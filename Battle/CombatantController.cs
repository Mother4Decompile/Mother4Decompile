using System;
using System.Collections.Generic;
using System.Linq;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle
{
	// Token: 0x020000CB RID: 203
	internal class CombatantController
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0001DF08 File Offset: 0x0001C108
		public List<Combatant> CombatantList
		{
			get
			{
				return this.combatants.Values.ToList<Combatant>();
			}
		}

		// Token: 0x170000C0 RID: 192
		public Combatant this[int i]
		{
			get
			{
				return this.combatants[i];
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001DF28 File Offset: 0x0001C128
		public CombatantController(CharacterType[] party, EnemyType[] enemies)
		{
			this.uidCounter = 0;
			this.combatants = new Dictionary<int, Combatant>();
			for (int i = 0; i < party.Length; i++)
			{
				this.Add(new PlayerCombatant(party[i], i));
			}
			for (int j = 0; j < enemies.Length; j++)
			{
				this.Add(new EnemyCombatant(enemies[j]));
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001DF9A File Offset: 0x0001C19A
		public int Add(Combatant c)
		{
			c.ID = this.uidCounter;	
			this.combatants.Add(this.uidCounter, c);
			this.uidCounter++;
			return c.ID;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001DFD0 File Offset: 0x0001C1D0
		public void Remove(Combatant c)
		{
			foreach (KeyValuePair<int, Combatant> keyValuePair in this.combatants.ToArray<KeyValuePair<int, Combatant>>())
			{
				if (keyValuePair.Value == c)
				{
					this.combatants.Remove(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001E021 File Offset: 0x0001C221
		public bool IsIdValid(int id)
		{
			return this.combatants.ContainsKey(id);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001E030 File Offset: 0x0001C230
		public Combatant GetFirstLiveCombatant(BattleFaction faction)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			Combatant result = null;
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID < num && combatant.Stats.HP > 0)
				{
					num = combatant.ID;
					result = combatant;
				}
			}
			return result;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001E088 File Offset: 0x0001C288
		public int SelectFirst(BattleFaction faction)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID < num)
				{
					num = combatant.ID;
				}
			}
			if (num < 2147483647)
			{
				return num;
			}
			return int.MinValue;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001E0D4 File Offset: 0x0001C2D4
		public int SelectNext(BattleFaction faction, int id)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID > id && combatant.ID < num)
				{
					num = combatant.ID;
				}
			}
			if (num < 2147483647)
			{
				return num;
			}
			return int.MinValue;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001E12C File Offset: 0x0001C32C
		public int SelectPrevious(BattleFaction faction, int id)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID > id && combatant.ID < num)
				{
					num = combatant.ID;
				}
			}
			if (num < 2147483647)
			{
				return num;
			}
			return int.MinValue;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001E181 File Offset: 0x0001C381
		public Combatant[] GetFactionCombatants(BattleFaction faction)
		{
			return this.GetFactionCombatants(faction, false);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001E18C File Offset: 0x0001C38C
		public Combatant[] GetFactionCombatants(BattleFaction faction, bool alive)
		{
			List<Combatant> list = new List<Combatant>();
			foreach (Combatant combatant in this.combatants.Values)
			{
				if (combatant.Faction == faction && (!alive || combatant.Stats.HP > 0))
				{
					list.Add(combatant);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0400066D RID: 1645
		private int uidCounter;

		// Token: 0x0400066E RID: 1646
		private Dictionary<int, Combatant> combatants;
	}
}
