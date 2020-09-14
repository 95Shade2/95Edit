namespace Minecraft_Biome_Changer
{
        partial class MainForm
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

                #region Windows Form Designer generated code

                /// <summary>
                /// Required method for Designer support - do not modify
                /// the contents of this method with the code editor.
                /// </summary>
                private void InitializeComponent()
                {
                        this.components = new System.ComponentModel.Container();
                        this.label1 = new System.Windows.Forms.Label();
                        this.StartX_Textbox = new System.Windows.Forms.TextBox();
                        this.MCSaves_Listbox = new System.Windows.Forms.ListBox();
                        this.label2 = new System.Windows.Forms.Label();
                        this.SelectWorld_Button = new System.Windows.Forms.Button();
                        this.label3 = new System.Windows.Forms.Label();
                        this.SelectedWorld_Label = new System.Windows.Forms.Label();
                        this.StartZ_Textbox = new System.Windows.Forms.TextBox();
                        this.label4 = new System.Windows.Forms.Label();
                        this.EndZ_Textbox = new System.Windows.Forms.TextBox();
                        this.label5 = new System.Windows.Forms.Label();
                        this.EndX_Textbox = new System.Windows.Forms.TextBox();
                        this.label6 = new System.Windows.Forms.Label();
                        this.Biome_ComboBox = new System.Windows.Forms.ComboBox();
                        this.label7 = new System.Windows.Forms.Label();
                        this.ChangeBiomes_Button = new System.Windows.Forms.Button();
                        this.Debug_RichTextbox = new System.Windows.Forms.RichTextBox();
                        this.ProgressBar_Label = new System.Windows.Forms.Label();
                        this.progressBar1 = new System.Windows.Forms.ProgressBar();
                        this.UpdateProgressBar_Timer = new System.Windows.Forms.Timer(this.components);
                        this.Done_Label = new System.Windows.Forms.Label();
                        this.DisplayLog_Checkbox = new System.Windows.Forms.CheckBox();
                        this.ActiveThreads_Label = new System.Windows.Forms.Label();
                        this.Warnings_RichTextbox = new System.Windows.Forms.RichTextBox();
                        this.ProgressPercentage_Label = new System.Windows.Forms.Label();
                        this.UpdatePercentate_Timer = new System.Windows.Forms.Timer(this.components);
                        this.Stop_Button = new System.Windows.Forms.Button();
                        this.SubChunk_Button = new System.Windows.Forms.Button();
                        this.SuspendLayout();
                        // 
                        // label1
                        // 
                        this.label1.AutoSize = true;
                        this.label1.Location = new System.Drawing.Point(268, 63);
                        this.label1.Name = "label1";
                        this.label1.Size = new System.Drawing.Size(53, 13);
                        this.label1.TabIndex = 0;
                        this.label1.Text = "Starting X";
                        // 
                        // StartX_Textbox
                        // 
                        this.StartX_Textbox.Location = new System.Drawing.Point(327, 60);
                        this.StartX_Textbox.Name = "StartX_Textbox";
                        this.StartX_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.StartX_Textbox.TabIndex = 1;
                        // 
                        // MCSaves_Listbox
                        // 
                        this.MCSaves_Listbox.FormattingEnabled = true;
                        this.MCSaves_Listbox.Location = new System.Drawing.Point(12, 25);
                        this.MCSaves_Listbox.Name = "MCSaves_Listbox";
                        this.MCSaves_Listbox.Size = new System.Drawing.Size(188, 381);
                        this.MCSaves_Listbox.TabIndex = 2;
                        // 
                        // label2
                        // 
                        this.label2.AutoSize = true;
                        this.label2.Location = new System.Drawing.Point(60, 7);
                        this.label2.Name = "label2";
                        this.label2.Size = new System.Drawing.Size(87, 13);
                        this.label2.TabIndex = 3;
                        this.label2.Text = "Minecraft Worlds";
                        // 
                        // SelectWorld_Button
                        // 
                        this.SelectWorld_Button.Location = new System.Drawing.Point(66, 410);
                        this.SelectWorld_Button.Name = "SelectWorld_Button";
                        this.SelectWorld_Button.Size = new System.Drawing.Size(78, 23);
                        this.SelectWorld_Button.TabIndex = 4;
                        this.SelectWorld_Button.Text = "Select World";
                        this.SelectWorld_Button.UseVisualStyleBackColor = true;
                        this.SelectWorld_Button.Click += new System.EventHandler(this.SelectWorld_Button_Click);
                        // 
                        // label3
                        // 
                        this.label3.AutoSize = true;
                        this.label3.Location = new System.Drawing.Point(206, 25);
                        this.label3.Name = "label3";
                        this.label3.Size = new System.Drawing.Size(86, 13);
                        this.label3.TabIndex = 5;
                        this.label3.Text = "Selected World: ";
                        // 
                        // SelectedWorld_Label
                        // 
                        this.SelectedWorld_Label.AutoSize = true;
                        this.SelectedWorld_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        this.SelectedWorld_Label.Location = new System.Drawing.Point(288, 25);
                        this.SelectedWorld_Label.Name = "SelectedWorld_Label";
                        this.SelectedWorld_Label.Size = new System.Drawing.Size(0, 13);
                        this.SelectedWorld_Label.TabIndex = 6;
                        // 
                        // StartZ_Textbox
                        // 
                        this.StartZ_Textbox.Location = new System.Drawing.Point(327, 86);
                        this.StartZ_Textbox.Name = "StartZ_Textbox";
                        this.StartZ_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.StartZ_Textbox.TabIndex = 8;
                        // 
                        // label4
                        // 
                        this.label4.AutoSize = true;
                        this.label4.Location = new System.Drawing.Point(268, 89);
                        this.label4.Name = "label4";
                        this.label4.Size = new System.Drawing.Size(53, 13);
                        this.label4.TabIndex = 7;
                        this.label4.Text = "Starting Z";
                        // 
                        // EndZ_Textbox
                        // 
                        this.EndZ_Textbox.Location = new System.Drawing.Point(519, 86);
                        this.EndZ_Textbox.Name = "EndZ_Textbox";
                        this.EndZ_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.EndZ_Textbox.TabIndex = 12;
                        // 
                        // label5
                        // 
                        this.label5.AutoSize = true;
                        this.label5.Location = new System.Drawing.Point(460, 89);
                        this.label5.Name = "label5";
                        this.label5.Size = new System.Drawing.Size(50, 13);
                        this.label5.TabIndex = 11;
                        this.label5.Text = "Ending Z";
                        // 
                        // EndX_Textbox
                        // 
                        this.EndX_Textbox.Location = new System.Drawing.Point(519, 60);
                        this.EndX_Textbox.Name = "EndX_Textbox";
                        this.EndX_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.EndX_Textbox.TabIndex = 10;
                        // 
                        // label6
                        // 
                        this.label6.AutoSize = true;
                        this.label6.Location = new System.Drawing.Point(460, 63);
                        this.label6.Name = "label6";
                        this.label6.Size = new System.Drawing.Size(50, 13);
                        this.label6.TabIndex = 9;
                        this.label6.Text = "Ending X";
                        // 
                        // Biome_ComboBox
                        // 
                        this.Biome_ComboBox.FormattingEnabled = true;
                        this.Biome_ComboBox.Location = new System.Drawing.Point(383, 156);
                        this.Biome_ComboBox.Name = "Biome_ComboBox";
                        this.Biome_ComboBox.Size = new System.Drawing.Size(121, 21);
                        this.Biome_ComboBox.TabIndex = 13;
                        // 
                        // label7
                        // 
                        this.label7.AutoSize = true;
                        this.label7.Location = new System.Drawing.Point(424, 137);
                        this.label7.Name = "label7";
                        this.label7.Size = new System.Drawing.Size(36, 13);
                        this.label7.TabIndex = 14;
                        this.label7.Text = "Biome";
                        // 
                        // ChangeBiomes_Button
                        // 
                        this.ChangeBiomes_Button.Location = new System.Drawing.Point(397, 194);
                        this.ChangeBiomes_Button.Name = "ChangeBiomes_Button";
                        this.ChangeBiomes_Button.Size = new System.Drawing.Size(92, 23);
                        this.ChangeBiomes_Button.TabIndex = 15;
                        this.ChangeBiomes_Button.Text = "Change Biomes";
                        this.ChangeBiomes_Button.UseVisualStyleBackColor = true;
                        this.ChangeBiomes_Button.Click += new System.EventHandler(this.ChangeBiomes_Button_Click);
                        // 
                        // Debug_RichTextbox
                        // 
                        this.Debug_RichTextbox.Location = new System.Drawing.Point(510, 112);
                        this.Debug_RichTextbox.Name = "Debug_RichTextbox";
                        this.Debug_RichTextbox.ReadOnly = true;
                        this.Debug_RichTextbox.Size = new System.Drawing.Size(182, 139);
                        this.Debug_RichTextbox.TabIndex = 16;
                        this.Debug_RichTextbox.Text = "";
                        this.Debug_RichTextbox.Visible = false;
                        // 
                        // ProgressBar_Label
                        // 
                        this.ProgressBar_Label.AutoSize = true;
                        this.ProgressBar_Label.Location = new System.Drawing.Point(206, 264);
                        this.ProgressBar_Label.Name = "ProgressBar_Label";
                        this.ProgressBar_Label.Size = new System.Drawing.Size(0, 13);
                        this.ProgressBar_Label.TabIndex = 17;
                        // 
                        // progressBar1
                        // 
                        this.progressBar1.Location = new System.Drawing.Point(394, 234);
                        this.progressBar1.Name = "progressBar1";
                        this.progressBar1.Size = new System.Drawing.Size(100, 23);
                        this.progressBar1.TabIndex = 18;
                        // 
                        // UpdateProgressBar_Timer
                        // 
                        this.UpdateProgressBar_Timer.Enabled = true;
                        this.UpdateProgressBar_Timer.Tick += new System.EventHandler(this.UpdateProgressBar_Timer_Tick);
                        // 
                        // Done_Label
                        // 
                        this.Done_Label.AutoSize = true;
                        this.Done_Label.Location = new System.Drawing.Point(426, 264);
                        this.Done_Label.Name = "Done_Label";
                        this.Done_Label.Size = new System.Drawing.Size(0, 13);
                        this.Done_Label.TabIndex = 19;
                        // 
                        // DisplayLog_Checkbox
                        // 
                        this.DisplayLog_Checkbox.AutoSize = true;
                        this.DisplayLog_Checkbox.Location = new System.Drawing.Point(500, 257);
                        this.DisplayLog_Checkbox.Name = "DisplayLog_Checkbox";
                        this.DisplayLog_Checkbox.Size = new System.Drawing.Size(81, 17);
                        this.DisplayLog_Checkbox.TabIndex = 20;
                        this.DisplayLog_Checkbox.Text = "Display Log";
                        this.DisplayLog_Checkbox.UseVisualStyleBackColor = true;
                        this.DisplayLog_Checkbox.CheckedChanged += new System.EventHandler(this.DisplayLog_Checkbox_CheckedChanged);
                        // 
                        // ActiveThreads_Label
                        // 
                        this.ActiveThreads_Label.AutoSize = true;
                        this.ActiveThreads_Label.Location = new System.Drawing.Point(206, 204);
                        this.ActiveThreads_Label.Name = "ActiveThreads_Label";
                        this.ActiveThreads_Label.Size = new System.Drawing.Size(91, 13);
                        this.ActiveThreads_Label.TabIndex = 21;
                        this.ActiveThreads_Label.Text = "Active Threads: 0";
                        // 
                        // Warnings_RichTextbox
                        // 
                        this.Warnings_RichTextbox.Location = new System.Drawing.Point(209, 280);
                        this.Warnings_RichTextbox.Name = "Warnings_RichTextbox";
                        this.Warnings_RichTextbox.ReadOnly = true;
                        this.Warnings_RichTextbox.Size = new System.Drawing.Size(483, 150);
                        this.Warnings_RichTextbox.TabIndex = 22;
                        this.Warnings_RichTextbox.Text = "";
                        // 
                        // ProgressPercentage_Label
                        // 
                        this.ProgressPercentage_Label.AutoSize = true;
                        this.ProgressPercentage_Label.BackColor = System.Drawing.SystemColors.Control;
                        this.ProgressPercentage_Label.Location = new System.Drawing.Point(427, 240);
                        this.ProgressPercentage_Label.MaximumSize = new System.Drawing.Size(33, 13);
                        this.ProgressPercentage_Label.MinimumSize = new System.Drawing.Size(33, 13);
                        this.ProgressPercentage_Label.Name = "ProgressPercentage_Label";
                        this.ProgressPercentage_Label.Size = new System.Drawing.Size(33, 13);
                        this.ProgressPercentage_Label.TabIndex = 23;
                        this.ProgressPercentage_Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        // 
                        // UpdatePercentate_Timer
                        // 
                        this.UpdatePercentate_Timer.Interval = 10;
                        this.UpdatePercentate_Timer.Tick += new System.EventHandler(this.UpdatePercentate_Timer_Tick);
                        // 
                        // Stop_Button
                        // 
                        this.Stop_Button.Location = new System.Drawing.Point(617, 15);
                        this.Stop_Button.Name = "Stop_Button";
                        this.Stop_Button.Size = new System.Drawing.Size(75, 23);
                        this.Stop_Button.TabIndex = 25;
                        this.Stop_Button.Text = "Cancel";
                        this.Stop_Button.UseVisualStyleBackColor = true;
                        this.Stop_Button.Click += new System.EventHandler(this.Stop_Button_Click);
                        // 
                        // SubChunk_Button
                        // 
                        this.SubChunk_Button.Location = new System.Drawing.Point(206, 251);
                        this.SubChunk_Button.Name = "SubChunk_Button";
                        this.SubChunk_Button.Size = new System.Drawing.Size(131, 23);
                        this.SubChunk_Button.TabIndex = 26;
                        this.SubChunk_Button.Text = "Sub-Chunk Biome Editor";
                        this.SubChunk_Button.UseVisualStyleBackColor = true;
                        this.SubChunk_Button.Click += new System.EventHandler(this.SubChunk_Button_Click);
                        // 
                        // MainForm
                        // 
                        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.ClientSize = new System.Drawing.Size(704, 442);
                        this.Controls.Add(this.SubChunk_Button);
                        this.Controls.Add(this.Stop_Button);
                        this.Controls.Add(this.ProgressPercentage_Label);
                        this.Controls.Add(this.Warnings_RichTextbox);
                        this.Controls.Add(this.ActiveThreads_Label);
                        this.Controls.Add(this.DisplayLog_Checkbox);
                        this.Controls.Add(this.Done_Label);
                        this.Controls.Add(this.progressBar1);
                        this.Controls.Add(this.ProgressBar_Label);
                        this.Controls.Add(this.Debug_RichTextbox);
                        this.Controls.Add(this.ChangeBiomes_Button);
                        this.Controls.Add(this.label7);
                        this.Controls.Add(this.Biome_ComboBox);
                        this.Controls.Add(this.EndZ_Textbox);
                        this.Controls.Add(this.label5);
                        this.Controls.Add(this.EndX_Textbox);
                        this.Controls.Add(this.label6);
                        this.Controls.Add(this.StartZ_Textbox);
                        this.Controls.Add(this.label4);
                        this.Controls.Add(this.SelectedWorld_Label);
                        this.Controls.Add(this.label3);
                        this.Controls.Add(this.SelectWorld_Button);
                        this.Controls.Add(this.label2);
                        this.Controls.Add(this.MCSaves_Listbox);
                        this.Controls.Add(this.StartX_Textbox);
                        this.Controls.Add(this.label1);
                        this.Name = "MainForm";
                        this.Text = "Redstone Biome Changer v0.95";
                        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
                        this.Load += new System.EventHandler(this.Form1_Load);
                        this.ResumeLayout(false);
                        this.PerformLayout();

                }

                #endregion

                private System.Windows.Forms.Label label1;
                private System.Windows.Forms.TextBox StartX_Textbox;
                private System.Windows.Forms.ListBox MCSaves_Listbox;
                private System.Windows.Forms.Label label2;
                private System.Windows.Forms.Button SelectWorld_Button;
                private System.Windows.Forms.Label label3;
                private System.Windows.Forms.Label SelectedWorld_Label;
                private System.Windows.Forms.TextBox StartZ_Textbox;
                private System.Windows.Forms.Label label4;
                private System.Windows.Forms.TextBox EndZ_Textbox;
                private System.Windows.Forms.Label label5;
                private System.Windows.Forms.TextBox EndX_Textbox;
                private System.Windows.Forms.Label label6;
                private System.Windows.Forms.ComboBox Biome_ComboBox;
                private System.Windows.Forms.Label label7;
                private System.Windows.Forms.Button ChangeBiomes_Button;
                private System.Windows.Forms.RichTextBox Debug_RichTextbox;
                private System.Windows.Forms.Label ProgressBar_Label;
                private System.Windows.Forms.ProgressBar progressBar1;
                private System.Windows.Forms.Timer UpdateProgressBar_Timer;
                private System.Windows.Forms.Label Done_Label;
                private System.Windows.Forms.CheckBox DisplayLog_Checkbox;
                private System.Windows.Forms.Label ActiveThreads_Label;
                private System.Windows.Forms.RichTextBox Warnings_RichTextbox;
                private System.Windows.Forms.Label ProgressPercentage_Label;
                private System.Windows.Forms.Timer UpdatePercentate_Timer;
                private System.Windows.Forms.Button Stop_Button;
                private System.Windows.Forms.Button SubChunk_Button;
        }
}

