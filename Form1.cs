using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;
using WindowsInput;
using WindowsInput.Native;

namespace MHAiM
{
    public partial class Form1 : Form
    {
        // ������ �������
        private void MainLogic()
        {
            // ������ ����
            while (true)
            {
                headPos = SetPoint(headColor);
                bluePos = SetPoint(bodyBlueColor);
                redPos = SetPoint(bodyRedColor);

                rageHead = FindColorPosition(headColor, 885, 465, 905, 485);

                // ����� �������
                switch (lastPressedKey)
                {
                    case VirtualKeyCode.F3:
                        UpdateSelectedModeLabel("STOP");
                        state = 0;
                        break;
                    case VirtualKeyCode.NUMPAD4:
                        UpdateSelectedModeLabel("AK-47");
                        state = 1;
                        break;
                    case VirtualKeyCode.NUMPAD5:
                        UpdateSelectedModeLabel("M4A1");
                        state = 2;
                        break;
                    case VirtualKeyCode.NUMPAD6:
                        UpdateSelectedModeLabel("AWP");
                        state = 3;
                        break;
                    case VirtualKeyCode.NUMPAD1:
                        UpdateSelectedModeLabel("Deagle");
                        state = 4;
                        break;
                    case VirtualKeyCode.NUMPAD2:
                        UpdateSelectedModeLabel("Glock");
                        state = 5;
                        break;
                    case VirtualKeyCode.NUMPAD3:
                        UpdateSelectedModeLabel("USP-S");
                        state = 6;
                        break;
                    case VirtualKeyCode.F4:
                        UpdateSelectedModeLabel("Copilot");
                        state = 7;
                        break;
                    case VirtualKeyCode.NUMPAD0:
                        UpdateSelectedModeLabel("QuickDraw");
                        state = 8;
                        break;
                }

                PerformMouseAction(headPos, bluePos, redPos, rageHead);
            }
        }

        // ������ ���������, ���������
        private void PerformMouseAction(Point headValue, Point blueValue, Point redValue, Point extraValue)
        {
            // �������� ������
            if (IsHotkeyPressed(notPilote))
            {
                return;
            }

            // �������� ������
            switch (state)
            {
                // STOP
                case 0:
                    break;
                // AK-47 - ����� ������
                case 1:
                // M4A1 - ������ ������
                case 2:
                    // ����� ������ ��� AK-47 � M4A1
                    Point targetPos = SetPoint(state == 1 ? bodyRedColor : bodyBlueColor);
                    pixelColor = GetColorPixel(CursorX, CursorY);
                    rifleLogic(targetPos, state);
                    break;
                // AWP
                case 3:
                    pixelColor = GetColorPixel(CursorX-3, CursorY-3);
                    awpLogic(bluePos, redPos, headPos);
                    break;
                // Deagle
                case 4:
                    break;
                // Glock
                case 5:
                    break;
                // USP-S
                case 6:
                    break;
                // Copilot - ����� ������
                case 7:
                // QuickDraw - ����� ������
                case 8:
                    copiloteAction(state == 8 ? extraValue : headValue, state == 8);
                    break;
            }
        }

