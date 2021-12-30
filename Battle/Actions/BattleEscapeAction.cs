using System;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000AE RID: 174
	internal class BattleEscapeAction : BattleAction
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00018173 File Offset: 0x00016373
		public BattleEscapeAction(ActionParams aparams) : base(aparams)
		{
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001817C File Offset: 0x0001637C
		protected override void UpdateAction()
		{
			base.UpdateAction();
			this.controller.InterfaceController.RemoveAllModifiers();
			Console.WriteLine("YOU RAN AWAY!");
			ITransition transition = new ColorFadeTransition(1f, Color.Black);
			transition.Blocking = true;
			SceneManager.Instance.Transition = transition;
			SceneManager.Instance.Pop();
			this.complete = true;
		}
	}
}
