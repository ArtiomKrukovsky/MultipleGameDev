using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private string _connectionString;

    public InputField login;

    public InputField password;

    public void LoginUser()
    {
        Debug.Log("Connecting to database...");

        _connectionString = @"Data Source = SQL5041.site4now.net; 
        User Id = DB_A50AD1_broadwood_admin;
        Password = qwe123ZXC.;
        Initial Catalog = DB_A50AD1_broadwood;";

        using (SqlConnection dbConnection = new SqlConnection(_connectionString))
        {
            try
            {
                dbConnection.Open();
                Debug.Log("Connected to database.");

                string query = "SELECT PasswordHash FROM Users WHERE Login = @loginUser;";

                SqlCommand command = new SqlCommand(query, dbConnection);

                command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Debug.LogWarning("User with this login not found!");
                    }

                    while (reader.Read())
                    {
                        // Index = 0 (select 1 parameter in query).
                        var passwordDbHash = reader.GetString(0);

                        //hash
                        var inputHash = HashPassword(password.text);

                        if (passwordDbHash == null || inputHash != passwordDbHash)
                        {
                            Debug.LogWarning("PasswordDbHash is incorrect");
                        }

                        Debug.Log("Login Confirmed.");
                        ResetFields();
                    }
                }
                dbConnection.Close();
                Debug.Log("Connection to database closed.");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.ToString());
                ResetFields();
                dbConnection.Close();
            }
        }
    }

    private string HashPassword(string pswd)
    {
        var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(pswd));
        var hashString = new StringBuilder();
        foreach (byte temp in hash)
        {
            hashString.AppendFormat("{0:x2}", temp);
        }

        return hashString.ToString();
    }

    private void ResetFields()
    {
        login.text = string.Empty;
        password.text = string.Empty;
    }

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
