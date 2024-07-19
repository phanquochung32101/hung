using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

class AsyncResolve : Form
{

    TextBox address;
    ListBox results;
    private AsyncCallback OnResolved;

    public AsyncResolve()
    {

        Text = "DNS Address Resolver";
        Size = new Size(400, 380);
        OnResolved = new AsyncCallback(Resolved);


        Label label1 = new Label();
        label1.Parent = this;
        label1.Text = "Enter address to resolve:";
        label1.AutoSize = true;
        label1.Location = new Point(10, 10);

        address = new TextBox();
        address.Parent = this;
        address.Size = new Size(200, 2 * Font.Height);
        address.Location = new Point(10, 35);

        results = new ListBox();
        results.Parent = this;
        results.Location = new Point(10, 65);
        results.Size = new Size(350, 20 * Font.Height);

        Button checkit = new Button();
        checkit.Parent = this;
        checkit.Text = "Resolve";
        checkit.Location = new Point(235, 32);
        checkit.Size = new Size(7 * Font.Height, 2 * Font.Height);
        checkit.Click += new EventHandler(ButtonResolveOnClick);
    }

    void ButtonResolveOnClick(object obj, EventArgs ea)
    {
        results.Items.Clear();
        string addr = address.Text;
        Object state = new Object();

        Dns.BeginGetHostEntry(addr, OnResolved, state);
    }

    private void Resolved(IAsyncResult ar)
    {
        if (this.InvokeRequired)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                Resolved(ar);
            }));
            return;
        }

        string buffer1;
        IPHostEntry iphe = Dns.EndGetHostEntry(ar);

        buffer1 = "Host name: " + iphe.HostName;
        results.Items.Add(buffer1);

        foreach (string alias in iphe.Aliases)
        {
            buffer1 = "Alias:       " + alias;
            results.Items.Add(buffer1);
        }

        foreach (IPAddress addr in iphe.AddressList)
        {
            buffer1 = "Address:    " + addr.ToString();
            results.Items.Add(buffer1);
        }
    }


    private void InitializeComponent()
    {
        SuspendLayout();
        // 
        // AsyncResolve
        // 
        ClientSize = new Size(282, 253);
        Name = "AsyncResolve";
        Load += AsyncResolve_Load;
        ResumeLayout(false);
    }

    public static void Main()
    {
        Application.Run(new AsyncResolve());
    }

    private void AsyncResolve_Load(object sender, EventArgs e)
    {

    }
}