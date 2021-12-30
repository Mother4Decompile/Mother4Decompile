using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Battle.UI;
using Mother4.GUI.Modifiers;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.PsiAnimation
{
	// Token: 0x0200006A RID: 106
	internal class PsiAnimator
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000E706 File Offset: 0x0000C906
		public bool Complete
		{
			get
			{
				return this.complete;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000251 RID: 593 RVA: 0x0000E710 File Offset: 0x0000C910
		// (remove) Token: 0x06000252 RID: 594 RVA: 0x0000E748 File Offset: 0x0000C948
		public event PsiAnimator.AnimationCompleteHandler OnAnimationComplete;

		// Token: 0x06000253 RID: 595 RVA: 0x0000E780 File Offset: 0x0000C980
		public PsiAnimator(RenderPipeline pipeline, List<IGraphicModifier> graphicModifiers, PsiElementList animation, Graphic senderGraphic, Graphic[] targetGraphics, CardBar cardBar, int[] targetCardIds)
		{
			this.pipeline = pipeline;
			this.graphicModifiers = graphicModifiers;
			this.animation = animation;
			this.senderGraphic = senderGraphic;
			this.targetGraphics = targetGraphics;
			this.cardBar = cardBar;
			this.targetCardIds = targetCardIds;
			this.screenShape = new RectangleShape(new Vector2f(320f, 180f));
			this.screenShape.FillColor = new Color(0, 0, 0, 0);
			this.fadeSpeed = 0.2f;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000E804 File Offset: 0x0000CA04
		private void DarkenScreen(Color darkenColor, int depth)
		{
			if (this.screenDarkenShape != null)
			{
				this.pipeline.Remove(this.screenDarkenShape);
			}
			this.targetAlpha = darkenColor.A;
			this.sourceAlpha = this.screenShape.FillColor.A;
			this.alphaMultiplier = 0f;
			FloatRect localBounds = this.screenShape.GetLocalBounds();
			this.screenShape.FillColor = new Color(darkenColor.R, darkenColor.G, darkenColor.B, this.sourceAlpha);
			this.screenDarkenShape = new ShapeGraphic(this.screenShape, new Vector2f(0f, 0f), new Vector2f(0f, 0f), new Vector2f(localBounds.Width, localBounds.Height), depth);
			this.pipeline.Add(this.screenDarkenShape);
			if (this.sourceAlpha == 0)
			{
				if (this.depthMemory == null)
				{
					this.depthMemory = new Dictionary<Graphic, int>();
				}
				else
				{
					this.depthMemory.Clear();
				}
				for (int i = 0; i < this.targetGraphics.Length; i++)
				{
					if (this.targetCardIds[i] < 0)
					{
						Graphic graphic = this.targetGraphics[i];
						this.depthMemory.Add(graphic, graphic.Depth);
						graphic.Depth = 32687;
					}
				}
			}
			this.darkenedFlag = false;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000E958 File Offset: 0x0000CB58
		private void UpdateDarkenColor()
		{
			Color fillColor = this.screenDarkenShape.Shape.FillColor;
			this.alphaMultiplier += this.fadeSpeed;
			fillColor.A = (byte)((float)this.sourceAlpha + (float)(this.targetAlpha - this.sourceAlpha) * this.alphaMultiplier);
			this.screenDarkenShape.Shape.FillColor = fillColor;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
		public void Update()
		{
			List<PsiElement> elementsAtTime = this.animation.GetElementsAtTime(this.step);
			if (elementsAtTime != null && elementsAtTime.Count > 0)
			{
				foreach (PsiElement psiElement in elementsAtTime)
				{
					if (psiElement.Animation != null)
					{
						this.pipeline.Add(psiElement.Animation);
						psiElement.Animation.OnAnimationComplete += this.GraphicAnimationComplete;
						this.animatingCount++;
						if (psiElement.LockToTargetPosition)
						{
							psiElement.Animation.Position = this.targetGraphics[psiElement.PositionIndex].Position;
							psiElement.Animation.Position += psiElement.Offset;
						}
					}
					if (psiElement.Sound != null)
					{
						psiElement.Sound.Play();
					}
					Color? screenDarkenColor = psiElement.ScreenDarkenColor;
					if (screenDarkenColor != null)
					{
						float? screenDarkenSpeed = psiElement.ScreenDarkenSpeed;
						this.fadeSpeed = ((screenDarkenSpeed != null) ? screenDarkenSpeed.GetValueOrDefault() : 0.2f);
						Color? screenDarkenColor2 = psiElement.ScreenDarkenColor;
						this.DarkenScreen(screenDarkenColor2.Value, psiElement.ScreenDarkenDepth ?? 32677);
						this.animatingCount++;
					}
					Color? targetFlashColor = psiElement.TargetFlashColor;
					if (targetFlashColor != null)
					{
						foreach (Graphic graphic in this.targetGraphics)
						{
							if (graphic is IndexedColorGraphic)
							{
								IndexedColorGraphic graphic2 = graphic as IndexedColorGraphic;
								Color? targetFlashColor2 = psiElement.TargetFlashColor;
								GraphicFader item = new GraphicFader(graphic2, targetFlashColor2.Value, psiElement.TargetFlashBlendMode, psiElement.TargetFlashFrames, psiElement.TargetFlashCount);
								this.graphicModifiers.Add(item);
							}
						}
					}
					Color? senderFlashColor = psiElement.SenderFlashColor;
					if (senderFlashColor != null && this.senderGraphic is IndexedColorGraphic)
					{
						IndexedColorGraphic graphic3 = this.senderGraphic as IndexedColorGraphic;
						Color? senderFlashColor2 = psiElement.SenderFlashColor;
						GraphicFader item2 = new GraphicFader(graphic3, senderFlashColor2.Value, psiElement.SenderFlashBlendMode, psiElement.SenderFlashFrames, psiElement.SenderFlashCount);
						this.graphicModifiers.Add(item2);
					}
					foreach (int num in this.targetCardIds)
					{
						if (num >= 0)
						{
							this.cardBar.SetSpring(num, psiElement.CardSpringMode, psiElement.CardSpringAmplitude, psiElement.CardSpringSpeed, psiElement.CardSpringDecay);
						}
					}
				}
			}
			if (this.screenDarkenShape != null && !this.darkenedFlag)
			{
				if (Math.Abs((int)(this.targetAlpha - this.screenDarkenShape.Shape.FillColor.A)) > 1)
				{
					this.UpdateDarkenColor();
				}
				else
				{
					Color fillColor = this.screenDarkenShape.Shape.FillColor;
					fillColor.A = this.targetAlpha;
					this.screenDarkenShape.Shape.FillColor = fillColor;
					if (this.targetAlpha == 0)
					{
						foreach (Graphic graphic4 in this.targetGraphics)
						{
							if (this.depthMemory.ContainsKey(graphic4))
							{
								graphic4.Depth = this.depthMemory[graphic4];
							}
						}
					}
					this.animatingCount--;
					this.darkenedFlag = true;
				}
			}
			this.step++;
			this.complete = (!this.animation.HasElements && this.animatingCount == 0);
			if (this.complete && !this.completedFlag)
			{
				if (this.OnAnimationComplete != null)
				{
					this.OnAnimationComplete(this);
				}
				this.completedFlag = true;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		private void GraphicAnimationComplete(AnimatedRenderable anim)
		{
			anim.Visible = false;
			this.pipeline.Remove(anim);
			anim.OnAnimationComplete -= this.GraphicAnimationComplete;
			this.animatingCount--;
		}

		// Token: 0x04000363 RID: 867
		private const int DARKEN_SHAPE_DEPTH = 32677;

		// Token: 0x04000364 RID: 868
		private const int DARKEN_GRAPHIC_DEPTH = 32687;

		// Token: 0x04000365 RID: 869
		private const float DEFAULT_FADE_SPEED = 0.2f;

		// Token: 0x04000366 RID: 870
		private RenderPipeline pipeline;

		// Token: 0x04000367 RID: 871
		private PsiElementList animation;

		// Token: 0x04000368 RID: 872
		private Graphic senderGraphic;

		// Token: 0x04000369 RID: 873
		private Graphic[] targetGraphics;

		// Token: 0x0400036A RID: 874
		private CardBar cardBar;

		// Token: 0x0400036B RID: 875
		private Shape screenShape;

		// Token: 0x0400036C RID: 876
		private ShapeGraphic screenDarkenShape;

		// Token: 0x0400036D RID: 877
		private byte sourceAlpha;

		// Token: 0x0400036E RID: 878
		private byte targetAlpha;

		// Token: 0x0400036F RID: 879
		private float alphaMultiplier;

		// Token: 0x04000370 RID: 880
		private bool darkenedFlag;

		// Token: 0x04000371 RID: 881
		private float fadeSpeed;

		// Token: 0x04000372 RID: 882
		private Dictionary<Graphic, int> depthMemory;

		// Token: 0x04000373 RID: 883
		private List<IGraphicModifier> graphicModifiers;

		// Token: 0x04000374 RID: 884
		private int[] targetCardIds;

		// Token: 0x04000375 RID: 885
		private bool complete;

		// Token: 0x04000376 RID: 886
		private bool completedFlag;

		// Token: 0x04000377 RID: 887
		private int step;

		// Token: 0x04000378 RID: 888
		private int animatingCount;

		// Token: 0x0200006B RID: 107
		// (Invoke) Token: 0x06000259 RID: 601
		public delegate void AnimationCompleteHandler(PsiAnimator anim);
	}
}
