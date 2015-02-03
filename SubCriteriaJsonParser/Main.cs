using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data;
using System.Data.Sql;


namespace SubCriteriaJsonParser
{
    
    public partial class Main : Form
    {
        public static string json;
        public Main()
        {
            InitializeComponent();
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            json = @textInput.Text;
        }
    }
}
