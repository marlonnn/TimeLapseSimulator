﻿using System.Windows.Forms;

namespace TimeLapseSimulator.UI
{
    partial class LogListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.MouseClick += new MouseEventHandler(this.logListView_MouseClick);

            this.FullRowSelect = true;
            this.GridLines = true;
            this.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.HideSelection = false;
            this.MultiSelect = false;
            this.Name = "logListView2";
            this.ShowGroups = false;
            this.TabIndex = 2;
            this.UseCompatibleStateImageBehavior = false;
            this.View = View.Details;
        }

        #endregion
    }
}
