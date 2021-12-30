using System;
using System.Collections.Generic;
using Mother4.Battle.Actions;

namespace Mother4.Battle.Combatants
{
	// Token: 0x020000CC RID: 204
	internal abstract class Combatant
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600049E RID: 1182 RVA: 0x0001E20C File Offset: 0x0001C40C
		// (remove) Token: 0x0600049F RID: 1183 RVA: 0x0001E244 File Offset: 0x0001C444
		public event Combatant.StatChangeHandler OnStatChange;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060004A0 RID: 1184 RVA: 0x0001E27C File Offset: 0x0001C47C
		// (remove) Token: 0x060004A1 RID: 1185 RVA: 0x0001E2B4 File Offset: 0x0001C4B4
		public event Combatant.StatusEffectChangeHandler OnStatusEffectChange;

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		public BattleFaction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001E2F1 File Offset: 0x0001C4F1
		public StatSet Stats
		{
			get
			{
				return this.stats;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001E2F9 File Offset: 0x0001C4F9
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x0001E301 File Offset: 0x0001C501
		public Combatant[] SavedTargets
		{
			get
			{
				return this.savedTargets;
			}
			set
			{
				this.savedTargets = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0001E30A File Offset: 0x0001C50A
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0001E312 File Offset: 0x0001C512
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				if (this.id < 0)
				{
					this.id = value;
					return;
				}
				throw new InvalidOperationException("Cannot reset combatant ID.");
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001E32F File Offset: 0x0001C52F
		public Combatant(BattleFaction faction)
		{
			this.faction = faction;
			this.id = -1;
			this.stats = default(StatSet);
			this.statusEffects = new Dictionary<StatusEffect, StatusEffectInstance>();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001E35C File Offset: 0x0001C55C
		public virtual void AlterStats(StatSet statChange)
		{
			this.stats += statChange;
			if (this.OnStatChange != null)
			{
				this.OnStatChange(this, statChange);
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001E388 File Offset: 0x0001C588
		public virtual bool AddStatusEffect(StatusEffectInstance effect)
		{
			bool result = false;
			if (!this.statusEffects.ContainsKey(effect.Type))
			{
				this.statusEffects.Add(effect.Type, effect);
				result = true;
				if (this.OnStatusEffectChange != null)
				{
					this.OnStatusEffectChange(this, effect.Type, true);
				}
			}
			return result;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001E3E0 File Offset: 0x0001C5E0
		public virtual bool RemoveStatusEffect(StatusEffect effect)
		{
			bool flag = this.statusEffects.Remove(effect);
			if (flag && this.OnStatusEffectChange != null)
			{
				this.OnStatusEffectChange(this, effect, false);
			}
			return flag;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001E414 File Offset: 0x0001C614
		public void DecrementStatusEffect(StatusEffect effect)
		{
			if (this.statusEffects.ContainsKey(effect))
			{
				StatusEffectInstance value = this.statusEffects[effect];
				value.TurnsRemaining--;
				if (value.TurnsRemaining > 0)
				{
					this.statusEffects[effect] = value;
					return;
				}
				this.RemoveStatusEffect(value.Type);
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001E474 File Offset: 0x0001C674
		public StatusEffectInstance[] GetStatusEffects()
		{
			ICollection<StatusEffectInstance> values = this.statusEffects.Values;
			StatusEffectInstance[] array = new StatusEffectInstance[values.Count];
			values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001E4A2 File Offset: 0x0001C6A2
		public DecisionAction GetDecisionAction(BattleController controller)
		{
			return this.GetDecisionAction(controller, 0);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001E4AC File Offset: 0x0001C6AC
		public virtual DecisionAction GetDecisionAction(BattleController controller, int priority, bool isFromUndo)
		{
			throw new NotImplementedException("GetDecisionAction was not overridden.");
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001E4B8 File Offset: 0x0001C6B8
		public DecisionAction GetDecisionAction(BattleController controller, int priority)
		{
			return this.GetDecisionAction(controller, priority, false);
		}

		// Token: 0x0400066F RID: 1647
		private const int PRIORITY_MAGIC = 0;

		// Token: 0x04000670 RID: 1648
		private BattleFaction faction;

		// Token: 0x04000671 RID: 1649
		private int id;

		// Token: 0x04000672 RID: 1650
		protected StatSet stats;

		// Token: 0x04000673 RID: 1651
		protected Dictionary<StatusEffect, StatusEffectInstance> statusEffects;

		// Token: 0x04000674 RID: 1652
		protected Combatant[] savedTargets;

		// Token: 0x020000CD RID: 205
		// (Invoke) Token: 0x060004B2 RID: 1202
		public delegate void StatChangeHandler(Combatant sender, StatSet change);

		// Token: 0x020000CE RID: 206
		// (Invoke) Token: 0x060004B6 RID: 1206
		public delegate void StatusEffectChangeHandler(Combatant sender, StatusEffect statusEffect, bool added);
	}
}
