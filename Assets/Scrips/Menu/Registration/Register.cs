using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private string connectionstring;
    private string query;

    public InputField login;

    public InputField userName;

    public InputField password;

    public void RegisterUser()
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

            query = "Insert into Users (Id, Login, Name, PasswordHash)"
                    + " values (@idUser, @loginUser, @nameUser, @passwordHash) ";

            SqlCommand command = new SqlCommand(query, dbConnection);

            //===== Добавить параметр @Id =====
            command.Parameters.Add("@idUser", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();

            //===== Добавить параметр @login =====
            command.Parameters.Add("@loginUser", SqlDbType.NVarChar).Value = login.text;

            //===== Добавить параметр @name =====
            command.Parameters.Add("@nameUser", SqlDbType.NVarChar).Value = userName.text;

            //===== Добавить параметр @passwordHash =====
            var inputHash = HashPassword(password.text);
            command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = inputHash;

            // Выполнить Command (Используется для delete, insert, update).
            int rowCount = command.ExecuteNonQuery();
            Debug.Log("User added to database.");

            SceneManager.LoadScene("Login");
            Debug.Log("Redirect to scene Login.");

            ResetFields();

            dbConnection.Close();
            Debug.Log("Connection to database closed.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.ToString());
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
        userName.text = string.Empty;
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
