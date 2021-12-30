using System;
using System.Collections.Generic;
using Mother4.Actors.NPCs;
using Mother4.GUI.Text.PrintActions;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000151 RID: 337
	internal class QuestionAction : RufiniAction
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x0002FFF4 File Offset: 0x0002E1F4
		public QuestionAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = QuestionAction.OPT_NAMES[0],
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = QuestionAction.OPT_NAMES[1],
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = QuestionAction.OPT_NAMES[2],
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = QuestionAction.OPT_NAMES[3],
					Type = typeof(RufiniString)
				},
				new ActionParam
				{
					Name = QuestionAction.OPT_NAMES[4],
					Type = typeof(RufiniString)
				}
			};
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000300F0 File Offset: 0x0002E2F0
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			int num = 0;
			for (int i = 0; i < QuestionAction.OPT_NAMES.Length; i++)
			{
				if (!base.HasValue(QuestionAction.OPT_NAMES[i]))
				{
					num = i;
					break;
				}
			}
			string[] array = new string[num];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = base.GetValue<RufiniString>(QuestionAction.OPT_NAMES[j]).Value;
			}
			this.context.TextBox.OnTextboxComplete += this.ContinueAfterTextbox;
			this.context.TextBox.Enqueue(new PrintAction(PrintActionType.PromptQuestion, array));
			this.context.TextBox.Show();
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000301AE File Offset: 0x0002E3AE
		private void ContinueAfterTextbox()
		{
			this.context.TextBox.OnTextboxComplete -= this.ContinueAfterTextbox;
			this.context.Executor.Continue();
		}

		// Token: 0x04000949 RID: 2377
		private static readonly string[] OPT_NAMES = new string[]
		{
			"opt1",
			"opt2",
			"opt3",
			"opt4",
			"opt5"
		};

		// Token: 0x0400094A RID: 2378
		private ExecutionContext context;

		// Token: 0x0400094B RID: 2379
		private NPC activeNpc;
	}
}