        // ������ ��������
        private void rifleLogic(Point teamPos, byte type)
        {
            // ���� ������� ������
            if (!teamPos.IsEmpty)
            {
                // ������ AK-47
                if (type == 1)
                {
                    // ������� ��������
                    xOffset = teamPos.X - 890;
                    yOffset = teamPos.Y - 475;

                    // �������� ����
                    inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);

                    // ������
                    rndValue = rnd.Next(92, 115);
                    stopValue = rnd.Next(480, 500);

                    inputSimulator.Mouse.LeftButtonClick();
                    Thread.Sleep(rndValue);
                    inputSimulator.Mouse.MoveMouseBy(0, 3);
                    inputSimulator.Mouse.LeftButtonClick();
                    Thread.Sleep(rndValue);
                    inputSimulator.Mouse.MoveMouseBy(0, 5);
                    inputSimulator.Mouse.LeftButtonClick();
                    inputSimulator.Mouse.MoveMouseBy(0, 7);
                    Thread.Sleep(stopValue);
                }
                else if (type == 2)
                {
                    // ������� ��������
                    xOffset = teamPos.X - 890;
                    yOffset = teamPos.Y - 475;

                    // �������� ����
                    inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                    Thread.Sleep(7);

                    // ������
                    rndValue = rnd.Next(95, 100);
                    stopValue = rnd.Next(380, 396);

                    inputSimulator.Mouse.LeftButtonDown();
                    Thread.Sleep(rndValue);
                    if (pixelColor != bodyRedColor || pixelColor != bodyRedColor)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            inputSimulator.Mouse.MoveMouseBy(0, i);
                            Thread.Sleep(3);
                        }
                    }
                    inputSimulator.Mouse.LeftButtonUp();
                    Thread.Sleep(stopValue);
                }
            }
        }

        private void copiloteAction(Point headValue, bool rage)
        {
            if (!headValue.IsEmpty)
            {
                xOffset = headValue.X - 890; 
                yOffset = headValue.Y - 475;

                // �������� ���� �� ������
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);

                if (rage == true)
                {
                    inputSimulator.Mouse.LeftButtonClick();
                    Thread.Sleep(30);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        // ������� ��� AWP
        public void awpLogic(Point blueValue, Point redValue, Point headValue)
        {
            if (!blueValue.IsEmpty || !redValue.IsEmpty || !headValue.IsEmpty)
            {
                inputSimulator.Mouse.LeftButtonClick();
            }
        }

        #region �������������
        // DLL ������� �������
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);
        // DLL ��������� �������
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        // ������������� �������� ������
        private InputSimulator inputSimulator = new InputSimulator();
        private VirtualKeyCode lastPressedKey = VirtualKeyCode.NONAME;
        private KeyboardHook keyboardHook;

        // �����������
        Random rnd = new Random();
        int rndValue;
        int stopValue;

        // ������������� ��������� �������� ����
        int xOffset;
        int yOffset;

        // ������� ������� � X � Y
        int CursorX = Cursor.Position.X;
        int CursorY = Cursor.Position.Y;

        // ������������ �������
        Point headPos = new Point();
        Point bluePos = new Point();
        Point redPos = new Point();
        Point rageHead = new Point();

        // ������������ �����
        public Form1()
        {
            InitializeComponent();
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDown += KeyboardHook_KeyDown;
        }

        // ��� ����������
        public void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            lastPressedKey = (VirtualKeyCode)e.KeyCode;
        }

        // �������� �����, ������ �����
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread logicThread = new Thread(MainLogic) { IsBackground = true };
            logicThread.Start();
        }

        #endregion

        #region �����
        // ���� ������
        private Keys notPilote = Keys.LButton;
        // ���� ��������
        private Keys TriggetBtn = Keys.T;
        #endregion

        #region �����
        // ������������� ������
        private Color headColor = Color.FromArgb(0x00, 0xFF, 0x00);
        private Color bodyRedColor = Color.FromArgb(0xFF, 0x00, 0x00);
        private Color bodyBlueColor = Color.FromArgb(0x00, 0x00, 0xFF);

        private Color pixelColor;
        #endregion

        #region ���������
        // state - ��������� �����
        private byte state = 0;

        // 0 - STOP
        // 1 - AK-47
        // 2 - M4A1
        // 3 - AWP
        // 4 - Deagle
        // 5 - Glock
        // 6 - USP-S
        // 7 - Copilot
        // 8 - QuickDraw
        #endregion

        #region �������
        // ����� ������� ������
        private void UpdateSelectedModeLabel(string state)
        {
            if (SelectedModeLabel.InvokeRequired)
            {
                SelectedModeLabel.Invoke(new Action(() => SelectedModeLabel.Text = $"������ �����: {state}"));
            }
            else
            {
                SelectedModeLabel.Text = $"������ �����: {state}";
            }
        }

        // ������� ��������� ������
        public static Point SetPoint(Color gotColor)
        {
            Point getPoint = FindColorPosition(gotColor, 885, 465, 905, 485);
            return getPoint;
        }

        // ������� ��������� ����� �������
        public static Color GetColorPixel(int x, int y)
        {
            Color resultColor = Color.Empty;
            Task.Run(() =>
            {
                try
                {
                    using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
                    {
                        // ����� ��� ������� AWP
                        Rectangle lockRectangle = new Rectangle(x - 1, y - 1, 1, 1);
                        BitmapData data = bmp.LockBits(lockRectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                        unsafe
                        {
                            byte* pointer = (byte*)data.Scan0;
                            byte blue = pointer[0];
                            byte green = pointer[1];
                            byte red = pointer[2];

                            return Color.FromArgb(red, green, blue);
                        }
                    }
                }
                catch
                {
                    return Color.Empty;
                }
            }).Wait();
            return resultColor;
        }


        // �������� �������� ������
        private static bool AreColorsSimilar(Color color1, Color color2, int maxColorDifference)
        {
            int redDifference = Math.Abs(color1.R - color2.R);
            int greenDifference = Math.Abs(color1.G - color2.G);
            int blueDifference = Math.Abs(color1.B - color2.B);
            return redDifference <= maxColorDifference && greenDifference <= maxColorDifference && blueDifference <= maxColorDifference;
        }

        // �������� ������� ������� (��� ������)
        private static bool IsHotkeyPressed(Keys vKey)
        {
            // ���������� ��������� �������
            short keyState = GetAsyncKeyState(vKey);
            
            // �������, ��� ������� ������
            return (keyState < 0);      
        }

        // ����� ��������� �����
        public static Point FindColorPosition(Color targetColor, int startX, int startY, int endX, int endY)
        {
            // try ��� ��������� ����������
            try
            {
                // ����� ������� �� �������
                using (Bitmap screenshot = new Bitmap(endX - startX, endY - startY))
                using (Graphics g = Graphics.FromImage(screenshot))
                {
                    g.CopyFromScreen(new Point(startX, startY), Point.Empty, screenshot.Size);
                    for (int x = 0; x < screenshot.Width; x++)
                    {
                        for (int y = 0; y < screenshot.Height; y++)
                        {
                            Color pixelColor = screenshot.GetPixel(x, y);
                            if (AreColorsSimilar(targetColor, pixelColor, 15))
                            {
                                return new Point(x + startX, y + startY);
                            }
                        }
                    }
                    return Point.Empty;
                }
            }
            // ��������� ����������
            catch
            {
                return Point.Empty;
            }
        }
        #endregion

        #region ���������� ������
        // ������ AK-47
        private void AKButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("AK-47");
            state = 1;
        }

        // ������ M4A1
        private void M4Button_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("M4A1");
            state = 2;
        }

        // ������ AWP
        private void AWPButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("AWP");
            state = 3;
        }

        // ������ Deagle
        private void DeagleButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("Deagle");
            state = 4;
        }

        // ������ Glock
        private void GlockButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("Glock");
            state = 5;
        }

        // ������ USP-S
        private void USPButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("USP-S");
            state = 6;
        }
        #endregion
    }
}
