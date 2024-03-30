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
        // Логика наводки
        private void MainLogic()
        {
            // Вечный цикл
            while (true)
            {
                headPos = SetPoint(headColor);
                bluePos = SetPoint(bodyBlueColor);
                redPos = SetPoint(bodyRedColor);

                rageHead = FindColorPosition(headColor, 885, 465, 905, 485);

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

        // Логика выстрелов, поведения
        private void PerformMouseAction(Point headValue, Point blueValue, Point redValue, Point extraValue)
        {
            // Проверка зажима
            if (IsHotkeyPressed(notPilote))
            {
                return;
            }

            // Проверка режима
            switch (state)
            {
                // STOP
                case 0:
                    break;
                // AK-47
                case 1:
                    // Поиск пикселя
                    if (SetPoint(bodyBlueColor).IsEmpty)
                    {
                        SetPoint(bodyRedColor);
                        akLogic(redPos);
                    }
                    else
                    {
                        SetPoint(bodyBlueColor);
                        akLogic(bluePos);
                    }
                    break;
                // M4A1
                case 2:
                    if (SetPoint(bodyBlueColor).IsEmpty)
                    {
                        SetPoint(bodyRedColor);
                        pixelColor = GetColorPixel(CursorX, CursorY);
                        m4Logic(redPos);
                    }
                    else
                    {
                        SetPoint(bodyBlueColor);
                        pixelColor = GetColorPixel(CursorX, CursorY);
                        m4Logic(bluePos);
                    }
                    break;
                // AWP
                case 3:
                    pixelColor = GetColorPixel(CursorX, CursorY);
                    awpLogic();
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
                // Copilot
                case 7:
                    copiloteAction(headPos, false);
                    break;
                // QuickDraw
                case 8:
                    copiloteAction(rageHead, true);
                    break;
            }
        }

        // Логика поведения AK-47
        private void akLogic(Point teamPos)
        {
            // Проверка пустоты пикселя
            if (!teamPos.IsEmpty)
            {

                // Оффсеты движения
                xOffset = teamPos.X - 890;
                yOffset = teamPos.Y - 475;

                // Движение мыши
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);

                // Рандом
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
        }

        // Логика поведения M4A1
        private void m4Logic(Point teamPos)
        {
            // Проверка пустоты пикселя
            if (!teamPos.IsEmpty)
            {
                // Оффсеты движения
                xOffset = teamPos.X - 890;
                yOffset = teamPos.Y - 475;

                // Движение мыши
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                Thread.Sleep(7);

                // Рандом
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

        private void copiloteAction(Point headValue, bool rage)
        {
            if (!headValue.IsEmpty)
            {
                xOffset = headValue.X - 890; 
                yOffset = headValue.Y - 475;

                // Движение мыши на голову
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);

                if (rage == true)
                {
                    inputSimulator.Mouse.LeftButtonClick();
                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(20);
                }
            }
        }

        // Триггер для AWP
        public void awpLogic()
        {
            Color newColor = GetColorPixel(CursorX, CursorY);
            Console.WriteLine(CursorX);

            // Проверка пикселя на соответствие цвету врага
            //if ((newColor.R > 180 && newColor.G < 10 && newColor.B < 10) ||
            //    (newColor.R < 10 && newColor.G < 10 && newColor.B > 180) ||
            //    (newColor.R < 10 && newColor.G > 180 && newColor.B < 10))
            //{
            //    inputSimulator.Mouse.LeftButtonClick();
            //    return;
            //}
        }

        #region Инициализации
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

        // Рандомайзер
        Random rnd = new Random();
        int rndValue;
        int stopValue;

        // Инициализация координат движения мыши
        int xOffset;
        int yOffset;

        // Позиция курсора в X и Y
        int CursorX = Cursor.Position.X;
        int CursorY = Cursor.Position.Y;

        // Иницилизация поинтов
        Point headPos = new Point();
        Point bluePos = new Point();
        Point redPos = new Point();
        Point rageHead = new Point();

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

        #endregion

        #region Бинды
        // Бинд зажима
        private Keys notPilote = Keys.LButton;
        // Бинд триггера
        private Keys TriggetBtn = Keys.T;
        #endregion

        #region Цвета
        // Инициализация цветов
        private Color headColor = Color.FromArgb(0x00, 0xFF, 0x00);
        private Color bodyRedColor = Color.FromArgb(0xFF, 0x00, 0x00);
        private Color bodyBlueColor = Color.FromArgb(0x00, 0x00, 0xFF);

        private Color pixelColor;
        #endregion

        #region Состояния
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
        #endregion

        #region Утилиты
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

        // Функция получения поинта
        public static Point SetPoint(Color gotColor)
        {
            Point getPoint = FindColorPosition(gotColor, 885, 465, 905, 485);
            return getPoint;
        }

        // Функция получение цвета пикселя
        public static Color GetColorPixel(int x, int y)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
                {
                    // Минус для прицела AWP
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
            // Переменная состояния клавиши
            short keyState = GetAsyncKeyState(vKey);
            
            // Возврат, что клавиша зажата
            return (keyState < 0);      
        }

        // Поиск координат цвета
        public static Point FindColorPosition(Color targetColor, int startX, int startY, int endX, int endY)
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
        #endregion

        #region Визуальные кнопки
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
        #endregion
    }
}
