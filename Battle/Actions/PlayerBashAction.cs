using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Input;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000B9 RID: 185
	internal class PlayerBashAction : BattleAction
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x00018E7C File Offset: 0x0001707C
		public PlayerBashAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as PlayerCombatant);
			if (this.targets.Length == 1)
			{
				this.target = this.targets[0];
				this.meterDelta = default(StatSet);
				this.meterDelta.Meter = 0.013333334f;
				this.messageStack = new Stack<string>();
				this.power = 2f;
				this.comboCount = 0;
				this.state = PlayerBashAction.State.Initialize;
				return;
			}
			throw new NotImplementedException("Cannot target more than one combatant while bashing.");
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00018F08 File Offset: 0x00017108
		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case PlayerBashAction.State.Initialize:
				this.Initialize();
				return;
			case PlayerBashAction.State.Combo:
				this.Combo();
				return;
			case PlayerBashAction.State.FinishCombo:
				this.FinishCombo();
				return;
			case PlayerBashAction.State.PopMessage:
				this.PopMessage();
				return;
			case PlayerBashAction.State.WaitForUI:
				break;
			case PlayerBashAction.State.Finish:
				this.Finish();
				break;
			default:
				return;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00018F63 File Offset: 0x00017163
		public void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.bgmPosition = AudioManager.Instance.BGM.Position;
				this.buttonPressed = true;
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00018F84 File Offset: 0x00017184
		private void Initialize()
		{
			Console.WriteLine("BASHMÖDE ({0})", this.combatant.Character);
			this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			this.controller.InterfaceController.PrePlayerAttack.Play();
			this.controller.InterfaceController.PopCard(this.combatant.ID, 12);
			if (!this.controller.CombatantController.IsIdValid(this.target.ID))
			{
				Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
				this.target = factionCombatants[Engine.Random.Next() % factionCombatants.Length];
			}
			this.statDelta = default(StatSet);
			this.comboCount = 0;
			string item = string.Format("{0} attacked!", CharacterNames.GetName(this.combatant.Character));
			this.messageStack.Push(item);
			this.state = PlayerBashAction.State.PopMessage;
			if (this.target is EnemyCombatant)
			{
				this.controller.InterfaceController.StartComboCircle(this.target as EnemyCombatant);
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000190C0 File Offset: 0x000172C0
		private void PopMessage()
		{
			if (this.messageStack.Count > 0)
			{
				string message = this.messageStack.Pop();
				this.controller.InterfaceController.ClearTextBox();
				this.controller.InterfaceController.ShowTextBox(message, true);
				this.state = PlayerBashAction.State.WaitForUI;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00019110 File Offset: 0x00017310
		private int AccumulateDamage(Combatant target, out bool smash)
		{
			int num;
			if (this.comboCount == 0)
			{
				num = -BattleCalculator.CalculatePhysicalDamage(this.power, this.combatant, target, out smash);
				this.firstHpDelta = num;
				this.statDelta.HP = num;
			}
			else
			{
				num = -BattleCalculator.CalculateComboDamage(this.power, this.combatant, target, (this.firstHpDelta == 0) ? 0 : 1, out smash);
				this.statDelta.HP = this.statDelta.HP + num;
			}
			Console.WriteLine(" hpDelta = {0}", num);
			return num;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00019194 File Offset: 0x00017394
		private void Combo()
		{
			if (!(this.target is EnemyCombatant))
			{
				throw new NotImplementedException("Bashing player characters is not yet supported.");
			}
			this.lastLastComboEdge = this.lastComboEdge;
			this.lastComboEdge = this.comboEdge;
			this.comboEdge = this.controller.ComboController.IsCombo(AudioManager.Instance.BGM.Position);
			if (this.buttonPressed)
			{
				if (this.comboCount < 16)
				{
					if (this.controller.ComboController.IsCombo(this.bgmPosition) || this.comboCount == 0)
					{
						bool smash;
						int damage = this.AccumulateDamage(this.target, out smash);
						this.controller.InterfaceController.AddComboHit(damage, this.comboCount, (this.sender as PlayerCombatant).Character, this.target, smash);
						this.combatant.AlterStats(this.meterDelta);
						Console.WriteLine(" COMBO x{0}", this.comboCount + 1);
						this.comboCount++;
					}
					else
					{
						Console.WriteLine(" COMBOVER");
						this.controller.InterfaceController.StopComboCircle(false);
						this.state = PlayerBashAction.State.FinishCombo;
					}
					if (this.comboCount >= 16)
					{
						Console.WriteLine(" THE GREAT COMBONI!!");
						this.controller.InterfaceController.StopComboCircle(true);
						this.state = PlayerBashAction.State.FinishCombo;
					}
				}
				this.buttonPressed = false;
				return;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000192FE File Offset: 0x000174FE
		private void FinishCombo()
		{
			if (this.controller.InterfaceController.IsComboCircleDone())
			{
				this.state = PlayerBashAction.State.Finish;
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001931C File Offset: 0x0001751C
		private void Finish()
		{
			Console.WriteLine("Total hpDelta={0}", this.statDelta.HP);
			this.target.AlterStats(this.statDelta);
			if (this.combatant.Stats.Meter < 1f && this.controller.ActionCount > 0)
			{
				this.controller.InterfaceController.PopCard(this.combatant.ID, 0);
			}
			this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			this.complete = true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000193D0 File Offset: 0x000175D0
		public void InteractionComplete()
		{
			if (this.state == PlayerBashAction.State.WaitForUI)
			{
				this.state = ((this.messageStack.Count == 0) ? PlayerBashAction.State.Combo : PlayerBashAction.State.PopMessage);
				if (this.state == PlayerBashAction.State.Combo)
				{
					this.controller.InterfaceController.HideTextBox();
					return;
				}
			}
			else if (this.state == PlayerBashAction.State.Combo)
			{
				this.state = PlayerBashAction.State.FinishCombo;
			}
		}

		// Token: 0x040005B3 RID: 1459
		private const Button COMBO_BUTTON = Button.A;

		// Token: 0x040005B4 RID: 1460
		private const int MAX_COMBOS = 16;

		// Token: 0x040005B5 RID: 1461
		private const float ONE_GP = 0.013333334f;

		// Token: 0x040005B6 RID: 1462
		private const int CARD_POP_HEIGHT = 12;

		// Token: 0x040005B7 RID: 1463
		private PlayerBashAction.State state;

		// Token: 0x040005B8 RID: 1464
		private float power;

		// Token: 0x040005B9 RID: 1465
		private PlayerCombatant combatant;

		// Token: 0x040005BA RID: 1466
		private Combatant target;

		// Token: 0x040005BB RID: 1467
		private StatSet statDelta;

		// Token: 0x040005BC RID: 1468
		private StatSet meterDelta;

		// Token: 0x040005BD RID: 1469
		private int firstHpDelta;

		// Token: 0x040005BE RID: 1470
		private Stack<string> messageStack;

		// Token: 0x040005BF RID: 1471
		private bool buttonPressed;

		// Token: 0x040005C0 RID: 1472
		private int comboCount;

		// Token: 0x040005C1 RID: 1473
		private uint bgmPosition;

		// Token: 0x040005C2 RID: 1474
		private bool comboEdge;

		// Token: 0x040005C3 RID: 1475
		private bool lastComboEdge;

		// Token: 0x040005C4 RID: 1476
		private bool lastLastComboEdge;

		// Token: 0x020000BA RID: 186
		private enum State
		{
			// Token: 0x040005C6 RID: 1478
			Initialize,
			// Token: 0x040005C7 RID: 1479
			Combo,
			// Token: 0x040005C8 RID: 1480
			FinishCombo,
			// Token: 0x040005C9 RID: 1481
			PopMessage,
			// Token: 0x040005CA RID: 1482
			WaitForUI,
			// Token: 0x040005CB RID: 1483
			Finish
		}
	}
}
