using System;
using System.Collections.Generic;
using Carbine.Input;
using Mother4.Actors.NPCs;
using Mother4.Scripts.Actions;
using Rufini.Actions.Types;

namespace Mother4.Scripts
{
	// Token: 0x0200016C RID: 364
	internal class ScriptExecutor
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00031B14 File Offset: 0x0002FD14
		public bool Running
		{
			get
			{
				return this.running;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00031B1C File Offset: 0x0002FD1C
		public int ProgramCounter
		{
			get
			{
				return this.programCounter;
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00031B24 File Offset: 0x0002FD24
		public ScriptExecutor(ExecutionContext context)
		{
			this.contextStack = new Stack<ScriptExecutor.ScriptContext>();
			this.context = context;
			this.context.Executor = this;
			this.waitMode = ScriptExecutor.WaitType.None;
			this.pausedInstruction = 0;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00031B58 File Offset: 0x0002FD58
		public void PushScript(Script script)
		{
			if (this.waitMode != ScriptExecutor.WaitType.None)
			{
				throw new InvalidOperationException("Cannot push a new script while waiting for an action to complete.");
			}
			if (this.contextStack.Count < 32)
			{
				if (this.script != null)
				{
					this.contextStack.Push(new ScriptExecutor.ScriptContext(this.context, this.script.Value, this.programCounter));
					this.Reset();
					this.context = new ExecutionContext(this.context);
				}
				this.script = new Script?(script);
				this.actions = this.script.Value.Actions;
				this.programCounter = 0;
				this.pushedScript = true;
				return;
			}
			string message = string.Format("Script Executor stack cannot exceed {0} levels.", 32);
			throw new StackOverflowException(message);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00031C1B File Offset: 0x0002FE1B
		public void SetCheckedNPC(NPC npc)
		{
			this.context.CheckedNPC = npc;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00031C2C File Offset: 0x0002FE2C
		private void Reset()
		{
			this.pausedInstruction = 0;
			this.running = false;
			this.script = null;
			if (this.context.ActiveNPC != null)
			{
				this.context.ActiveNPC.StopTalking();
			}
			this.context.ActiveNPC = null;
			this.context.CheckedNPC = null;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00031C88 File Offset: 0x0002FE88
		public void Continue()
		{
			this.waitMode = ScriptExecutor.WaitType.None;
			Console.WriteLine("EX: {0} Continued", this.programCounter);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00031CA8 File Offset: 0x0002FEA8
		public void JumpToElseOrEndIf()
		{
			if (this.script != null)
			{
				Script value = this.script.Value;
				int num = value.Actions.Length;
				int num2 = 0;
				for (int i = this.programCounter; i < num; i++)
				{
					RufiniAction rufiniAction = value.Actions[i];
					if (rufiniAction is IfFlagAction || rufiniAction is IfValueAction || rufiniAction is IfReturnAction)
					{
						num2++;
					}
					else if (rufiniAction is EndIfAction)
					{
						num2--;
						if (num2 == 0)
						{
							this.programCounter = i;
							return;
						}
					}
					else if (rufiniAction is ElseAction)
					{
						if (i == this.programCounter)
						{
							num2++;
						}
						else if (num2 - 1 == 0)
						{
							this.programCounter = i;
							return;
						}
					}
				}
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00031D58 File Offset: 0x0002FF58
		private bool PopScript()
		{
			bool result = true;
			this.Reset();
			if (this.contextStack.Count > 0)
			{
				ScriptExecutor.ScriptContext scriptContext = this.contextStack.Pop();
				this.context = scriptContext.ExecutionContext;
				this.script = new Script?(scriptContext.Script);
				this.actions = this.script.Value.Actions;
				this.pausedInstruction = scriptContext.ProgramCounter + 1;
			}
			else
			{
				Console.WriteLine("EX: End of execution");
				result = false;
				if (this.context.Player != null)
				{
					this.context.Player.InputLocked = false;
				}
				InputManager.Instance.Enabled = true;
				this.context.TextBox.Reset();
				if (this.context.TextBox.Visible)
				{
					this.context.TextBox.Hide();
				}
			}
			return result;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00031E38 File Offset: 0x00030038
		public void Execute()
		{
			if (this.script != null)
			{
				this.running = true;
				if (this.waitMode != ScriptExecutor.WaitType.Event)
				{
					bool flag = true;
					while (flag)
					{
						flag = false;
						this.programCounter = 0;
						this.programCounter = this.pausedInstruction;
						while (this.programCounter < this.actions.Length)
						{
							if (this.pushedScript)
							{
								this.pushedScript = false;
								this.programCounter = 0;
							}
							RufiniAction rufiniAction = this.actions[this.programCounter];
							Console.WriteLine("EX: {0} Execute {1}", this.programCounter, rufiniAction.GetType().Name);
							this.waitMode = rufiniAction.Execute(this.context).Wait;
							if (this.waitMode != ScriptExecutor.WaitType.None)
							{
								this.pausedInstruction = this.programCounter + 1;
								Console.WriteLine("EX: {0} Paused (Next: {1})", this.programCounter, this.pausedInstruction);
								break;
							}
							this.programCounter++;
						}
						if (this.waitMode == ScriptExecutor.WaitType.None && this.programCounter >= this.actions.Length)
						{
							Console.WriteLine("EX: End of script; popping");
							flag = this.PopScript();
						}
					}
				}
			}
		}

		// Token: 0x0400096A RID: 2410
		private const int STACK_MAX_SIZE = 32;

		// Token: 0x0400096B RID: 2411
		private Stack<ScriptExecutor.ScriptContext> contextStack;

		// Token: 0x0400096C RID: 2412
		private ExecutionContext context;

		// Token: 0x0400096D RID: 2413
		private Script? script;

		// Token: 0x0400096E RID: 2414
		private RufiniAction[] actions;

		// Token: 0x0400096F RID: 2415
		private bool running;

		// Token: 0x04000970 RID: 2416
		private ScriptExecutor.WaitType waitMode;

		// Token: 0x04000971 RID: 2417
		private int pausedInstruction;

		// Token: 0x04000972 RID: 2418
		private int programCounter;

		// Token: 0x04000973 RID: 2419
		private bool pushedScript;

		// Token: 0x0200016D RID: 365
		public enum WaitType
		{
			// Token: 0x04000975 RID: 2421
			None,
			// Token: 0x04000976 RID: 2422
			Frame,
			// Token: 0x04000977 RID: 2423
			Event
		}

		// Token: 0x0200016E RID: 366
		private struct ScriptContext
		{
			// Token: 0x17000125 RID: 293
			// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00031F6F File Offset: 0x0003016F
			public ExecutionContext ExecutionContext
			{
				get
				{
					return this.context;
				}
			}

			// Token: 0x17000126 RID: 294
			// (get) Token: 0x060007BA RID: 1978 RVA: 0x00031F77 File Offset: 0x00030177
			public Script Script
			{
				get
				{
					return this.script;
				}
			}

			// Token: 0x17000127 RID: 295
			// (get) Token: 0x060007BB RID: 1979 RVA: 0x00031F7F File Offset: 0x0003017F
			public int ProgramCounter
			{
				get
				{
					return this.programCounter;
				}
			}

			// Token: 0x060007BC RID: 1980 RVA: 0x00031F87 File Offset: 0x00030187
			public ScriptContext(ExecutionContext context, Script script, int programCounter)
			{
				this.context = context;
				this.script = script;
				this.programCounter = programCounter;
			}

			// Token: 0x04000978 RID: 2424
			private ExecutionContext context;

			// Token: 0x04000979 RID: 2425
			private Script script;

			// Token: 0x0400097A RID: 2426
			private int programCounter;
		}
	}
}
