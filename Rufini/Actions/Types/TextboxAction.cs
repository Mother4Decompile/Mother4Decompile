using System;
using System.Collections.Generic;
using Mother4.Actors.NPCs;
using Mother4.GUI.Text;
using Mother4.GUI.Text.PrintActions;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Rufini.Strings;

namespace Rufini.Actions.Types
{
	// Token: 0x02000161 RID: 353
	internal class TextboxAction : RufiniAction
	{
		// Token: 0x06000783 RID: 1923 RVA: 0x0003129C File Offset: 0x0002F49C
		public TextboxAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "text",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000312E4 File Offset: 0x0002F4E4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			string value = base.GetValue<string>("text");
			string value2 = StringFile.Instance.Get(value).Value;
			this.context.TextBox.OnTextboxComplete += this.ContinueAfterTextbox;
			TextProcessor textProcessor = new TextProcessor(value2);
			if (this.context.TextBox.HasPrinted)
			{
				this.context.TextBox.Enqueue(new PrintAction(PrintActionType.LineBreak, new object[0]));
			}
			this.context.TextBox.EnqueueAll(textProcessor.Actions);
			this.context.TextBox.Enqueue(new PrintAction(PrintActionType.Prompt, new object[0]));
			this.context.TextBox.Show();
			if (this.context.ActiveNPC != null)
			{
				this.activeNpc = this.context.ActiveNPC;
				this.activeNpc.StartTalking();
			}
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000313E6 File Offset: 0x0002F5E6
		private void StopTalking()
		{
			if (this.context.ActiveNPC != null)
			{
				this.activeNpc.StopTalking();
				this.activeNpc = null;
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00031407 File Offset: 0x0002F607
		private void ContinueAfterTextbox()
		{
			this.StopTalking();
			this.context.TextBox.OnTextboxComplete -= this.ContinueAfterTextbox;
			this.context.Executor.Continue();
		}

		// Token: 0x0400095A RID: 2394
		private ExecutionContext context;

		// Token: 0x0400095B RID: 2395
		private NPC activeNpc;
	}
}
