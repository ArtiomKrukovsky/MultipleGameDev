using System.Data.SqlClient;
using UnityEngine;

public class Connection : MonoBehaviour
{
    private string connectionstring;


    // Use this for initialization
    void Start()
    {

        Debug.Log("Connecting to database...");
        connectionstring = @"Data Source = 127.0.0.1; 
     user id = sa;
     password = saify1234;
     Initial Catalog = login;";



        SqlConnection dbConnection = new SqlConnection(connectionstring);


        try
        {

            dbConnection.Open();
            Debug.Log("Connected to database.");
        }
        catch (SqlException _exception)
        {
            Debug.LogWarning(_exception.ToString());

        }


        //  conn.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
