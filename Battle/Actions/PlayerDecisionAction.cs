using System;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Battle.UI;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000BB RID: 187
	internal class PlayerDecisionAction : DecisionAction
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x00019428 File Offset: 0x00017628
		public PlayerDecisionAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as PlayerCombatant);
			this.character = this.combatant.Character;
			if (aparams.data != null)
			{
				if (aparams.data.Length > 0)
				{
					this.isGroovy = (bool)aparams.data[0];
				}
				if (aparams.data.Length > 1)
				{
					this.isFromUndo = (bool)aparams.data[1];
				}
			}
			this.state = PlayerDecisionAction.State.Initialize;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000194B0 File Offset: 0x000176B0
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case PlayerDecisionAction.State.Initialize:
			{
				this.HandleStatusEffects(false);
				bool flag = this.CanSkipNormalAction();
				this.controller.InterfaceController.OnInteractionComplete += this.InteractionComplete;
				if (!flag)
				{
					this.controller.InterfaceController.BeginPlayerInteraction(this.character);
					this.controller.InterfaceController.ShowButtonBar();
					this.controller.InterfaceController.PopCard(this.combatant.ID, 28);
					this.controller.InterfaceController.AllowUndo = (this.controller.CanRevert() && !this.isGroovy);
					this.state = PlayerDecisionAction.State.WaitForUI;
					return;
				}
				if (this.isFromUndo)
				{
					this.controller.RevertDecision();
				}
				this.state = PlayerDecisionAction.State.Finish;
				return;
			}
			case PlayerDecisionAction.State.WaitForUI:
				break;
			case PlayerDecisionAction.State.Finish:
				this.controller.InterfaceController.OnInteractionComplete -= this.InteractionComplete;
				this.controller.InterfaceController.EndPlayerInteraction();
				if (!this.isGroovy)
				{
					this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
				}
				Console.WriteLine("Finished deciding.");
				this.complete = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000195FC File Offset: 0x000177FC
		private ActionParams BuildPsiActionParams(SelectionState selectionState, int actionPriority)
		{
			ActionParams result;
			if (selectionState.Psi.PsiType.Identifier == Hash.Get("psi.shield"))
			{
				result = new ActionParams
				{
					actionType = typeof(ShieldAction),
					controller = this.controller,
					sender = this.combatant,
					priority = actionPriority,
					targets = selectionState.Targets,
					data = new object[]
					{
						new StatusEffectInstance
						{
							Type = StatusEffect.Shield,
							TurnsRemaining = -1,
							Strength = 1f
						}
					}
				};
			}
			else
			{
				result = new ActionParams
				{
					actionType = typeof(PsiAction),
					controller = this.controller,
					sender = this.combatant,
					priority = actionPriority,
					targets = selectionState.Targets,
					data = new object[]
					{
						selectionState.Psi
					}
				};
			}
			return result;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00019728 File Offset: 0x00017928
		public void InteractionComplete(SelectionState selectionState)
		{
			Console.WriteLine(" Type: {0}", selectionState.Type);
			if (selectionState.AttackIndex >= 0)
			{
				Console.WriteLine(" Attack Index: {0}", selectionState.AttackIndex);
			}
			if (selectionState.ItemIndex >= 0)
			{
				Console.WriteLine(" Item Index: {0}", selectionState.ItemIndex);
			}
			if (selectionState.Targets != null)
			{
				Console.Write(" Targets: ");
				for (int i = 0; i < selectionState.Targets.Length; i++)
				{
					Console.Write("{0}, ", selectionState.Targets[i].ToString());
				}
				Console.WriteLine();
			}
			int num = this.isGroovy ? 2147483646 : this.combatant.Stats.Speed;
			bool flag = false;
			ActionParams? actionParams = null;
			StatusEffectInstance[] statusEffects = this.sender.GetStatusEffects();
			if (selectionState.Type == SelectionState.SelectionType.Undo)
			{
				this.controller.RevertDecision();
				flag = true;
			}
			else if (statusEffects.Length > 0)
			{
				this.HandleStatusEffects(true);
				flag = this.CanSkipNormalAction();
			}
			if (!flag)
			{
				switch (selectionState.Type)
				{
				case SelectionState.SelectionType.Bash:
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(PlayerBashAction),
						controller = this.controller,
						sender = this.combatant,
						priority = num,
						targets = selectionState.Targets
					});
					break;
				case SelectionState.SelectionType.PSI:
					actionParams = new ActionParams?(this.BuildPsiActionParams(selectionState, num));
					break;
				case SelectionState.SelectionType.Talk:
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(FloydTalkAction),
						controller = this.controller,
						sender = this.combatant,
						priority = num,
						targets = selectionState.Targets
					});
					break;
				case SelectionState.SelectionType.Guard:
					actionParams = new ActionParams?(new ActionParams
					{
						actionType = typeof(MessageAction),
						controller = this.controller,
						sender = this.combatant,
						priority = num,
						targets = selectionState.Targets,
						data = new object[]
						{
							CharacterNames.GetName((this.sender as PlayerCombatant).Character) + " is guarding.",
							false
						}
					});
					break;
				}
			}
			if (actionParams != null)
			{
				BattleAction instance = BattleAction.GetInstance(actionParams.Value);
				this.controller.AddAction(instance);
				if (this.isGroovy)
				{
					instance.OnActionComplete += this.OnGroovyBonusActionComplete;
				}
			}
			this.state = PlayerDecisionAction.State.Finish;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000199FC File Offset: 0x00017BFC
		private void OnGroovyBonusActionComplete(BattleAction action)
		{
			this.controller.InterfaceController.SetCardGroovy(this.combatant.ID, false);
			this.combatant.AlterStats(new StatSet
			{
				Meter = -this.combatant.Stats.Meter
			});
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00019A54 File Offset: 0x00017C54
		private bool CanSkipNormalAction()
		{
			bool flag = false;
			foreach (StatusEffectInstance statusEffectInstance in this.sender.GetStatusEffects())
			{
				flag |= (statusEffectInstance.Type == StatusEffect.Talking || statusEffectInstance.Type == StatusEffect.Diamondized || statusEffectInstance.Type == StatusEffect.Unconscious);
			}
			return flag;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00019AB0 File Offset: 0x00017CB0
		private void HandleStatusEffects(bool addActions)
		{
			foreach (StatusEffectInstance statusEffectInstance in this.sender.GetStatusEffects())
			{
				Type actionType;
				if (addActions && StatusEffectActions.TryGet(statusEffectInstance.Type, out actionType))
				{
					ActionParams aparams = new ActionParams
					{
						actionType = actionType,
						controller = this.controller,
						sender = this.sender,
						targets = this.sender.SavedTargets,
						priority = this.sender.Stats.Speed,
						data = new object[]
						{
							statusEffectInstance
						}
					};
					this.controller.AddAction(BattleAction.GetInstance(aparams));
				}
			}
		}

		// Token: 0x040005CC RID: 1484
		private const int CARD_POP_HEIGHT = 28;

		// Token: 0x040005CD RID: 1485
		private PlayerCombatant combatant;

		// Token: 0x040005CE RID: 1486
		private CharacterType character;

		// Token: 0x040005CF RID: 1487
		private PlayerDecisionAction.State state;

		// Token: 0x040005D0 RID: 1488
		private bool isGroovy;

		// Token: 0x040005D1 RID: 1489
		private bool isFromUndo;

		// Token: 0x020000BC RID: 188
		private enum State
		{
			// Token: 0x040005D3 RID: 1491
			Initialize,
			// Token: 0x040005D4 RID: 1492
			WaitForUI,
			// Token: 0x040005D5 RID: 1493
			Finish
		}
	}
}
