using System.Drawing.Imaging;
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

        // ������������� ��������� �������� ����
        int xOffset;
        int yOffset;

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
                // ����� ������
                Point headColorPosition = FindColorPosition(headColor, 885, 465, 905, 485);
                Point bodyRedColorPosition = FindColorPosition(bodyRedColor, 860, 440, 910, 490);
                Point bodyBlueColorPosition = FindColorPosition(bodyBlueColor, 255, 115, 1535, 835);

                Point rageHead = FindColorPosition(headColor, 681, 367, 1078, 603);

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
                        PerformMouseAction(rageHead, Point.Empty, Point.Empty);
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
                //if (!foundHeadColorPosition.IsEmpty && state != 3 && state != 0 && state != 8)
                //{
                //    PerformMouseAction(foundHeadColorPosition);
                //}
                //// ��������� �� ����
                //else if ((!foundBodyRedColorPosition.IsEmpty || !foundBodyBlueColorPosition.IsEmpty) && (state == 3))
                //{
                //    AWPtrigger();
                //}

                // ������ ��� ��������������
                //if (state == 8)
                //{
                //    if (!foundHeadColorPosition.IsEmpty)
                //    {
                //        QuickDrawAction(foundHeadColorPosition);
                //    }
                //    if (!foundBodyBlueColorPosition.IsEmpty)
                //    {
                //        QuickDrawAction(foundBodyBlueColorPosition);
                //    }
                //    if (!foundBodyRedColorPosition.IsEmpty)
                //    {
                //        QuickDrawAction(foundBodyRedColorPosition);
                //    }
                //}
                //else
                //{
                //    Thread.Sleep(10);
                //}
            }
        }
        // ������ ���������, ���������
        private void PerformMouseAction(Point headValue, Point blueValue, Point redValue)
        {
            // �������� ������
            if (IsHotkeyPressed(notPilote))
            {
                return;
            }

            Color pixelColor = GetColorPixel(CursorX, CursorY);

            // ������ ������
            if (!headValue.IsEmpty && blueValue.IsEmpty && redValue.IsEmpty)
            {
                // ����� ������������ ��������� ������
                xOffset = headValue.X - 886; // 886
                yOffset = headValue.Y - 472; // 472

                // �������� ���� �� ������
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                Thread.Sleep(250);
                inputSimulator.Mouse.LeftButtonClick();
                
            }
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
            if (!IsHotkeyPressed(notPilote))
            {
                if (!foundColorPosition.IsEmpty)
                {
                    Color pixelColor = GetColorPixel(CursorX, CursorY);

                    if (!foundColorPosition.IsEmpty)
                    {
                        // ���������� �������� ����
                        int xOffset = foundColorPosition.X - 886;
                        int yOffset = foundColorPosition.Y - 472;

                        inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                        Thread.Sleep(20);
                        inputSimulator.Mouse.LeftButtonClick();
                    }
                }
            }
        }

        // ������� ��� AWP
        public void AWPtrigger()
        {
            while (state == 3)
            {
                Color pixelColor = GetColorPixel(CursorX, CursorY);

                // ��������� ������� �� �����������
                if ((pixelColor == bodyBlueColor) || (pixelColor == bodyRedColor))
                {
                    if (!IsHotkeyPressed(TriggetBtn))
                    {
                        inputSimulator.Mouse.LeftButtonClick();
                    }
                }
            }
            
            

            // �������� ����� �� RGB
            //if ((pixelColor.R > 180 && pixelColor.G < 10 && pixelColor.B < 10) || // �������
            //   (pixelColor.R < 10 && pixelColor.G < 10 && pixelColor.B > 180) || // �����
            //   (pixelColor.R < 10 && pixelColor.G > 180 && pixelColor.B < 10))  // ������
            //{
            //    if (!IsHotkeyPressed(TriggetBtn))
            //    {
            //        inputSimulator.Mouse.LeftButtonClick();
            //        return;
            //    }
            //}
        }

        // ������� ��������� ����� �������
        public static Color GetColorPixel(int x, int y)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
                {
                    Rectangle lockRectangle = new Rectangle(x - 1, y - 1, 1, 1);
                    BitmapData data = bmp.LockBits(lockRectangle,ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    unsafe
                    {
                        byte* pointer = (byte*)data.Scan0;
                        byte blue = pointer[0];
                        byte green = pointer[1];
                        byte red = pointer[2];
                        byte alpha = pointer[3];
                        return Color.FromArgb(alpha, red, green, blue);
                    }
                }
            }
            catch
            {
                return Color.Empty;
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