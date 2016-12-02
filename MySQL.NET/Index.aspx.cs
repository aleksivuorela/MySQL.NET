using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // 1. Bind multiple parameters
        db.Bind(new string[] { "@tunnus", txtTunnus.Text, "@snimi", txtSnimi.Text, "@enimi", txtEnimi.Text, "@osoite", txtOsoite.Text, "@puhnro", txtPuhnro.Text, "@email", txtEmail.Text });
        db.nQuery("INSERT INTO henkilot (tunnus, sukunimi, etunimi, osoite, puhnro, email) VALUES (@tunnus, @snimi, @enimi, @osoite, @puhnro, @email)");
        BindGridView();
        lblMessages.Text = "Henkilö " + txtTunnus.Text + " lisätty.";
        Clear();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        // 2. Give parameters to method
        db.nQuery("UPDATE henkilot SET sukunimi=@snimi, etunimi=@enimi, osoite=@osoite, puhnro=@puhnro, email=@email WHERE tunnus=@tunnus",
        new string[] { "@tunnus", txtTunnus.Text, "@snimi", txtSnimi.Text, "@enimi", txtEnimi.Text, "@osoite", txtOsoite.Text, "@puhnro", txtPuhnro.Text, "@email", txtEmail.Text });
        BindGridView();
        lblMessages.Text = "Henkilö " + txtTunnus.Text + " päivitetty.";
        Clear();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
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
        btnSubmit.Visible = false;
        btnUpdate.Visible = true;
    }

    protected void gvHenkilot_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string tunnus = gvHenkilot.Rows[e.RowIndex].Cells[2].Text;
        // 3. Bind one parameter
        db.Bind("tunnus", tunnus);
        db.nQuery("DELETE FROM henkilot WHERE tunnus=@tunnus");
        gvHenkilot.EditIndex = -1;
        BindGridView();
        lblMessages.Text = "Henkilö " + tunnus + " poistettu.";
    }

    protected void Clear()
    {
        txtTunnus.Text = string.Empty;
        txtEnimi.Text = string.Empty;
        txtSnimi.Text = string.Empty;
        txtOsoite.Text = string.Empty;
        txtPuhnro.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtTunnus.Enabled = true;
        txtTunnus.Focus();
        btnSubmit.Visible = true;
        btnUpdate.Visible = false;
    }
}