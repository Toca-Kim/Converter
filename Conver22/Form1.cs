using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Conver22.Models;
using Conver22.Models.Currencies;

namespace Conver22
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Массив валют
        /// </summary>
        public List<Currency> Currencies { get; set; }

        /// <summary>
        /// Первая выбранная валюта
        /// </summary>
        public Currency SelectedCurrency1 { get; set; }

        /// <summary>
        /// Вторая выбранная валюта
        /// </summary>
        public Currency SelectedCurrency2 { get; set; }

        /// <summary>
        /// Конвертер, который забирает курс валют из интернета и создает из них массив
        /// </summary>
        private Converter _converter = new Converter();

        private BindingSource _bindingSource1 = new BindingSource();
        private BindingSource _bindingSource2 = new BindingSource();

        private List<Currency> _currencies1 = new List<Currency>();
        private List<Currency> _currencies2 = new List<Currency>();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается когда форма загружена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Забираем текущие курсы валют
                Currencies = _converter.GetCurrenciesAsync("https://www.cbr-xml-daily.ru/daily_json.js").Result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                label1.Text = "Сервер не отвечает";
                return;
            }
            
            // Добавляем рубль
            Currencies.Add(new Currency {Name = "Российский рубль", Value = 1, Nominal = 1, CharCode = "RUB"});

            // Создаем 2 доп. массива с валютами а затем привязываем их к комбо боксам
            _currencies1.AddRange(Currencies);
            _currencies2.AddRange(Currencies);

            _bindingSource1.DataSource = _currencies1;
            _bindingSource2.DataSource = _currencies2;

            comboBox1.DataSource = _bindingSource1.DataSource;
            comboBox2.DataSource = _bindingSource2.DataSource;

            //Выбираем что будет отображатся в комбо боксе ( в нашем случае символьные коды валют )
            comboBox1.DisplayMember = "CharCode";
            comboBox1.ValueMember = "CharCode";

            comboBox2.DisplayMember = "CharCode";
            comboBox2.ValueMember = "CharCode";

            SelectedCurrency1 = GetCurrency(comboBox1.Text);
            SelectedCurrency2 = GetCurrency(comboBox2.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        /// <summary>
        /// Метод, в котором происходит рассчет курса
        /// </summary>
        private void Calculate()
        {
            try
            {
                textBox2.Text = ((double.Parse(textBox1.Text) * SelectedCurrency1.Value / SelectedCurrency1.Nominal) / (SelectedCurrency2.Value / SelectedCurrency2.Nominal)).ToString();
                // количество валюты 2 = ( количество валюты 1 * значение в рублях 1 / кратность первой валюты ) / ( значение в рублях 2 / кратность второй валюты )
            }
            catch (Exception exception)
            {
                textBox2.Text = "ERROR";
                Console.WriteLine(exception.Message);
            }
        }

        #region Buttons

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text += 1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text += 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += 5;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += 6;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Text += 7;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text += 8;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text += 7;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text += 0;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox1.Text.Contains(".")) return;
            textBox1.Text += ",";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) return;
            textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
        }

        #endregion Buttons

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedCurrency1 = GetCurrency(comboBox1.Text);
            label1.Text = SelectedCurrency1 == null ? "" : SelectedCurrency1.Name; // Будем отобржать полное название валюты после ее выбора
            Calculate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedCurrency2 = GetCurrency(comboBox2.Text);
            label2.Text = SelectedCurrency2 == null ? "" : SelectedCurrency2.Name; // Будем отобржать полное название валюты после ее выбора
            Calculate();
        }

        /// <summary>
        /// Возвращает валюту по ее символьному коду
        /// </summary>
        /// <param name="currencyCharCode"></param>
        /// <returns></returns>
        private Currency GetCurrency(string currencyCharCode)
        {
            return Currencies.Find(c => c.CharCode == currencyCharCode);
        }
    }
}
