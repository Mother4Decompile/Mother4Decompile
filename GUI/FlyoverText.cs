using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Actors;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.GUI.Text;
using Mother4.GUI.Text.PrintActions;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x0200003A RID: 58
	internal class FlyoverText : Actor
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000128 RID: 296 RVA: 0x0000805C File Offset: 0x0000625C
		// (remove) Token: 0x06000129 RID: 297 RVA: 0x00008094 File Offset: 0x00006294
		public event FlyoverText.OnCompletionHandler OnCompletion;

		// Token: 0x0600012A RID: 298 RVA: 0x000080CC File Offset: 0x000062CC
		public FlyoverText(RenderPipeline pipeline, FontData font, string text, FlyoverText.TextPosition textPosition, Color backColor, Color textColor, int transitionDuration, int holdDuration)
		{
			this.pipeline = pipeline;
			this.transitionDuration = transitionDuration;
			this.holdDuration = holdDuration;
			this.textPosition = textPosition;
			this.duration = this.transitionDuration * 2 + this.holdDuration;
			this.backColor = backColor;
			this.backColorTrans = new Color(backColor.R, backColor.G, backColor.B, 0);
			this.backgroundShape = new RectangleShape(Engine.SCREEN_SIZE);
			this.backgroundShape.FillColor = Color.Transparent;
			this.background = new ShapeGraphic(this.backgroundShape, ViewManager.Instance.View.Center, Engine.HALF_SCREEN_SIZE, Engine.SCREEN_SIZE, 2147467264);
			this.pipeline.Add(this.background);
			this.font = font;
			this.textColor = textColor;
			this.textColorTrans = new Color(textColor.R, textColor.G, textColor.B, 0);
			this.SetupTextRegions(text);
			this.PositionTextRegions(ViewManager.Instance.Center);
			this.UpdateTextColor(Color.Transparent);
			ViewManager.Instance.OnMove += this.ViewMoved;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008204 File Offset: 0x00006404
		private void SetupTextRegions(string text)
		{
			TextProcessor textProcessor = new TextProcessor(text);
			IList<PrintAction> actions = textProcessor.Actions;
			actions.Add(new PrintAction(PrintActionType.LineBreak, new object[0]));
			List<string> list = new List<string>();
			string text2 = string.Empty;
			foreach (PrintAction printAction in actions)
			{
				if (printAction.Type == PrintActionType.PrintText)
				{
					text2 += (string)printAction.Data;
				}
				else if (printAction.Type == PrintActionType.LineBreak && text2.Length > 0)
				{
					list.Add(text2);
					text2 = string.Empty;
				}
			}
			this.texts = new TextRegion[list.Count];
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i] = new TextRegion(VectorMath.ZERO_VECTOR, 2147467265, this.font, list[i]);
				this.texts[i].Color = this.textColorTrans;
				this.texts[i].Origin = new Vector2f((float)this.font.XCompensation, (float)this.font.YCompensation);
				this.pipeline.Add(this.texts[i]);
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000835C File Offset: 0x0000655C
		private void PositionTextRegions(Vector2f center)
		{
			int lineHeight = this.font.LineHeight;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.texts.Length; i++)
			{
				num2 += lineHeight;
				num = Math.Max(num, (int)this.texts[i].Size.X);
			}
			for (int j = 0; j < this.texts.Length; j++)
			{
				Vector2f zero_VECTOR;
				switch (this.textPosition)
				{
				case FlyoverText.TextPosition.Center:
					zero_VECTOR = new Vector2f((float)(-(float)(num / 2)), (float)(-(float)(num2 / 2) + lineHeight * j));
					break;
				case FlyoverText.TextPosition.TopLeft:
					zero_VECTOR = new Vector2f(-144f, (float)(-74L + (long)(lineHeight * j)));
					break;
				case FlyoverText.TextPosition.Top:
					zero_VECTOR = new Vector2f((float)(-(float)(num / 2)), (float)(-74L + (long)(lineHeight * j)));
					break;
				case FlyoverText.TextPosition.TopRight:
					zero_VECTOR = new Vector2f((float)(144L - (long)num), (float)(-74L + (long)(lineHeight * j)));
					break;
				case FlyoverText.TextPosition.Left:
					zero_VECTOR = new Vector2f(-144f, (float)(-(float)(num2 / 2) + lineHeight * j));
					break;
				case FlyoverText.TextPosition.Right:
					zero_VECTOR = new Vector2f((float)(144L - (long)num), (float)(-(float)(num2 / 2) + lineHeight * j));
					break;
				case FlyoverText.TextPosition.BottomLeft:
					zero_VECTOR = new Vector2f(-144f, (float)(74L - (long)num2 + (long)(lineHeight * j)));
					break;
				case FlyoverText.TextPosition.Bottom:
					zero_VECTOR = new Vector2f((float)(-(float)(num / 2)), (float)(74L - (long)num2 + (long)(lineHeight * j)));
					break;
				case FlyoverText.TextPosition.BottomRight:
					zero_VECTOR = new Vector2f((float)(144L - (long)num), (float)(74L - (long)num2 + (long)(lineHeight * j)));
					break;
				default:
					zero_VECTOR = VectorMath.ZERO_VECTOR;
					break;
				}
				this.texts[j].Position = center + zero_VECTOR;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008518 File Offset: 0x00006718
		private void UpdateTextColor(Color color)
		{
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i].Color = color;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008546 File Offset: 0x00006746
		private void ViewMoved(ViewManager sender, Vector2f newCenter)
		{
			this.background.Position = newCenter;
			this.PositionTextRegions(newCenter);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000855C File Offset: 0x0000675C
		public override void Update()
		{
			base.Update();
			if (this.timer < this.duration)
			{
				if (this.timer < this.transitionDuration)
				{
					this.backgroundShape.FillColor = ColorHelper.BlendAlpha(this.backColorTrans, this.backColor, (float)this.timer / (float)this.transitionDuration);
					this.UpdateTextColor(ColorHelper.BlendAlpha(this.textColorTrans, this.textColor, (float)this.timer / (float)this.transitionDuration));
				}
				else if (this.timer == this.transitionDuration)
				{
					this.backgroundShape.FillColor = this.backColor;
					this.UpdateTextColor(this.textColor);
				}
				else if (this.timer > this.transitionDuration + this.holdDuration)
				{
					this.backgroundShape.FillColor = ColorHelper.BlendAlpha(this.backColor, this.backColorTrans, (float)(this.timer - (this.transitionDuration + this.holdDuration)) / (float)this.transitionDuration);
					this.UpdateTextColor(ColorHelper.BlendAlpha(this.textColor, this.textColorTrans, (float)(this.timer - (this.transitionDuration + this.holdDuration)) / (float)this.transitionDuration));
				}
				this.timer++;
				if (this.timer == this.duration && this.OnCompletion != null)
				{
					this.OnCompletion();
				}
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000086C8 File Offset: 0x000068C8
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.texts.Length; i++)
					{
						this.texts[i].Dispose();
					}
					this.background.Dispose();
					this.backgroundShape.Dispose();
				}
				this.pipeline.Remove(this.background);
				for (int j = 0; j < this.texts.Length; j++)
				{
					this.pipeline.Remove(this.texts[j]);
				}
				ViewManager.Instance.OnMove -= this.ViewMoved;
				base.Dispose(disposing);
			}
		}

		// Token: 0x04000212 RID: 530
		private const int DEPTH = 2147467264;

		// Token: 0x04000213 RID: 531
		private const int MARGIN = 16;

		// Token: 0x04000214 RID: 532
		private const int WIDTH = 288;

		// Token: 0x04000215 RID: 533
		private const int HEIGHT = 148;

		// Token: 0x04000216 RID: 534
		private RenderPipeline pipeline;

		// Token: 0x04000217 RID: 535
		private Color textColor;

		// Token: 0x04000218 RID: 536
		private Color textColorTrans;

		// Token: 0x04000219 RID: 537
		private TextRegion[] texts;

		// Token: 0x0400021A RID: 538
		private int timer;

		// Token: 0x0400021B RID: 539
		private int duration;

		// Token: 0x0400021C RID: 540
		private int transitionDuration;

		// Token: 0x0400021D RID: 541
		private int holdDuration;

		// Token: 0x0400021E RID: 542
		private FlyoverText.TextPosition textPosition;

		// Token: 0x0400021F RID: 543
		private FontData font;

		// Token: 0x04000220 RID: 544
		private Color backColor;

		// Token: 0x04000221 RID: 545
		private Color backColorTrans;

		// Token: 0x04000222 RID: 546
		private Shape backgroundShape;

		// Token: 0x04000223 RID: 547
		private ShapeGraphic background;

		// Token: 0x0200003B RID: 59
		public enum TextPosition
		{
			// Token: 0x04000226 RID: 550
			Center,
			// Token: 0x04000227 RID: 551
			TopLeft,
			// Token: 0x04000228 RID: 552
			Top,
			// Token: 0x04000229 RID: 553
			TopRight,
			// Token: 0x0400022A RID: 554
			Left,
			// Token: 0x0400022B RID: 555
			Right,
			// Token: 0x0400022C RID: 556
			BottomLeft,
			// Token: 0x0400022D RID: 557
			Bottom,
			// Token: 0x0400022E RID: 558
			BottomRight
		}

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x06000132 RID: 306
		public delegate void OnCompletionHandler();
	}
}
