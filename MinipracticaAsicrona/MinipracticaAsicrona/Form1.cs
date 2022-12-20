using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinipracticaAsicrona
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet contenedor = new DataSet(); // Aqui se manda todos los datos del xml para el gridview 1
        DataSet contenedor2 = new DataSet();// Aqui se manda todos los datos del xml para el gridview 2
        DataTable cadenas = new DataTable(); //Se crea un campo para entrar las propiedades de un datatable de burbuja
        DataTable cadenas2 = new DataTable();//Se crea un campo para entrar las propiedades de un datatable de shell

        private void button1_Click(object sender, EventArgs e) //Boton para cargar los archivos
        {
            contenedor.ReadXml(@"C:\Users\rhlb-\source\repos\MinipracticaAsicrona\MinipracticaAsicrona\ArchivoXML\datos.xml");// Ruta donde esta el archivo xml para el contendor 1
            contenedor2.ReadXml(@"C:\Users\rhlb-\source\repos\MinipracticaAsicrona\MinipracticaAsicrona\ArchivoXML\datos.xml");// Ruta donde esta el archivo xml para el contendor 2
            MessageBox.Show("Archivos cargados");//Mensaje que ya esta cargado los archivos
        }

        private void button2_Click(object sender, EventArgs e) //Boton para que cargue los datos y se almacene en los contenedores y visualizarlo en los gridviews
        {
                dataGridView1.DataSource = (contenedor.Tables[0]);
                MessageBox.Show("Tiene " + contenedor.Tables[0].Rows.Count + " total de registros" + " para el datagridview 1");

                dataGridView2.DataSource = (contenedor2.Tables[0]);
                MessageBox.Show("Tiene " + contenedor2.Tables[0].Rows.Count + " total de registros" + " para el datagridview 2");
        }

        private void button3_Click(object sender, EventArgs e) //Boton para ejecutar el metodo burbuja de forma ascedente
        {
            button3.Enabled = false;
            Thread hilo = new Thread(delegate () //Se crea un hilo para trabajar forma asicrona para el metodo burbuja
            {
                burbujaAsc();
                if (dataGridView1.InvokeRequired) 
                {
                    dataGridView1.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("Termino el procedimiento burbuja");
                    }));
                }
                if (button3.InvokeRequired)
                {
                    button3.Invoke(new MethodInvoker(delegate
                    {
                        button3.Enabled = true;
                    }
                    ));
                }
            }
            );
            hilo.Start();
        }

        private void button4_Click(object sender, EventArgs e) //Boton para ejecutar el metodo shell de forma ascendente
        {
            button4.Enabled = false;
           
            Thread hilo = new Thread(delegate ()//Se crea un hilo para trabajar forma asicrona para el metodo shell
            {
                Shell();
                if (dataGridView2.InvokeRequired)
                {
                    dataGridView2.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("Termino el procedimiento Shell");
                    }));
                }
                if (button4.InvokeRequired)
                {
                    button4.Invoke(new MethodInvoker(delegate
                    {
                        button4.Enabled = true;
                    }
                    ));
                }
            }
                );
            hilo.Start();
        }


        public void burbujaAsc() //Metodo burbuja 
        {
            cadenas = contenedor.Tables[0];//se pone el campo "cadenas" para que agarra los datos del contenedor
            int i = 0, indexer = 0;
            for (i = 0; i < cadenas.Rows.Count; i++)
            {
                for (indexer = 0; indexer < cadenas.Rows.Count - 1; indexer++)
                {
                    int indprox = indexer + 1;
                    if ((cadenas.Rows[indexer]["descripcion"].ToString().Trim()).CompareTo(cadenas.Rows[indprox]["descripcion"].ToString().Trim()) > 0)
                    {
                        string desc = cadenas.Rows[indexer]["descripcion"].ToString();
                        string sat = cadenas.Rows[indexer]["sat_unimed"].ToString();
                        string sim = cadenas.Rows[indexer]["simbolo"].ToString();
                        cadenas.Rows[indexer]["descripcion"] = cadenas.Rows[indprox]["descripcion"];
                        cadenas.Rows[indexer]["sat_unimed"] = cadenas.Rows[indprox]["sat_unimed"];
                        cadenas.Rows[indexer]["simbolo"] = cadenas.Rows[indprox]["simbolo"];
                        cadenas.Rows[indprox]["descripcion"] = desc;
                        cadenas.Rows[indprox]["sat_unimed"] = sat;
                        cadenas.Rows[indprox]["simbolo"] = sim;
                    }
                }

            }

            dataGridView1.DataSource = cadenas;

        }

        public void Shell() //Metodo Shell
        {
            cadenas2 = contenedor2.Tables[0];//se pone el campo "cadenas2" para que agarra los datos del contenedor2
            int pum = 0;
            int x = 0;

            int e = 0;
            pum = cadenas2.Rows.Count;
            while (pum > 0)
            {
                x = 1;
                while (x != 0)
                {
                    x = 0;
                    e = 1;
                    while (e <= (cadenas2.Rows.Count - pum))
                    {
                        if ((cadenas2.Rows[e - 1]["descripcion"].ToString().Trim().CompareTo((cadenas2.Rows[e - 1 + pum]["descripcion"].ToString().Trim())) > 0))
                        {
                            string aux = cadenas2.Rows[(e - 1) + pum]["descripcion"].ToString();
                            string auxi1 = cadenas2.Rows[(e - 1) + pum]["sat_unimed"].ToString();
                            string auxi2 = cadenas2.Rows[(e - 1) + pum]["simbolo"].ToString();
                            cadenas2.Rows[(e - 1) + pum]["descripcion"] = cadenas2.Rows[e - 1]["descripcion"].ToString();
                            cadenas2.Rows[(e - 1) + pum]["sat_unimed"] = cadenas2.Rows[e - 1]["sat_unimed"].ToString();
                            cadenas2.Rows[(e - 1) + pum]["simbolo"] = cadenas2.Rows[e - 1]["simbolo"].ToString();
                            cadenas2.Rows[(e - 1)]["descripcion"] = aux;
                            cadenas2.Rows[(e - 1)]["sat_unimed"] = auxi1;
                            cadenas2.Rows[(e - 1)]["simbolo"] = auxi2;
                            x = 1;
                        }
                        e++;
                    }
                }
                pum = pum / 2;
            }
        }

        private void button5_Click(object sender, EventArgs e) //Manera de ordenar el gridview con el metodo Sort
        {
            if(button3.Enabled == false)
            {
                MessageBox.Show("Esperar el proceso asicrono de burbuja que acabe");
            }
            else
            {
                dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
             
            }
           
        }

        private void button6_Click(object sender, EventArgs e)//Manera de ordenar el gridview con el metodo Sort
        {
            
            if (button3.Enabled == false)
            {
                MessageBox.Show("Esperar el proceso asicrono de burbuja que acabe");
            }
            else
            {
                
                dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Descending);
                
            }
       
        }

        private void button7_Click(object sender, EventArgs e)//Manera de ordenar el gridview con el metodo Sort
        {
            if (button4.Enabled == false)
            {
                MessageBox.Show("Esperar el proceso asicrono de shell que acabe");
            }
            else
            {
                dataGridView2.Sort(dataGridView2.Columns[1], ListSortDirection.Ascending);
            }
        }

        private void button8_Click(object sender, EventArgs e)//Manera de ordenar el gridview con el metodo Sort
        {
            if (button4.Enabled == false)
            {
                MessageBox.Show("Esperar el proceso asicrono de shell que acabe");
            }
            else
            {
                dataGridView2.Sort(dataGridView2.Columns[1], ListSortDirection.Descending);
            }
        }

        
    }
}
