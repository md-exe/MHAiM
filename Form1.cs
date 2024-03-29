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
        private Color nowColor;
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
                // Положение цветов на экране
                //Point foundHeadColorPosition = new Point();
                //Point foundBodyRedColorPosition = new Point();
                //Point foundBodyBlueColorPosition = new Point();
                Point foundHeadColorPosition = FindColorPosition(headColor, 885, 465, 905, 485);
                Point foundBodyRedColorPosition = FindColorPosition(bodyRedColor, 860, 440, 910, 490);
                Point foundBodyBlueColorPosition = FindColorPosition(bodyBlueColor, 860, 440, 910, 490);
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
                // Наведение на голову
                if (!foundHeadColorPosition.IsEmpty && state != 3 && state != 0 && state != 8)
                {
                    PerformMouseAction(foundHeadColorPosition);
                }
                // Наведение на тело
                else if ((!foundBodyRedColorPosition.IsEmpty || !foundBodyBlueColorPosition.IsEmpty) && (state == 3))
                {
                    AWPtrigger();
                }

                // Таймер для реалистичности
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
        // Логика выстрелов, поведения
        private void PerformMouseAction(Point foundColorPosition)
        {
            // Координаты движения мыши
            int xOffset = foundColorPosition.X - 886;
            int yOffset = foundColorPosition.Y - 472;

            // Проверка зажима
            if (!IsHotkeyPressed(notPilote))
            {
                // Движение курсора
                inputSimulator.Mouse.MoveMouseBy(xOffset, yOffset);
                // Проверка выбранного режима
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
            if (!foundColorPosition.IsEmpty)
            {
                Color pixelColor = GetColorPixel(CursorX, CursorY);
                Point foundHeadColorPosition = FindColorPosition(headColor, 885, 465, 905, 485);

                if (!foundHeadColorPosition.IsEmpty)
                {
                    // Координаты движения мыши
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

        // Триггер для AWP
        public void AWPtrigger()
        {
            // Получение пикселя по координатам
            Color pixelColor = GetColorPixel(CursorX, CursorY);
            // Проверка цвета по RGB
            if ((pixelColor.R > 180 && pixelColor.G < 10 && pixelColor.B < 10) || // Красный
               (pixelColor.R < 10 && pixelColor.G < 10 && pixelColor.B > 180) || // Синий
               (pixelColor.R < 10 && pixelColor.G > 180 && pixelColor.B < 10))  // Зелёный
            {
                if (!IsHotkeyPressed(TriggetBtn))
                {
                    inputSimulator.Mouse.LeftButtonClick();
                    return;
                }
            }
        }

        // Функция получение цвета пикселя
        public static Color GetColorPixel(int x, int y)
        {
            try
            {
                // Цвет пикселя под курсором
                Bitmap bmp = new Bitmap(3, 3);
                // -1, -1 для смещения на пиксель выше, ибо прицел у AWP красный
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