using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Utility;
using Mother4.GUI;
using Mother4.GUI.Text;
using Mother4.GUI.Text.PrintActions;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x02000113 RID: 275
	internal class TextTestScene : StandardScene
	{
		// Token: 0x0600069D RID: 1693 RVA: 0x00029B04 File Offset: 0x00027D04
		public TextTestScene()
		{
			this.textProcessor = new TextProcessor("@Here's some long text. It's long long long! Wow look [c:FF00FF00]how[c:FFFFFFFF] long this line is! Gee golly, it just keeps going!\n@Phew it's over! Here's a new line! Oh no it's starting to get long again. Aw geez, it's super long now! It's longer than last time![g:fly left]\n@LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOONG-WORD.");
			this.index = 0;
			Vector2f vector2f = new Vector2f(200f, 48f);
			this.typewriter = new Typewriter(Fonts.Main, Engine.HALF_SCREEN_SIZE, VectorMath.Truncate(vector2f / 2f), vector2f, true);
			this.typewriter.OnTypewriterComplete += this.typewriter_OnTypewriterComplete;
			this.pipeline.Add(this.typewriter);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00029B90 File Offset: 0x00027D90
		private void DoTypewriterAction()
		{
			IList<PrintAction> actions = this.textProcessor.Actions;
			bool flag = true;
			while (flag)
			{
				flag = false;
				PrintAction printAction = actions[this.index % actions.Count];
				PrintActionType type = printAction.Type;
				switch (type)
				{
				case PrintActionType.PrintText:
					this.typewriter.PrintText((string)printAction.Data);
					break;
				case PrintActionType.PrintGraphic:
					this.typewriter.PrintGraphic((string)printAction.Data);
					break;
				default:
					switch (type)
					{
					case PrintActionType.Pause:
						this.pauseTimerIndex = TimerManager.Instance.StartTimer((int)printAction.Data);
						TimerManager.Instance.OnTimerEnd += this.OnTimerEnd;
						goto IL_F1;
					case PrintActionType.LineBreak:
						this.typewriter.PrintNewLine();
						flag = true;
						goto IL_F1;
					case PrintActionType.Color:
						this.typewriter.SetTextColor((Color)printAction.Data, true);
						flag = true;
						goto IL_F1;
					}
					flag = true;
					break;
				}
				IL_F1:
				this.index++;
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00029CA2 File Offset: 0x00027EA2
		private void OnTimerEnd(int timerIndex)
		{
			if (this.pauseTimerIndex == timerIndex)
			{
				this.updateFlag = true;
				TimerManager.Instance.OnTimerEnd -= this.OnTimerEnd;
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00029CCA File Offset: 0x00027ECA
		private void typewriter_OnTypewriterComplete(object sender, EventArgs e)
		{
			this.updateFlag = true;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00029CD3 File Offset: 0x00027ED3
		public override void Focus()
		{
			base.Focus();
			this.DoTypewriterAction();
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00029CE1 File Offset: 0x00027EE1
		public override void Update()
		{
			base.Update();
			if (this.updateFlag)
			{
				this.DoTypewriterAction();
				this.updateFlag = false;
			}
			this.typewriter.Update();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00029D09 File Offset: 0x00027F09
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.typewriter.Dispose();
				}
				this.typewriter.OnTypewriterComplete -= this.typewriter_OnTypewriterComplete;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000897 RID: 2199
		private TextProcessor textProcessor;

		// Token: 0x04000898 RID: 2200
		private Typewriter typewriter;

		// Token: 0x04000899 RID: 2201
		private int index;

		// Token: 0x0400089A RID: 2202
		private bool updateFlag;

		// Token: 0x0400089B RID: 2203
		private int pauseTimerIndex;
	}
}
