using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

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
            ShowAllAnimals();

        }

        private void ShowZoos()
        {
            try
            {

                string query = "select * from Zoo";
                // the SqlAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
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

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void ShowAllAnimals()
        {
            try
            {

                string query = "select * from Animal";
                // the SqlAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    //Which information of the Table in the DataTable shoild be shown in our ListBox?
                    listAllAnimals.DisplayMemberPath = "Name";
                    //Which value should be delivered, when an Iten from our ListBox is selected?
                    listAllAnimals.SelectedValuePath = "Id";
                    //The Reference to the Data the ListBox should populate
                    listAllAnimals.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception e)
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
                // MessageBox.Show(e.ToString());
            }

        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("List of Zoos were cliked!");
            //MessageBox.Show(listZoos.SelectedValue.ToString());
            ShowAssociatedAnimals();
            ShowSelectedZooInTextBox();
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                //MessageBox.Show("Delete Zoo was succesful!");
            }
        }

        private void AddZoo_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                //MessageBox.Show("Add Zoo was succesful!");
            }
        }

        private void ShowSelectedZooInTextBox()
        {
            try
            {

                string query = "select location from zoo where Id = @ZooId";

                SqlCommand sqlCommad = new SqlCommand(query, sqlConnection);

                // the SqlAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommad);

                using (sqlDataAdapter)
                {

                    //hand over specific ID, which is based what we have selected in our list box
                    sqlCommad.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();

                    sqlDataAdapter.Fill(zooDataTable);

                    myTextBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                }

            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        } 
        
        private void ShowSelectedAnimalInTextBox()
        {
            try
            {

                string query = "select name from animal where Id = @AnimalId";

                SqlCommand sqlCommad = new SqlCommand(query, sqlConnection);

                // the SqlAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommad);

                using (sqlDataAdapter)
                {

                    //hand over specific ID, which is based what we have selected in our list box
                    sqlCommad.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();

                    sqlDataAdapter.Fill(animalDataTable);

                    myTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                }

            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        }

        private void AddAnimal_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                //MessageBox.Show("Animal was added succesful!");
            }
        }

        private void addAnimalToZoo_Cick(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
                //MessageBox.Show("Add Animal to Zoo was successful!");
            }
        }

        private void updateZoo_Cick(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo Set Location = @Location where Id = @Zooid";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                //MessageBox.Show("Add Animal to Zoo was successful!");
            }
        }
        
        private void updateAnimal_Cick(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal Set Name = @Name where Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                //MessageBox.Show("Add Animal to Zoo was successful!");
            }
        }

        private void deleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                //MessageBox.Show("Delete Animal from Table was succesful!");
            }
        }

        private void removeAnimalFromZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from ZooAnimal where AnimalId = @AnimalId and ZooId = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAssociatedAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
                //MessageBox.Show("Remove Animal from Zoo was succesful!");
            }
        }



        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }
    }
}
