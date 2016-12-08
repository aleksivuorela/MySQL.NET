using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected static Database db;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            db = new Database();
            BindGridView();
        }
    }

    protected void BindGridView()
    {
        gvHenkilot.DataSource = db.Table("henkilot");
        gvHenkilot.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (gvHenkilot.SelectedIndex != -1)
        { 
            if (UpdatePerson(txtTunnus.Text, txtSnimi.Text, txtEnimi.Text, txtOsoite.Text, txtPuhnro.Text, txtEmail.Text))
            {
                BindGridView();
                lblMessages.Text = "Henkilö " + txtTunnus.Text + " päivitetty.";
            }
            else
            {
                lblMessages.Text = "Päivitys epäonnistui, tarkista syötteet.";
            }
        }
        else
        {
            lblMessages.Text = "Valitse henkilö.";
        }
    }

    protected void gvHenkilot_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gvHenkilot.SelectedRow;
        txtTunnus.Text = row.Cells[2].Text;
        txtSnimi.Text = row.Cells[3].Text;
        txtEnimi.Text = row.Cells[4].Text;
        txtOsoite.Text = row.Cells[5].Text;
        txtPuhnro.Text = row.Cells[6].Text;
        txtEmail.Text = row.Cells[7].Text;
        txtTunnus.Enabled = false;
    }

    protected void gvHenkilot_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string tunnus = gvHenkilot.Rows[e.RowIndex].Cells[2].Text;
        if (DeletePerson(tunnus))
        {
            gvHenkilot.EditIndex = -1;
            BindGridView();
            lblMessages.Text = "Henkilö " + tunnus + " poistettu.";
        }
        else
        {
            lblMessages.Text = "Poistaminen epäonnistui.";
        }       
    }

    // ** REGEX **

    // Allow only characters a-z, A-Z, 0-9. Username cannot start with numbers. Length min 1, max 6.
    protected string tunnusRegex = @"^[a-zA-Z][a-zA-Z0-9]{1,6}$";

    // Allow only characters a-z, A-Z, - and space
    protected string nimiRegex = @"^[a-zA-Z- ]+$";

    // Allow only characters a-z, A-Z, 0-9, - and space
    protected string osoiteRegex = @"^[a-zA-Z0-9- ]+$";

    // Allow only numbers and spaces
    protected string puhnroRegex = @"^[0-9 ]+$";

    // Valid email addresses
    protected string emailRegex = @"^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";


    // ** UPDATE **
    public bool UpdatePerson(string tunnus, string snimi, string enimi, string osoite, string puhnro, string email)
    {
        if (Regex.IsMatch(tunnus, tunnusRegex) && Regex.IsMatch(snimi, nimiRegex) && Regex.IsMatch(enimi, nimiRegex) &&
            Regex.IsMatch(osoite, osoiteRegex) && Regex.IsMatch(puhnro, puhnroRegex) && Regex.IsMatch(email, emailRegex)) {
            try
            {
                // Give parameters straight to Query-method and update database
                int i = db.nQuery("UPDATE henkilot SET sukunimi=@snimi, etunimi=@enimi, osoite=@osoite, puhnro=@puhnro, email=@email WHERE tunnus=@tunnus",
                new string[] { "@tunnus", tunnus, "@snimi", snimi, "@enimi", enimi, "@osoite", osoite, "@puhnro", puhnro, "@email", email });
                if (i > 0) return true;
            }
            catch (Exception)
            {
            }
        }
        return false;
    }

    // ** DELETE **
    public bool DeletePerson(string tunnus)
    {
        if (Regex.IsMatch(tunnus, tunnusRegex)) {
            try
            {
                // Bind one parameter
                db.Bind("tunnus", tunnus);
                // Delete from database
                int i = db.nQuery("DELETE FROM henkilot WHERE tunnus=@tunnus");
                if (i > 0) return true;
            }
            catch (Exception)
            {
            }
        }
        return false;
    }
}