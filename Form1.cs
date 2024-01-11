using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ado.netCourse
{
    public partial class Form1 : Form
    {
        //connection string
        string ConStr = ConfigurationManager.ConnectionStrings["webc"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            PopulateCities();
            PopulateEmployeeStatus();
            GetCountofEmployees();
            FillDataGridview();
        }
        //Execure Reader
        private void PopulateCities()
        {
            SqlConnection Con = new SqlConnection(ConStr);
            try
            {
               
                List<string> listofCities = new List<string>();
                
                
                string getcitiesSql = "select * from City";
                SqlCommand cmd = new SqlCommand(getcitiesSql, Con);
                Con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listofCities.Add(reader.GetString(1));

                }
                comboCity.DataSource = listofCities;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                Con.Close();
            }

        }
        //Execure Reader
        private void PopulateEmployeeStatus()
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                List<string> listofEmployeeStatus = new List<string>();
               
                string getIsactiveSql = "Select * from EmployeeStatus";
                SqlCommand cmd = new SqlCommand(getIsactiveSql, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listofEmployeeStatus.Add(reader.GetString(1));
                }
                comboIsactive.DataSource = listofEmployeeStatus;


            }
            catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        //Execure Scalar
        private void GetCountofEmployees()
        {
        SqlConnection con = new SqlConnection (ConStr);
            try
            {
                string getEmployeeCountSql = "select count (*) As 'Total Employees' from Employees";
                SqlCommand cmd = new SqlCommand(getEmployeeCountSql, con);
                con.Open();
                int EmployeeCount = Convert.ToInt32(cmd.ExecuteScalar());
                if(EmployeeCount>0)
                {
                    label6.Text = "Total number of Employees at present is " + EmployeeCount;
                }
                else
                {
                    label6.Text = "No Employees Hired yet!";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        //Dataset and DataAdaptor
        private void FillDataGridview()
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                string getAllEmployeeSql = "Select * from Employees";
                SqlDataAdapter da = new SqlDataAdapter(getAllEmployeeSql, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            }
            catch(Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
            finally
            {
                con.Close();
            }


        }

        //Execute Reader
        private void btnSelect_Click(object sender, EventArgs e)
        {
           
            //connection
            SqlConnection Con = new SqlConnection(ConStr);
            
            try
            {
                if (!string.IsNullOrEmpty(textId.Text))
                {
                    string SqlQuery = "Select * from Employees where Id=@Id";

                    //command
                    SqlCommand cmd = new SqlCommand(SqlQuery, Con);
                    cmd.Parameters.AddWithValue("@Id", textId.Text);
                    Con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        txtFirstName.Text = reader.GetString(1);
                        textlastname.Text = reader.GetString(2);
                        txtGender.Text = reader.GetString(3);
                        comboCity.Text = reader.GetString(4);
                        comboIsactive.Text = reader.GetString(5);


                    }
                }
                else
                {
                    MessageBox.Show("Employeed Id cannot be left blank");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Con.Close();
            }

        }
        //Execute Non Query

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                //string addEmployeesSql = @"INSERT INTO [dbo].[Employees]VALUES('" + txtFirstName.Text + "','" + textlastname.Text + "','" + txtGender.Text + "','" + comboCity.SelectedValue + "','" + comboIsactive.SelectedValue + "')";
                String addEmployeesSql = @"INSERT INTO [dbo].[Employees] VALUES(@FirstName,@LastName,@Gender,@City,@IsActive)";
                SqlCommand cmd = new SqlCommand(addEmployeesSql, con);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", textlastname.Text);
                cmd.Parameters.AddWithValue("@Gender", txtGender.Text);
                cmd.Parameters.AddWithValue("@City", comboCity.SelectedValue);
                cmd.Parameters.AddWithValue("@IsActive",comboIsactive.SelectedValue);
                con.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Record inserted Successfully");

                }
                else
                {
                    MessageBox.Show("Record was not Inserted");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

        }

        private void btnupdateEmployee_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                string updateEmpSql = @"update[dbo].[Employees] SET [FirstName]=@FirstName,
                                                                    [LastName]=@LastName,
                                                                      [Gender]=@Gender,
                                                                      [City]=@City,
                                                                       [IsActive]=@IsActive where Id=@Id";
                SqlCommand cmd = new SqlCommand(updateEmpSql, con);
                cmd.Parameters.AddWithValue("@Id", textId.Text);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", textlastname.Text);
                cmd.Parameters.AddWithValue("@Gender", txtGender.Text);
                cmd.Parameters.AddWithValue("@City", comboCity.SelectedValue);
                cmd.Parameters.AddWithValue("@IsActive",comboIsactive.SelectedValue);
                con.Open();

                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Record update succesfully");
                    FillDataGridview();
                }
                else
                {
                    MessageBox.Show("Record was not updated Successfully");
                }

            }
            catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();


            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConStr);
            try
            {
                string deleteEmpSql = @"Delete From [dbo].[Employees] where Id=@Id";
                SqlCommand cmd = new SqlCommand(deleteEmpSql, con);
                cmd.Parameters.AddWithValue("@Id", textId.Text);
               
                con.Open();

                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    MessageBox.Show("Record Delete succesfully");
                    FillDataGridview();
                }
                else
                {
                    MessageBox.Show("Record was not Deleted Successfully");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();


            }
        }


    }
}

