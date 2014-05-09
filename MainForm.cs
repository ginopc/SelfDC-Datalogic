using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using SelfDC.Models;

namespace SelfDC
{
    public partial class MainForm : Form
    {
        bool ItemEdit = false;
        private List<OrderItem> listaProdotti;
        private Settings appSettings;
        // private string appFileName = string.Format("{0}\\conf.txt", Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
        private string appFileName = string.Format("{0}\\conf.txt", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase));

        /** costruttore della classe */
        public MainForm()
        {
            InitializeComponent();
            txtCode.Text = "";
            txtQta.Text = "";
            listBox.Items.Clear();

            listaProdotti = new List<OrderItem>();
        }

        /** Caricamento della maschera */
        private void FormLoad(object sender, EventArgs e)
        {
            // Carico le impostazioni dell'applicazione
            appSettings = new Settings();
            appSettings.LoadFromFile(appFileName);

            // Imposto la maschera a tutto schermo
            this.WindowState = FormWindowState.Maximized;
        }

        /** modifica l'elemento selezionato */
        private void actEdit(object sender, EventArgs e)
        {
            int index = listBox.SelectedIndices[0];

            if (listBox.Items[index].Text == "")
            {
                cbCodInterno.Checked = true;
                txtCode.Text = listBox.Items[index].SubItems[1].Text;
            }
            else
            {
                cbCodInterno.Checked = false;
                txtCode.Text = listBox.Items[index].Text;
            }

            txtQta.Text = listBox.Items[index].SubItems[2].Text;

            // blocco la checkbox
            cbCodInterno.Enabled = false;
            txtQta.Focus();
        }

        /** Inizia un nuovo inserimento manuale */
        private void actNew(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtCode.Enabled = true;
            txtQta.Text = "";
            txtQta.Enabled = true;
            cbCodInterno.Checked = false;
            cbCodInterno.Enabled = true;
            btnSave.Enabled = true;

            ItemEdit = false;
            txtCode.Focus();
        }

        /** Elimina l'elemento selezionato */
        private void actRemove(object sender, EventArgs e)
        {
            if (listBox.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Seleziona l'elemento da eliminare","Elimina Riga");
                return;
            }

            DialogResult res = MessageBox.Show(
                "Vuoi eliminare la riga selezionata?"
                , "Elimina Record"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button1);

            if (res == DialogResult.No) return;

            listBox.Items.Remove(listBox.Items[listBox.SelectedIndices[0]]);
        }

        /*  Form Resize */
        private void FormResize(object sender, EventArgs e)
        {
            double ctlWidth = (double) ClientSize.Width;

            /** Resize listBox */
            listBox.Width = (int) ctlWidth;
            listBox.Columns[0].Width = (int) (ctlWidth * 0.50);
            listBox.Columns[1].Width = (int) (ctlWidth * 0.30);
            listBox.Columns[1].Width = (int) (ctlWidth * 0.20);
        }

        private void actQuit(object sender, EventArgs e)
        {
            this.Close();
        }

        /** Esporta la lista in un file */
        private void actExport(object sender, EventArgs e)
        {
            if (listBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "Nessun dato da esportare"
                    ,"ATTENZIONE!"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Asterisk
                    , MessageBoxDefaultButton.Button1);
                return;
            }

            DialogResult res = MessageBox.Show(
                                    "Vuoi esportare l'ordine?", 
                                    "Esporta Ordine", 
                                    MessageBoxButtons.YesNo, 
                                    MessageBoxIcon.Question, 
                                    MessageBoxDefaultButton.Button1);
            if (res == DialogResult.No) return;
/*
            // Scrive il file
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(appSettings.OrdineFileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Errore di creazione del file dati" + ex.StackTrace
                    , "Errore"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Exclamation
                    , MessageBoxDefaultButton.Button1);
                return;
            }

            // Esporta i dati
            string line;
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                line = string.Format("{0};{1};{2};{3}",appSettings.CodiceCliente , listBox.Items[i].Text, listBox.Items[i].SubItems[1].Text, listBox.Items[i].SubItems[2].Text);
                sw.WriteLine(line);
            }
            sw.Close();
*/
            if (EsportaOrdine(appSettings.OrdineFileName) < 0)
            {
                return;
            }

            listBox.Items.Clear();
            listBox_SelectedIndexChanged(sender, e);

