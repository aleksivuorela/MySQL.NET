using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected static Database db;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            db = new Database();
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;
        string repassword = txtRepassword.Text;
        try
        {
            if (CreateUser(username, password, repassword))
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            lblMessages.Text = "Rekisteröityminen epäonnistui.";
        }
    }
 
    protected bool CreateUser(string username, string password, string repassword)
    {       
        string usernameRegex = @"^[a-zA-Z][a-zA-Z0-9]{1,6}$";
        DataTable dt = new DataTable();

        // Check that username is valid and passwords match
        if (Regex.IsMatch(username, usernameRegex) && password == repassword)
        {
            try
            {
                // Check if username exists already
                dt = db.Query("SELECT * FROM henkilot WHERE tunnus=@username", new string[] { "@username", username });
                if (dt.Rows.Count > 0)
                {
                    lblMessages.Text = "Tunnus on jo olemassa.";
                }
                // Create new user if username is not taken
                else
                {
                    return AddUserToDB(username, password);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        else
        {
            lblMessages.Text = "Käyttäjätunnus väärän muotoinen (huom. 1-6 merkkiä) tai salasanat eivät täsmää.";
        }
        return false;
    }

    // ** INSERT **
    protected bool AddUserToDB(string username, string password)
    {
        // Hash password and salt it
        string salt = pwMixer.CreateSalt();
        string pw = username + salt + password;
        byte[] pw_bytes = ASCIIEncoding.ASCII.GetBytes(pw);
        SHA512Managed sha512 = new SHA512Managed();
        var hashed_byte_array = sha512.ComputeHash(pw_bytes);
        string hashedPassword = Convert.ToBase64String(hashed_byte_array);

        try
        {
            // Add new user to DB
            int rows = db.nQuery("INSERT INTO henkilot(tunnus, password, salt) values(@username,@hashedPassword,@salt)", 
                                new string[] { "@username", username, "@hashedPassword", hashedPassword, "@salt", salt});
            if (rows > 0)
                return true; // Added successfully
        }
        catch (Exception)
        {
            throw;
        }
        return false;
    }
}