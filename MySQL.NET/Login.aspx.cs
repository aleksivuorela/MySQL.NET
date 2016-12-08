using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected static Database db;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            db = new Database();
        }
    }

    protected void LogIn_Click(object sender, EventArgs e)
    {
        string username = usernamelogin.Text;
        string password = passwordlogin.Text;
        try
        {
            if (ValidateUser(username, password))
            {
                FormsAuthentication.RedirectFromLoginPage(username, false);
            }
            else
            {
                lblMessages.Text = "Väärä tunnus tai salasana.";
            }
        }
        catch (Exception ex)
        {
            lblMessages.Text = "Väärä tunnus tai salasana.";
        }
    }

    protected bool ValidateUser(string username, string password)
    {
        string usernameRegex = @"^[a-zA-Z][a-zA-Z0-9]{1,6}$";
        if (!Regex.IsMatch(username, usernameRegex)) return false;

        DataTable dt = new DataTable();
        string checkSALT = "";
        string checkPW = "";
        try
        {
            // Fast and secure queries to db!
            dt = db.Query("SELECT password, salt FROM henkilot WHERE tunnus=@username", new string[] { "@username", username });
            checkPW = Convert.ToString(dt.Rows[0]["password"]);
            checkSALT = Convert.ToString(dt.Rows[0]["salt"]);
        }
        catch (Exception ex)
        {
            throw;
        }

        // Generates check pattern from user inputs to check if hash matches one in db
        string pw = username + checkSALT + password;
        byte[] pw_bytes = ASCIIEncoding.ASCII.GetBytes(pw);
        SHA512Managed sha512 = new SHA512Managed();
        var hashed_byte_array = sha512.ComputeHash(pw_bytes);

        if (Convert.ToBase64String(hashed_byte_array) == checkPW)
        {
            Session["LoggedUser"] = username; // Save logged user to session
            return true;
        }
        return false;
    }
}