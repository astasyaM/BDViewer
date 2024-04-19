using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;

using TextBox = System.Windows.Forms.TextBox;
using Button = System.Windows.Forms.Button;

namespace BDViewer
{
    public partial class DBViewer : Form
    {
        string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        Button saveEditBtn = new Button();
        Button saveBtn = new Button();

        public DBViewer()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            try
            {
                conn.Open();
                InitializeComponent();
                LoadTables(); // Загружаем список таблиц в базе данных
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message + "\nПроверьте строку подключения в файле конфигурации.");
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Загрузка списка таблиц из базы данных
        /// </summary>
        public void LoadTables()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Запрос для получения списка таблиц
                    var cmd = new NpgsqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader["table_name"].ToString();
                            tablesNames.Items.Add(tableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        /// <summary>
        /// Загрузка данных из выбранной таблицы
        /// </summary>
        /// <param name="tableName">Название выбранной таблицы</param>
        public void LoadDataToDataGridView(string tableName)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Запрос для выборки данных из таблицы
                    string query = $"SELECT * FROM {tableName}";

                    using (var da = new NpgsqlDataAdapter(query, conn))
                    {
                        var dt = new DataTable(); // Объект для представления данных в виде таблицы
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        /// <summary>
        /// Создание полей для ввода названия таблицы и количества полей в ней
        /// </summary>
        /// <param name="strTableName">Название таблицы</param>
        /// <param name="startAmount">Текущее количество полей в таблице</param>
        public void CreateTableBasicElements(string strTableName="", int startAmount=0)
        {
            // Удаляем с панелей элементы, если они были
            panelEdit.Controls.Clear();
            panelTable.Controls.Clear();

            Label tableName = new Label();
            tableName.Location = new Point(3, 10);
            tableName.Text = "Название таблицы:";
            panelTable.Controls.Add(tableName);

            TextBox tbTableName = new TextBox();
            tbTableName.Location = new Point(112, 3);
            tbTableName.Size = new Size(187, 20);
            tbTableName.Text = strTableName;
            panelTable.Controls.Add(tbTableName);

            Label fieldsAmount = new Label();
            fieldsAmount.Location = new Point(3, 36);
            fieldsAmount.Text = "Кол-во полей:";
            panelTable.Controls.Add(fieldsAmount);

            NumericUpDown amount = new NumericUpDown();
            amount.Location = new Point(112, 29);
            amount.Minimum = startAmount;
            amount.Maximum = 100;
            amount.Size = new Size(57, 20);
            panelTable.Controls.Add(amount);

            // Привязываем событие изменения количества полей в таблице
            amount.ValueChanged += new EventHandler(amount_ValueChanged);

            // Создаем ScrollBar
            panelEdit.AutoScroll = true;
        }

        /// <summary>
        /// Создание полей для ввода информации о полях таблицы
        /// </summary>
        /// <param name="numberOfFields">Количество полей</param>
        /// <param name="fieldNames">Названия полей</param>
        /// <param name="fieldTypes">Типы полей</param>
        /// <param name="primaryKeyFields">Ключевые поля</param>
        public void CreateInputFields(int numberOfFields, List<string> fieldNames, List<string> fieldTypes, HashSet<string> primaryKeyFields)
        {
            Label lineName = new Label();
            lineName.Location = new Point(6, 0);
            lineName.Text = "Название поля";
            panelEdit.Controls.Add(lineName);

            Label type = new Label();
            type.Location = new Point(171, 0);
            type.Text = "Тип данных";
            panelEdit.Controls.Add(type);

            Label key = new Label();
            key.Location = new Point(278, 0);
            key.Text = "Ключ";
            panelEdit.Controls.Add(key);

            for (int i = 1; i <= numberOfFields; i++)
            {
                TextBox tbTableName = new TextBox();
                tbTableName.Location = new Point(6, i * 26);
                tbTableName.Size = new Size(165, 20);
                if (fieldNames.Count>0)
                    tbTableName.Text = fieldNames[i-1];
                panelEdit.Controls.Add(tbTableName);

                DomainUpDown domain = new DomainUpDown();
                domain.Location = new Point(171, i * 26);
                domain.Size = new Size(87, 20);
                domain.Items.AddRange(new string[] { "integer", "real", "text", "date", "time", "timestamp" });
                if (fieldTypes.Count>0)
                    domain.Text = fieldTypes[i-1];
                panelEdit.Controls.Add(domain);

                CheckBox checkBox = new CheckBox();
                checkBox.Location = new Point(278, i * 26);
                checkBox.Size = new Size(15, 14);
                if (primaryKeyFields.Count>0 && primaryKeyFields.Contains(fieldNames[i - 1]))
                    checkBox.Checked = true;
                panelEdit.Controls.Add(checkBox);
            }
        }

        private void createTableBtn_Click(object sender, EventArgs e)
        {
            //Поля для ввода названия таблицы и количества полей в ней создаются динамически зразу 
            //после нажатия пользователем кнопки "создать таблицу"
            CreateTableBasicElements();

            // Кнопка для создания новой таблицы также создаётся динамически при переходе к созданию новой таблицы
            saveBtn.Location = new Point(983, 576);
            saveBtn.Size = new Size(118, 38);
            saveBtn.Text = "Сохранить";
            saveBtn.Click += new EventHandler(saveChangesBtn_Click);
            Controls.Add(saveBtn);
        }

        private void amount_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown amount = (NumericUpDown)sender;
            int numberOfFields = (int)amount.Value;

            List<string> fieldNames = new List<string>();
            List<string> fieldTypes = new List<string>();
            HashSet<string> primaryKeyFields = new HashSet<string>();
            // Поля для ввода информации о полях в таблице создаются только после того, как пользователь меняет кол-во
            // полей на число, большее ноля
            CreateInputFields(numberOfFields, fieldNames, fieldTypes, primaryKeyFields);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panelEdit.VerticalScroll.Value = e.NewValue;
        }

        private void saveChangesBtn_Click(object sender, EventArgs e)
        {
            string tableName = "";
            // Получаем название таблицы, введенное пользователем
            foreach (Control control in panelTable.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    tableName = textBox.Text;
                }
            }

            // Собираем данные о столбцах из элементов управления на panelEdit
            List<string> columnNames = new List<string>();
            List<string> columnTypes = new List<string>();
            List<string> keyColumns = new List<string>();
            int count = -1;
            foreach (Control control in panelEdit.Controls)
            {
                if (control is TextBox && control.Text!="")
                {
                    TextBox textBox = (TextBox)control;
                    columnNames.Add(textBox.Text);
                    count++;
                }
                else if (control is DomainUpDown && control.Text != "")
                {
                    DomainUpDown domainUpDown = (DomainUpDown)control;
                    columnTypes.Add(domainUpDown.Text);
                }
                else if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    if (checkBox.Checked)
                    {
                        // Помечаем столбец как ключевой, если соответствующий флажок отмечен
                        int index = count; 
                        keyColumns.Add(columnNames[index]);
                    }
                }
            }

