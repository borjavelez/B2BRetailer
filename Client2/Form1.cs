using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingGateway;
using Models;

namespace Client2
{
    public partial class Form1 : Form
    {
        SynchronousMessagingGateway gateway;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            comboBox1.SelectedIndex = 0;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            gateway = new SynchronousMessagingGateway(customer);
            int input = 0;
            try
            {
                input = Int32.Parse(textBox1.Text);
                if (input >= 1 && input <= 4)
                {
                    label7.Visible = false;
                    String country = comboBox1.Text;
                    int productId = Int32.Parse(textBox1.Text);
                    Order order = gateway.send(productId, country, customer);
                    label3.Text = "From warehouse: " + order.Warehouse;
                    label4.Text = "Delivery days: " + order.DeliveryDays;
                    label5.Text = "Shipping cost: " + order.ShippingCost + "€";
                    label6.Text = "Total cost: " + order.TotalCost.ToString() + "€";
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    //gateway.deleteQueues();
                } else
                {
                    throw new Exception();
                }
            } catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                label7.Visible = true;
                label7.Text = "Error! \nThe product ID must be a number between 1 and 4";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gateway.Close();
        }
    }
}
