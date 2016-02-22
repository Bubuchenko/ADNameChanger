using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices;

namespace ADRenamer
{
    public partial class Form1 : Form
    {
        public string LDAP_scope = "LDAP://OU=MLA,DC=MCO,DC=local";
        public string profile_folder = "";
        public string userfile_folder = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(getLoginName(textBox1.Text));

            //MessageBox.Show()
        }

        public string getLoginName(string displayName)
        {
            using (DirectoryEntry de = new DirectoryEntry(LDAP_scope))
            {
                using (DirectorySearcher ds = new DirectorySearcher(de))
                {
                    ds.Filter = string.Format("(&(objectcategory=user)({0}={1}))", "displayName", displayName);

                    SearchResult result = ds.FindOne();

                    using (DirectoryEntry uEntry = result.GetDirectoryEntry())
                    {
                        return (uEntry.Properties["givenName"].Value.ToString()[0] + "." + uEntry.Properties["sn"].Value.ToString()).ToLower();
                    }
                }
            }
        }

        public void setUserProperties(string displayName, ADUserProperties Properties)
        {
            using (DirectoryEntry de = new DirectoryEntry(LDAP_scope))
            {
                using (DirectorySearcher ds = new DirectorySearcher(de))
                {
                    ds.Filter = string.Format("(&(objectcategory=user)({0}={1}))", "displayName", displayName);

                    SearchResult result = ds.FindOne();

                    using (DirectoryEntry uEntry = result.GetDirectoryEntry())
                    {
                        //Change values in AD
                        uEntry.Properties["SamAccountName"].Value = Properties.SamAccountName;
                        uEntry.Properties["cn"].Value = Properties.cn;
                        uEntry.Properties["mailNickname"].Value = Properties.mailNickname;
                        uEntry.Properties["userPrincipalName"].Value = Properties.userPrincipalName;
                        uEntry.Properties["name"].Value = Properties.name;

                        //Path values
                        uEntry.Properties["homeDirectory"].Value = Properties.homeDirectory;
                        uEntry.Properties["profilePath"].Value = Properties.profilePath;

                        uEntry.CommitChanges();
                        uEntry.Close();
                    }
                }
            }
        }
    }

    public class ADUserProperties
    {
        public string SamAccountName { get; set; }
        public string cn { get; set; }
        public string fullName { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public string userPrincipalName { get; set; }
        public string name { get; set; }

        public string profilePath { get; set; }
        public string homeDirectory { get; set; }

    }
}
