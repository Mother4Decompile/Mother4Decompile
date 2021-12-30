using System;
using System.Collections.Generic;
using System.Text;
using Carbine.Utility;
using fNbt;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using Mother4.Scripts.Actions.Types;
using Rufini.Actions.Types;
using Rufini.Strings;
using SFML.Graphics;

namespace Mother4.Scripts
{
	// Token: 0x02000118 RID: 280
	internal class ActionFactory
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x0002AF7C File Offset: 0x0002917C
		public static RufiniAction FromCode(string code)
		{
			Type type;
			if (ActionFactory.actions.TryGetValue(code, out type))
			{
				return (RufiniAction)Activator.CreateInstance(type);
			}
			throw new ArgumentException(string.Format("\"{0}\" does not correspond to an action type.", code));
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002AFBC File Offset: 0x000291BC
		private static string GetType(NbtCompound tag)
		{
			string result = null;
			NbtTag nbtTag = tag.Get("_typ");
			if (nbtTag != null)
			{
				if (nbtTag is NbtInt)
				{
					uint intValue = (uint)nbtTag.IntValue;
					ActionFactory.tagNameBuf[3] = (char)((byte)intValue);
					ActionFactory.tagNameBuf[2] = (char)((byte)(intValue >> 8));
					ActionFactory.tagNameBuf[1] = (char)((byte)(intValue >> 16));
					ActionFactory.tagNameBuf[0] = (char)((byte)(intValue >> 24));
					result = ActionFactory.tagNameBuf.ToString();
				}
				else if (nbtTag is NbtString)
				{
					result = nbtTag.StringValue.Substring(0, 4);
				}
			}
			return result;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002B06C File Offset: 0x0002926C
		public static RufiniAction FromNbt(NbtCompound tag)
		{
			string type = ActionFactory.GetType(tag);
			RufiniAction rufiniAction = null;
			if (type != null)
			{
				Type type2 = null;
				if (ActionFactory.actions.TryGetValue(type, out type2))
				{
					rufiniAction = (RufiniAction)Activator.CreateInstance(type2);
					NbtCompound nbtCompound = tag.Get<NbtCompound>("params");
					if (nbtCompound == null)
					{
						return rufiniAction;
					}
					using (IEnumerator<NbtTag> enumerator = nbtCompound.Tags.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NbtTag paramTag = enumerator.Current;
							ActionParam actionParam = rufiniAction.Params.Find((ActionParam x) => x.Name == paramTag.Name);
							if (actionParam != null)
							{
								if (paramTag is NbtString)
								{
									if (actionParam.Type == typeof(RufiniString))
									{
										RufiniString value = StringFile.Instance.Get(paramTag.StringValue);
										rufiniAction.SetValue<RufiniString>(paramTag.Name, value);
									}
									else if (actionParam.Type == typeof(string))
									{
										rufiniAction.SetValue<string>(paramTag.Name, paramTag.StringValue);
									}
								}
								else if (paramTag is NbtInt)
								{
									if (actionParam.Type == typeof(RufiniOption))
									{
										rufiniAction.SetValue<RufiniOption>(paramTag.Name, new RufiniOption
										{
											Option = paramTag.IntValue
										});
									}
									else if (actionParam.Type == typeof(int))
									{
										rufiniAction.SetValue<int>(paramTag.Name, paramTag.IntValue);
									}
									else if (actionParam.Type == typeof(Color))
									{
										Color value2 = ColorHelper.FromInt(paramTag.IntValue);
										rufiniAction.SetValue<Color>(paramTag.Name, value2);
									}
								}
								else if (paramTag is NbtIntArray)
								{
									rufiniAction.SetValue<int[]>(paramTag.Name, ((NbtIntArray)paramTag).IntArrayValue);
								}
								else if (paramTag is NbtFloat)
								{
									rufiniAction.SetValue<float>(paramTag.Name, paramTag.FloatValue);
								}
								else if (paramTag is NbtByte)
								{
									if (actionParam.Type == typeof(byte))
									{
										rufiniAction.SetValue<byte>(paramTag.Name, paramTag.ByteValue);
									}
									else if (actionParam.Type == typeof(bool))
									{
										rufiniAction.SetValue<bool>(paramTag.Name, paramTag.ByteValue != 0);
									}
								}
							}
						}
						return rufiniAction;
					}
				}
				rufiniAction = new NoopAction(type);
			}
			return rufiniAction;
		}

		// Token: 0x040008C0 RID: 2240
		private const string TYPE_TAG = "_typ";

		// Token: 0x040008C1 RID: 2241
		private const string PARAMS_TAG = "params";

		// Token: 0x040008C2 RID: 2242
		private static StringBuilder tagNameBuf = new StringBuilder("\0\0\0\0", 4);

		// Token: 0x040008C3 RID: 2243
		private static Dictionary<string, Type> actions = new Dictionary<string, Type>
		{
			{
				"NOOP",
				typeof(NoopAction)
			},
			{
				"PRLN",
				typeof(PrintLnAction)
			},
			{
				"TXBX",
				typeof(TextboxAction)
			},
			{
				"SNPC",
				typeof(SetNpcAction)
			},
			{
				"SNTG",
				typeof(SetNametagAction)
			},
			{
				"QSTN",
				typeof(QuestionAction)
			},
			{
				"CMOV",
				typeof(CameraMoveAction)
			},
			{
				"CFLP",
				typeof(CameraPlayerAction)
			},
			{
				"CFLN",
				typeof(CameraNPCAction)
			},
			{
				"WAIT",
				typeof(WaitAction)
			},
			{
				"CSPP",
				typeof(ChangeSpritePlayerAction)
			},
			{
				"CSPN",
				typeof(ChangeSpriteNPCAction)
			},
			{
				"CSSP",
				typeof(ChangeSubspritePlayerAction)
			},
			{
				"CSSN",
				typeof(ChangeSubspriteNPCAction)
			},
			{
				"EADD",
				typeof(EntityAddAction)
			},
			{
				"EDEL",
				typeof(EntityDeleteAction)
			},
			{
				"EDIR",
				typeof(EntityDirectionAction)
			},
			{
				"EMOV",
				typeof(EntityMoveAction)
			},
			{
				"IADD",
				typeof(ItemAddAction)
			},
			{
				"IREM",
				typeof(ItemRemoveAction)
			},
			{
				"MPMK",
				typeof(MapMarkSetAction)
			},
			{
				"MPCL",
				typeof(MapMarkClearAction)
			},
			{
				"SFLG",
				typeof(SetFlagAction)
			},
			{
				"ANIM",
				typeof(AnimationAction)
			},
			{
				"SSHK",
				typeof(ScreenShakeAction)
			},
			{
				"SMOD",
				typeof(StatModifyAction)
			},
			{
				"SSET",
				typeof(StatSetAction)
			},
			{
				"AMNY",
				typeof(AddMoneyAction)
			},
			{
				"SMNY",
				typeof(SetMoneyAction)
			},
			{
				"SGVL",
				typeof(ValueSetAction)
			},
			{
				"AGVL",
				typeof(ValueAddAction)
			},
			{
				"IFFL",
				typeof(IfFlagAction)
			},
			{
				"IFVL",
				typeof(IfValueAction)
			},
			{
				"IFEN",
				typeof(EndIfAction)
			},
			{
				"ELSE",
				typeof(ElseAction)
			},
			{
				"CALL",
				typeof(CallAction)
			},
			{
				"WTHR",
				typeof(WeatherAction)
			},
			{
				"SCEF",
				typeof(ScreenEffectAction)
			},
			{
				"SCFD",
				typeof(ScreenFadeAction)
			},
			{
				"TIME",
				typeof(TimeAction)
			},
			{
				"AEXP",
				typeof(AddExpAction)
			},
			{
				"SCFL",
				typeof(ScreenFlashAction)
			},
			{
				"EMNP",
				typeof(EmoticonNPCAction)
			},
			{
				"EMPL",
				typeof(EmoticonPlayerAction)
			},
			{
				"SBGM",
				typeof(SetBGMAction)
			},
			{
				"PSFX",
				typeof(PlaySFXAction)
			},
			{
				"ENMM",
				typeof(EntityMoveModeAction)
			},
			{
				"HNPC",
				typeof(HopNPCAction)
			},
			{
				"HPLR",
				typeof(HopPlayerAction)
			},
			{
				"APRT",
				typeof(AddPartyMemberAction)
			},
			{
				"RPRT",
				typeof(RemovePartyMemberAction)
			},
			{
				"SBTL",
				typeof(StartBattleAction)
			},
			{
				"IRIS",
				typeof(IrisOverlayAction)
			},
			{
				"GOMP",
				typeof(GoToMapAction)
			},
			{
				"PPMV",
				typeof(PlayerPathMoveAction)
			},
			{
				"FOTX",
				typeof(FlyoverTextAction)
			},
			{
				"MVPL",
				typeof(PlayerPositionAction)
			},
			{
				"EDPT",
				typeof(EntityDepthAction)
			},
			{
				"PMOV",
				typeof(PlayerMoveAction)
			},
			{
				"TSPL",
				typeof(PlayerShadowAction)
			},
			{
				"STSP",
				typeof(SetTilesetPaletteAction)
			},
			{
				"STEF",
				typeof(SetStatusEffectAction)
			},
			{
				"RSSP",
				typeof(ResetSubspritePlayerAction)
			},
			{
				"RSSN",
				typeof(ResetSubspriteNPCAction)
			},
			{
				"CTRL",
				typeof(SetControlAction)
			},
			{
				"PALC",
				typeof(SetPlayerAnimationLoopCountAction)
			},
			{
				"NALC",
				typeof(SetNPCAnimationLoopCountAction)
			},
			{
				"LTBX",
				typeof(ToggleLetterboxingAction)
			},
			{
				"PDIR",
				typeof(SetPlayerDirectionAction)
			},
			{
				"STBX",
				typeof(ToggleTextboxAction)
			}
		};
	}
}
