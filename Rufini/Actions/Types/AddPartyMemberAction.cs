using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Scenes;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.Data;
using Mother4.GUI.Text.PrintActions;
using Mother4.Overworld;
using Mother4.Scenes;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;
using SFML.System;

namespace Rufini.Actions.Types
{
	// Token: 0x02000121 RID: 289
	internal class AddPartyMemberAction : RufiniAction
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x0002BE98 File Offset: 0x0002A098
		public AddPartyMemberAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "char",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "sup",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002BF5C File Offset: 0x0002A15C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			CharacterType byOptionInt = CharacterType.GetByOptionInt(base.GetValue<RufiniOption>("char").Option);
			string npcName = base.GetValue<string>("name");
			bool value = base.GetValue<bool>("sup");
			PartyManager.Instance.Add(byOptionInt);
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == npcName);
				Vector2f position;
				int direction;
				if (npc != null)
				{
					position = npc.Position;
					direction = npc.Direction;
					context.ActorManager.Remove(npc);
					context.CollisionManager.Remove(npc);
				}
				else
				{
					position = context.Player.Position;
					direction = context.Player.Direction;
				}
				PartyTrain partyTrain = ((OverworldScene)scene).PartyTrain;
				PartyFollower partyFollower = new PartyFollower(context.Pipeline, context.CollisionManager, partyTrain, byOptionInt, position, direction, true);
				partyTrain.Add(partyFollower);
				context.CollisionManager.Add(partyFollower);
			}
			if (!value)
			{
				this.context = context;
				string text = StringFile.Instance.Get("system.partyJoin").Value;
				text = text.Replace("$newMember", CharacterNames.GetName(byOptionInt));
				this.context.TextBox.OnTextboxComplete += this.ContinueAfterTextbox;
				this.context.TextBox.Enqueue(new PrintAction(PrintActionType.PrintText, text));
				this.context.TextBox.Enqueue(new PrintAction(PrintActionType.Prompt, new object[0]));
				this.context.TextBox.Show();
				result = new ActionReturnContext
				{
					Wait = ScriptExecutor.WaitType.Event
				};
			}
			return result;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002C130 File Offset: 0x0002A330
		private void ContinueAfterTextbox()
		{
			this.context.TextBox.OnTextboxComplete -= this.ContinueAfterTextbox;
			this.context.Executor.Continue();
		}

		// Token: 0x04000916 RID: 2326
		private const string MSG_KEY = "system.partyJoin";

		// Token: 0x04000917 RID: 2327
		private ExecutionContext context;
	}
}
