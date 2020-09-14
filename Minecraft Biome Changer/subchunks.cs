using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Minecraft_Biome_Changer
{
        public partial class SubChunks_Form : Form
        {
                //row then col
                public Dictionary<int, Dictionary<int, Panel>> sub_chunks;
                int mouse_button;

                public Dictionary<string, byte> mc_biomes;
                public Dictionary<string, byte>.KeyCollection mc_biome_names;
                public MainForm Main;
                public string Selected_World;
                byte[] chunk_biome_data;
                int chunk_biome_data_size;
                int chunkX;
                int chunkZ;
                bool chunks_set;
                Panel CurrentTool;
                Dictionary<string, bool> Bucket_Colored;
                Point Line_Start;
                Point Line_End;

                public SubChunks_Form()
                {
                        InitializeComponent();
                }

                public void ComboBox_Fill(ComboBox cBox, string[] cAry)
                {
                        for (int cf = 0; cf < cAry.Length; cf++)
                        {
                                cBox.Items.Add(cAry[cf]);
                        }
                }

                private byte[] Plains_Biome()
                {
                        byte[] biome = new byte[chunk_biome_data_size];

                        for (int b = 0; b < chunk_biome_data_size; b++)
                        {
                                if (b % 4 == 3)
                                {
                                        biome[b] = 1;
                                }
                                else
                                {
                                        biome[b] = 0;
                                }
                        }

                        return biome;
                }

                private void SubChunks_Form_Load(object sender, EventArgs e)
                {
                        chunk_biome_data_size = 1024;

                        mouse_button = 0;       //0 = none, 1 = left, 2 = right, 3 = middle
                        sub_chunks = new Dictionary<int, Dictionary<int, Panel>>();
                        ComboBox_Fill(RedBiome_Combobox, mc_biome_names.ToArray());
                        ComboBox_Fill(BlueBiome_Combobox, mc_biome_names.ToArray());
                        chunk_biome_data = Plains_Biome();
                        chunkX = 0;
                        chunkZ = 0;
                        chunks_set = false;
                        CurrentTool = Tools_Pencil_Panel;
                        Bucket_Colored = new Dictionary<string, bool>();
                        Line_Start = new Point(-1, -1);
                        Line_End = new Point(-1, -1);

                        int height = 16;
                        int width = 16;

                        for (int r = 0; r < 16; r++)
                        {
                                Dictionary<int, Panel> sc_row = new Dictionary<int, Panel>();

                                for (int c = 0; c < 16; c++)
                                {
                                        Panel sub_chunk = new Panel();

                                        //sub_chunk.BackColor = Color.Red;
                                        sub_chunk.BackColor = Color.Gray;
                                        sub_chunk.BorderStyle = BorderStyle.FixedSingle;
                                        sub_chunk.Height = height;
                                        sub_chunk.Width = width;
                                        sub_chunk.Location = new Point(width * c, height * r);

                                        sub_chunk.MouseDown += SubChunks_MouseDown;
                                        sub_chunk.MouseUp += SubChunks_MouseUp;

                                        SubChunksChunk_Panel.Controls.Add(sub_chunk);

                                        sc_row.Add(c, sub_chunk);
                                }

                                sub_chunks.Add(r, sc_row);
                        }
                }

                private void SubChunks_MouseDown(object sender, MouseEventArgs e)
                {
                        if (e.Button == MouseButtons.Left)
                        {
                                mouse_button = 1;
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                                mouse_button = 2;
                        }
                        else
                        {
                                mouse_button = 3;
                        }
                }

                private void SubChunks_MouseUp(object sender, MouseEventArgs e)
                {
                        mouse_button = 0;
                }

                private void Check_Mouse_Pencil()
                {
                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        if (MouseIsInControl(sub_chunks[r][c]))
                                        {
                                                Panel active_panel = sub_chunks[r][c];

                                                //left
                                                if (mouse_button == 1)
                                                {
                                                        active_panel.BackColor = Color.Blue;
                                                }
                                                //right
                                                else if (mouse_button == 2)
                                                {
                                                        active_panel.BackColor = Color.Red;
                                                }
                                                //middle
                                                else
                                                {

                                                }

                                        }
                                }
                        }
                }

                private bool Already_Colored(int r, int c)
                {
                        string key = r.ToString() + "." + c.ToString();

                        return Bucket_Colored.ContainsKey(key);
                }

                private void Bucket_Fill(int r, int c, Color New_Color, Color Old_Color)   //direction is where it came from
                {
                        //if reached an edge, then return
                        if (r > 15 || r < 0 || c > 15 || c < 0 || Already_Colored(r, c))
                        {
                                return;
                        }
                        else
                        {
                                Panel panel = sub_chunks[r][c];

                                //if this color needs to be changed to the new color
                                if (panel.BackColor == Old_Color)
                                {
                                        //update current back color
                                        panel.BackColor = New_Color;

                                        string key = r.ToString() + "." + c.ToString();

                                        Bucket_Colored.Add(key, true);

                                        Bucket_Fill(r + 1, c, New_Color, Old_Color);
                                        Bucket_Fill(r - 1, c, New_Color, Old_Color);
                                        Bucket_Fill(r, c + 1, New_Color, Old_Color);
                                        Bucket_Fill(r, c - 1, New_Color, Old_Color);
                                }
                        }
                }

                private void Check_Mouse_Bucket()
                {
                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        if (MouseIsInControl(sub_chunks[r][c]))
                                        {
                                                Panel active_panel = sub_chunks[r][c];

                                                //left
                                                if (mouse_button == 1)
                                                {
                                                        Bucket_Fill(r, c, Color.Blue, active_panel.BackColor);
                                                        Bucket_Colored = new Dictionary<string, bool>();        //clear the already colored values for next usage of bucket
                                                }
                                                //right
                                                else if (mouse_button == 2)
                                                {
                                                        Bucket_Fill(r, c, Color.Red, active_panel.BackColor);
                                                        Bucket_Colored = new Dictionary<string, bool>();        //clear the already colored values for next usage of bucket
                                                }
                                                //middle
                                                else
                                                {

                                                }

                                        }
                                }
                        }
                }

                private double Slope(Point p1, Point p2)
                {
                        double slope;
                        int r1 = p1.X;
                        int c1 = p1.Y;
                        int r2 = p2.X;
                        int c2 = p2.Y;
                        double top = (c2 - c1);
                        double bottom = (r2 - r1);

                        if (bottom != 0) {
                                slope = top / bottom;
                        }
                        else
                        {
                                slope = 0;
                        }

                        return slope;
                }

                private int Min(int num1, int num2)
                {
                        if (num1 < num2)
                        {
                                return num1;
                        }
                        else
                        {
                                return num2;
                        }
                }

                private int Max(int num1, int num2)
                {
                        if (num1 > num2)
                        {
                                return num1;
                        }
                        else
                        {
                                return num2;
                        }
                }

                private int Slope_Add(int start, double slope, int times)
                {
                        double added = start;

                        for (int t = 0; t < times; t++)
                        {
                                added += slope;
                        }

                        return (int)added;
                }

                private void Line_Set(int row, int col, Color line_color)
                {
                        if (row >= 0 && col >= 0 && row < 16 && col < 16)
                        {
                                if (sub_chunks[row][col].Tag == null)
                                {
                                        sub_chunks[row][col].Tag = sub_chunks[row][col].BackColor;
                                        sub_chunks[row][col].BackColor = line_color;
                                }
                        }
                }
                
                private void Update_Line(Point start, Point end, Color line_color)
                {
                        double slope = Slope(start, end);
                        int r1 = Line_Start.X;
                        int r2 = Line_End.X;
                        int c1 = Line_Start.Y;
                        int c2 = Line_End.Y;
                        int min_r = Min(r1, r2);
                        int max_r = Max(r1, r2);
                        int min_c = Min(c1, c2);
                        int max_c = Max(c1, c2);
                        double cd;
                        double rd;
                        double r_slope;

                        if (slope == 0)
                        {
                                r_slope = 0;
                        }
                        else
                        {
                                r_slope = 1 / slope;
                        }
                        
                        if (slope < 0)
                        {
                                cd = max_c;
                                rd = max_r;
                        }
                        else
                        {
                                cd = min_c;
                                rd = min_r;
                        }
                        
                        for (int r = min_r; r <= max_r; r++)
                        {
                                int c = Round(cd);
                                //int c_slope = Round(cd + slope);

                                Line_Set(r, c, line_color);

                                //int starting_min = Min(c, c_slope) + c_add;
                                //int ending_max = Max(c, c_slope);
                                
                                //Line_Col_Fill(r, starting_min, ending_max, min_c, max_c);
                                
                                //while (c != (int)(cd + slope) && c >= min_c && c <= max_c)
                                //{
                                  //      Line_Set(r, c);

                                    //    c += c_add;
                                //}

                                cd += slope;
                        }

                        for (int c = min_c; c <= max_c; c++)
                        {
                                int r = Round(rd);

                                Line_Set(r, c, line_color);
                                
                                rd += r_slope;
                        }
                }

                private void Restore_Tag_Colors()
                {
                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        //update the back colors from Tag if tag is set to a color
                                        if (sub_chunks[r][c].Tag != null)
                                        {
                                                sub_chunks[r][c].BackColor = (Color)sub_chunks[r][c].Tag;
                                                sub_chunks[r][c].Tag = null;
                                        }
                                }
                        }
                }

                private void Remove_Tags()
                {
                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        //remove the tag from the sub chunk
                                        if (sub_chunks[r][c].Tag != null)
                                        {
                                                sub_chunks[r][c].Tag = null;
                                        }
                                }
                        }
                }

                private void Check_Mouse_Line()
                {
                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        if (MouseIsInControl(sub_chunks[r][c]))
                                        {
                                                Panel active_panel = sub_chunks[r][c];

                                                //left
                                                if (mouse_button == 1)
                                                {
                                                        if (Line_Start.X == -1)
                                                        {
                                                                Line_Start = new Point(r, c);
                                                        }
                                                        else
                                                        {
                                                                Line_End = new Point(r, c);
                                                                Restore_Tag_Colors();
                                                                Update_Line(Line_Start, Line_End, Color.Blue);
                                                        }
                                                }
                                                //right
                                                else if (mouse_button == 2)
                                                {
                                                        if (Line_Start.X == -1)
                                                        {
                                                                Line_Start = new Point(r, c);
                                                        }
                                                        else
                                                        {
                                                                Line_End = new Point(r, c);
                                                                Restore_Tag_Colors();
                                                                Update_Line(Line_Start, Line_End, Color.Red);
                                                        }
                                                }
                                                //middle
                                                else if (mouse_button == 3)
                                                {

                                                }
                                                //no click
                                                else
                                                {
                                                        //if just finished making a line
                                                        if (Line_Start.X != -1)
                                                        {
                                                                Remove_Tags();

                                                                Line_Start = new Point(-1, -1);
                                                                Line_End = new Point(-1, -1);
                                                        }
                                                }

                                        }
                                }
                        }
                }

                private void Check_Mouse_SubChunks()
                {
                        if (!chunks_set) return;        //do nothing if a chukn hasn't been imported yet

                        if (CurrentTool.Name == "Tools_Line_Panel")
                        {
                                Check_Mouse_Line();
                        }
                        else if (mouse_button == 0)
                        {
                                return;  //do nothing if mouse is not clicked
                        }

                        if (CurrentTool.Name == "Tools_Pencil_Panel")
                        {
                                Check_Mouse_Pencil();
                        }
                        else if (CurrentTool.Name == "Tools_Bucket_Panel")
                        {
                                
                                Check_Mouse_Bucket();
                        }
                        else if (CurrentTool.Name == "Tools_Line_Panel")
                        {
                                Check_Mouse_Line();
                        }
                }

                private void MousePos_Timer_Tick(object sender, EventArgs e)
                {
                        Check_Mouse_SubChunks();
                }

                private void SubChunksChunk_Panel_MouseDown(object sender, MouseEventArgs e)
                {
                        
                }
                
                //https://stackoverflow.com/questions/8159534/net-how-to-check-if-the-mouse-is-in-a-control
                public Boolean MouseIsInControl(Control control)
                {
                        //return (control.Bounds.Contains(MousePosition));
                        return control.ClientRectangle.Contains(control.PointToClient(MousePosition));
                        //return control.Bounds.Contains(Cursor.Position);
                }

                private int Get_Chunk(int cordinate)
                {
                        int chunk_cord = cordinate / 16;
                        if (cordinate < 0) chunk_cord--;
                        return chunk_cord;
                }

                private void button1_Click(object sender, EventArgs e)
                {
                        string x_str = X_Textbox.Text;
                        string z_str = Z_Textbox.Text;
                        string x_chunk_str = XChunk_Textbox.Text;
                        string z_chunk_str = ZChunk_Textbox.Text;
                        int x;
                        int z;
                        int x_chunk;
                        int z_chunk;
                        string zlib_path;
                        string chunk_path;

                        if (Is_Num(x_str) && Is_Num(z_str))
                        {
                                x = Parse_Int(x_str);
                                z = Parse_Int(z_str);

                                x_chunk = Get_Chunk(x);
                                z_chunk = Get_Chunk(z);
                        }
                        else if (Is_Num(x_chunk_str) && Is_Num(z_chunk_str))
                        {
                                x_chunk = Parse_Int(x_chunk_str);
                                z_chunk = Parse_Int(z_chunk_str);
                        }
                        else
                        {
                                log("Invalid X or Z");
                                return;
                        }

                        //remove data from previous loaded chunk (if there was one)
                        if (chunks_set)
                        {
                                //get the path of the previously loaded chunk to delete the files
                                zlib_path = Get_Zlib_Path(chunkX, chunkZ);
                                chunk_path = Get_Chunk_Path(chunkX, chunkZ);

                                //delete the zlib file
                                Delete_File(zlib_path);

                                //delete the chunk file
                                Delete_File(chunk_path);

                                chunkX = chunkZ = 0;    //clear the cordinates
                                zlib_path = chunk_path = "";    //clear the paths
                                chunks_set = false;     //let the program know there is no currently loaded chunk
                        }

                        //get the default biome data
                        chunk_biome_data = Plains_Biome();

                        //extract the zlib file from the mca file
                        Extract_Zlib(x_chunk, z_chunk);

                        //decompress zlib file
                        zlib_path = Get_Zlib_Path(x_chunk, z_chunk);
                        Decompress_Zlib(zlib_path);

                        //open the decompressed chunk file, and get the current biome data
                        chunk_path = Get_Chunk_Path(x_chunk, z_chunk);
                        chunk_biome_data = Get_Biome_From_File(chunk_path);

                        //update the gui
                        Update_Biome_GUI();

                        //enable the biome boxes and change biome button
                        RedBiome_Combobox.Enabled = true;
                        BlueBiome_Combobox.Enabled = true;
                        ChangeBiomes_Button.Enabled = true;

                        chunkX = x_chunk;
                        chunkZ = z_chunk;
                        chunks_set = true;
                }

                private void Update_Biome_GUI()
                {
                        byte red_biome = chunk_biome_data[3];   //first biome found in the data will be the "red" biome
                        byte blue_biome = red_biome;    //blue biome doesnt exist until we find a value different than "red" biome
                        byte cur_biome;
                        int index;
                        string red_biome_name;
                        string blue_biome_name;

                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        index = c + r * 16;
                                        index *= 4;     //skip every 4 bytes because each entry is 4 bytes long
                                        cur_biome = chunk_biome_data[index + 3];        //skip the first 3 bytes, they don't matter

                                        if (cur_biome == red_biome)
                                        {
                                                sub_chunks[r][c].BackColor = Color.Red;
                                        }
                                        else
                                        {
                                                if (blue_biome == red_biome)
                                                {
                                                        blue_biome = cur_biome;
                                                }

                                                sub_chunks[r][c].BackColor = Color.Blue;
                                        }
                                }
                        }
                        
                        //update the biomes list in the gui
                        red_biome_name = Get_Key(mc_biomes, (byte)red_biome);
                        blue_biome_name = Get_Key(mc_biomes, (byte)blue_biome);
                        Combobox_Select_Item(RedBiome_Combobox, red_biome_name);
                        Combobox_Select_Item(BlueBiome_Combobox, blue_biome_name);
                }

                private void Combobox_Select_Item(ComboBox box, string value)
                {
                        for (int i = 0; i < box.Items.Count; i++)
                        {
                                box.SelectedIndex = i;

                                if (box.SelectedItem.ToString() == value)
                                {
                                        return;
                                }
                        }

                        box.SelectedIndex = -1; //could not find the value in the combobox
                }

                private string Get_Key(Dictionary<string, byte> the_list, byte value)
                {
                        string key = "";
                        byte val;
                        Dictionary<string, byte>.KeyCollection keys = the_list.Keys;

                        for (int k = 0; k < keys.Count; k++)
                        {
                                key = keys.ElementAt(k);
                                val = the_list[key];

                                if (val == value)
                                {
                                        return key;
                                }
                        }

                        return "";      //could not find the given value
                }

                private byte[] Get_Biome_From_File(string chunk_path)
                {
                        byte[] biome_data = new byte[chunk_biome_data_size + 1];
                        byte[] biomes_in_binary = { 0x42, 0x69, 0x6F, 0x6D, 0x65, 0x73 };
                        int biome_offset = -1;
                        FileStream chunk_file;

                        chunk_file = Open_File(chunk_path);
                        if (chunk_file == null)
                        {
                                log("Could not open " + chunk_path);
                                return null;
                        }

                        biome_offset = File_Find(chunk_file, biomes_in_binary, 6);
                        if (biome_offset == -1)
                        {
                                log("Could not find biome offset inside decompressed chunk file");
                                chunk_file.Close();
                                return null;
                        }
                        biome_offset += 10;

                        chunk_file.Seek(biome_offset, 0);
                        chunk_file.Read(biome_data, 0, chunk_biome_data_size);

                        chunk_file.Close();

                        return biome_data;
                }

                private string Get_Chunk_Path(int x_chunk, int z_chunk)
                {
                        string chunk_path;
                        int x_region;
                        int z_region;
                        int x_index;
                        int z_index;

                        x_region = Get_Region(x_chunk);
                        z_region = Get_Region(z_chunk);
                        x_index = Get_Index(x_chunk);
                        z_index = Get_Index(z_chunk);

                        chunk_path = "r." + x_region + "." + z_region + ".chunk." + x_index + "." + z_index;

                        return chunk_path;
                }

                private string Get_Zlib_Path(int x_chunk, int z_chunk)
                {
                        string zlib_path;
                        string chunk_path;
                        
                        chunk_path = Get_Chunk_Path(x_chunk, z_chunk);
                        zlib_path = chunk_path + ".zlib";

                        return zlib_path;
                }

                private bool Extract_Zlib(int x_chunk, int z_chunk)
                {
                        int zlib_per_sector = 4096;

                        int x_region;
                        int z_region;
                        int header_offset;
                        int zlib_offset;
                        int zlib_sectors;
                        int zlib_length_length;
                        int zlib_length_int;
                        int zlib_max_size;
                        int x_index;
                        int z_index;
                        string mca_path;
                        string Regions_Folder = Main.mc_saves + Selected_World + "\\Region\\";
                        string chunk_path;
                        string zlib_path;
                        byte[] zlib_length;
                        byte[] zlib_data;
                        FileStream mca_file;

                        x_region = Get_Region(x_chunk);
                        z_region = Get_Region(z_chunk);

                        //open the mca file
                        mca_path = Regions_Folder + "r." + x_region + "." + z_region + ".mca";
                        mca_file = Open_File(mca_path);
                        if (mca_file == null)
                        {
                                log("Could not open " + mca_path);
                                return false;
                        }

                        //get the offset from the header
                        header_offset = Get_Offset(x_chunk, z_chunk);

                        //get the offset of the zlib data
                        zlib_offset = Get_File_Data(mca_file, header_offset, 3);
                        zlib_sectors = Get_File_Data(mca_file, header_offset + 3, 1);
                        if (zlib_offset == 0)
                        {
                                log("No chunk data found");
                                mca_file.Close();
                                return false;
                        }
                        zlib_offset *= 4096;

                        //get the length of the zlib file +1 for the compressrion type
                        mca_file.Seek(zlib_offset, 0);
                        zlib_length_length = 4;
                        zlib_length = new byte[zlib_length_length + 1];
                        mca_file.Read(zlib_length, 0, zlib_length_length);
                        zlib_length_int = Parse_Int(zlib_length, zlib_length_length);
                        zlib_length_int--;      //go ahead and make zlib_length_int equal to the zlib file length (removing the compression type from being counted)

                        //get the zlib data
                        mca_file.Seek(zlib_offset + zlib_length_length + 1, 0); //get passed the length field and compression type field
                        zlib_max_size = zlib_sectors * zlib_per_sector;
                        zlib_data = new byte[zlib_max_size];
                        mca_file.Read(zlib_data, 0, zlib_length_int);

                        x_index = Get_Index(x_chunk);
                        z_index = Get_Index(z_chunk);

                        //save zlib to file
                        chunk_path = "r." + x_region + "." + z_region + ".chunk." + x_index + "." + z_index;
                        zlib_path = chunk_path + ".zlib";

                        if (!Write_File(zlib_path, zlib_data, 0, zlib_length_int))
                        {
                                log("Could not save zlib file");
                                mca_file.Close();
                                return false;
                        }

                        mca_file.Close();
                        return true;
                }

                private void log(string text)
                {
                        ErrorBox_Richtextbox.Text += text + Environment.NewLine;
                        ErrorBox_Richtextbox.ScrollToCaret();
                }

                //main form functions used in this form
                private int Round(double num)
                {
                        return Main.Round(num);
                }
                private double abs(double value)
                {
                        return Main.abs(value);
                }
                private int abs(int value)
                {
                        return Main.abs(value);
                }
                private bool Import_Zlib(FileStream mca_file, string zlib_path, byte[] sector_offset, int number_sectors)
                {
                        return Main.Import_Zlib(mca_file, zlib_path, sector_offset, number_sectors);
                }
                private bool Import_Zlib(FileStream mca_file, string zlib_path, int calculated_zlib_offset, int number_sectors)
                {
                        return Main.Import_Zlib(mca_file, zlib_path, calculated_zlib_offset, number_sectors);
                }
                private bool Compress_Zlib(string file_path)
                {
                        return Main.Compress_Zlib(file_path);
                }
                private bool Delete_File(string file_path)
                {
                        return Main.Delete_File(file_path);
                }
                private int File_Find(FileStream file, byte[] data, int size)
                {
                        return Main.File_Find(file, data, size);
                }
                private bool Decompress_Zlib(string zlib_path)
                {
                        return Main.Decompress_Zlib(zlib_path);
                }
                private bool Write_File(string path, byte[] data, int offset, int size)
                {
                        return Main.Write_File(path, data, offset, size);
                }

                private bool Write_File(FileStream file, byte[] data, int offset_file_file, int data_size)
                {
                        return Main.Write_File(file, data, offset_file_file, data_size);
                }

                private int Get_File_Data(FileStream file, int offset, int num_bytes)
                {
                        return Main.Get_File_Data(file, offset, num_bytes);
                }

                private int Get_Offset(int chunkX, int chunkZ)
                {
                        return Main.Get_Offset(chunkX, chunkZ);
                }

                private FileStream Open_File(string path)
                {
                        return Main.Open_File(path);
                }

                private int Get_Region(int chunkCordinate)
                {
                        return Main.Get_Region(chunkCordinate);
                }

                private int Get_Index(int chunkCordinate)
                {
                        return Main.Get_Index(chunkCordinate);
                }

                private int Parse_Int(string data)
                {
                        return Main.Parse_Int(data);
                }
                
                private int Power(int number, int power)
                {
                        return Main.Power(number, power);
                }

                private int Parse_Int(byte[] data, int size)
                {
                        return Main.Parse_Int(data, size);
                }

                private bool Is_Num(string number)
                {
                        return Main.Is_Num(number);
                }

                //re-enable everything on the main form when this one closes
                private void SubChunks_Form_FormClosing(object sender, FormClosingEventArgs e)
                {
                        string zlib_path;
                        string chunk_path;
                        int x_chunk = chunkX;
                        int z_chunk = chunkZ;

                        Main.Set_Enabled(true);

                        if (chunks_set)
                        {
                                zlib_path = Get_Zlib_Path(x_chunk, z_chunk);
                                chunk_path = Get_Chunk_Path(x_chunk, z_chunk);

                                //delete the zlib file (if any)
                                Delete_File(zlib_path);

                                //delete the chunk file (if any)
                                Delete_File(chunk_path);
                        }

                        Main.SubChunk_Form = null;      //let the main form no that we no longer exist
                }

                private void ChangeBiomes_Button_Click(object sender, EventArgs e)
                {
                        string zlib_path;
                        string chunk_path;
                        int x_chunk = chunkX;
                        int z_chunk = chunkZ;

                        if (!chunks_set)
                        {
                                log("Load a chunk to edit before saving");
                                return;
                        }

                        if (RedBiome_Combobox.SelectedIndex == -1 || BlueBiome_Combobox.SelectedIndex == -1)
                        {
                                log("Enter a biome for BOTH red and blue");
                                return;
                        }

                        //update the chunk biome variable from the gui grid and comboboxes
                        Update_From_GUI();

                        //save the new updated chunk biome to the file
                        if (!Save_Biome())
                        {
                                log("Could not update biome data in file");
                                return;
                        }

                        //delete the old zlib file
                        zlib_path = Get_Zlib_Path(x_chunk, z_chunk);
                        if (!Delete_File(zlib_path))
                        {
                                log("Could not delete the old zlib file");
                                return;
                        }

                        //compress the updated chunk
                        chunk_path = Get_Chunk_Path(x_chunk, z_chunk);
                        if (!Compress_Zlib(chunk_path))
                        {
                                log("Could not compress chunk file");
                                return;
                        }

                        //import the new zlib into the mca file
                        if (!Import_Zlib(x_chunk, z_chunk))
                        {
                                log("Could not import new zlib file into the mca file");
                                Delete_File(zlib_path);
                                return;
                        }
                        
                        //delete the zlib file
                        if (!Delete_File(zlib_path))
                        {
                                log("Could not delete the new zlib file");
                        }

                        //delete the chunk file
                        //if (!Delete_File(chunk_path))
                        //{
                          //      log("Could not delete the new chunk file");
                        //}

                        log("Updated Chunk Biome(s)");
                }

                private bool Import_Zlib(int x_chunk, int z_chunk)
                {
                        int calculated_zlib_offset;
                        int number_sectors;
                        string zlib_path;
                        FileStream mca_file;
                        bool success;

                        mca_file = Get_Mca_File(x_chunk, z_chunk);
                        if (mca_file == null)
                        {
                                log("Could not open mca file");
                                return false;
                        }

                        zlib_path = Get_Zlib_Path(x_chunk, z_chunk);
                        calculated_zlib_offset = Get_Zlib_Offset(mca_file, x_chunk, z_chunk);
                        number_sectors = Get_Number_Sectors(mca_file, x_chunk, z_chunk);

                        success = Import_Zlib(mca_file, zlib_path, calculated_zlib_offset, number_sectors);

                        mca_file.Close();

                        return success;
                }

                private int Get_Zlib_Offset(FileStream mca_file, int x_chunk, int z_chunk)
                {
                        int header_offset = Get_Offset(x_chunk, z_chunk);
                        byte[] zlib_offset = new byte[4];
                        int zlib_offset_int = -1;

                        mca_file.Seek(header_offset, 0);
                        mca_file.Read(zlib_offset, 0, 3);
                        zlib_offset_int = Parse_Int(zlib_offset, 3);

                        return zlib_offset_int * 4096;
                }

                private int Get_Number_Sectors(FileStream mca_file, int x_chunk, int z_chunk)
                {
                        int header_offset = Get_Offset(x_chunk, z_chunk) + 3;
                        byte[] num_sectors = new byte[2];
                        int num_sectors_int = -1;

                        mca_file.Seek(header_offset, 0);
                        mca_file.Read(num_sectors, 0, 1);
                        num_sectors_int = Parse_Int(num_sectors, 1);

                        return num_sectors_int;
                }

                private FileStream Get_Mca_File(int x_chunk, int z_chunk)
                {
                        int x_region = Get_Region(x_chunk);
                        int z_region = Get_Region(z_chunk);
                        string Regions_Folder = Main.mc_saves + Selected_World + "\\Region\\";
                        string mca_path = Regions_Folder + "r." + x_region + "." + z_region + ".mca";
                        FileStream mca_file = Open_File(mca_path);
                        
                        return mca_file;
                }
                
                private bool Save_Biome()
                {
                        int x_chunk = chunkX;
                        int z_chunk = chunkZ;
                        int chunk_biome_data_offset;
                        string chunk_path = Get_Chunk_Path(x_chunk, z_chunk);
                        byte[] biomes_in_binary = { 0x42, 0x69, 0x6F, 0x6D, 0x65, 0x73 };
                        FileStream chunk_file = Open_File(chunk_path);

                        chunk_biome_data_offset = File_Find(chunk_file, biomes_in_binary, 6);
                        chunk_biome_data_offset += 10;  //get passed "biomes" title
                        if (!Write_File(chunk_file, chunk_biome_data, chunk_biome_data_offset, chunk_biome_data_size))
                        {
                                chunk_file.Close();
                                return false;
                        }
                        
                        chunk_file.Close();

                        return true;
                }

                private void Update_From_GUI()
                {
                        string red_biome_name = RedBiome_Combobox.SelectedItem.ToString();
                        string blue_biome_name = BlueBiome_Combobox.SelectedItem.ToString();
                        byte red_biome = mc_biomes[red_biome_name];
                        byte blue_biome = mc_biomes[blue_biome_name];
                        byte new_biome;
                        int index;
                        Color cur_color;
                        
                        for (int r = 0; r < 16; r++)
                        {
                                for (int c = 0; c < 16; c++)
                                {
                                        cur_color = sub_chunks[r][c].BackColor;

                                        if (cur_color == Color.Red)
                                        {
                                                new_biome = red_biome;
                                        }
                                        else
                                        {
                                                new_biome = blue_biome;
                                        }

                                        index = c + r * 16;
                                        index *= 4;     //skip every 4 bytes because each entry is 4 bytes long
                                        chunk_biome_data[index + 3] = new_biome;        //update the sub chunk biome to the new biome
                                }
                        }
                }

                private void Tools_Pencil_Panel_MouseEnter(object sender, EventArgs e)
                {
                        Panel tool = (Panel)sender;

                        //if this tool is not selected
                        if (tool != CurrentTool)
                        {
                                tool.BackColor = Color.DarkGray;
                        }
                }

                private void Tools_Pencil_Panel_MouseLeave(object sender, EventArgs e)
                {
                        Panel tool = (Panel)sender;

                        //if this tool is not selected
                        if (tool != CurrentTool)
                        {
                                tool.BackColor = Color.Transparent;
                        }
                }

                private void Tools_Pencil_Panel_MouseClick(object sender, MouseEventArgs e)
                {
                        Panel tool = (Panel)sender;

                        //if this tool is not selected
                        if (tool != CurrentTool)
                        {
                                CurrentTool.BackColor = Color.Transparent;

                                CurrentTool = tool;

                                CurrentTool.BackColor = Color.Gray;
                        }
                }
        }
}
