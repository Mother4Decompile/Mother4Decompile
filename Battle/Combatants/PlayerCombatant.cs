using System;
using Mother4.Battle.Actions;
using Mother4.Data;

namespace Mother4.Battle.Combatants
{
	// Token: 0x020000D0 RID: 208
	internal class PlayerCombatant : Combatant
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001E52C File Offset: 0x0001C72C
		public CharacterType Character
		{
			get
			{
				return this.character;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001E534 File Offset: 0x0001C734
		public int PartyIndex
		{
			get
			{
				return this.partyIndex;
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001E53C File Offset: 0x0001C73C
		public PlayerCombatant(CharacterType character, int partyIndex) : base(BattleFaction.PlayerTeam)
		{
			this.character = character;
			this.partyIndex = partyIndex;
			this.stats = CharacterStats.GetStats(character);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001E560 File Offset: 0x0001C760
		public override DecisionAction GetDecisionAction(BattleController controller, int priority, bool isFromUndo)
		{
			ActionParams aparams = new ActionParams
			{
				actionType = typeof(PlayerDecisionAction),
				controller = controller,
				sender = this,
				priority = priority,
				data = new object[]
				{
					false,
					isFromUndo
				}
			};
			return new PlayerDecisionAction(aparams);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001E5C7 File Offset: 0x0001C7C7
		public void HandleStatusChangeFromOther(Combatant sender, StatusEffect statusEffect, bool added)
		{
			if (sender.Faction == BattleFaction.EnemyTeam && !added && statusEffect == StatusEffect.Talking)
			{
				this.RemoveStatusEffect(StatusEffect.Talking);
				sender.OnStatusEffectChange -= this.HandleStatusChangeFromOther;
			}
		}

		// Token: 0x04000678 RID: 1656
		private CharacterType character;

		// Token: 0x04000679 RID: 1657
		private int partyIndex;
	}
}
