using System;
using System.Text;
using Carbine;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Data;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000058 RID: 88
	internal class FloydTalkAction : BattleAction
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000D100 File Offset: 0x0000B300
		public FloydTalkAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (aparams.sender as PlayerCombatant);
			this.target = (aparams.targets[0] as EnemyCombatant);
			string like = EnemyThoughts.GetLike(this.target.Enemy);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0} tried chatting up {1}{2}.\n", CharacterNames.GetName(this.combatant.Character), EnemyNames.GetArticle(this.target.Enemy), EnemyNames.GetName(this.target.Enemy));
			stringBuilder.AppendFormat("{0} had a lot to say about {1}.\n", Capitalizer.Capitalize(EnemyNames.GetSubjectivePronoun(this.target.Enemy)), like);
			stringBuilder.Append("@[p:10].[p:10].[p:30].They really hit it off!");
			this.controller.Data.AddReplace("topicOfDiscussion", like);
			this.message = stringBuilder.ToString();
			this.state = FloydTalkAction.State.Initialize;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case FloydTalkAction.State.Initialize:
				this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
				this.controller.InterfaceController.ShowTextBox(this.message, false);
				this.controller.InterfaceController.TalkSound.Play();
				this.controller.InterfaceController.PopCard(this.combatant.ID, 12);
				if (!this.controller.CombatantController.IsIdValid(this.target.ID))
				{
					Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
					this.target = (factionCombatants[Engine.Random.Next() % factionCombatants.Length] as EnemyCombatant);
				}
				this.state = FloydTalkAction.State.WaitForUI;
				return;
			case FloydTalkAction.State.WaitForUI:
				break;
			case FloydTalkAction.State.Finish:
			{
				this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
				this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
				int turnsRemaining = 2 + (int)Math.Round(Math.Abs(GaussianRandom.Next(0.0, 0.6)));
				this.combatant.SavedTargets = new Combatant[]
				{
					this.target
				};
				this.combatant.AddStatusEffect(new StatusEffectInstance
				{
					Type = StatusEffect.Talking,
					TurnsRemaining = int.MaxValue
				});
				this.target.SavedTargets = new Combatant[]
				{
					this.combatant
				};
				this.target.AddStatusEffect(new StatusEffectInstance
				{
					Type = StatusEffect.Talking,
					TurnsRemaining = turnsRemaining
				});
				this.target.OnStatusEffectChange += this.combatant.HandleStatusChangeFromOther;
				this.complete = true;
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000D3D6 File Offset: 0x0000B5D6
		public void InteractionComplete()
		{
			this.state = FloydTalkAction.State.Finish;
		}

		// Token: 0x04000310 RID: 784
		private const string TOPIC_OF_DISCUSSION = "topicOfDiscussion";

		// Token: 0x04000311 RID: 785
		private const int CARD_POP_HEIGHT = 12;

		// Token: 0x04000312 RID: 786
		private const bool USE_BUTTON = false;

		// Token: 0x04000313 RID: 787
		private FloydTalkAction.State state;

		// Token: 0x04000314 RID: 788
		private string message;

		// Token: 0x04000315 RID: 789
		private PlayerCombatant combatant;

		// Token: 0x04000316 RID: 790
		private EnemyCombatant target;

		// Token: 0x02000059 RID: 89
		private enum State
		{
			// Token: 0x04000318 RID: 792
			Initialize,
			// Token: 0x04000319 RID: 793
			WaitForUI,
			// Token: 0x0400031A RID: 794
			Finish
		}
	}
}
