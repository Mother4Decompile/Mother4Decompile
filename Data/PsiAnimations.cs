using System;
using System.Collections.Generic;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Battle.PsiAnimation;
using Mother4.Battle.UI;
using Mother4.Data.Psi;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Data
{
	// Token: 0x02000089 RID: 137
	internal class PsiAnimations
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0001160C File Offset: 0x0000F80C
		private static PsiElementList GenerateFreezeAlpha()
		{
			List<PsiElement> elements = new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "freeze_a.sdat", default(Vector2f), 0.5f, 32767),
					Sound = AudioManager.Instance.Use(Paths.AUDIO + "pkFreezeA.wav", AudioType.Sound),
					LockToTargetPosition = true,
					PositionIndex = 0,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 128))
				},
				new PsiElement
				{
					Timestamp = 20,
					TargetFlashColor = new Color?(Color.Cyan),
					TargetFlashBlendMode = ColorBlendMode.Screen,
					TargetFlashFrames = 10,
					TargetFlashCount = 1
				},
				new PsiElement
				{
					Timestamp = 50,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0))
				}
			};
			return new PsiElementList(elements);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00011728 File Offset: 0x0000F928
		private static PsiElementList GenerateBeamAlpha()
		{
			MultipartAnimation animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam1.sdat", default(Vector2f), 0.5f, 32767);
			MultipartAnimation multipartAnimation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam1.sdat", default(Vector2f), 0.5f, 32767);
			multipartAnimation.Scale = new Vector2f(-1f, 1f);
			List<PsiElement> list = new List<PsiElement>();
			list.Add(new PsiElement
			{
				Timestamp = 0,
				Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam2.sdat", new Vector2f(160f, 90f), 0.3f, 32767),
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 128)),
				Sound = AudioManager.Instance.Use(Paths.AUDIO + "pkBeamA.wav", AudioType.Sound)
			});
			list.Add(new PsiElement
			{
				Timestamp = 50,
				Animation = animation,
				Offset = new Vector2f(-52f, -48f),
				LockToTargetPosition = true,
				PositionIndex = 0
			});
			list.Add(new PsiElement
			{
				Timestamp = 50,
				Animation = multipartAnimation,
				Offset = new Vector2f(52f, -48f),
				LockToTargetPosition = true,
				PositionIndex = 0
			});
			list.Add(new PsiElement
			{
				Timestamp = 80,
				TargetFlashColor = new Color?(Color.Yellow),
				TargetFlashBlendMode = ColorBlendMode.Screen,
				TargetFlashFrames = 20,
				TargetFlashCount = 1
			});
			for (int i = 0; i < 6; i++)
			{
				list.Add(new PsiElement
				{
					Timestamp = 80 + i * 5,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam3.sdat", default(Vector2f), 0.5f, 32767),
					Offset = new Vector2f((float)(i * -8), 0f),
					LockToTargetPosition = true,
					PositionIndex = 0
				});
				list.Add(new PsiElement
				{
					Timestamp = 80 + i * 5,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "beam3.sdat", default(Vector2f), 0.5f, 32767),
					Offset = new Vector2f((float)(i * 8), 0f),
					LockToTargetPosition = true,
					PositionIndex = 0
				});
			}
			list.Add(new PsiElement
			{
				Timestamp = list[list.Count - 1].Timestamp + 30,
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0))
			});
			return new PsiElementList(list);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00011A38 File Offset: 0x0000FC38
		private static PsiElementList GenerateHitback()
		{
			Vector2f position = new Vector2f(160f, 90f);
			return new PsiElementList(new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "comet_reflect.sdat", position, 0.5f, 32767),
					Sound = AudioManager.Instance.Use(Paths.AUDIO + "rocketReflect.wav", AudioType.Sound),
					CardSpringMode = BattleCard.SpringMode.BounceUp,
					CardSpringAmplitude = new Vector2f(0f, 4f),
					CardSpringSpeed = new Vector2f(0f, 0.2f),
					CardSpringDecay = new Vector2f(0f, 0.5f)
				}
			});
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00011B0C File Offset: 0x0000FD0C
		private static List<PsiElement> GenerateThrow()
		{
			Vector2f position = new Vector2f(160f, 90f);
			return new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "comet.sdat", position, 0.5f, 32767),
					Sound = AudioManager.Instance.Use(Paths.AUDIO + "rocket.wav", AudioType.Sound)
				}
			};
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00011BA4 File Offset: 0x0000FDA4
		private static List<PsiElement> GenerateExplosion(int startTimestamp)
		{
			Vector2f v = new Vector2f(160f, 90f);
			List<PsiElement> list = new List<PsiElement>();
			list.Add(new PsiElement
			{
				Timestamp = startTimestamp,
				ScreenDarkenColor = new Color?(Color.Cyan),
				ScreenDarkenDepth = new int?(0),
				Sound = AudioManager.Instance.Use(Paths.AUDIO + "explosion.wav", AudioType.Sound)
			});
			int num = 98;
			int[] array = new int[]
			{
				1,
				2,
				3,
				2,
				1
			};
			int num2 = 180 / (array.Length + 1);
			for (int i = 0; i < array.Length; i++)
			{
				int num3 = array[i];
				int num4 = (num3 - 1) * (num + 20);
				int num5 = num4 / 2;
				for (int j = 0; j < num3; j++)
				{
					Vector2f vector2f = v + new Vector2f((float)(-(float)num5 + (num + 20) * j), -v.Y + (float)(num2 * (i + 1)));
					int num6 = (int)(VectorMath.Magnitude(v - vector2f) / 10f);
					list.Add(new PsiElement
					{
						Timestamp = startTimestamp + num6,
						Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "comet_boom.sdat", vector2f, 0.4f, 32767)
					});
				}
			}
			list.Add(new PsiElement
			{
				Timestamp = list[list.Count - 1].Timestamp + 10,
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0)),
				ScreenDarkenDepth = new int?(0)
			});
			return list;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00011D5C File Offset: 0x0000FF5C
		private static PsiElementList GenerateComet()
		{
			List<PsiElement> list = new List<PsiElement>();
			list.AddRange(PsiAnimations.GenerateThrow());
			list.AddRange(PsiAnimations.GenerateExplosion(40));
			return new PsiElementList(list);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00011D90 File Offset: 0x0000FF90
		private static PsiElementList GenerateFireAlpha()
		{
			List<PsiElement> elements = new List<PsiElement>
			{
				new PsiElement
				{
					Timestamp = 0,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 128))
				},
				new PsiElement
				{
					Timestamp = 10,
					Animation = new MultipartAnimation(Paths.PSI_GRAPHICS + "fire_a.sdat", new Vector2f(160f, 90f), 0.4f, 32767),
					Sound = AudioManager.Instance.Use(Paths.AUDIO + "pkFireA.wav", AudioType.Sound)
				},
				new PsiElement
				{
					Timestamp = 50,
					TargetFlashColor = new Color?(Color.Red),
					TargetFlashBlendMode = ColorBlendMode.Screen,
					TargetFlashFrames = 10,
					TargetFlashCount = 1
				},
				new PsiElement
				{
					Timestamp = 90,
					ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0))
				}
			};
			return new PsiElementList(elements);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00012140 File Offset: 0x00010340
		private static PsiElementList GenerateShield()
		{
			List<PsiElement> list = new List<PsiElement>();
			int[][] array = new int[][]
			{
				new int[]
				{
					0,
					0,
					0,
					144,
					300,
					0,
					300,
					144
				},
				new int[]
				{
					20,
					0,
					20,
					144,
					280,
					0,
					280,
					144,
					0,
					36,
					0,
					108,
					300,
					36,
					300,
					108
				},
				new int[]
				{
					40,
					0,
					40,
					144,
					260,
					0,
					260,
					144,
					20,
					36,
					20,
					108,
					280,
					36,
					280,
					108,
					0,
					72,
					300,
					72
				},
				new int[]
				{
					60,
					0,
					60,
					144,
					240,
					0,
					240,
					144,
					40,
					36,
					40,
					108,
					260,
					36,
					260,
					108,
					20,
					72,
					280,
					72
				},
				new int[]
				{
					80,
					0,
					80,
					144,
					220,
					0,
					220,
					144,
					60,
					36,
					60,
					108,
					240,
					36,
					240,
					108,
					40,
					72,
					260,
					72
				},
				new int[]
				{
					100,
					0,
					100,
					144,
					200,
					0,
					200,
					144,
					80,
					36,
					80,
					108,
					220,
					36,
					220,
					108,
					60,
					72,
					240,
					72
				},
				new int[]
				{
					120,
					0,
					120,
					144,
					180,
					0,
					180,
					144,
					100,
					36,
					100,
					108,
					200,
					36,
					200,
					108,
					80,
					72,
					220,
					72
				},
				new int[]
				{
					140,
					0,
					140,
					144,
					160,
					0,
					160,
					144,
					120,
					36,
					120,
					108,
					180,
					36,
					180,
					108,
					100,
					72,
					200,
					72
				},
				new int[]
				{
					140,
					36,
					140,
					108,
					160,
					36,
					160,
					108,
					120,
					72,
					180,
					72
				},
				new int[]
				{
					140,
					72,
					160,
					72
				}
			};
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array[i].Length; j += 2)
				{
					list.Add(new PsiElement
					{
						Timestamp = (i + 1) * 4,
						Animation = new IndexedColorGraphic(Paths.PSI_GRAPHICS + "shield.dat", "anim", new Vector2f((float)array[i][j], (float)array[i][j + 1]), 32767)
					});
				}
			}
			list.Add(new PsiElement
			{
				Timestamp = 16,
				ScreenDarkenColor = new Color?(new Color(142, 240, 251, 204)),
				ScreenDarkenSpeed = new float?(0.02f),
				ScreenDarkenDepth = new int?(32766)
			});
			list.Add(new PsiElement
			{
				Timestamp = 80,
				ScreenDarkenColor = new Color?(new Color(0, 0, 0, 0)),
				ScreenDarkenSpeed = new float?(0.1f),
				ScreenDarkenDepth = new int?(32766)
			});
			return new PsiElementList(list);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00012354 File Offset: 0x00010554
		public static PsiElementList Get(int psiId)
		{
			switch (psiId)
			{
			case 1:
				return PsiAnimations.GenerateFreezeAlpha();
			case 2:
				return PsiAnimations.GenerateBeamAlpha();
			case 3:
				return PsiAnimations.GenerateComet();
			case 4:
				return new PsiElementList(PsiAnimations.GenerateThrow());
			case 5:
				return PsiAnimations.GenerateHitback();
			case 6:
				return new PsiElementList(PsiAnimations.GenerateExplosion(0));
			case 7:
				return PsiAnimations.GenerateFireAlpha();
			case 8:
				return PsiAnimations.GenerateShield();
			default:
				return PsiAnimations.GenerateHitback();
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000123D0 File Offset: 0x000105D0
		public static PsiElementList Get(PsiLevel level)
		{
			PsiData data = PsiFile.Instance.GetData(level.PsiType);
			return PsiAnimations.Get((int)data.Animations[level.Level]);
		}

		// Token: 0x0400042C RID: 1068
		private const int PSI_DEPTH = 32767;
	}
}
