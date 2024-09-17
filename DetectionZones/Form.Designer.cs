namespace DetectionZones
{
    partial class Ui
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox images;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ui));
            this.imagesBox = new System.Windows.Forms.ComboBox();
            this.searchNumber = new System.Windows.Forms.GroupBox();
            this.numberBox = new System.Windows.Forms.TextBox();
            this.datetime = new System.Windows.Forms.GroupBox();
            this.carsBox = new System.Windows.Forms.ComboBox();
            this.folder = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.search = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            images = new System.Windows.Forms.GroupBox();
            images.SuspendLayout();
            this.searchNumber.SuspendLayout();
            this.datetime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // images
            // 
            images.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            images.Controls.Add(this.imagesBox);
            images.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            images.ForeColor = System.Drawing.Color.White;
            images.Location = new System.Drawing.Point(781, -3);
            images.Name = "images";
            images.Size = new System.Drawing.Size(370, 86);
            images.TabIndex = 1;
            images.TabStop = false;
            images.Text = "Images";
            // 
            // imagesBox
            // 
            this.imagesBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imagesBox.FormattingEnabled = true;
            this.imagesBox.Location = new System.Drawing.Point(6, 31);
            this.imagesBox.Name = "imagesBox";
            this.imagesBox.Size = new System.Drawing.Size(358, 32);
            this.imagesBox.TabIndex = 2;
            this.imagesBox.SelectedIndexChanged += new System.EventHandler(this.imagesBox_SelectedIndexChanged);
            // 
            // searchNumber
            // 
            this.searchNumber.Controls.Add(this.search);
            this.searchNumber.Controls.Add(this.numberBox);
            this.searchNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchNumber.ForeColor = System.Drawing.Color.White;
            this.searchNumber.Location = new System.Drawing.Point(6, -3);
            this.searchNumber.Name = "searchNumber";
            this.searchNumber.Size = new System.Drawing.Size(308, 86);
            this.searchNumber.TabIndex = 3;
            this.searchNumber.TabStop = false;
            this.searchNumber.Text = "Number";
            // 
            // numberBox
            // 
            this.numberBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numberBox.Location = new System.Drawing.Point(9, 23);
            this.numberBox.Name = "numberBox";
            this.numberBox.Size = new System.Drawing.Size(224, 49);
            this.numberBox.TabIndex = 5;
            this.numberBox.Text = "X888XX888";
            this.numberBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numberBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numberBox_KeyDown);
            // 
            // datetime
            // 
            this.datetime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datetime.Controls.Add(this.carsBox);
            this.datetime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.datetime.ForeColor = System.Drawing.Color.White;
            this.datetime.Location = new System.Drawing.Point(321, -3);
            this.datetime.Name = "datetime";
            this.datetime.Size = new System.Drawing.Size(454, 86);
            this.datetime.TabIndex = 6;
            this.datetime.TabStop = false;
            this.datetime.Text = "Date and Time";
            // 
            // carsBox
            // 
            this.carsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.carsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carsBox.FormattingEnabled = true;
            this.carsBox.Location = new System.Drawing.Point(6, 32);
            this.carsBox.Name = "carsBox";
            this.carsBox.Size = new System.Drawing.Size(442, 32);
            this.carsBox.TabIndex = 7;
            this.carsBox.SelectedIndexChanged += new System.EventHandler(this.dateAndTimeBox_SelectedIndexChanged);
            // 
            // folder
            // 
            this.folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.folder.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.folder.BackgroundImage = global::DetectionZones.Properties.Resources.folder;
            this.folder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.folder.FlatAppearance.BorderSize = 0;
            this.folder.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.folder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.folder.Location = new System.Drawing.Point(1226, 12);
            this.folder.Name = "folder";
            this.folder.Size = new System.Drawing.Size(60, 60);
            this.folder.TabIndex = 8;
            this.folder.UseVisualStyleBackColor = false;
            this.folder.Click += new System.EventHandler(this.folder_Click);
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.save.BackgroundImage = global::DetectionZones.Properties.Resources.save;
            this.save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.save.FlatAppearance.BorderSize = 0;
            this.save.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save.Location = new System.Drawing.Point(1160, 12);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(60, 60);
            this.save.TabIndex = 9;
            this.save.UseVisualStyleBackColor = false;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // imageBox
            // 
            this.imageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.imageBox.Image = ((System.Drawing.Image)(resources.GetObject("imageBox.Image")));
            this.imageBox.InitialImage = null;
            this.imageBox.Location = new System.Drawing.Point(6, 88);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(1280, 720);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox.TabIndex = 10;
            this.imageBox.TabStop = false;
            this.imageBox.DoubleClick += new System.EventHandler(this.imageBox_DoubleClick);
            // 
            // search
            // 
            this.search.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.search.BackgroundImage = global::DetectionZones.Properties.Resources.search;
            this.search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.search.FlatAppearance.BorderSize = 0;
            this.search.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.search.Location = new System.Drawing.Point(240, 18);
            this.search.Margin = new System.Windows.Forms.Padding(5);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(60, 60);
            this.search.TabIndex = 4;
            this.search.UseVisualStyleBackColor = false;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // Ui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1292, 816);
            this.Controls.Add(this.folder);
            this.Controls.Add(this.save);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(images);
            this.Controls.Add(this.datetime);
            this.Controls.Add(this.searchNumber);
            this.MinimumSize = new System.Drawing.Size(1308, 855);
            this.Name = "Ui";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form";
            this.Load += new System.EventHandler(this.Ui_Load);
            images.ResumeLayout(false);
            this.searchNumber.ResumeLayout(false);
            this.searchNumber.PerformLayout();
            this.datetime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox searchNumber;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.TextBox numberBox;
        private System.Windows.Forms.GroupBox datetime;
        private System.Windows.Forms.ComboBox carsBox;
        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.ComboBox imagesBox;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button folder;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

