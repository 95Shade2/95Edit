namespace Minecraft_Biome_Changer
{
        partial class SubChunks_Form
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
                        this.SubChunksChunk_Panel = new System.Windows.Forms.Panel();
                        this.X_Textbox = new System.Windows.Forms.TextBox();
                        this.label1 = new System.Windows.Forms.Label();
                        this.label2 = new System.Windows.Forms.Label();
                        this.Z_Textbox = new System.Windows.Forms.TextBox();
                        this.label3 = new System.Windows.Forms.Label();
                        this.label4 = new System.Windows.Forms.Label();
                        this.XChunk_Textbox = new System.Windows.Forms.TextBox();
                        this.label5 = new System.Windows.Forms.Label();
                        this.ZChunk_Textbox = new System.Windows.Forms.TextBox();
                        this.LoadChunk_Button = new System.Windows.Forms.Button();
                        this.MousePos_Timer = new System.Windows.Forms.Timer(this.components);
                        this.RedBiome_Combobox = new System.Windows.Forms.ComboBox();
                        this.BlueBiome_Combobox = new System.Windows.Forms.ComboBox();
                        this.label6 = new System.Windows.Forms.Label();
                        this.label7 = new System.Windows.Forms.Label();
                        this.ErrorBox_Richtextbox = new System.Windows.Forms.RichTextBox();
                        this.ChangeBiomes_Button = new System.Windows.Forms.Button();
                        this.Tools_Panel = new System.Windows.Forms.Panel();
                        this.Tools_Line_Panel = new System.Windows.Forms.Panel();
                        this.Tools_Bucket_Panel = new System.Windows.Forms.Panel();
                        this.Tools_Pencil_Panel = new System.Windows.Forms.Panel();
                        this.Tools_Panel.SuspendLayout();
                        this.SuspendLayout();
                        // 
                        // SubChunksChunk_Panel
                        // 
                        this.SubChunksChunk_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        this.SubChunksChunk_Panel.Location = new System.Drawing.Point(15, 83);
                        this.SubChunksChunk_Panel.Name = "SubChunksChunk_Panel";
                        this.SubChunksChunk_Panel.Size = new System.Drawing.Size(256, 256);
                        this.SubChunksChunk_Panel.TabIndex = 0;
                        // 
                        // X_Textbox
                        // 
                        this.X_Textbox.Location = new System.Drawing.Point(72, 6);
                        this.X_Textbox.Name = "X_Textbox";
                        this.X_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.X_Textbox.TabIndex = 1;
                        // 
                        // label1
                        // 
                        this.label1.AutoSize = true;
                        this.label1.Location = new System.Drawing.Point(12, 9);
                        this.label1.Name = "label1";
                        this.label1.Size = new System.Drawing.Size(20, 13);
                        this.label1.TabIndex = 2;
                        this.label1.Text = "X: ";
                        // 
                        // label2
                        // 
                        this.label2.AutoSize = true;
                        this.label2.Location = new System.Drawing.Point(178, 9);
                        this.label2.Name = "label2";
                        this.label2.Size = new System.Drawing.Size(20, 13);
                        this.label2.TabIndex = 3;
                        this.label2.Text = "Z: ";
                        // 
                        // Z_Textbox
                        // 
                        this.Z_Textbox.Location = new System.Drawing.Point(238, 6);
                        this.Z_Textbox.Name = "Z_Textbox";
                        this.Z_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.Z_Textbox.TabIndex = 4;
                        // 
                        // label3
                        // 
                        this.label3.AutoSize = true;
                        this.label3.Location = new System.Drawing.Point(166, 29);
                        this.label3.Name = "label3";
                        this.label3.Size = new System.Drawing.Size(23, 13);
                        this.label3.TabIndex = 5;
                        this.label3.Text = "OR";
                        // 
                        // label4
                        // 
                        this.label4.AutoSize = true;
                        this.label4.Location = new System.Drawing.Point(12, 51);
                        this.label4.Name = "label4";
                        this.label4.Size = new System.Drawing.Size(54, 13);
                        this.label4.TabIndex = 6;
                        this.label4.Text = "X Chunk: ";
                        // 
                        // XChunk_Textbox
                        // 
                        this.XChunk_Textbox.Location = new System.Drawing.Point(72, 48);
                        this.XChunk_Textbox.Name = "XChunk_Textbox";
                        this.XChunk_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.XChunk_Textbox.TabIndex = 7;
                        // 
                        // label5
                        // 
                        this.label5.AutoSize = true;
                        this.label5.Location = new System.Drawing.Point(178, 51);
                        this.label5.Name = "label5";
                        this.label5.Size = new System.Drawing.Size(54, 13);
                        this.label5.TabIndex = 8;
                        this.label5.Text = "Z Chunk: ";
                        // 
                        // ZChunk_Textbox
                        // 
                        this.ZChunk_Textbox.Location = new System.Drawing.Point(238, 48);
                        this.ZChunk_Textbox.Name = "ZChunk_Textbox";
                        this.ZChunk_Textbox.Size = new System.Drawing.Size(100, 20);
                        this.ZChunk_Textbox.TabIndex = 9;
                        // 
                        // LoadChunk_Button
                        // 
                        this.LoadChunk_Button.Location = new System.Drawing.Point(362, 24);
                        this.LoadChunk_Button.Name = "LoadChunk_Button";
                        this.LoadChunk_Button.Size = new System.Drawing.Size(75, 23);
                        this.LoadChunk_Button.TabIndex = 10;
                        this.LoadChunk_Button.Text = "Load Chunk";
                        this.LoadChunk_Button.UseVisualStyleBackColor = true;
                        this.LoadChunk_Button.Click += new System.EventHandler(this.button1_Click);
                        // 
                        // MousePos_Timer
                        // 
                        this.MousePos_Timer.Enabled = true;
                        this.MousePos_Timer.Interval = 10;
                        this.MousePos_Timer.Tick += new System.EventHandler(this.MousePos_Timer_Tick);
                        // 
                        // RedBiome_Combobox
                        // 
                        this.RedBiome_Combobox.Enabled = false;
                        this.RedBiome_Combobox.FormattingEnabled = true;
                        this.RedBiome_Combobox.Location = new System.Drawing.Point(12, 358);
                        this.RedBiome_Combobox.Name = "RedBiome_Combobox";
                        this.RedBiome_Combobox.Size = new System.Drawing.Size(121, 21);
                        this.RedBiome_Combobox.TabIndex = 12;
                        // 
                        // BlueBiome_Combobox
                        // 
                        this.BlueBiome_Combobox.Enabled = false;
                        this.BlueBiome_Combobox.FormattingEnabled = true;
                        this.BlueBiome_Combobox.Location = new System.Drawing.Point(313, 358);
                        this.BlueBiome_Combobox.Name = "BlueBiome_Combobox";
                        this.BlueBiome_Combobox.Size = new System.Drawing.Size(121, 21);
                        this.BlueBiome_Combobox.TabIndex = 13;
                        // 
                        // label6
                        // 
                        this.label6.AutoSize = true;
                        this.label6.ForeColor = System.Drawing.Color.Red;
                        this.label6.Location = new System.Drawing.Point(42, 342);
                        this.label6.Name = "label6";
                        this.label6.Size = new System.Drawing.Size(59, 13);
                        this.label6.TabIndex = 14;
                        this.label6.Text = "Red Biome";
                        // 
                        // label7
                        // 
                        this.label7.AutoSize = true;
                        this.label7.ForeColor = System.Drawing.Color.Blue;
                        this.label7.Location = new System.Drawing.Point(344, 342);
                        this.label7.Name = "label7";
                        this.label7.Size = new System.Drawing.Size(60, 13);
                        this.label7.TabIndex = 15;
                        this.label7.Text = "Blue Biome";
                        // 
                        // ErrorBox_Richtextbox
                        // 
                        this.ErrorBox_Richtextbox.Location = new System.Drawing.Point(12, 414);
                        this.ErrorBox_Richtextbox.Name = "ErrorBox_Richtextbox";
                        this.ErrorBox_Richtextbox.ReadOnly = true;
                        this.ErrorBox_Richtextbox.Size = new System.Drawing.Size(422, 198);
                        this.ErrorBox_Richtextbox.TabIndex = 16;
                        this.ErrorBox_Richtextbox.Text = "";
                        // 
                        // ChangeBiomes_Button
                        // 
                        this.ChangeBiomes_Button.Enabled = false;
                        this.ChangeBiomes_Button.Location = new System.Drawing.Point(178, 385);
                        this.ChangeBiomes_Button.Name = "ChangeBiomes_Button";
                        this.ChangeBiomes_Button.Size = new System.Drawing.Size(90, 23);
                        this.ChangeBiomes_Button.TabIndex = 17;
                        this.ChangeBiomes_Button.Text = "Change Biomes";
                        this.ChangeBiomes_Button.UseVisualStyleBackColor = true;
                        this.ChangeBiomes_Button.Click += new System.EventHandler(this.ChangeBiomes_Button_Click);
                        // 
                        // Tools_Panel
                        // 
                        this.Tools_Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        this.Tools_Panel.Controls.Add(this.Tools_Line_Panel);
                        this.Tools_Panel.Controls.Add(this.Tools_Bucket_Panel);
                        this.Tools_Panel.Controls.Add(this.Tools_Pencil_Panel);
                        this.Tools_Panel.Location = new System.Drawing.Point(277, 83);
                        this.Tools_Panel.Name = "Tools_Panel";
                        this.Tools_Panel.Size = new System.Drawing.Size(157, 256);
                        this.Tools_Panel.TabIndex = 18;
                        // 
                        // Tools_Line_Panel
                        // 
                        this.Tools_Line_Panel.BackgroundImage = global::Minecraft_Biome_Changer.Properties.Resources.line;
                        this.Tools_Line_Panel.Location = new System.Drawing.Point(79, 3);
                        this.Tools_Line_Panel.Name = "Tools_Line_Panel";
                        this.Tools_Line_Panel.Size = new System.Drawing.Size(32, 32);
                        this.Tools_Line_Panel.TabIndex = 2;
                        this.Tools_Line_Panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tools_Pencil_Panel_MouseClick);
                        this.Tools_Line_Panel.MouseEnter += new System.EventHandler(this.Tools_Pencil_Panel_MouseEnter);
                        this.Tools_Line_Panel.MouseLeave += new System.EventHandler(this.Tools_Pencil_Panel_MouseLeave);
                        // 
                        // Tools_Bucket_Panel
                        // 
                        this.Tools_Bucket_Panel.BackColor = System.Drawing.Color.Transparent;
                        this.Tools_Bucket_Panel.BackgroundImage = global::Minecraft_Biome_Changer.Properties.Resources.bucket;
                        this.Tools_Bucket_Panel.Location = new System.Drawing.Point(41, 3);
                        this.Tools_Bucket_Panel.Name = "Tools_Bucket_Panel";
                        this.Tools_Bucket_Panel.Size = new System.Drawing.Size(32, 32);
                        this.Tools_Bucket_Panel.TabIndex = 1;
                        this.Tools_Bucket_Panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tools_Pencil_Panel_MouseClick);
                        this.Tools_Bucket_Panel.MouseEnter += new System.EventHandler(this.Tools_Pencil_Panel_MouseEnter);
                        this.Tools_Bucket_Panel.MouseLeave += new System.EventHandler(this.Tools_Pencil_Panel_MouseLeave);
                        // 
                        // Tools_Pencil_Panel
                        // 
                        this.Tools_Pencil_Panel.BackColor = System.Drawing.Color.Gray;
                        this.Tools_Pencil_Panel.BackgroundImage = global::Minecraft_Biome_Changer.Properties.Resources.pencil;
                        this.Tools_Pencil_Panel.Location = new System.Drawing.Point(3, 3);
                        this.Tools_Pencil_Panel.Name = "Tools_Pencil_Panel";
                        this.Tools_Pencil_Panel.Size = new System.Drawing.Size(32, 32);
                        this.Tools_Pencil_Panel.TabIndex = 0;
                        this.Tools_Pencil_Panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tools_Pencil_Panel_MouseClick);
                        this.Tools_Pencil_Panel.MouseEnter += new System.EventHandler(this.Tools_Pencil_Panel_MouseEnter);
                        this.Tools_Pencil_Panel.MouseLeave += new System.EventHandler(this.Tools_Pencil_Panel_MouseLeave);
                        // 
                        // SubChunks_Form
                        // 
                        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.ClientSize = new System.Drawing.Size(446, 624);
                        this.Controls.Add(this.Tools_Panel);
                        this.Controls.Add(this.ChangeBiomes_Button);
                        this.Controls.Add(this.ErrorBox_Richtextbox);
                        this.Controls.Add(this.label7);
                        this.Controls.Add(this.label6);
                        this.Controls.Add(this.BlueBiome_Combobox);
                        this.Controls.Add(this.RedBiome_Combobox);
                        this.Controls.Add(this.LoadChunk_Button);
                        this.Controls.Add(this.ZChunk_Textbox);
                        this.Controls.Add(this.label5);
                        this.Controls.Add(this.XChunk_Textbox);
                        this.Controls.Add(this.label4);
                        this.Controls.Add(this.label3);
                        this.Controls.Add(this.Z_Textbox);
                        this.Controls.Add(this.label2);
                        this.Controls.Add(this.label1);
                        this.Controls.Add(this.X_Textbox);
                        this.Controls.Add(this.SubChunksChunk_Panel);
                        this.Name = "SubChunks_Form";
                        this.Text = "Sub Chunk Biome Editor";
                        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SubChunks_Form_FormClosing);
                        this.Load += new System.EventHandler(this.SubChunks_Form_Load);
                        this.Tools_Panel.ResumeLayout(false);
                        this.ResumeLayout(false);
                        this.PerformLayout();

                }

                #endregion

                private System.Windows.Forms.Panel SubChunksChunk_Panel;
                private System.Windows.Forms.TextBox X_Textbox;
                private System.Windows.Forms.Label label1;
                private System.Windows.Forms.Label label2;
                private System.Windows.Forms.TextBox Z_Textbox;
                private System.Windows.Forms.Label label3;
                private System.Windows.Forms.Label label4;
                private System.Windows.Forms.TextBox XChunk_Textbox;
                private System.Windows.Forms.Label label5;
                private System.Windows.Forms.TextBox ZChunk_Textbox;
                private System.Windows.Forms.Button LoadChunk_Button;
                private System.Windows.Forms.Timer MousePos_Timer;
                private System.Windows.Forms.ComboBox RedBiome_Combobox;
                private System.Windows.Forms.ComboBox BlueBiome_Combobox;
                private System.Windows.Forms.Label label6;
                private System.Windows.Forms.Label label7;
                private System.Windows.Forms.RichTextBox ErrorBox_Richtextbox;
                private System.Windows.Forms.Button ChangeBiomes_Button;
                private System.Windows.Forms.Panel Tools_Panel;
                private System.Windows.Forms.Panel Tools_Pencil_Panel;
                private System.Windows.Forms.Panel Tools_Bucket_Panel;
                private System.Windows.Forms.Panel Tools_Line_Panel;
        }
}