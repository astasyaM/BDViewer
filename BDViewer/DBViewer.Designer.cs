namespace BDViewer
{
    partial class DBViewer
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
            this.tablesNames = new System.Windows.Forms.ListBox();
            this.createTableBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deleteTableBtn = new System.Windows.Forms.Button();
            this.editTableBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelEdit = new System.Windows.Forms.Panel();
            this.panelTable = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tablesNames
            // 
            this.tablesNames.FormattingEnabled = true;
            this.tablesNames.Location = new System.Drawing.Point(4, 12);
            this.tablesNames.Name = "tablesNames";
            this.tablesNames.Size = new System.Drawing.Size(197, 602);
            this.tablesNames.TabIndex = 0;
            this.tablesNames.SelectedIndexChanged += new System.EventHandler(this.tablesNames_SelectedIndexChanged);
            // 
            // createTableBtn
            // 
            this.createTableBtn.Location = new System.Drawing.Point(6, 19);
            this.createTableBtn.Name = "createTableBtn";
            this.createTableBtn.Size = new System.Drawing.Size(330, 33);
            this.createTableBtn.TabIndex = 0;
            this.createTableBtn.Text = "Создать таблицу";
            this.createTableBtn.UseVisualStyleBackColor = true;
            this.createTableBtn.Click += new System.EventHandler(this.createTableBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deleteTableBtn);
            this.groupBox1.Controls.Add(this.editTableBtn);
            this.groupBox1.Controls.Add(this.createTableBtn);
            this.groupBox1.Location = new System.Drawing.Point(760, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 139);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Действия";
            // 
            // deleteTableBtn
            // 
            this.deleteTableBtn.Location = new System.Drawing.Point(6, 97);
            this.deleteTableBtn.Name = "deleteTableBtn";
            this.deleteTableBtn.Size = new System.Drawing.Size(329, 33);
            this.deleteTableBtn.TabIndex = 2;
            this.deleteTableBtn.Text = "Удалить таблицу";
            this.deleteTableBtn.UseVisualStyleBackColor = true;
            this.deleteTableBtn.Click += new System.EventHandler(this.deleteTableBtn_Click);
            // 
            // editTableBtn
            // 
            this.editTableBtn.Location = new System.Drawing.Point(6, 58);
            this.editTableBtn.Name = "editTableBtn";
            this.editTableBtn.Size = new System.Drawing.Size(330, 33);
            this.editTableBtn.TabIndex = 1;
            this.editTableBtn.Text = "Редактировать таблицу";
            this.editTableBtn.UseVisualStyleBackColor = true;
            this.editTableBtn.Click += new System.EventHandler(this.editTableBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(207, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(547, 602);
            this.dataGridView1.TabIndex = 3;
            // 
            // panelEdit
            // 
            this.panelEdit.Location = new System.Drawing.Point(760, 234);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Size = new System.Drawing.Size(341, 340);
            this.panelEdit.TabIndex = 17;
            // 
            // panelTable
            // 
            this.panelTable.Location = new System.Drawing.Point(760, 159);
            this.panelTable.Name = "panelTable";
            this.panelTable.Size = new System.Drawing.Size(341, 69);
            this.panelTable.TabIndex = 19;
            // 
            // DBViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 618);
            this.Controls.Add(this.panelTable);
            this.Controls.Add(this.panelEdit);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tablesNames);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(1093, 657);
            this.Name = "DBViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DB Viewer";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox tablesNames;
        private System.Windows.Forms.Button createTableBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button deleteTableBtn;
        private System.Windows.Forms.Button editTableBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelEdit;
        private System.Windows.Forms.Panel panelTable;
    }
}

