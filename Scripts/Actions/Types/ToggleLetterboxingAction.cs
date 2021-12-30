using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Mother4.Scenes;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000163 RID: 355
	internal class ToggleLetterboxingAction : RufiniAction
	{
		// Token: 0x06000789 RID: 1929 RVA: 0x000314C0 File Offset: 0x0002F6C0
		public ToggleLetterboxingAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "lbx",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00031508 File Offset: 0x0002F708
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			bool value = base.GetValue<bool>("lbx");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				OverworldScene overworldScene = (OverworldScene)scene;
				overworldScene.SetLetterboxing(value);
			}
			return default(ActionReturnContext);
		}
	}
}
