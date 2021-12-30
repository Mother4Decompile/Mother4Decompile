using System;
using System.Collections.Generic;
using Carbine.Audio;
using Carbine.Flags;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Battle.Combatants;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.GUI.Text;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000AF RID: 175
	internal class BattleWinAction : BattleAction
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x000181DD File Offset: 0x000163DD
		public BattleWinAction(ActionParams aparams) : base(aparams)
		{
			this.state = BattleWinAction.State.Initialization;
			this.levelup = new LevelUpBuilder(null);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000181F9 File Offset: 0x000163F9
		private void ChangeState(BattleWinAction.State state)
		{
			if (this.state != BattleWinAction.State.WaitForUI && this.state != BattleWinAction.State.WaitForTimer)
			{
				this.previousState = this.state;
			}
			this.state = state;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00018220 File Offset: 0x00016420
		protected override void UpdateAction()
		{
			base.UpdateAction();
			bool flag;
			do
			{
				flag = false;
				if (this.state == BattleWinAction.State.Initialization)
				{
					Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
					foreach (Combatant combatant in factionCombatants)
					{
						if (combatant.Stats.HP > 0)
						{
							this.controller.InterfaceController.PopCard(combatant.ID, 28);
						}
						if (combatant.Stats.Meter >= 1f)
						{
							StatSet statChange = new StatSet
							{
								Meter = 1f - combatant.Stats.Meter - 0.013333334f
							};
							combatant.AlterStats(statChange);
							this.controller.InterfaceController.SetCardGroovy(combatant.ID, false);
						}
					}
					this.controller.SetFinalStatSets();
					this.controller.InterfaceController.RemoveAllModifiers();
					AudioManager.Instance.BGM.Stop();
					this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
					this.ChangeState(BattleWinAction.State.YouWon);
				}
				else if (this.state == BattleWinAction.State.YouWon)
				{
					int num = 0;
					int num2 = 180;
					string message = string.Format("[t:0,{0}][p:{1}]", num, num2);
					this.controller.InterfaceController.ClearTextBox();
					this.controller.InterfaceController.ShowTextBox(message, false);
					this.controller.InterfaceController.PlayWinBGM(num);
					this.controller.InterfaceController.SetLetterboxing(0f);
					this.ChangeState(BattleWinAction.State.WaitForUI);
					flag = true;
				}
				else if (this.state == BattleWinAction.State.ItemDrop)
				{
					this.ChangeState(BattleWinAction.State.Experience);
					flag = true;
				}
				else if (this.state == BattleWinAction.State.Experience)
				{
					List<EnemyType> defeatedEnemies = this.controller.DefeatedEnemies;
					int num3 = 0;
					int num4 = 0;
					for (int j = 0; j < defeatedEnemies.Count; j++)
					{
						EnemyData data = EnemyFile.Instance.GetData(defeatedEnemies[j]);
						num3 += data.Experience;
						num4 += data.Reward;
					}
					ValueManager instance;
					(instance = ValueManager.Instance)[1] = instance[1] + num4;
					string message2 = string.Empty;
					RufiniString rufiniString = StringFile.Instance.Get("system.battle.experience");
					if (rufiniString.Value != null)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("exp", num3.ToString());
						message2 = TextProcessor.ProcessReplacements(rufiniString.Value, dictionary);
					}
					this.controller.InterfaceController.ClearTextBox();
					this.controller.InterfaceController.ShowTextBox(message2, true);
					this.ChangeState(BattleWinAction.State.WaitForUI);
				}
				else if (this.state == BattleWinAction.State.LevelUp)
				{
					this.controller.InterfaceController.StopWinBGM();
					this.controller.InterfaceController.PlayLevelUpBGM();
					this.controller.InterfaceController.ShowTextBox(this.levelup.GetLevelUpString(), true);
					this.ChangeState(BattleWinAction.State.WaitForUI);
				}
				else if (this.state == BattleWinAction.State.WaitForTimer)
				{
					if (this.timer == 0U && this.previousState == BattleWinAction.State.LevelUp)
					{
						this.controller.InterfaceController.EndLevelUpBGM();
					}
					this.timer += 1U;
					if (this.timer > BattleWinAction.BATTLE_END_DELAY)
					{
						this.ChangeState(BattleWinAction.State.Done);
						this.timer = 0U;
						flag = true;
					}
				}
				else if (this.state == BattleWinAction.State.Done)
				{
					this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
					ITransition transition = new ColorFadeTransition(1f, Color.Black);
					transition.Blocking = true;
					SceneManager.Instance.Transition = transition;
					SceneManager.Instance.Pop();
					this.complete = true;
				}
			}
			while (flag);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000185E0 File Offset: 0x000167E0
		private void InteractionComplete()
		{
			switch (this.previousState)
			{
			case BattleWinAction.State.YouWon:
				this.ChangeState(BattleWinAction.State.ItemDrop);
				return;
			case BattleWinAction.State.ItemDrop:
				this.ChangeState(BattleWinAction.State.Experience);
				return;
			case BattleWinAction.State.Experience:
				this.ChangeState(BattleWinAction.State.Done);
				return;
			case BattleWinAction.State.LevelUp:
				this.ChangeState(BattleWinAction.State.WaitForTimer);
				return;
			default:
				return;
			}
		}

		// Token: 0x0400057B RID: 1403
		private const int CARD_POP_HEIGHT = 28;

		// Token: 0x0400057C RID: 1404
		private static uint BATTLE_END_DELAY = 180U;

		// Token: 0x0400057D RID: 1405
		private BattleWinAction.State previousState;

		// Token: 0x0400057E RID: 1406
		private BattleWinAction.State state;

		// Token: 0x0400057F RID: 1407
		private LevelUpBuilder levelup;

		// Token: 0x04000580 RID: 1408
		private uint timer;

		// Token: 0x020000B0 RID: 176
		private enum State
		{
			// Token: 0x04000582 RID: 1410
			Initialization,
			// Token: 0x04000583 RID: 1411
			WaitForUI,
			// Token: 0x04000584 RID: 1412
			YouWon,
			// Token: 0x04000585 RID: 1413
			ItemDrop,
			// Token: 0x04000586 RID: 1414
			Experience,
			// Token: 0x04000587 RID: 1415
			LevelUp,
			// Token: 0x04000588 RID: 1416
			WaitForTimer,
			// Token: 0x04000589 RID: 1417
			Done
		}
	}
}
