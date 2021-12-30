using System;
using System.Collections.Generic;
using Carbine.Maps;
using Carbine.Scenes;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.Data;
using Mother4.Overworld;
using Mother4.Scenes;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000152 RID: 338
	internal class RemovePartyMemberAction : RufiniAction
	{
		// Token: 0x06000761 RID: 1889 RVA: 0x00030220 File Offset: 0x0002E420
		public RemovePartyMemberAction()
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
				}
			};
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00030290 File Offset: 0x0002E490
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			CharacterType byOptionInt = CharacterType.GetByOptionInt(base.GetValue<RufiniOption>("char").Option);
			string value = base.GetValue<string>("name");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				PartyTrain partyTrain = ((OverworldScene)scene).PartyTrain;
				IList<PartyFollower> list = partyTrain.Remove(byOptionInt);
				for (int i = 0; i < list.Count; i++)
				{
					PartyFollower partyFollower = list[i];
					Map.NPC npcData = new Map.NPC
					{
						Name = value,
						Solid = true,
						Shadow = true,
						X = (int)partyFollower.Position.X,
						Y = (int)partyFollower.Position.Y,
						DepthOverride = int.MinValue,
						Direction = (short)partyFollower.Direction,
						Sprite = CharacterGraphics.GetFile(partyFollower.Character, false)
					};
					NPC actor = new NPC(context.Pipeline, context.CollisionManager, npcData, null);
					context.ActorManager.Add(actor);
					context.CollisionManager.Remove(partyFollower);
					partyFollower.Dispose();
				}
			}
			PartyManager.Instance.Remove(byOptionInt);
			return default(ActionReturnContext);
		}
	}
}
