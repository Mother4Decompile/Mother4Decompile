using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI.Text;
using Mother4.GUI.Text.PrintActions;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x02000046 RID: 70
	internal abstract class TextBox : Renderable
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000A3CE File Offset: 0x000085CE
		public bool HasPrinted
		{
			get
			{
				return this.hasPrinted;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000184 RID: 388 RVA: 0x0000A3D8 File Offset: 0x000085D8
		// (remove) Token: 0x06000185 RID: 389 RVA: 0x0000A410 File Offset: 0x00008610
		public event TextBox.CompletionHandler OnTextboxComplete;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000186 RID: 390 RVA: 0x0000A448 File Offset: 0x00008648
		// (remove) Token: 0x06000187 RID: 391 RVA: 0x0000A480 File Offset: 0x00008680
		public event TextBox.TextTriggerHandler OnTextTrigger;

		// Token: 0x06000188 RID: 392 RVA: 0x0000A4B8 File Offset: 0x000086B8
		protected TextBox(Vector2f relativePosition, Vector2f size, bool showBullets)
		{
			this.relativePosition = relativePosition;
			this.size = size;
			this.depth = 2147450880;
			this.actionQueue = new Queue<PrintAction>();
			this.relativeTypewriterPosition = this.relativePosition + TextBox.TYPEWRITER_POSITION_OFFSET;
			this.typewriter = new Typewriter(Fonts.Main, VectorMath.ZERO_VECTOR, VectorMath.ZERO_VECTOR, this.size + TextBox.TYPEWRITER_SIZE_OFFSET, showBullets);
			this.window = new WindowBox(Settings.WindowStyle, Settings.WindowFlavor, VectorMath.ZERO_VECTOR, this.size, 0);
			this.window.Visible = false;
			this.relativeArrowPosition = this.relativePosition + this.size + TextBox.ARROW_POSITION_OFFSET;
			this.advanceArrow = new IndexedColorGraphic(Paths.GRAPHICS + "cursor.dat", "down", VectorMath.ZERO_VECTOR, 0);
			this.advanceArrow.Visible = false;
			this.visible = false;
			this.hideAdvanceArrow = false;
			this.typewriter.OnTypewriterComplete += this.typewriter_OnTypewriterComplete;
			ViewManager.Instance.OnMove += this.OnViewMove;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000A5EA File Offset: 0x000087EA
		private void typewriter_OnTypewriterComplete(object sender, EventArgs e)
		{
			if (!this.isPaused && !this.isWaitingOnPlayer)
			{
				this.Dequeue();
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000A602 File Offset: 0x00008802
		private void TimerManager_OnTimerEnd(int timerIndex)
		{
			if (this.pauseTimerIndex == timerIndex && this.isPaused)
			{
				this.isPaused = false;
				this.Dequeue();
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000A622 File Offset: 0x00008822
		private void OnViewMove(ViewManager sender, Vector2f center)
		{
			if (this.visible)
			{
				this.Recenter();
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000A632 File Offset: 0x00008832
		protected void ContinueFromWait()
		{
			if (this.isWaitingOnPlayer)
			{
				this.isWaitingOnPlayer = false;
				this.advanceArrow.Visible = false;
				this.Dequeue();
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000A655 File Offset: 0x00008855
		public void Enqueue(PrintAction action)
		{
			if (action.Type != PrintActionType.None)
			{
				this.actionQueue.Enqueue(action);
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000A66C File Offset: 0x0000886C
		public void EnqueueAll(IEnumerable<PrintAction> actions)
		{
			foreach (PrintAction action in actions)
			{
				this.Enqueue(action);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000A6B4 File Offset: 0x000088B4
		protected virtual void HandlePrintText(string text)
		{
			this.typewriter.PrintText(text);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000A6C2 File Offset: 0x000088C2
		protected virtual void HandlePrintGraphic(string subsprite)
		{
			this.typewriter.PrintGraphic(subsprite);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000A6D8 File Offset: 0x000088D8
		protected virtual void HandlePromptQuestion(object[] options)
		{
			string[] options2 = Array.ConvertAll<object, string>(options, (object x) => (string)x);
			this.typewriter.PrintQuestion(options2);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000A71D File Offset: 0x0000891D
		protected virtual void HandlePromptList(object[] options)
		{
			Array.ConvertAll<object, string>(options, (object x) => (string)x);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000A743 File Offset: 0x00008943
		protected virtual void HandlePromptNumeric(int minValue, int maxValue)
		{
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000A745 File Offset: 0x00008945
		protected virtual void HandlePrompt()
		{
			this.isWaitingOnPlayer = true;
			this.advanceArrow.Visible = !this.hideAdvanceArrow;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000A762 File Offset: 0x00008962
		protected virtual void HandlePause(int duration)
		{
			this.isPaused = true;
			this.pauseTimerIndex = TimerManager.Instance.StartTimer(duration);
			TimerManager.Instance.OnTimerEnd += this.TimerManager_OnTimerEnd;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000A792 File Offset: 0x00008992
		protected virtual void HandleLineBreak()
		{
			this.typewriter.PrintNewLine();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000A7A0 File Offset: 0x000089A0
		protected virtual void HandleTrigger(object[] args)
		{
			if (this.OnTextTrigger != null)
			{
				int type = int.Parse((string)args[0]);
				string[] args2 = new string[args.Length - 1];
				this.OnTextTrigger(type, args2);
			}
			this.Dequeue();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000A7E1 File Offset: 0x000089E1
		protected virtual void HandleColor(Color color)
		{
			this.typewriter.SetTextColor(color, true);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000A7F0 File Offset: 0x000089F0
		protected virtual void HandleSound(int type)
		{
			int num = Math.Max(0, Math.Min(6, type));
			Typewriter.BlipSound soundType = Typewriter.BlipSound.None;
			switch (num)
			{
			case 0:
				soundType = Typewriter.BlipSound.None;
				break;
			case 1:
				soundType = Typewriter.BlipSound.Narration;
				break;
			case 2:
				soundType = Typewriter.BlipSound.Male;
				break;
			case 3:
				soundType = Typewriter.BlipSound.Female;
				break;
			case 4:
				soundType = Typewriter.BlipSound.Awkward;
				break;
			case 5:
				soundType = Typewriter.BlipSound.Robot;
				break;
			}
			this.typewriter.SetTextSound(soundType, true);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000A854 File Offset: 0x00008A54
		private void HandleAction(PrintAction action)
		{
			switch (action.Type)
			{
			case PrintActionType.PrintText:
				this.HandlePrintText((string)action.Data);
				return;
			case PrintActionType.PrintGraphic:
				this.HandlePrintGraphic((string)action.Data);
				return;
			case PrintActionType.PromptQuestion:
				this.HandlePromptQuestion((object[])action.Data);
				return;
			case PrintActionType.PromptList:
				this.HandlePromptList((object[])action.Data);
				return;
			case PrintActionType.PromptNumeric:
			{
				int[] array = (int[])action.Data;
				this.HandlePromptNumeric(array[0], array[1]);
				return;
			}
			case PrintActionType.Prompt:
				this.HandlePrompt();
				return;
			case PrintActionType.Pause:
				this.HandlePause((int)action.Data);
				return;
			case PrintActionType.LineBreak:
				this.HandleLineBreak();
				return;
			case PrintActionType.Trigger:
				this.HandleTrigger((object[])action.Data);
				return;
			case PrintActionType.Color:
				break;
			case PrintActionType.Sound:
				this.HandleSound((int)action.Data);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000A94C File Offset: 0x00008B4C
		protected void Dequeue()
		{
			if (this.actionQueue.Count > 0)
			{
				PrintAction action = this.actionQueue.Dequeue();
				try
				{
					this.HandleAction(action);
					this.hasPrinted = true;
					return;
				}
				catch (InvalidCastException ex)
				{
					Console.WriteLine("Ate an InvalidCastException in the PrintReceiver:");
					Console.WriteLine(ex.Message);
					return;
				}
			}
			if (!this.isComplete && !this.isWaitingOnPlayer)
			{
				this.isComplete = true;
				if (this.OnTextboxComplete != null)
				{
					this.OnTextboxComplete();
				}
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		public virtual void Show()
		{
			if (!this.visible)
			{
				this.Recenter();
				this.visible = true;
				this.typewriter.Visible = true;
				this.window.Visible = true;
				this.advanceArrow.Visible = false;
				this.Dequeue();
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000AA24 File Offset: 0x00008C24
		public virtual void Hide()
		{
			if (this.visible)
			{
				this.visible = false;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000AA35 File Offset: 0x00008C35
		public virtual void Clear()
		{
			this.typewriter.Clear();
			this.hasPrinted = false;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000AA49 File Offset: 0x00008C49
		public virtual void Reset()
		{
			this.Clear();
			this.isWaitingOnPlayer = false;
			this.hasPrinted = false;
			this.isComplete = false;
			this.typewriter.SetTextColor(Color.White, false);
			this.typewriter.SetTextSound(Typewriter.BlipSound.Narration, false);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000AA84 File Offset: 0x00008C84
		protected virtual void Recenter()
		{
			Vector2f finalCenter = ViewManager.Instance.FinalCenter;
			Vector2f v = finalCenter - ViewManager.Instance.View.Size / 2f;
			this.position = v + this.relativePosition;
			this.window.Position = this.position;
			this.advanceArrow.Position = v + this.relativeArrowPosition;
			this.typewriter.Position = v + this.relativeTypewriterPosition;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000AB0D File Offset: 0x00008D0D
		public virtual void Update()
		{
			if (this.visible)
			{
				if (this.isComplete && this.actionQueue.Count > 0)
				{
					this.Dequeue();
					this.isComplete = false;
				}
				this.typewriter.Update();
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000AB48 File Offset: 0x00008D48
		public override void Draw(RenderTarget target)
		{
			if (this.window.Visible)
			{
				this.window.Draw(target);
			}
			if (this.typewriter.Visible)
			{
				this.typewriter.Draw(target);
			}
			if (this.advanceArrow.Visible)
			{
				this.advanceArrow.Draw(target);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.advanceArrow.Dispose();
					this.window.Dispose();
					this.typewriter.Dispose();
				}
				this.typewriter.OnTypewriterComplete -= this.typewriter_OnTypewriterComplete;
				ViewManager.Instance.OnMove -= this.OnViewMove;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000276 RID: 630
		public const int DEPTH = 2147450880;

		// Token: 0x04000277 RID: 631
		private static readonly Vector2f TYPEWRITER_POSITION_OFFSET = new Vector2f(10f, 8f);

		// Token: 0x04000278 RID: 632
		private static readonly Vector2f TYPEWRITER_SIZE_OFFSET = new Vector2f(-31f, -14f);

		// Token: 0x04000279 RID: 633
		private static readonly Vector2f ARROW_POSITION_OFFSET = new Vector2f(-14f, -6f);

		// Token: 0x0400027A RID: 634
		private Vector2f relativePosition;

		// Token: 0x0400027B RID: 635
		private Vector2f relativeArrowPosition;

		// Token: 0x0400027C RID: 636
		private Vector2f relativeTypewriterPosition;

		// Token: 0x0400027D RID: 637
		protected Graphic advanceArrow;

		// Token: 0x0400027E RID: 638
		protected WindowBox window;

		// Token: 0x0400027F RID: 639
		protected Typewriter typewriter;

		// Token: 0x04000280 RID: 640
		private Queue<PrintAction> actionQueue;

		// Token: 0x04000281 RID: 641
		private bool isPaused;

		// Token: 0x04000282 RID: 642
		private int pauseTimerIndex;

		// Token: 0x04000283 RID: 643
		protected bool isWaitingOnPlayer;

		// Token: 0x04000284 RID: 644
		protected bool hideAdvanceArrow;

		// Token: 0x04000285 RID: 645
		private bool hasPrinted;

		// Token: 0x04000286 RID: 646
		private bool isComplete;

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x060001A8 RID: 424
		public delegate void CompletionHandler();

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x060001AC RID: 428
		public delegate void TextTriggerHandler(int type, string[] args);
	}
}