            // Формируем SQL-запрос для создания таблицы
            string createTableQuery = $"CREATE TABLE {tableName} (";

            for (int i = 0; i < columnNames.Count; i++)
                createTableQuery += $"{columnNames[i]} {columnTypes[i]}, ";

            if (keyColumns.Count > 0)
                createTableQuery += $"PRIMARY KEY ({string.Join(", ", keyColumns)})";
            else
                createTableQuery = createTableQuery.TrimEnd(' ', ',');

            createTableQuery += ")";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                        command.ExecuteNonQuery();

                    connection.Close();
                }

                tablesNames.Items.Clear();
                LoadTables();
                MessageBox.Show($"Таблица {tableName} успешно создана!");
                panelEdit.Controls.Clear();
                panelTable.Controls.Clear();
                Controls.Remove(saveBtn);
            }
            catch (Exception ex)
            {
                if (tableName=="")
                    MessageBox.Show("Ошибка при создании таблицы: введите название таблицы!");
                else
                    MessageBox.Show("Ошибка при создании таблицы: " + ex.Message);
            }
            
        }

        private void tablesNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверяем, выбран ли какой-либо элемент в ListBox
            if (tablesNames.SelectedIndex != -1)
            {
                // Загружаем информацию о выбранной таблице
                string selectedItem = tablesNames.SelectedItem.ToString();
                LoadDataToDataGridView(selectedItem);
            }
        }

        private void deleteTableBtn_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли какой-либо элемент в ListBox
            if (tablesNames.SelectedIndex != -1)
            {
                string tableName = tablesNames.SelectedItem.ToString();

                // Удаляем выбранную таблицу из ListBox
                tablesNames.Items.Remove(tableName);

                // Удаляем выбранную таблицу из базы данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Формируем SQL-запрос на удаление таблицы
                    string deleteTableQuery = $"DROP TABLE IF EXISTS {tableName}";

                    using (NpgsqlCommand command = new NpgsqlCommand(deleteTableQuery, connection))
                        command.ExecuteNonQuery();

                    connection.Close();
                }

                dataGridView1.DataSource = null;
                MessageBox.Show($"Таблица {tableName} успешно удалена!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите таблицу для удаления.");
            }
        }

        private void editTableBtn_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли какой-либо элемент в ListBox
            if (tablesNames.SelectedIndex != -1)
            {
                panelEdit.Controls.Clear();
                panelTable.Controls.Clear();

                List<string> fieldNames = new List<string>();
                List<string> fieldTypes = new List<string>();
                HashSet<string> primaryKeyFields = new HashSet<string>();

                string tableNameOriginal = tablesNames.SelectedItem.ToString();

                // Выгружаем информацию из таблицы
                UploadTableInformation(tableNameOriginal, ref fieldNames, ref fieldTypes, ref primaryKeyFields);

                // Создаём поля с информацией о таблице для её редактирования
                CreateTableBasicElements(tableNameOriginal, fieldNames.Count());
                CreateInputFields(fieldTypes.Count, fieldNames, fieldTypes, primaryKeyFields);

                // Кнопка для создания новой таблицы также создаётся динамически при переходе к созданию новой таблицы
                saveEditBtn.Location = new Point(983, 576);
                saveEditBtn.Size = new Size(118, 38);
                saveEditBtn.Text = "Сохранить";
                saveEditBtn.Click += new EventHandler(saveEditBtn_Click);
                Controls.Add(saveEditBtn);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите таблицу для редактирования.");
            }
        }

        /// <summary>
        /// Выгрузка данных из таблицы
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="fieldNames">Список названий полей</param>
        /// <param name="fieldTypes">Список типов полей</param>
        /// <param name="primaryKeyFields">Ключевые поля</param>
        public void UploadTableInformation(string tableName, ref List<string> fieldNames, ref List<string> fieldTypes, ref HashSet<string> primaryKeyFields)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Формируем SQL-запрос для получения названий полей и их типов данных в таблице
                string query = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}' AND table_schema = 'public'";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fieldName = reader.GetString(0);
                            string fieldType = reader.GetString(1);
                            fieldNames.Add(fieldName);
                            fieldTypes.Add(fieldType);
                        }
                    }
                }

                // Формируем SQL-запрос для определения ключевых полей
                string pkQuery = $"SELECT kcu.column_name FROM information_schema.table_constraints tc JOIN information_schema.key_column_usage kcu ON tc.constraint_name = kcu.constraint_name WHERE tc.table_name = '{tableName}' AND tc.table_schema = 'public' AND tc.constraint_type = 'PRIMARY KEY'";

                using (NpgsqlCommand pkCommand = new NpgsqlCommand(pkQuery, connection))
                {
                    using (NpgsqlDataReader pkReader = pkCommand.ExecuteReader())
                    {
                        while (pkReader.Read())
                        {
                            primaryKeyFields.Add(pkReader.GetString(0));
                        }
                    }
                }

                connection.Close();
            }
        }

        private void saveEditBtn_Click(object sender, EventArgs e)
        {
            string newTableName = "";
            // Получаем название таблицы, введенное пользователем
            foreach (Control control in panelTable.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (System.Windows.Forms.TextBox)control;
                    newTableName = textBox.Text;
                }
            }

            // Список запросов
            List<string> queries = new List<string>();

            // Если название таблицы было изменено, то формируем запрос за переименование таблицы
            if (tablesNames.SelectedItem.ToString() != newTableName)
                queries.Add($"ALTER TABLE {tablesNames.SelectedItem} RENAME TO {newTableName};");

            string oldName = tablesNames.SelectedItem.ToString();

            List<string> fieldNames = new List<string>();
            List<string> fieldTypes = new List<string>();
            HashSet<string> primaryKeyFields = new HashSet<string>();
            UploadTableInformation(oldName, ref fieldNames, ref fieldTypes, ref primaryKeyFields);

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                int count = -1;
                string newName = "";
                string newType = "";
                foreach (Control control in panelEdit.Controls)
                {
                    if (control is TextBox && control.Text != "")
                    {
                        System.Windows.Forms.TextBox textBox = (TextBox)control;
                        newName = textBox.Text;
                        count++;
                    }
                    else if (control is DomainUpDown && control.Text != "")
                    {
                        DomainUpDown domainUpDown = (DomainUpDown)control;
                        newType = domainUpDown.Text;
                    }
                    else if (control is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)control;
                        bool isKey = checkBox.Checked;

                        if (count < fieldNames.Count-1 && fieldNames[count]!= newName)
                            queries.Add($"ALTER TABLE {newTableName} RENAME COLUMN {fieldNames[count]} TO {newName};");

                        // Запрос на изменение типа данных поля
                        if (count < fieldNames.Count - 1 && fieldTypes[count]!= newType)
                            queries.Add($"ALTER TABLE {newTableName} ALTER COLUMN {newName} TYPE {newType};");   

                        // Добавление или удаление ключевого поля (это более сложная операция и требует дополнительной логики)
                        // Пример добавления первичного ключа
                        if (count < fieldNames.Count - 1 && isKey && !primaryKeyFields.Contains(fieldNames[count]))
                        {
                            queries.Add($"ALTER TABLE {newTableName} ADD CONSTRAINT pk_{newName} PRIMARY KEY ({newName});");
                        }
                        else if (count < fieldNames.Count - 1 && !isKey && primaryKeyFields.Contains(fieldNames[count]))
                        {
                            string constraintName = "";
                            string pkToDelete = $"SELECT constraint_name FROM information_schema.key_column_usage WHERE table_name = '{newTableName}' AND column_name = '{newName}';";
                            using (NpgsqlCommand pkCommand = new NpgsqlCommand(pkToDelete, connection))
                            {
                                using (NpgsqlDataReader pkToDeleteReader = pkCommand.ExecuteReader())
                                {
                                    if (pkToDeleteReader.Read())
                                    {
                                        constraintName = pkToDeleteReader.GetString(0);
                                    }
                                }
                            }
                            queries.Add($"ALTER TABLE {newTableName} DROP CONSTRAINT {constraintName};");
                        }
                        else if (count>=fieldNames.Count)
                        {
                            if (!queries.Contains($"ALTER TABLE {newTableName} ADD {newName} {newType};"))
                            {
                                queries.Add($"ALTER TABLE {newTableName} ADD {newName} {newType};");
                                if (isKey)
                                {
                                    queries.Add($"ALTER TABLE {newTableName} ADD CONSTRAINT pk_{newName} PRIMARY KEY ({newName});");
                                }
                            }
                            
                        }
                    }
                }

                NpgsqlCommand command = connection.CreateCommand();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    foreach (string query in queries)
                    {
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();

                    tablesNames.Items.Clear();
                    LoadTables();
                    MessageBox.Show($"Таблица успешно изменена!");
                    panelEdit.Controls.Clear();
                    panelTable.Controls.Clear();
                    dataGridView1.DataSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении таблицы: " + ex.Message);
                    transaction.Rollback();
                }
                connection.Close();
            }
            Controls.Remove(saveEditBtn);
        }
    }
}
