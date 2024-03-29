using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace MHAiM
{
    public partial class Form1 : Form
    {
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

        // ���� ������
        private Keys notPilote = Keys.LButton;
        // ���� ��������
        private Keys TriggetBtn = Keys.T;

        // ������������� ������
        private Color headColor = Color.FromArgb(0x00, 0xFF, 0x00);
        private Color nowColor;
        private Color bodyRedColor = Color.FromArgb(0xFF, 0x00, 0x00);
        private Color bodyBlueColor = Color.FromArgb(0x00, 0x00, 0xFF);

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

        // �����������
        Random rnd = new Random();
        int rndValue;

        // ������� ������� � X � Y
        int CursorX = Cursor.Position.X;
        int CursorY = Cursor.Position.Y;

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

        // ������ �������
        private void MainLogic()
        {
            // ������ ����
            while (true)
            {
                // ��������� ������ �� ������
                //Point foundHeadColorPosition = new Point();
                //Point foundBodyRedColorPosition = new Point();
                //Point foundBodyBlueColorPosition = new Point();
                Point foundHeadColorPosition = FindColorPosition(headColor, 885, 465, 905, 485);
                Point foundBodyRedColorPosition = FindColorPosition(bodyRedColor, 860, 440, 910, 490);
                Point foundBodyBlueColorPosition = FindColorPosition(bodyBlueColor, 860, 440, 910, 490);
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
                // ��������� �� ������
                if (!foundHeadColorPosition.IsEmpty && state != 3 && state != 0 && state != 8)
                {
                    PerformMouseAction(foundHeadColorPosition);
                }
                // ��������� �� ����
                else if ((!foundBodyRedColorPosition.IsEmpty || !foundBodyBlueColorPosition.IsEmpty) && (state == 3))
                {
                    AWPtrigger();
                }

                // ������ ��� ��������������
                if ((!foundHeadColorPosition.IsEmpty) && (state == 8))
                {
                    QuickDrawAction(foundHeadColorPosition);
                }

                if (state == 8)
                {
                    Thread.Sleep(0);
                }
                else
                {
                    Thread.Sleep(10);
                }  
            }
        }
        // ������ ���������, ���������
        private void PerformMouseAction(Point foundColorPosition)
        {
            // ���������� �������� ����
            int xOffset = foundColorPosition.X - 886;
            int yOffset = foundColorPosition.Y - 472;

            // �������� ������
            if (!IsHotkeyPressed(notPilote))
            {
                // �������� �������
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                // �������� ���������� ������
                switch (state)
                {
                    // STOP
                    case 0:
                        break;
                    // AK-47
                    case 1:
                        rndValue = rnd.Next(10, 20);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        break;
                    // M4A1
                    case 2:
                        rndValue = rnd.Next(200, 220);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        break;
                    // AWP
                    case 3:
                        AWPtrigger();
                        break;
                    // Deagle
                    case 4:
                        rndValue = rnd.Next(550, 620);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        break;
                    // Glock
                    case 5:
                        rndValue = rnd.Next(5, 15);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        break;
                    // USP-S
                    case 6:
                        rndValue = rnd.Next(15, 25);
                        inputSimulator.Mouse.LeftButtonClick();
                        Thread.Sleep(rndValue);
                        break;
                    // QuickDraw
                    //case 8:
                    //    Point foundHeadColorPosition = FindColorPosition(headColor, 900, 480, 920, 495);
                    //    QuickDrawAction(foundHeadColorPosition);
                    //    break;
                }
            }
            //else
            //{
            //    while (IsHotkeyPressed(notPilote))
            //    {
            //        inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset+1);
            //        Thread.Sleep(15);
            //    }
            //}
        }

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

        // ����� ��������� �����
        private Point FindColorPosition(Color targetColor, int startX, int startY, int endX, int endY)
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

        // QuickDraw script
        public void QuickDrawAction(Point foundColorPosition)
        {
            if (!foundColorPosition.IsEmpty)
            {
                Color pixelColor = GetColorPixel(CursorX, CursorY);
                Point foundHeadColorPosition = FindColorPosition(headColor, 885, 465, 905, 485);

                if (!foundHeadColorPosition.IsEmpty)
                {
                    // ���������� �������� ����
                    int xOffset = foundHeadColorPosition.X - 886;
                    int yOffset = foundHeadColorPosition.Y - 472;

                    inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                    if (pixelColor != headColor)
                    {
                        foundHeadColorPosition = FindColorPosition(headColor, 825, 440, 960, 575);
                        inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                    }
                    inputSimulator.Mouse.LeftButtonClick();
                }
            }
        }

        // ������� ��� AWP
        public void AWPtrigger()
        {
            // ��������� ������� �� �����������
            Color pixelColor = GetColorPixel(CursorX, CursorY);
            // �������� ����� �� RGB
            if ((pixelColor.R > 180 && pixelColor.G < 10 && pixelColor.B < 10) || // �������
               (pixelColor.R < 10 && pixelColor.G < 10 && pixelColor.B > 180) || // �����
               (pixelColor.R < 10 && pixelColor.G > 180 && pixelColor.B < 10))  // ������
            {
                if (!IsHotkeyPressed(TriggetBtn))
                {
                    inputSimulator.Mouse.LeftButtonClick();
                    return;
                }
            }
        }

        // ������� ��������� ����� �������
        public static Color GetColorPixel(int x, int y)
        {
            try
            {
                // ���� ������� ��� ��������
                Bitmap bmp = new Bitmap(3, 3);
                // -1, -1 ��� �������� �� ������� ����, ��� ������ � AWP �������
                Rectangle bounds = new Rectangle(x - 1, y - 1, 3, 3);
                using (Graphics g = Graphics.FromImage(bmp))
                    g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                return bmp.GetPixel(1, 0);
            }
            catch
            {
                Bitmap bmp = new Bitmap(1, 1);
                return bmp.GetPixel(0, 0);
            }
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
            return (GetAsyncKeyState(vKey) < 0);
        }

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
    }
}