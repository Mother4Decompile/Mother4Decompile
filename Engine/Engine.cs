using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Carbine
{
    // Token: 0x0200001C RID: 2
    public static class Engine
    {
        // Token: 0x17000032 RID: 50
        // (get) Token: 0x060000E0 RID: 224 RVA: 0x000052A3 File Offset: 0x000034A3
        public static RenderWindow Window
        {
            get
            {
                return Engine.window;
            }
        }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x060000E1 RID: 225 RVA: 0x000052AA File Offset: 0x000034AA
        public static RenderTexture FrameBuffer
        {
            get
            {
                return Engine.frameBuffer;
            }
        }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x060000E2 RID: 226 RVA: 0x000052B1 File Offset: 0x000034B1
        public static Random Random
        {
            get
            {
                return Engine.rand;
            }
        }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x060000E3 RID: 227 RVA: 0x000052B8 File Offset: 0x000034B8
        public static FontData DefaultFont
        {
            get
            {
                return Engine.defaultFont;
            }
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x060000E4 RID: 228 RVA: 0x000052BF File Offset: 0x000034BF
        // (set) Token: 0x060000E5 RID: 229 RVA: 0x000052C6 File Offset: 0x000034C6
        public static bool Running { get; private set; }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x060000E6 RID: 230 RVA: 0x000052CE File Offset: 0x000034CE
        // (set) Token: 0x060000E7 RID: 231 RVA: 0x000052D5 File Offset: 0x000034D5
        public static uint ScreenScale
        {
            get
            {
                return Engine.frameBufferScale;
            }
            set
            {
                Engine.frameBufferScale = Math.Max(0U, value);
                Engine.switchScreenMode = true;
            }
        }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x060000E8 RID: 232 RVA: 0x000052E9 File Offset: 0x000034E9
        // (set) Token: 0x060000E9 RID: 233 RVA: 0x000052F0 File Offset: 0x000034F0
        public static bool Fullscreen
        {
            get
            {
                return Engine.isFullscreen;
            }
            set
            {
                Engine.isFullscreen = value;
                Engine.switchScreenMode = true;
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x060000EA RID: 234 RVA: 0x000052FE File Offset: 0x000034FE
        public static float FPS
        {
            get
            {
                return Engine.fps;
            }
        }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x060000EB RID: 235 RVA: 0x00005305 File Offset: 0x00003505
        public static long Frame
        {
            get
            {
                return Engine.frameIndex;
            }
        }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x060000EC RID: 236 RVA: 0x0000530C File Offset: 0x0000350C
        // (set) Token: 0x060000ED RID: 237 RVA: 0x00005313 File Offset: 0x00003513
        public static SFML.Graphics.Color ClearColor { get; set; }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x060000EE RID: 238 RVA: 0x0000531C File Offset: 0x0000351C
        public static int SessionTime
        {
            get
            {
                return (int)TimeSpan.FromTicks(DateTime.Now.Ticks - Engine.startTicks).TotalSeconds;
            }
        }

        // Token: 0x060000EF RID: 239 RVA: 0x0000534C File Offset: 0x0000354C
        public static void Initialize(string[] args)
        {
            Engine.frameStopwatch = Stopwatch.StartNew();
            Engine.startTicks = DateTime.Now.Ticks;
            bool vsync = false;
            bool goFullscreen = false;
            for (int i = 0; i < args.Length; i++)
            {
                string a;
                if ((a = args[i]) != null)
                {
                    if (!(a == "-fullscreen"))
                    {
                        if (!(a == "-vsync"))
                        {
                            if (a == "-scale")
                            {
                                uint screenScale = 1U;
                                if (uint.TryParse(args[++i], out screenScale))
                                {
                                    Engine.ScreenScale = screenScale;
                                }
                            }
                        }
                        else
                        {
                            //vsync = true;
                        }
                    }
                    else
                    {
                        goFullscreen = true;
                    }
                }
            }
            Engine.frameBuffer = new RenderTexture(320U, 180U);
            Engine.frameBufferState = new RenderStates(BlendMode.Alpha, Transform.Identity, Engine.frameBuffer.Texture, null);
            Engine.frameBufferVertArray = new VertexArray(PrimitiveType.Quads, 4U);
            Engine.SetWindow(goFullscreen, vsync);
            InputManager.Instance.ButtonPressed += Engine.OnButtonPressed;
            int num = 160;
            int num2 = 90;
            Engine.frameBufferVertArray[0U] = new Vertex(new Vector2f((float)(-(float)num), (float)(-(float)num2)), new Vector2f(0f, 0f));
            Engine.frameBufferVertArray[1U] = new Vertex(new Vector2f((float)num, (float)(-(float)num2)), new Vector2f(320f, 0f));
            Engine.frameBufferVertArray[2U] = new Vertex(new Vector2f((float)num, (float)num2), new Vector2f(320f, 180f));
            Engine.frameBufferVertArray[3U] = new Vertex(new Vector2f((float)(-(float)num), (float)num2), new Vector2f(0f, 180f));
            Engine.rand = new Random();
            Engine.defaultFont = new FontData();
            Engine.debugText = new Text(string.Empty, Engine.defaultFont.Font, Engine.defaultFont.Size);
            Engine.ClearColor = SFML.Graphics.Color.Black;
            decimal num3 = decimal.Parse(string.Format("{0}.{1}", Engine.window.Settings.MajorVersion, Engine.window.Settings.MinorVersion));
            if (num3 < 2.1m)
            {
                string message = string.Format("OpenGL version {0} or higher is required. This system has version {1}.", 2.1m, num3);
                throw new InvalidOperationException(message);
            }
            Console.WriteLine("OpenGL v{0}.{1}", Engine.window.Settings.MajorVersion, Engine.window.Settings.MinorVersion);
            Engine.fpsString = new StringBuilder(32);
            Engine.SetCursorTimer(90);
            Engine.Running = true;
        }

        // Token: 0x060000F0 RID: 240 RVA: 0x000055F4 File Offset: 0x000037F4
        public static void StartSession()
        {
            Engine.startTicks = DateTime.Now.Ticks;
        }

        // Token: 0x060000F1 RID: 241 RVA: 0x00005613 File Offset: 0x00003813
        private static void SetCursorTimer(int duration)
        {
            Engine.cursorTimer = Engine.frameIndex + (long)duration;
        }

        // Token: 0x060000F2 RID: 242 RVA: 0x00005624 File Offset: 0x00003824
        private static void SetWindow(bool goFullscreen, bool vsync)
        {
            if (Engine.window != null)
            {
                Engine.window.Closed -= Engine.OnWindowClose;
                Engine.window.MouseMoved -= Engine.MouseMoved;
                InputManager.Instance.DetachFromWindow(Engine.window);
                Engine.window.Close();
                Engine.window.Dispose();
            }
            float num = (float)Math.Cos((double)Engine.screenAngle);
            float num2 = (float)Math.Sin((double)Engine.screenAngle);
            Styles style;
            VideoMode desktopMode;
            if (goFullscreen)
            {
                style = Styles.Fullscreen;
                desktopMode = VideoMode.DesktopMode;
                float num3 = Math.Min(desktopMode.Width / 320U, desktopMode.Height / 180U);
                float num4 = (desktopMode.Width - 320f * num3) / 2f;
                float num5 = (desktopMode.Height - 180f * num3) / 2f;
                int num6 = (int)(160f * num3);
                int num7 = (int)(90f * num3);
                Engine.frameBufferState.Transform = new Transform(num * num3, num2, num4 + (float)num6, -num2, num * num3, num5 + (float)num7, 0f, 0f, 1f);
            }
            else
            {
                int num8 = (int)(160U * Engine.ScreenScale);
                int num9 = (int)(90U * Engine.ScreenScale);
                style = Styles.Close;
                desktopMode = new VideoMode(320U * Engine.frameBufferScale, 180U * Engine.frameBufferScale);
                Engine.frameBufferState.Transform = new Transform(num * Engine.frameBufferScale, num2 * Engine.frameBufferScale, (float)num8, -num2 * Engine.frameBufferScale, num * Engine.frameBufferScale, (float)num9, 0f, 0f, 1f);
            }
            Engine.window = new RenderWindow(desktopMode, "Mother 4", style);
            Engine.window.Closed += Engine.OnWindowClose;
            Engine.window.MouseMoved += Engine.MouseMoved;
            Engine.window.MouseButtonPressed += Engine.MouseButtonPressed;
            InputManager.Instance.AttachToWindow(Engine.window);
            Engine.window.SetMouseCursorVisible(!goFullscreen);
            if (vsync || goFullscreen)
            {
                Engine.window.SetVerticalSyncEnabled(false);
            }
            else
            {
                Engine.window.SetFramerateLimit(60U);
            }
            if (Engine.iconFile != null)
            {
                Engine.window.SetIcon(32U, 32U, Engine.iconFile.GetBytesForSize(32));
            }
        }

        // Token: 0x060000F3 RID: 243 RVA: 0x00005884 File Offset: 0x00003A84
        private static void MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                Engine.showCursor = true;
                Engine.window.SetMouseCursorVisible(Engine.showCursor);
                Engine.SetCursorTimer(90);
                if (Engine.frameIndex < Engine.clickFrame + 20L)
                {
                    Engine.switchScreenMode = true;
                    Engine.isFullscreen = !Engine.isFullscreen;
                    Engine.clickFrame = long.MinValue;
                    return;
                }
                Engine.clickFrame = Engine.frameIndex;
            }
        }

        // Token: 0x060000F4 RID: 244 RVA: 0x000058F1 File Offset: 0x00003AF1
        private static void MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (!Engine.showCursor)
            {
                Engine.showCursor = true;
                Engine.window.SetMouseCursorVisible(Engine.showCursor);
            }
            Engine.SetCursorTimer(90);
        }

        // Token: 0x060000F5 RID: 245 RVA: 0x00005918 File Offset: 0x00003B18
        public static void OnWindowClose(object sender, EventArgs e)
        {
            RenderWindow renderWindow = (RenderWindow)sender;
            renderWindow.Close();
            Engine.quit = true;
        }

        // Token: 0x060000F6 RID: 246 RVA: 0x00005938 File Offset: 0x00003B38
        public unsafe static void OnButtonPressed(InputManager sender, Carbine.Input.Button b)
        {
            switch (b)
            {
                case Carbine.Input.Button.Escape:
                    if (!Engine.isFullscreen)
                    {
                        Engine.quit = true;
                        return;
                    }
                    Engine.switchScreenMode = true;
                    Engine.isFullscreen = !Engine.isFullscreen;
                    return;
                case Carbine.Input.Button.Tilde:
                    Engine.debugDisplay = !Engine.debugDisplay;
                    return;
                case Carbine.Input.Button.F1:
                case Carbine.Input.Button.F2:
                case Carbine.Input.Button.F3:
                case Carbine.Input.Button.F6:
                case Carbine.Input.Button.F7:
                    break;
                case Carbine.Input.Button.F4:
                    Engine.switchScreenMode = true;
                    Engine.isFullscreen = !Engine.isFullscreen;
                    return;
                case Carbine.Input.Button.F5:
                    Engine.frameBufferScale = Engine.frameBufferScale % 5U + 1U;
                    Engine.switchScreenMode = true;
                    return;
                case Carbine.Input.Button.F8:
                    {
                        SFML.Graphics.Image image = Engine.frameBuffer.Texture.CopyToImage();
                        byte[] array = new byte[image.Pixels.Length];
                        fixed (byte* pixels = image.Pixels, ptr = array)
                        {
                            for (int i = 0; i < array.Length; i += 4)
                            {
                                ptr[i] = pixels[i + 2];
                                ptr[i + 1] = pixels[i + 1];
                                ptr[i + 2] = pixels[i];
                                ptr[i + 3] = pixels[i + 3];
                            }
                            IntPtr scan = new IntPtr((void*)ptr);
                            Bitmap image2 = new Bitmap((int)image.Size.X, (int)image.Size.Y, (int)(4U * image.Size.X), PixelFormat.Format32bppArgb, scan);
                            Clipboard.SetImage(image2);
                        }
                        Console.WriteLine("Screenshot copied to clipboard");
                        return;
                    }
                case Carbine.Input.Button.F9:
                    {
                        SFML.Graphics.Image image3 = Engine.frameBuffer.Texture.CopyToImage();
                        string text = string.Format("screenshot{0}.png", Directory.GetFiles("./", "screenshot*.png").Length);
                        image3.SaveToFile(text);
                        Console.WriteLine("Screenshot saved as \"{0}\"", text);
                        return;
                    }
                default:
                    if (b != Carbine.Input.Button.F12)
                    {
                        return;
                    }
                    if (!SceneManager.Instance.IsTransitioning)
                    {
                        SceneManager.Instance.Transition = new InstantTransition();
                        SceneManager.Instance.Pop();
                    }
                    break;
            }
        }

        // Token: 0x060000F7 RID: 247 RVA: 0x00005B48 File Offset: 0x00003D48
        public static void Update()
        {
            Engine.frameStopwatch.Restart();
            if (Engine.switchScreenMode)
            {
                Engine.SetWindow(Engine.isFullscreen, false);
                Engine.switchScreenMode = false;
            }
            if (Engine.frameIndex > Engine.cursorTimer)
            {
                Engine.showCursor = false;
                Engine.window.SetMouseCursorVisible(Engine.showCursor);
                Engine.cursorTimer = long.MaxValue;
            }
            AudioManager.Instance.Update();
            Engine.window.DispatchEvents();
            try
            {
                SceneManager.Instance.Update();
                TimerManager.Instance.Update();
                ViewManager.Instance.Update();
                ViewManager.Instance.UseView();
                Engine.frameBuffer.Clear(Engine.ClearColor);
                SceneManager.Instance.Draw();
            }
            catch (EmptySceneStackException)
            {
                Engine.quit = true;
            }
            catch (Exception ex)
            {
                SceneManager.Instance.AbortTransition();
                SceneManager.Instance.Clear();
                SceneManager.Instance.Transition = new InstantTransition();
                SceneManager.Instance.Push(new ErrorScene(ex));
            }
            ViewManager.Instance.UseDefault();
            if (Engine.debugDisplay)
            {
                if (Engine.frameIndex % 10L == 0L)
                {
                    Engine.fpsString.Clear();
                    Engine.fpsString.AppendFormat("GC: {0:D5} KB\n", GC.GetTotalMemory(false) / 1024L);
                    Engine.fpsString.AppendFormat("FPS: {0:F1}", Engine.fpsAverage);
                    Engine.debugText.DisplayedString = Engine.fpsString.ToString();
                }
                Engine.frameBuffer.Draw(Engine.debugText);
            }
            Engine.frameBuffer.Display();
            Engine.window.Clear(SFML.Graphics.Color.Black);
            Engine.window.Draw(Engine.frameBufferVertArray, Engine.frameBufferState);
            Engine.window.Display();
            Engine.Running = (!SceneManager.Instance.IsEmpty && !Engine.quit);
            Engine.frameStopwatch.Stop();
            Engine.fps = 1f / (float)Engine.frameStopwatch.ElapsedTicks * (float)Stopwatch.Frequency;
            Engine.fpsAverage = (Engine.fpsAverage + Engine.fps) / 2f;
            Engine.frameIndex += 1L;
        }

        // Token: 0x060000F8 RID: 248 RVA: 0x00005D80 File Offset: 0x00003F80
        public static void SetWindowIcon(string file)
        {
            if (File.Exists(file))
            {
                Engine.iconFile = new IconFile(file);
                Engine.window.SetIcon(32U, 32U, Engine.iconFile.GetBytesForSize(32));
            }
        }

        // Token: 0x0400006D RID: 109
        public const string CAPTION = "Mother 4";

        // Token: 0x0400006E RID: 110
        public const uint TARGET_FRAMERATE = 60U;

        // Token: 0x0400006F RID: 111
        private const decimal REQUIRED_OGL_VERSION = 2.1m;

        // Token: 0x04000070 RID: 112
        public const uint SCREEN_WIDTH = 320U;

        // Token: 0x04000071 RID: 113
        public const uint SCREEN_HEIGHT = 180U;

        // Token: 0x04000072 RID: 114
        public const uint HALF_SCREEN_WIDTH = 160U;

        // Token: 0x04000073 RID: 115
        public const uint HALF_SCREEN_HEIGHT = 90U;

        // Token: 0x04000074 RID: 116
        private const int CURSOR_TIMEOUT = 90;

        // Token: 0x04000075 RID: 117
        private const int ICON_SIZE = 32;

        // Token: 0x04000076 RID: 118
        private const int DOUBLE_CLICK_TIME = 20;

        // Token: 0x04000077 RID: 119
        public static readonly Vector2f SCREEN_SIZE = new Vector2f(320f, 180f);

        // Token: 0x04000078 RID: 120
        public static readonly Vector2f HALF_SCREEN_SIZE = Engine.SCREEN_SIZE / 2f;

        // Token: 0x04000079 RID: 121
        private static uint frameBufferScale = 2U;

        // Token: 0x0400007A RID: 122
        private static RenderWindow window;

        // Token: 0x0400007B RID: 123
        private static RenderTexture frameBuffer;

        // Token: 0x0400007C RID: 124
        private static RenderStates frameBufferState;

        // Token: 0x0400007D RID: 125
        private static VertexArray frameBufferVertArray;

        // Token: 0x0400007E RID: 126
        private static Random rand;

        // Token: 0x0400007F RID: 127
        private static FontData defaultFont;

        // Token: 0x04000080 RID: 128
        private static Text debugText;

        // Token: 0x04000081 RID: 129
        private static bool quit;

        // Token: 0x04000082 RID: 130
        private static bool isFullscreen;

        // Token: 0x04000083 RID: 131
        private static bool switchScreenMode;

        // Token: 0x04000084 RID: 132
        private static float fps;

        // Token: 0x04000085 RID: 133
        private static float fpsAverage;

        // Token: 0x04000086 RID: 134
        private static long frameIndex;

        // Token: 0x04000087 RID: 135
        private static long startTicks;

        // Token: 0x04000088 RID: 136
        private static long cursorTimer;

        // Token: 0x04000089 RID: 137
        private static bool showCursor;

        // Token: 0x0400008A RID: 138
        private static long clickFrame = long.MinValue;

        // Token: 0x0400008B RID: 139
        private static IconFile iconFile;

        // Token: 0x0400008C RID: 140
        private static StringBuilder fpsString;

        // Token: 0x0400008D RID: 141
        private static float screenAngle = 0f;

        // Token: 0x0400008E RID: 142
        public static bool debugDisplay;

        // Token: 0x0400008F RID: 143
        private static Stopwatch frameStopwatch;
    }
}