            MessageBox.Show("File esportato con successo","Esporta Dati");
        }

        /** Abilita menu di modifica solo se è selezionata un riga della lista */
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((listBox.Items.Count == 0) || (listBox.SelectedIndices.Count == 0))
            {
                txtCode.Enabled = false;
                txtQta.Enabled = false;
                cbCodInterno.Enabled = false;
                btnSave.Enabled = false;
                return;
            }

            int index = listBox.SelectedIndices[0];

            // Disable Edit menu if nothing is selected
            if (index >= 0)
            {
                // editing abilitato
                ItemEdit = true;
                mnuDelete.Enabled = true;
                cmnuDelete.Enabled = true;

                txtQta.Enabled = true;
                btnSave.Enabled = true;

                txtQta.SelectAll();
                actEdit(sender, e);
            }
            else {
                // editing disabilitato
                ItemEdit = false;
                mnuDelete.Enabled = false;
                cmnuDelete.Enabled = false;


                txtCode.Enabled = true;
                txtQta.Enabled = true;

                btnSave.Enabled = false;
            }
        }

        /** Salva modifica */
        private void actSave(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                MessageBox.Show("Niente da inserire", "Salva", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            // disabilito i campi x evitare modifiche durante il salvataggio
            txtCode.Enabled = false;
            txtQta.Enabled = false;
            cbCodInterno.Enabled = false;

            // se ci sono elementi selezionati => è una modifica
            ListViewItem item;
            if ((listBox.SelectedIndices.Count > 0) && ItemEdit)
            {
                item = listBox.Items[listBox.SelectedIndices[0]];
                if (cbCodInterno.Checked)
                    item.SubItems[1].Text = txtCode.Text; // cod interno
                else
                    item.Text = txtCode.Text; // ean
                item.SubItems[2].Text = txtQta.Text; // qta
                ItemEdit = false;
            }
            else // Nuovo inserimento manuale
            {
                item = new ListViewItem();
                if (cbCodInterno.Checked) 
                {
                    item.Text = "";
                    item.SubItems.Add(txtCode.Text);
                }
                else
                {
                    item.Text = txtCode.Text;
                    item.SubItems.Add("");
                }

                if (txtQta.Text == "") txtQta.Text = "1";
                item.SubItems.Add(txtQta.Text);

                listBox.Items.Add(item);
            }

            // pulisco e disabilito i campi
            cbCodInterno.Checked = false;
            cbCodInterno.Enabled = false;
            txtCode.Text = "";
            txtCode.Enabled = false;
            txtQta.Text = "";
            txtQta.Enabled = false;
            btnSave.Enabled = false;
        }

        /** Annulla modifica */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtQta.Text = "";
            cbCodInterno.Checked = false;
            btnSave.Enabled = false;
            // panEdit.Visible = false;
        }


        private void actAbout(object sender, EventArgs e)
        {
            string ProductName = Assembly.GetExecutingAssembly().FullName;
            MessageBox.Show(
                ProductName + "\ndesigned by Maurizio Aru"
                , "Info"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Asterisk
                ,MessageBoxDefaultButton.Button1);
        }

        private void MainForm2_Closing(object sender, CancelEventArgs e)
        {
            DialogResult res = MessageBox.Show(
                "Vuoi uscire dall'applicazione?"
                , "Uscita"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button1);

            if (res == DialogResult.No) e.Cancel=true;
        }

        private void MainForm2_Closed(object sender, EventArgs e)
        {
            // Salvo le impostazioni
            appSettings.SaveToFile(appFileName);

            // TODO: se esiste un ordine in fase di compilazione lo esporto su un file temporaneo

        }

        private void txtQta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                actSave(sender, e);
                
        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                actSave(sender, e);
        }

        private void cbCodInterno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                actSave(sender, e);

        }

        private int EsportaOrdine(string FileName)
        {
            int result = -1;

            // Scrive il file
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(FileName);
            }
            catch (Exception ex)
            {
                return -1;
            }

            // Esporta i dati
            string line;
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                line = string.Format("{0};{1};{2};{3}", appSettings.CodiceCliente, listBox.Items[i].Text, listBox.Items[i].SubItems[1].Text, listBox.Items[i].SubItems[2].Text);
                sw.WriteLine(line);
                result++;
            }
            sw.Close();

            return result;
        }
    }
}