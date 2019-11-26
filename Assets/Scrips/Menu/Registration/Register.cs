using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using UnityEngine;
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

        connectionstring = @"Data Source = 127.0.0.1; 
        User Id = sa;
        Password = 123456;
        Initial Catalog = MultipleGameDev;";

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
            var hash = HashPassword(password.text);
            command.Parameters.Add("@passwordHash", SqlDbType.NVarChar).Value = hash;

            // Выполнить Command (Используется для delete, insert, update).
            int rowCount = command.ExecuteNonQuery();
            Debug.Log("User added to database.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
    }

    public void ResetFields()
    {
        login.text = string.Empty;
        userName.text = string.Empty;
        password.text = string.Empty;
    }

    public static string HashPassword(string password)
    {
        byte[] salt;
        byte[] buffer2;
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }
        byte[] dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        return Convert.ToBase64String(dst);
    }

    //public static bool VerifyHashedPassword(string hashedPassword, string password)
    //{
    //    byte[] buffer4;
    //    if (hashedPassword == null)
    //    {
    //        return false;
    //    }
    //    if (password == null)
    //    {
    //        throw new ArgumentNullException("password");
    //    }
    //    byte[] src = Convert.FromBase64String(hashedPassword);
    //    if ((src.Length != 0x31) || (src[0] != 0))
    //    {
    //        return false;
    //    }
    //    byte[] dst = new byte[0x10];
    //    Buffer.BlockCopy(src, 1, dst, 0, 0x10);
    //    byte[] buffer3 = new byte[0x20];
    //    Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
    //    using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
    //    {
    //        buffer4 = bytes.GetBytes(0x20);
    //    }
    //    return ByteArraysEqual(buffer3, buffer4);
    //}

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
