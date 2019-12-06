using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private string connectionstring;
    private string query;
    private string passwordDbHash;

    public InputField login;

    public InputField password;

    public void LoginUser()
    {
        Debug.Log("Connecting to database...");

        connectionstring = @"Data Source = SQL5041.site4now.net; 
        User Id = DB_A50AD1_broadwood_admin;
        Password = qwe123ZXC.;
        Initial Catalog = DB_A50AD1_broadwood;";

        SqlConnection dbConnection = new SqlConnection(connectionstring);

        try
        {

            dbConnection.Open();
            Debug.Log("Connected to database.");

            query = "SELECT PasswordHash FROM Users WHERE Login = @loginUser;";

            SqlCommand command = new SqlCommand(query, dbConnection);

            command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Index = 0 (select 1 parameter in query).
                        passwordDbHash = reader.GetString(0);

                        //hash
                        var inputHash = HashPassword(password.text);

                        if (passwordDbHash != null)
                        {
                            if (inputHash == passwordDbHash)
                            {
                                Debug.Log("Login Confirmed.");
                                ResetFields();
                            }
                            else
                            {
                                Debug.LogWarning("Login Not Confirmed.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("PasswordDbHash is null");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("User with this login not found!");
                }
            }

            dbConnection.Close();
            Debug.Log("Connection to database closed.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.ToString());
            ResetFields();
        }
    }

    public string HashPassword(string pswd)
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

    public void ResetFields()
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
