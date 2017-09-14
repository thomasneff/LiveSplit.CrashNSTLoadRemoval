using System;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

namespace LiveSplit.UI.Components
{
    public partial class CrashNSTLoadRemovalSettings : UserControl {
        public int category;
        public bool AutoReset;
       
        public CrashNSTLoadRemovalSettings() {
            InitializeComponent();
            this.comboBox1.SelectedIndex = category = 0;
            this.lblVersion.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            AutoReset = false;
        }

        public XmlNode GetSettings(XmlDocument document) {
           

            var settingsNode = document.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

            settingsNode.AppendChild(ToElement(document, "AutoReset", AutoReset.ToString()));
            settingsNode.AppendChild(ToElement(document, "Category", category.ToString()));


            return settingsNode;
        }

        public void SetSettings(XmlNode settings) {
            var element = (XmlElement)settings;
            if (!element.IsEmpty) {
                Version version;
                if (element["Version"] != null) {
                    version = Version.Parse(element["Version"].InnerText);
                } else {
                    version = new Version(1, 0, 0);
                }

                if (element["AutoReset"] != null) {
                    AutoReset = Convert.ToBoolean(element["AutoReset"].InnerText);
                    chkAutoReset.Checked = AutoReset;
                }

              

                if (element["Category"] != null) {
                    category = Convert.ToInt32(element["Category"].InnerText);
                }

                comboBox1.SelectedIndex = category;
                this.checkedListBox1.Items.Clear();

            }
        }

        private void checkAutoReset_CheckedChanged(object sender, EventArgs e) {
            AutoReset = chkAutoReset.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
          
            category = comboBox1.SelectedIndex;
            this.checkedListBox1.Items.Clear();

           
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
           
        }

        private XmlElement ToElement<T>(XmlDocument document, String name, T value)
        {
            var element = document.CreateElement(name);
            element.InnerText = value.ToString();
            return element;
        }
    }
}
