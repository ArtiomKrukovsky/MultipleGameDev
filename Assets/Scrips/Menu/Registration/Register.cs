using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private string _connectionString;

    public InputField login;

    public InputField userName;

    public InputField password;

    public void RegisterUser()
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
                if (!login.text.Any() || !password.text.Any())
                {
                    Debug.LogWarning("Login or Password is incorrect");
                    return;
                }

                string selectLoginQuery = "SELECT Login FROM Users WHERE Login = @loginUserSelect;";
                using (SqlCommand selectLoginCommand = new SqlCommand(selectLoginQuery, dbConnection))
                {
                    selectLoginCommand.Parameters.Add("@loginUserSelect", SqlDbType.NVarChar).Value = login.text;
                    using (DbDataReader reader = selectLoginCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Debug.LogWarning("User with this login is exist!");
                            return;
                        }
                    }
                }

                if (!IsLoginValid(login.text))
                {
                    Debug.LogWarning("This login is incorrect!");
                    return;
                }

                string query = "Insert into Users (Id, Login, Name, PasswordHash)"
                               + " values (@idUser, @loginUser, @nameUser, @passwordHash) ";
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
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
                }
                dbConnection.Close();
                Debug.Log("Connection to database closed.");
            }
            catch (Exception ex)
            {
                dbConnection.Close();
                Debug.LogWarning(ex.ToString());
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
        userName.text = string.Empty;
        password.text = string.Empty;
    }

    private bool IsLoginValid(string userLogin)
    {
        bool isSuccess = true;

        bool result = userLogin.Contains("@");
        isSuccess = result;

        Regex loginRegex = new Regex("^[a-zA-Z0-9]");
        result = loginRegex.Match(userLogin).Success;

        isSuccess = result && isSuccess;

        return isSuccess;
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
