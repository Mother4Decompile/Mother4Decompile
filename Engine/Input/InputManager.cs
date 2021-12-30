using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;

namespace Carbine.Input
{
	// Token: 0x02000039 RID: 57
	public class InputManager
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000AF46 File Offset: 0x00009146
		public static InputManager Instance
		{
			get
			{
				if (InputManager.instance == null)
				{
					InputManager.instance = new InputManager();
				}
				return InputManager.instance;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600020E RID: 526 RVA: 0x0000AF60 File Offset: 0x00009160
		// (remove) Token: 0x0600020F RID: 527 RVA: 0x0000AF98 File Offset: 0x00009198
		public event InputManager.ButtonPressedHandler ButtonPressed;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000210 RID: 528 RVA: 0x0000AFD0 File Offset: 0x000091D0
		// (remove) Token: 0x06000211 RID: 529 RVA: 0x0000B008 File Offset: 0x00009208
		public event InputManager.ButtonReleasedHandler ButtonReleased;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000212 RID: 530 RVA: 0x0000B040 File Offset: 0x00009240
		// (remove) Token: 0x06000213 RID: 531 RVA: 0x0000B078 File Offset: 0x00009278
		public event InputManager.AxisPressedHandler AxisPressed;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000214 RID: 532 RVA: 0x0000B0B0 File Offset: 0x000092B0
		// (remove) Token: 0x06000215 RID: 533 RVA: 0x0000B0E8 File Offset: 0x000092E8
		public event InputManager.AxisReleasedHandler AxisReleased;

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000B11D File Offset: 0x0000931D
		public Dictionary<Button, bool> State
		{
			get
			{
				return this.currentState;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000B125 File Offset: 0x00009325
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000B12D File Offset: 0x0000932D
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000B138 File Offset: 0x00009338
		public Vector2f Axis
		{
			get
			{
				float x = Math.Max(-1f, Math.Min(1f, this.xAxis + this.xKeyAxis));
				float y = Math.Max(-1f, Math.Min(1f, this.yAxis + this.yKeyAxis));
				return new Vector2f(x, y);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000B190 File Offset: 0x00009390
		private InputManager()
		{
			this.currentState = new Dictionary<Button, bool>();
			foreach (object obj in Enum.GetValues(typeof(Button)))
			{
				Button key = (Button)obj;
				this.currentState.Add(key, false);
			}
			this.enabled = true;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B3AC File Offset: 0x000095AC
		public void AttachToWindow(Window window)
		{
			window.SetKeyRepeatEnabled(false);
			window.JoystickButtonPressed += this.JoystickButtonPressed;
			window.JoystickButtonReleased += this.JoystickButtonReleased;
			window.JoystickMoved += this.JoystickMoved;
			window.JoystickConnected += this.JoystickConnected;
			window.JoystickDisconnected += this.JoystickDisconnected;
			window.KeyPressed += this.KeyPressed;
			window.KeyReleased += this.KeyReleased;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B440 File Offset: 0x00009640
		public void DetachFromWindow(Window window)
		{
			window.JoystickButtonPressed -= this.JoystickButtonPressed;
			window.JoystickButtonReleased -= this.JoystickButtonReleased;
			window.JoystickMoved -= this.JoystickMoved;
			window.JoystickConnected -= this.JoystickConnected;
			window.JoystickDisconnected -= this.JoystickDisconnected;
			window.KeyPressed -= this.KeyPressed;
			window.KeyReleased -= this.KeyReleased;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B4CC File Offset: 0x000096CC
		private void KeyPressed(object sender, KeyEventArgs e)
		{
			if (this.keyMap.ContainsKey(e.Code))
			{
				Button button = this.keyMap[e.Code];
				if (this.enabled && !this.currentState[button] && this.ButtonPressed != null)
				{
					this.ButtonPressed(this, button);
				}
				this.currentState[button] = true;
				return;
			}
			bool flag = false;
			switch (e.Code)
			{
			case Keyboard.Key.Left:
				this.leftPress = true;
				flag = true;
				break;
			case Keyboard.Key.Right:
				this.rightPress = true;
				flag = true;
				break;
			case Keyboard.Key.Up:
				this.upPress = true;
				flag = true;
				break;
			case Keyboard.Key.Down:
				this.downPress = true;
				flag = true;
				break;
			}
			this.xKeyAxis = (this.leftPress ? -1f : 0f) + (this.rightPress ? 1f : 0f);
			this.yKeyAxis = (this.upPress ? -1f : 0f) + (this.downPress ? 1f : 0f);
			if (this.enabled && flag && this.AxisPressed != null)
			{
				this.AxisPressed(this, this.Axis);
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B608 File Offset: 0x00009808
		private void KeyReleased(object sender, KeyEventArgs e)
		{
			if (this.keyMap.ContainsKey(e.Code))
			{
				Button button = this.keyMap[e.Code];
				if (this.enabled && this.ButtonReleased != null)
				{
					this.ButtonReleased(this, button);
				}
				this.currentState[button] = false;
				return;
			}
			bool flag = false;
			switch (e.Code)
			{
			case Keyboard.Key.Left:
				this.leftPress = false;
				flag = true;
				break;
			case Keyboard.Key.Right:
				this.rightPress = false;
				flag = true;
				break;
			case Keyboard.Key.Up:
				this.upPress = false;
				flag = true;
				break;
			case Keyboard.Key.Down:
				this.downPress = false;
				flag = true;
				break;
			}
			this.xKeyAxis = (this.leftPress ? -1f : 0f) + (this.rightPress ? 1f : 0f);
			this.yKeyAxis = (this.upPress ? -1f : 0f) + (this.downPress ? 1f : 0f);
			if (this.enabled && flag && this.AxisReleased != null)
			{
				this.AxisReleased(this, this.Axis);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B738 File Offset: 0x00009938
		private void JoystickMoved(object sender, JoystickMoveEventArgs e)
		{
			Joystick.Axis axis = e.Axis;
			switch (axis)
			{
			case Joystick.Axis.X:
				this.xAxis = Math.Max(-1f, Math.Min(1f, e.Position / 70f));
				if (this.xAxis > 0f && this.xAxis < 0.5f)
				{
					this.xAxis = 0f;
				}
				if (this.xAxis < 0f && this.xAxis > -0.5f)
				{
					this.xAxis = 0f;
				}
				break;
			case Joystick.Axis.Y:
				this.yAxis = Math.Max(-1f, Math.Min(1f, e.Position / 70f));
				if (this.yAxis > 0f && this.yAxis < 0.5f)
				{
					this.yAxis = 0f;
				}
				if (this.yAxis < 0f && this.yAxis > -0.5f)
				{
					this.yAxis = 0f;
				}
				break;
			default:
				switch (axis)
				{
				case Joystick.Axis.PovX:
					this.xAxis = Math.Max(-1f, Math.Min(1f, e.Position));
					break;
				case Joystick.Axis.PovY:
					this.yAxis = Math.Max(-1f, Math.Min(1f, -e.Position));
					break;
				}
				break;
			}
			this.axisZeroLast = this.axisZero;
			this.axisZero = (this.xAxis == 0f && this.yAxis == 0f);
			bool flag = this.axisZeroLast && !this.axisZero;
			if (this.enabled && flag && this.AxisPressed != null)
			{
				this.AxisPressed(this, this.Axis);
				return;
			}
			bool flag2 = !this.axisZeroLast && this.axisZero;
			if (this.enabled && flag2 && this.AxisReleased != null)
			{
				this.AxisReleased(this, this.Axis);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B940 File Offset: 0x00009B40
		private void JoystickConnected(object sender, JoystickConnectEventArgs e)
		{
			Joystick.Update();
			Joystick.Identification identification = Joystick.GetIdentification(e.JoystickId);
			Console.WriteLine("Gamepad {0} connected: {1} ({2}, {3})", new object[]
			{
				e.JoystickId,
				identification.Name,
				identification.VendorId,
				identification.ProductId
			});
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B9A6 File Offset: 0x00009BA6
		private void JoystickDisconnected(object sender, JoystickConnectEventArgs e)
		{
			Console.WriteLine("Gamepad {0} disconnected", e.JoystickId);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B9C0 File Offset: 0x00009BC0
		private void JoystickButtonPressed(object sender, JoystickButtonEventArgs e)
		{
			if (!this.joyMap.ContainsKey(e.Button))
			{
				return;
			}
			Button button = this.joyMap[e.Button];
			this.currentState[button] = true;
			if (this.enabled && this.ButtonPressed != null)
			{
				this.ButtonPressed(this, button);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000BA20 File Offset: 0x00009C20
		private void JoystickButtonReleased(object sender, JoystickButtonEventArgs e)
		{
			if (!this.joyMap.ContainsKey(e.Button))
			{
				return;
			}
			Button button = this.joyMap[e.Button];
			this.currentState[button] = false;
			if (this.enabled && this.ButtonReleased != null)
			{
				this.ButtonReleased(this, button);
			}
		}

		// Token: 0x04000151 RID: 337
		private const float DEAD_ZONE = 0.5f;

		// Token: 0x04000152 RID: 338
		private static InputManager instance;

		// Token: 0x04000157 RID: 343
		private Dictionary<Keyboard.Key, Button> keyMap = new Dictionary<Keyboard.Key, Button>
		{
			{
				Keyboard.Key.Z,
				Button.A
			},
			{
				Keyboard.Key.X,
				Button.B
			},
			{
				Keyboard.Key.S,
				Button.X
			},
			{
				Keyboard.Key.D,
				Button.Y
			},
			{
				Keyboard.Key.A,
				Button.L
			},
			{
				Keyboard.Key.F,
				Button.R
			},
			{
				Keyboard.Key.Return,
				Button.Start
			},
			{
				Keyboard.Key.BackSpace,
				Button.Select
			},
			{
				Keyboard.Key.Escape,
				Button.Escape
			},
			{
				Keyboard.Key.Tilde,
				Button.Tilde
			},
			{
				Keyboard.Key.F1,
				Button.F1
			},
			{
				Keyboard.Key.F2,
				Button.F2
			},
			{
				Keyboard.Key.F3,
				Button.F3
			},
			{
				Keyboard.Key.F4,
				Button.F4
			},
			{
				Keyboard.Key.F5,
				Button.F5
			},
			{
				Keyboard.Key.F6,
				Button.F6
			},
			{
				Keyboard.Key.F7,
				Button.F7
			},
			{
				Keyboard.Key.F8,
				Button.F8
			},
			{
				Keyboard.Key.F9,
				Button.F9
			},
			{
				Keyboard.Key.F10,
				Button.F10
			},
			{
				Keyboard.Key.F11,
				Button.F11
			},
			{
				Keyboard.Key.F12,
				Button.F12
			},
			{
				Keyboard.Key.Num0,
				Button.Zero
			},
			{
				Keyboard.Key.Num1,
				Button.One
			},
			{
				Keyboard.Key.Num2,
				Button.Two
			},
			{
				Keyboard.Key.Num3,
				Button.Three
			},
			{
				Keyboard.Key.Num4,
				Button.Four
			},
			{
				Keyboard.Key.Num5,
				Button.Five
			},
			{
				Keyboard.Key.Num6,
				Button.Six
			},
			{
				Keyboard.Key.Num7,
				Button.Seven
			},
			{
				Keyboard.Key.Num8,
				Button.Eight
			},
			{
				Keyboard.Key.Num9,
				Button.Nine
			}
		};

		// Token: 0x04000158 RID: 344
		private Dictionary<uint, Button> joyMap = new Dictionary<uint, Button>
		{
			{
				0U,
				Button.A
			},
			{
				1U,
				Button.B
			},
			{
				2U,
				Button.X
			},
			{
				3U,
				Button.Y
			},
			{
				4U,
				Button.L
			},
			{
				5U,
				Button.R
			},
			{
				6U,
				Button.Select
			},
			{
				7U,
				Button.Start
			},
			{
				8U,
				Button.Tilde
			}
		};

		// Token: 0x04000159 RID: 345
		private Dictionary<Button, bool> currentState;

		// Token: 0x0400015A RID: 346
		private float xAxis;

		// Token: 0x0400015B RID: 347
		private float yAxis;

		// Token: 0x0400015C RID: 348
		private float xKeyAxis;

		// Token: 0x0400015D RID: 349
		private float yKeyAxis;

		// Token: 0x0400015E RID: 350
		private bool axisZero;

		// Token: 0x0400015F RID: 351
		private bool axisZeroLast;

		// Token: 0x04000160 RID: 352
		private bool enabled;

		// Token: 0x04000161 RID: 353
		private bool leftPress;

		// Token: 0x04000162 RID: 354
		private bool rightPress;

		// Token: 0x04000163 RID: 355
		private bool upPress;

		// Token: 0x04000164 RID: 356
		private bool downPress;

		// Token: 0x0200003A RID: 58
		// (Invoke) Token: 0x06000226 RID: 550
		public delegate void ButtonPressedHandler(InputManager sender, Button b);

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x0600022A RID: 554
		public delegate void ButtonReleasedHandler(InputManager sender, Button b);

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x0600022E RID: 558
		public delegate void AxisPressedHandler(InputManager sender, Vector2f axis);

		// Token: 0x0200003D RID: 61
		// (Invoke) Token: 0x06000232 RID: 562
		public delegate void AxisReleasedHandler(InputManager sender, Vector2f axis);
	}
}
