using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Mother4.Scenes;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x0200015C RID: 348
	internal class SetTilesetPaletteAction : RufiniAction
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x00030CD4 File Offset: 0x0002EED4
		public SetTilesetPaletteAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "pal",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00030D1C File Offset: 0x0002EF1C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("pal");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				OverworldScene overworldScene = (OverworldScene)scene;
				overworldScene.SetTilesetPalette(value);
			}
			return default(ActionReturnContext);
		}
	}
}
