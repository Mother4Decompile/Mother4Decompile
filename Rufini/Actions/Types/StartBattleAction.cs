using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Data;
using Mother4.Scenes;
using Mother4.Scenes.Transitions;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using SFML.Graphics;

namespace Rufini.Actions.Types
{
	// Token: 0x0200015D RID: 349
	internal class StartBattleAction : RufiniAction
	{
		// Token: 0x0600077A RID: 1914 RVA: 0x00030D60 File Offset: 0x0002EF60
		public StartBattleAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "type",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "enm",
					Type = typeof(int[])
				},
				new ActionParam
				{
					Name = "bgm",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "bbg",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00030E24 File Offset: 0x0002F024
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			RufiniOption value = base.GetValue<RufiniOption>("type");
			bool letterboxing = value.Option != 3;
			int value2 = base.GetValue<int>("bgm");
			int value3 = base.GetValue<int>("bbg");
			int[] value4 = base.GetValue<int[]>("enm");
			if (value4 != null && value4.Length > 0)
			{
				EnemyType[] array = new EnemyType[value4.Length];
				for (int i = 0; i < value4.Length; i++)
				{
					array[i] = (EnemyType)value4[i];
				}
				BattleScene newScene = new BattleScene(array, letterboxing, value2, value3);
				ITransition transition;
				switch (value.Option)
				{
				case 0:
					transition = new BattleFadeTransition(1f, Color.Blue);
					break;
				case 1:
					transition = new BattleFadeTransition(1f, Color.Green);
					break;
				case 2:
					transition = new BattleFadeTransition(1f, Color.Red);
					break;
				default:
					transition = new BattleFadeTransition(1f, Color.Black);
					break;
				}
				transition.Blocking = true;
				SceneManager.Instance.Transition = transition;
				SceneManager.Instance.Push(newScene);
			}
			else
			{
				Console.WriteLine("There were no enemies specified.");
			}
			return default(ActionReturnContext);
		}

		// Token: 0x04000955 RID: 2389
		private const int NORMAL_MODE = 0;

		// Token: 0x04000956 RID: 2390
		private const int PLAYER_ADV_MODE = 1;

		// Token: 0x04000957 RID: 2391
		private const int ENEMY_ADV_MODE = 2;

		// Token: 0x04000958 RID: 2392
		private const int BOSS_MODE = 3;
	}
}
