using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace MHAiM
{
    public partial class Form1 : Form
    {
        // DLL позиции курсора
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);
        // DLL положения клавиши
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        // Инициализация имитации клавиш
        private InputSimulator inputSimulator = new InputSimulator();
        private VirtualKeyCode lastPressedKey = VirtualKeyCode.NONAME;
        private KeyboardHook keyboardHook;

        // Бинд зажима
        private Keys notPilote = Keys.LButton;
        // Бинд триггера
        private Keys TriggetBtn = Keys.T;

        // Инициализация цветов
        private Color headColor = Color.FromArgb(0x00, 0xFF, 0x00);
        private Color bodyRedColor = Color.FromArgb(0xFF, 0x00, 0x00);
        private Color bodyBlueColor = Color.FromArgb(0x00, 0x00, 0xFF);

        // state - выбранный режим
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

        // Рандомайзер
        Random rnd = new Random();
        int rndValue;

        // Позиция курсора в X и Y
        int CursorX = Cursor.Position.X;
        int CursorY = Cursor.Position.Y;

        // Инициализация координат движения мыши
        int xOffset;
        int yOffset;

        // Иницализация формы
        public Form1()
        {
            InitializeComponent();
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDown += KeyboardHook_KeyDown;
        }

        // Хук клавиатуры
        public void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            lastPressedKey = (VirtualKeyCode)e.KeyCode;
        }

        // Загрузка формы, запуск цикла
        private void Form1_Load(object sender, EventArgs e)
        {
            Thread logicThread = new Thread(MainLogic) { IsBackground = true };
            logicThread.Start();
        }

        // Логика наводки
        private void MainLogic()
        {
            // Вечный цикл
            while (true)
            {
                // Поиск цветов
                Point headColorPosition = FindColorPosition(headColor, 885, 465, 905, 485);
                Point bodyRedColorPosition = FindColorPosition(bodyRedColor, 860, 440, 910, 490);
                Point bodyBlueColorPosition = FindColorPosition(bodyBlueColor, 255, 115, 1535, 835);

                Point rageHead = FindColorPosition(headColor, 681, 367, 1078, 603);

                // Смена режимов
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



                // Наведение на голову
                //if (!foundHeadColorPosition.IsEmpty && state != 3 && state != 0 && state != 8)
                //{
                //    PerformMouseAction(foundHeadColorPosition);
                //}
                //// Наведение на тело
                //else if ((!foundBodyRedColorPosition.IsEmpty || !foundBodyBlueColorPosition.IsEmpty) && (state == 3))
                //{
                //    AWPtrigger();
                //}

                // Таймер для реалистичности
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
        // Логика выстрелов, поведения
        private void PerformMouseAction(Point headValue, Point blueValue, Point redValue)
        {
            // Проверка зажима
            if (IsHotkeyPressed(notPilote))
            {
                return;
            }

            Color pixelColor = GetColorPixel(CursorX, CursorY);

            // Логика головы
            if (!headValue.IsEmpty && blueValue.IsEmpty && redValue.IsEmpty)
            {
                // Сдвиг относительно найденной головы
                xOffset = headValue.X - 886; // 886
                yOffset = headValue.Y - 472; // 472

                // Движение мыши на голову
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                Thread.Sleep(250);
                inputSimulator.Mouse.LeftButtonClick();
                
            }
        }

        // Смена надписи режима
        private void UpdateSelectedModeLabel(string state)
        {
            if (SelectedModeLabel.InvokeRequired)
            {
                SelectedModeLabel.Invoke(new Action(() => SelectedModeLabel.Text = $"Выбран режим: {state}"));
            }
            else
            {
                SelectedModeLabel.Text = $"Выбран режим: {state}";
            }
        }

        // Поиск координат цвета
        private Point FindColorPosition(Color targetColor, int startX, int startY, int endX, int endY)
        {
            // try для обработки исключений
            try
            {
                // Поиск пикселя по битмапе
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
            // Обработка исключения
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
                        // Координаты движения мыши
                        int xOffset = foundColorPosition.X - 886;
                        int yOffset = foundColorPosition.Y - 472;

                        inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                        Thread.Sleep(20);
                        inputSimulator.Mouse.LeftButtonClick();
                    }
                }
            }
        }

        // Триггер для AWP
        public void AWPtrigger()
        {
            while (state == 3)
            {
                Color pixelColor = GetColorPixel(CursorX, CursorY);

                // Получение пикселя по координатам
                if ((pixelColor == bodyBlueColor) || (pixelColor == bodyRedColor))
                {
                    if (!IsHotkeyPressed(TriggetBtn))
                    {
                        inputSimulator.Mouse.LeftButtonClick();
                    }
                }
            }
            
            

            // Проверка цвета по RGB
            //if ((pixelColor.R > 180 && pixelColor.G < 10 && pixelColor.B < 10) || // Красный
            //   (pixelColor.R < 10 && pixelColor.G < 10 && pixelColor.B > 180) || // Синий
            //   (pixelColor.R < 10 && pixelColor.G > 180 && pixelColor.B < 10))  // Зелёный
            //{
            //    if (!IsHotkeyPressed(TriggetBtn))
            //    {
            //        inputSimulator.Mouse.LeftButtonClick();
            //        return;
            //    }
            //}
        }

        // Функция получение цвета пикселя
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


        // Проверка схожести цветов
        private static bool AreColorsSimilar(Color color1, Color color2, int maxColorDifference)
        {
            int redDifference = Math.Abs(color1.R - color2.R);
            int greenDifference = Math.Abs(color1.G - color2.G);
            int blueDifference = Math.Abs(color1.B - color2.B);
            return redDifference <= maxColorDifference && greenDifference <= maxColorDifference && blueDifference <= maxColorDifference;
        }

        // Проверка ЗАЖАТИЯ клавиши (для зажима)
        private static bool IsHotkeyPressed(Keys vKey)
        {
            return (GetAsyncKeyState(vKey) < 0);
        }

        // Кнопка AK-47
        private void AKButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("AK-47");
            state = 1;
        }

        // Кнопка M4A1
        private void M4Button_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("M4A1");
            state = 2;
        }

        // Кнопка AWP
        private void AWPButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("AWP");
            state = 3;
        }

        // Кнопка Deagle
        private void DeagleButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("Deagle");
            state = 4;
        }

        // Кнопка Glock
        private void GlockButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("Glock");
            state = 5;
        }

        // Кнопка USP-S
        private void USPButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedModeLabel("USP-S");
            state = 6;
        }
    }
}