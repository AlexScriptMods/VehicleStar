using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleStar
{
    public partial class Form_PlayYVR : Form
    {

        private static string lastFileName = "";
        private static int lastFileIndex = 1;
        private static bool lastShouldWarpIntoVehicle = true;

        public string selectedFileName { get; private set; }
        public int selectedFileIndex { get; private set; }
        public bool isCancelled { get; private set; } = true;
        public bool shouldWarpIntoVehicle { get; private set; } = true;

        public Form_PlayYVR()
        {
            InitializeComponent();

            Function.Call(Hash.SET_MOUSE_CURSOR_VISIBLE, true);

            //center the cursor so its visible to user
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int centerX = screenWidth / 2;
            int centerY = screenHeight / 2;

            Cursor.Position = new System.Drawing.Point(centerX, centerY);

            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += Form_PlayYVR_FormClosed;

            //Init with last values
            fileName.Text = lastFileName;
            fileIndex.Text = lastFileIndex.ToString();
            warp.Checked = lastShouldWarpIntoVehicle;
        }

        private void Form_PlayYVR_FormClosed(object sender, FormClosedEventArgs e)
        {
            Function.Call(Hash.SET_MOUSE_CURSOR_VISIBLE, false);
        }

        private void submit_click(object sender, EventArgs e)
        {
            selectedFileName = fileName.Text;
            selectedFileIndex = int.Parse(fileIndex.Text);
            isCancelled = false;
            shouldWarpIntoVehicle = warp.Checked;

            //update static fields
            lastFileName = selectedFileName;
            lastFileIndex = selectedFileIndex;
            lastShouldWarpIntoVehicle = shouldWarpIntoVehicle;

            Close();
        }

        private void cancel_click(object sender, EventArgs e)
        {
            isCancelled = true;
            Close();
        }

        private void help(object sender, EventArgs e)
        {
            MessageBox.Show("YVR File should follow format <name>_index.yvr. \r\n\r\nEXAMPLE:\r\n\r\ntest_1.yvr:\r\n\r\nName: \"test\", Index: 1");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
