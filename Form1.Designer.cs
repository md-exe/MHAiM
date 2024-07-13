namespace MHAiM
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AKButton = new Button();
            M4Button = new Button();
            USPButton = new Button();
            AWPButton = new Button();
            GlockButton = new Button();
            DeagleButton = new Button();
            SelectedModeLabel = new Label();
            SuspendLayout();
            // 
            // AKButton
            // 
            AKButton.Location = new Point(14, 14);
            AKButton.Margin = new Padding(4, 3, 4, 3);
            AKButton.Name = "AKButton";
            AKButton.Size = new Size(88, 27);
            AKButton.TabIndex = 0;
            AKButton.Text = "AK-47";
            AKButton.UseVisualStyleBackColor = true;
            AKButton.Click += AKButton_Click;
            // 
            // M4Button
            // 
            M4Button.Location = new Point(108, 14);
            M4Button.Margin = new Padding(4, 3, 4, 3);
            M4Button.Name = "M4Button";
            M4Button.Size = new Size(88, 27);
            M4Button.TabIndex = 1;
            M4Button.Text = "M4A1";
            M4Button.UseVisualStyleBackColor = true;
            M4Button.Click += M4Button_Click;
            // 
            // USPButton
            // 
            USPButton.Location = new Point(203, 47);
            USPButton.Margin = new Padding(4, 3, 4, 3);
            USPButton.Name = "USPButton";
            USPButton.Size = new Size(88, 27);
            USPButton.TabIndex = 2;
            USPButton.Text = "USP-S";
            USPButton.UseVisualStyleBackColor = true;
            USPButton.Click += USPButton_Click;
            // 
            // AWPButton
            // 
            AWPButton.Location = new Point(203, 14);
            AWPButton.Margin = new Padding(4, 3, 4, 3);
            AWPButton.Name = "AWPButton";
            AWPButton.Size = new Size(88, 27);
            AWPButton.TabIndex = 7;
            AWPButton.Text = "AWP";
            AWPButton.Click += AWPButton_Click;
            // 
            // GlockButton
            // 
            GlockButton.Location = new Point(108, 47);
            GlockButton.Margin = new Padding(4, 3, 4, 3);
            GlockButton.Name = "GlockButton";
            GlockButton.Size = new Size(88, 27);
            GlockButton.TabIndex = 4;
            GlockButton.Text = "Glock";
            GlockButton.UseVisualStyleBackColor = true;
            GlockButton.Click += GlockButton_Click;
            // 
            // DeagleButton
            // 
            DeagleButton.Location = new Point(14, 47);
            DeagleButton.Margin = new Padding(4, 3, 4, 3);
            DeagleButton.Name = "DeagleButton";
            DeagleButton.Size = new Size(88, 27);
            DeagleButton.TabIndex = 3;
            DeagleButton.Text = "Deagle";
            DeagleButton.UseVisualStyleBackColor = true;
            DeagleButton.Click += DeagleButton_Click;
            // 
            // SelectedModeLabel
            // 
            SelectedModeLabel.AutoSize = true;
            SelectedModeLabel.Location = new Point(14, 81);
            SelectedModeLabel.Margin = new Padding(4, 0, 4, 0);
            SelectedModeLabel.Name = "SelectedModeLabel";
            SelectedModeLabel.Size = new Size(124, 15);
            SelectedModeLabel.TabIndex = 6;
            SelectedModeLabel.Text = "Выбран режим: STOP";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(307, 102);
            Controls.Add(SelectedModeLabel);
            Controls.Add(AWPButton);
            Controls.Add(GlockButton);
            Controls.Add(DeagleButton);
            Controls.Add(USPButton);
            Controls.Add(M4Button);
            Controls.Add(AKButton);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(323, 141);
            MinimumSize = new Size(323, 141);
            Name = "Form1";
            Text = "AiM";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button AKButton;
        private Button M4Button;
        private Button USPButton;
        private Button AWPButton;
        private Button GlockButton;
        private Button DeagleButton;
        private Label SelectedModeLabel;
        private Label fovTrackValue;
        public TrackBar fovTrackBar;
    }
}