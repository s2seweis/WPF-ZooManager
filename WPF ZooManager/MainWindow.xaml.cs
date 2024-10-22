using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_ZooManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.ZooDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            ShowZoos();

        }

        private void ShowZoos()
        {
            try
            {

            string query = "select * from Zoo";
            // the SqlAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            using(sqlDataAdapter)
            {
                DataTable zooTable = new DataTable();

                sqlDataAdapter.Fill(zooTable);

                //Which information of the Table in the DataTable shoild be shown in our ListBox?
                listZoos.DisplayMemberPath = "Location";
                //Which value should be delivered, when an Iten from our ListBox is selected?
                listZoos.SelectedValuePath = "Id";
                //The Reference to the Data the ListBox should populate
                listZoos.ItemsSource = zooTable.DefaultView;
            }

            }catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ShowAssociatedAnimals()
        {
            try
            {

                string query = "select * from Animal a inner join ZooAnimal " +
                    "za on a.Id = za.animalId where za.ZooId = @ZooId"; 
                
                SqlCommand sqlCommad = new SqlCommand(query, sqlConnection);

                // the SqlAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommad);

                using (sqlDataAdapter)
                {

                    //hand over specific ID, which is based what we have selected in our list box
                    sqlCommad.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    //Which information of the Table in the DataTable shoild be shown in our ListBox?
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    //Which value should be delivered, when an Iten from our ListBox is selected?
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("List of Zoos were cliked!");
            //MessageBox.Show(listZoos.SelectedValue.ToString());
            ShowAssociatedAnimals();
        }
    }
}
