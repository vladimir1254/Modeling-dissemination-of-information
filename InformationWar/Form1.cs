using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using InformationWar.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace InformationWar
{
    public partial class Form1 : Form
    {
        Model model,model_example1,model_example2,model_exercise;
        bool rus = true;
        private void TabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверка, что выбрана именно tabPage6
            if (tabControl3.SelectedTab == tabPage6)
            {
                model_example1.N1 = 20000;
                model_example1.N2 = 30000;
                model_example1.a = 1;
                model_example1.b = 1;
                model_example1.d = 0.5;
                model_example1.y = 0.5;
                model_example1.f1 = "1/x**8+500";
                model_example1.f2 = "1/x**8+1";
                model_example1.calc();
                chart_output(model_example1, chart5);
              //  MessageBox.Show("Вы перешли на вкладку tabPage6!");
            }
            // Проверка, что выбрана именно tabPage6
            if (tabControl3.SelectedTab == tabPage7)
            {
                model_example2.N1 = 20000;
                model_example2.N2 = 30000;
                model_example2.a = 1;
                model_example2.b = 1;
                model_example2.d = 0.5;
                model_example2.y = 0.5;
                model_example2.f1 = "0.01";
                model_example2.f2 = "25000";
                model_example2.calc();
                chart_output(model_example2, chart3);
                //  MessageBox.Show("Вы перешли на вкладку tabPage6!");
            }
            if (tabControl3.SelectedTab == tabPage8)
            {
                model_exercise.N1 = 20000;
                model_exercise.N2 = 30000;
                model_exercise.a = 1;
                model_exercise.b = 1;
                model_exercise.d = 0.5;
                model_exercise.y = 0.5;
                model_exercise.f1 = "0.01";
                model_exercise.f2 = "25000";
                model_exercise.calc();
                chart_output(model_exercise, chart4);
                //  MessageBox.Show("Вы перешли на вкладку tabPage6!");
            }
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверка, что выбрана именно tabPage6
            if (tabControl1.SelectedTab == tabPage3)
            {
                model_example1.N1 = 20000;
                model_example1.N2 = 30000;
                model_example1.a = 1;
                model_example1.b = 1;
                model_example1.d = 0.5;
                model_example1.y = 0.5;
                model_example1.f1 = "1/x**8+500";

                model_example1.f2 = "1/x**8+1";
                model_example1.calc();
                chart_output(model_example1, chart5);
            }
        }
        public Form1()
        {
            model = new Model();
            model_example1 = new Model();
            model_example2 = new Model();
            model_exercise = new Model();
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
            InitializeComponent();
            tabControl3.SelectedIndexChanged += TabControl3_SelectedIndexChanged;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;


            textBox1.Text = trackBar1.Value.ToString();
            textBox2.Text = trackBar2.Value.ToString();
            textBox3.Text = trackBar3.Value.ToString();

            textBox6.Text = TB_d.Value.ToString();
            textBox5.Text = TB_v.Value.ToString();
            textBox4.Text = tb_betta.Value.ToString();

            pictureBox1.Image = Image.FromFile("Formula.JPG");


        }
        public void calc_graph()
        {

            try
            {
                model.N1 = Convert.ToDouble(double.Parse(TB_d.Value.ToString()));
                model.N2 = Convert.ToDouble(double.Parse(TB_v.Value.ToString()));
                model.a = Convert.ToDouble(double.Parse(tb_betta.Value.ToString()))/100;
                model.b = Convert.ToDouble(double.Parse(trackBar1.Value.ToString()))/100;
                model.d = Convert.ToDouble(double.Parse(trackBar2.Value.ToString()))/100;
                model.y = Convert.ToDouble(double.Parse(trackBar3.Value.ToString()))/100;
                model.f1 = string.IsNullOrEmpty(textBox7.Text) ? "x" : textBox7.Text;
                model.f2 = string.IsNullOrEmpty(textBox8.Text) ? "x" : textBox8.Text;

            }
            catch (Exception ex)
            {
                if (rus) MessageBox.Show("Неверный формат входных данных", "Ошибка");
                else MessageBox.Show("Wrong data format", "Error");
                return;
            }
            if (model.wrong_number())
            {
                if (rus) MessageBox.Show("Неверные значения входных данных", "Ошибка");
                else MessageBox.Show("Wrong values", "Error");
                return;
            }
            model.calc();
            chart_output(model, chart2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            calc_graph();
        }
        public void chart_output(Model model, Chart chart)
        {
            chart.Series[0].Points.Clear();
            chart.Series[1].Points.Clear();
            chart.Series[2].Points.Clear();
            chart.Series[3].Points.Clear();
            chart.Palette = ChartColorPalette.Berry; //SeaGreen

            chart.Legends[0].Enabled = true;
            chart.Series[0].BorderWidth = 3;
            chart.Series[0].LegendText = "X1(t)";
            chart.Series[1].BorderWidth = 3;
            chart.Series[1].LegendText = "X2(t)";
            chart.Series[2].BorderWidth = 3;
            chart.Series[2].LegendText = "x1(t)";
            chart.Series[3].BorderWidth = 3;
            chart.Series[3].LegendText = "x2(t)";

            Axis ay = new Axis();
            if (rus) ay.Title = "число «адептов», принявших информацию";
            else ay.Title = "number of adherents";
            chart.ChartAreas[0].AxisY = ay;

            for (int i = 0; i < model.X1.Count; i++)
            {
                chart.Series[0].Points.AddXY(i * model.h, model.X1[i]);
                chart.Series[1].Points.AddXY(i * model.h, model.X2[i]);
                chart.Series[2].Points.AddXY(i * model.h, model.x1[i]);
                chart.Series[3].Points.AddXY(i * model.h, model.x2[i]);
            }
        }

        

        #region примеры
        public void example1_func()
        {
            /*
            Model example1 = new Model();

            example1.n00 = 20000;
            example1.n10 = 0;
            example1.n20 = 0;
            example1.alpha1 = 1;
            example1.alpha2 = 0.1;
            example1.beta1 = 0.0036;
            example1.beta2 = 0.0032;

            example1.euler_n1_list();

            chart_output(example1, chart3);
            */
        }
        public void example2_func()
        {
            /*
            Model example2 = new Model();

            example2.n00 = 20000;
            example2.n10 = 0;
            example2.n20 = 0;
            example2.alpha1 = 1;
            example2.alpha2 = 0.1;
            example2.beta1 = 0.002;
            example2.beta2 = 0.0045;

            example2.euler_n1_list();

            chart_output(example2, chart4);
            */
        }
        #endregion

        private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            resources.ApplyResources(this, "$this");
            applyResources(resources, this.Controls);
            rus = true;
            языкToolStripMenuItem.Text = "Язык";
            русскийToolStripMenuItem.Text = "Русский";
            englishToolStripMenuItem.Text = "English";
            оПрограммеToolStripMenuItem.Text = "О программе";
            this.Text = "Распространение информации в социуме";
            example1_func();
            example2_func();
        }
        private void applyResources(ComponentResourceManager resources, Control.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                resources.ApplyResources(ctl, ctl.Name);
                applyResources(resources, ctl.Controls);
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            resources.ApplyResources(this, "$this");
            applyResources(resources, this.Controls);
            rus = false;
            языкToolStripMenuItem.Text = "Language";
            русскийToolStripMenuItem.Text = "Русский";
            englishToolStripMenuItem.Text = "English";
            оПрограммеToolStripMenuItem.Text = "About program";
            this.Text = "Dissemination of information in society";
            example1_func();
            example2_func();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1(System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
            aboutBox.Show();
        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = trackBar3.Value.ToString();
            calc_graph();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
            calc_graph();


        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                trackBar1.Value = int.Parse(textBox1.Text);
               
            }
            catch
            {

                Message();
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar2.Value = int.Parse(textBox2.Text);

            }
            catch
            {
                Message();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar3.Value = int.Parse(textBox3.Text);

            }
            catch
            {
                Message();
            }
        }
        private void Message()
        {
            string error = "Error";
            string descr = "Incorrect value";
            if (rus)
            {
                error = "Ошибка";
                descr = "Некорректное значение";
            }
            MessageBox.Show(descr, error);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TB_d.Value = int.Parse(textBox6.Text);

            }
            catch
            {
                Message();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TB_v.Value = int.Parse(textBox5.Text);

            }
            catch
            {
                Message();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                tb_betta.Value = int.Parse(textBox4.Text);

            }
            catch
            {
                Message();
            }
        }

        private void TB_d_Scroll(object sender, EventArgs e)
        {
            textBox6.Text = TB_d.Value.ToString();

            calc_graph();

        }

        private void TB_v_Scroll(object sender, EventArgs e)
        {
            textBox5.Text = TB_v.Value.ToString();
            calc_graph();
        }

        private void tb_betta_Scroll(object sender, EventArgs e)
        {
            textBox4.Text = tb_betta.Value.ToString();
            calc_graph();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = trackBar2.Value.ToString();
            calc_graph();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void языкToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void TabPage6_Click(object sender, EventArgs e)
        {
            // Обработчик для первой вкладки
            MessageBox.Show("Вы кликнули на первой вкладке!");
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void betta_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void a_Click(object sender, EventArgs e)
        {

        }

        private void n_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }


        public void calc_graph2()
        {

            try
            {
                model_exercise.N1 = 20000;
                model_exercise.N2 = 30000;
                model_exercise.a = Convert.ToDouble(double.Parse(trackBar7.Value.ToString())) / 100;
                model_exercise.b = Convert.ToDouble(double.Parse(trackBar6.Value.ToString())) / 100;
                model_exercise.d = Convert.ToDouble(double.Parse(trackBar5.Value.ToString())) / 100;
                model_exercise.y = Convert.ToDouble(double.Parse(trackBar4.Value.ToString())) / 100;
                model_exercise.f1 = string.IsNullOrEmpty(textBox14.Text) ? "x" : textBox14.Text;
                model_exercise.f2 = string.IsNullOrEmpty(textBox9.Text) ? "x" : textBox9.Text;

            }
            catch (Exception ex)
            {
                if (rus) MessageBox.Show("Неверный формат входных данных", "Ошибка");
                else MessageBox.Show("Wrong data format", "Error");
                return;
            }
            if (model_exercise.wrong_number())
            {
                if (rus) MessageBox.Show("Неверные значения входных данных", "Ошибка");
                else MessageBox.Show("Wrong values", "Error");
                return;
            }
            model_exercise.calc();
            chart_output(model_exercise, chart4);
            if (model_exercise.X1[model_exercise.X1.Count-1] >1 && model_exercise.X2[model_exercise.X2.Count - 1] >1 
                && model_exercise.x1[model_exercise.x1.Count - 1] > 1 && model_exercise.x2[model_exercise.x2.Count - 1] > 1)
            {
                if (rus)
                {
                    label54.Text = "Упражнение решено!";
               }
                else
                {
                    label54.Text = "Exercise solved!";
                }
            }
            else
            {
                if (rus)
                {
                    label54.Text = "Упражнение не решено, попробуйте изменить параметры.";
               }
                else
                {
                    label54.Text = "The exercise is not solved, try changing the parameters.";
                }
            }
        }
        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            textBox10.Text = trackBar7.Value.ToString();

            calc_graph2();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            textBox13.Text = trackBar6.Value.ToString();

            calc_graph2();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            textBox12.Text = trackBar5.Value.ToString();

            calc_graph2();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            textBox11.Text = trackBar4.Value.ToString();

            calc_graph2();
        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }
    }
}
