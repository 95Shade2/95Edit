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
using System.Diagnostics;
using System.IO.Compression;
using System.Threading;

namespace Minecraft_Biome_Changer
{
        public partial class MainForm : Form
        {
                public string mc_saves;
                public Dictionary<string, byte> mc_biomes;
                public Dictionary<string, byte>.KeyCollection mc_biome_names;
                bool debug_mode;
                static string directoryPath;
                string biome_name;
                int progress;
                int max_progress;
                string log_text;
                string progress_bar_text;
                int progress_bar_value;
                int active_threads;
                string warning_text;
                bool processing;
                Random rnd;
                ReaderWriterLockSlim progress_lock;
                bool stop;
                public SubChunks_Form SubChunk_Form;

                public MainForm()
                {
                        InitializeComponent();
                }

                private void Form1_Load(object sender, EventArgs e)
                {
                        string[] save_folders;
                        debug_mode = false;
                        
                        //optimize code
                        //      make a thread for each region to process
                        //      keep region open until all edited chunks are saved back into the region
                        //add estimated time left
                        //add ignoring redudent chunk writing
                        //be able to generate empty chunks
                        //be able to generate a region
                        //be able to generate based on a user-defined generation
                        //      flat
                        //      custom layers
                        //              y=1 bedrock, y=2 stone, y=3 gold_blocks, etc.
                        //might add other mc generation options if can figure out how that works

                        mc_saves = "C:\\Users\\Shade\\AppData\\Roaming\\.minecraft\\saves\\";
                        mc_biomes = new Dictionary<string, byte>()
                        {
                                {"Badlands", 37 },
                                {"Badlands Plateau", 39 },
                                {"Bamboo Jungle", 168 },
                                {"Bamboo Jungle Hills", 169 },
                                {"Beach", 16 },
                                {"Birch Forest", 27 },
                                {"Birch Forest Hills", 28 },
                                {"Cold Ocean", 46 },
                                {"Dark Forest", 29 },
                                {"Dark Forest Hills", 157 },
                                {"Deep Cold Ocean", 49 },
                                {"Deep Frozen Ocean", 50 },
                                {"Deep Lukewarm Ocean", 48 },
                                {"Deep Ocean", 24 },
                                {"Deep Warm Ocean", 47 },
                                {"Desert", 2 },
                                {"Desert Hills", 17 },
                                {"Desert Lakes", 130 },
                                {"End Barrens", 43 },
                                {"End Highlands", 42 },
                                {"End Midlands", 41 },
                                {"Eroded Badlands", 165 },
                                {"Flower Forest", 132 },
                                {"Forest", 4 },
                                {"Frozen Ocean", 10 },
                                {"Frozen River", 11 },
                                {"Giant Spruce Taiga", 160 },
                                {"Giant Spruce Taiga Hills", 161 },
                                {"Giant Tree Taiga", 32 },
                                {"Giant Tree Taiga Hills", 33 },
                                {"Gravelly Mountains", 131 },
                                {"Ice Spikes", 140 },
                                {"Jungle", 21 },
                                {"Jungle Edge", 23 },
                                {"Jungle Hills", 22 },
                                {"Lukewarm Ocean", 45 },
                                {"Modified Badlands Plateau", 167 },
                                {"Gravelly Mountains+", 162 },
                                {"Modified Jungle", 149 },
                                {"Modified Jungle Edge", 151 },
                                {"Modified Wooded Badlands Plateau", 166 },
                                {"Mountain Edge", 20 },
                                {"Mountains", 3 },
                                {"Mushroom Field Shore", 15 },
                                {"Mushroom Fields", 14 },
                                {"Nether", 8 },
                                {"Ocean", 0 },
                                {"Plains", 1 },
                                {"River", 7 },
                                {"Savanna", 35 },
                                {"Savanna Plateau", 36 },
                                {"Shattered Savanna", 163 },
                                {"Shattered Savanna Plateau", 164 },
                                {"Small End Islands", 40 },
                                {"Snowy Beach", 26 },
                                {"Snowy Mountains", 13 },
                                {"Snowy Taiga", 30 },
                                {"Snowy Taiga Hills", 31 },
                                {"Snowy Taiga Mountains", 158 },
                                {"Snowy Tundra", 12 },
                                {"Stone Shore", 25 },
                                {"Sunflower Plains", 129 },
                                {"Swamp", 6 },
                                {"Swamp Hills", 134 },
                                {"Taiga", 5 },
                                {"Taiga Hills", 19 },
                                {"Taiga Mountains", 133 },
                                {"Tall Birch Forest", 155 },
                                {"Tall Birch Hills", 156 },
                                {"The End", 9 },
                                {"The Void", 127 },
                                {"Warm Ocean", 44 },
                                {"Wooded Badlands Plateau", 38 },
                                {"Wooded Hills", 18 },
                                {"Wooded Mountains", 34 }
                        };
                        mc_biome_names = mc_biomes.Keys;
                        Debug_RichTextbox.Visible = debug_mode;
                        directoryPath = @"c:\temp";
                        progress = 0;
                        ProgressBar_Label.Visible = debug_mode;
                        log_text = "";
                        progress_bar_text = "";
                        progress_bar_value = -1;
                        DisplayLog_Checkbox.Visible = debug_mode;
                        DisplayLog_Checkbox.Checked = debug_mode;
                        max_progress = 0;
                        active_threads = 0;
                        ActiveThreads_Label.Visible = debug_mode;
                        warning_text = "";
                        processing = false;
                        rnd = new Random();
                        progress_lock = new ReaderWriterLockSlim();
                        stop = false;
                        SubChunk_Form = null;

                        save_folders = System.IO.Directory.GetDirectories(mc_saves);
                        save_folders = Parse_Paths(save_folders);

                        Listbox_Fill(MCSaves_Listbox, save_folders);
                        ComboBox_Fill(Biome_ComboBox, mc_biome_names.ToArray());

                        //make the background "transparent" of the percentage over the progress bar
                        TransparetBackground(ProgressPercentage_Label);
                }

                public void ComboBox_Fill(ComboBox cBox, string[] cAry)
                {
                        for (int cf = 0; cf < cAry.Length; cf++)
                        {
                                cBox.Items.Add(cAry[cf]);
                        }
                }

                private string[] Parse_Paths(string[] PathsArray)
                {
                        for (int pp = 0; pp < PathsArray.Length; pp++)
                        {
                                PathsArray[pp] = Parse_Path(PathsArray[pp]);
                        }

                        return PathsArray;
                }

                private string Parse_Path(string path)
                {
                        int indexLastFolder = path.LastIndexOf("\\");

                        return path.Substring(indexLastFolder+1);
                }

                private void Listbox_Fill(ListBox TheListBox, string[] StrArray)
                {
                        for (int lf = 0; lf < StrArray.Length; lf++)
                        {
                                TheListBox.Items.Add(StrArray[lf]);
                        }
                }

                private void SelectWorld_Button_Click(object sender, EventArgs e)
                {
                        string world;

                        if (MCSaves_Listbox.SelectedIndex != -1)
                        {
                                world = MCSaves_Listbox.SelectedItem.ToString();
                                SelectedWorld_Label.Text = world;
                        }

                }

                public int abs(int value)
                {
                        if (value < 0)
                        {
                                return value * -1;
                        }
                        else
                        {
                                return value;
                        }
                }

                public double abs(double value)
                {
                        if (value < 0)
                        {
                                return value * -1;
                        }
                        else
                        {
                                return value;
                        }
                }

                private int diff_pos(int val1, int val2)
                {
                        if (val1 > val2)
                        {
                                return val1 - val2;
                        }
                        else
                        {
                                return val2 - val1;
                        }
                }

                private int diff_neg(int val1, int val2)
                {
                        return diff_pos(val1 * -1, val2 * -1);
                }

                private int diff_mix(int val1, int val2)
                {
                        int neg = 0;
                        int pos = 0;
                        
                        if (val1 > val2)
                        {
                                neg = val2;
                                pos = val1;
                        }
                        else
                        {
                                neg = val1;
                                pos = val2;
                        }

                        neg = abs(neg);  //make the negative positive

                        return pos + neg;       //the difference is how much each one is away from zero combined, so we add them together
                }

                private int difference(int val1, int val2)
                {
                        if (val1 >= 0 && val2 >= 0)
                        {
                                return diff_pos(val1, val2);
                        }
                        else if (val1 < 0 && val2 < 0)
                        {
                                return diff_neg(val1, val2);
                        }
                        else {
                                return diff_mix(val1, val2);
                        }
                }

                //taken from: https://stackoverflow.com/questions/1469764/run-command-prompt-commands
                public bool Delete_File(string file_path)
                {
                        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                        try
                        {
                                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C del " + file_path;
                                cmd.StartInfo = startInfo;
                                cmd.Start();
                                //wait until the file is deleted
                                cmd.WaitForExit();
                                log("Deleted " + file_path);
                                return true;
                        }
                        catch(IOException)
                        {
                                log("Could not delete file " + file_path);
                                return false;
                        }
                }

                public bool Decompress_Zlib(string zlib_path)
                {
                        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                        try
                        {
                                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C zdrop " + zlib_path;
                                cmd.StartInfo = startInfo;
                                cmd.Start();
                                //wait until the conversion is complete
                                cmd.WaitForExit();
                                log("Decompressed " + zlib_path);
                                return true;
                        }
                        catch (IOException)
                        {
                                log("Could not Decompress zlib file " + zlib_path);
                                return false;
                        }
                }

                public bool Compress_Zlib(string file_path)
                {
                        return Decompress_Zlib(file_path);
                }

                private bool Change_Biome(int chunkX, int chunkZ)
                {
                        int regionX;
                        int regionZ;
                        int indexX;
                        int indexZ;
                        int offset;
                        int offset_data_length = 3;
                        int sector_data_length = 1;
                        int offset_data_int;
                        int zlib_size_int;
                        int sector_count_int;
                        int zlib_size_length = 4;
                        int zlib_data_size = 4096;
                        int biome_offset;
                        int biome_size = 1024;
                        int zlib_byte = 0;
                        byte[] offset_data = new byte[offset_data_length + 1];
                        byte[] sector_count = new byte[sector_data_length + 1];
                        byte[] zlib_size = new byte[zlib_size_length + 1];
                        byte[] zlib_data = new byte[zlib_data_size + 1];
                        byte[] zlib_data2 = new byte[zlib_data_size + 1];       //for the 2nd sector if there is one
                        byte[] biomes_in_binary = { 0x42, 0x69, 0x6F, 0x6D, 0x65, 0x73 };
                        byte[] biome_single = { 0x0, 0x0, 0x0, 0x0 };
                        byte[] biome_data = new byte[biome_size + 1];
                        string chunk_filename;
                        string mca_path;
                        string Regions_Folder = mc_saves + SelectedWorld_Label.Text + "\\Region\\";
                        string chunk_path;
                        string zlib_path;
                        FileStream mca_file;
                        FileStream chunk_file;
                        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                        regionX = chunkX >> 5;
                        regionZ = chunkZ >> 5;

                        log("regionX: " + regionX);
                        log("regionZ: " + regionZ);

                        chunk_filename = "r." + regionX + "." + regionZ + ".mca";

                        log("chunk_filename: " + chunk_filename);

                        mca_path = Regions_Folder + chunk_filename;

                        log("mca_path: " + mca_path);

                        mca_file = Open_File(mca_path);
                        if (mca_file == null)
                        {
                                return false;
                        }
                        
                        log("mca_file: " + mca_file);

                        indexX = chunkX % 32;
                        indexZ = chunkZ % 32;

                        if (indexX < 0) indexX += 32;
                        if (indexZ < 0) indexZ += 32;

                        log("indexX: " + indexX);
                        log("indexZ: " + indexZ);
                        

                        //      get the chunk data location from the region file header

                        offset = 4 * ((chunkX & 31) + (chunkZ & 31) * 32);

                        log("offset: " + offset);

                        mca_file.Seek(offset, 0);

                        mca_file.Read(offset_data, 0, 3);
                        mca_file.Read(sector_count, 0, 1);
                        
                        offset_data_int = Parse_Int(offset_data, 3);
                        offset_data_int *= 4096;

                        log("offset_data_int: " + offset_data_int);

                        //if there is no chunk data
                        if (offset_data_int == 0)
                        {
                                log("No chunk data found at chunk " + chunkX + " " + chunkZ + " in region " + regionX + " " + regionZ);
                                mca_file.Close();
                                return false;
                        }

                        mca_file.Seek(offset_data_int, 0);

                        mca_file.Read(zlib_size, 0, 4);
                        
                        //size includes the compression type byte
                        zlib_size_int = Parse_Int(zlib_size, 4) - 1;

                        log("zlib_size_int: " + zlib_size_int);
                        
                        //      extract the zlib compressed chunk data

                        mca_file.Seek(offset_data_int + 5, 0);
                        sector_count_int = Parse_Int(sector_count, 1);  //get the number of sectors in int form to increase the size of the zlib byte holder
                        //if more than one sector
                        if (sector_count_int > 1)
                        {
                                zlib_data = new byte[zlib_data_size * sector_count_int + 1];    //plus one for added padding at end, simulatoring a null terminator
                        }
                        mca_file.Read(zlib_data, 0, zlib_size_int);

                        chunk_path = chunk_filename + ".chunk." + indexX + "." + indexZ;
                        zlib_path = chunk_path + ".zlib";
                        if (!Write_File(zlib_path, zlib_data, 0, zlib_size_int)) {
                                mca_file.Close();
                                return false;   //return if cannot write zlib file
                        }

                        mca_file.Close();

                        //      Decompress the chunk data

                        //return false if unable to decompress zlib file
                        if (!Decompress_Zlib(zlib_path)) return false;

                        //      Change the biome in the chunk data

                        //chunk_file = new FileStream(chunk_path, FileMode.Open);
                        chunk_file = Open_File(chunk_path);
                        if (chunk_file == null)
                        {
                                return false;
                        }

                        biome_offset = File_Find(chunk_file, biomes_in_binary, 6);

                        if (biome_offset == -1)
                        {
                                log("Cannot find biome offset inside decompressed chunk file for chunk " + chunkX + " " + chunkZ + " in region " + regionX + " " + regionZ);
                                chunk_file.Close();
                                return false;
                        }

                        biome_offset += 10;     //get passed "biomes" header before the actual biome data

                        log("biome_name: " + biome_name);

                        biome_single[3] = mc_biomes[biome_name];
                        
                        Fill_Bytes_From_Bytes(biome_data, biome_single, biome_size, 4);
                        
                        chunk_file.Close();

                        if (!Write_File(chunk_path, biome_data, biome_size, biome_offset)) return false; //return if cannot write the biome data
                        log("Wrote new biome data to " + chunk_path);

                        //      Compress the new chunk data

                        //return false if unable to delete zlib file
                        //remove the old zlib file before making the new one
                        if (!Delete_File(zlib_path)) return false;

                        //return if unable to compress chunk file
                        if (!Compress_Zlib(chunk_path)) return false;

                        //return false if unable to delete chunk file
                        //if (!Delete_File(chunk_path)) return false;

                        //      Import new compressed chunk data back into the region file

                        //chunk_file = new FileStream(zlib_path, FileMode.Open);
                        chunk_file = Open_File(zlib_path);
                        if (chunk_file == null)
                        {
                                return false;
                        }

                        //get the new zlib entry
                        //chunk_file.Read(zlib_data, 0, zlib_size_int);
                        for (int i = 0; i < zlib_data_size * sector_count_int; i++)
                        {
                                zlib_byte = chunk_file.ReadByte();

                                if (zlib_byte == -1)
                                {
                                        //i points to the byte after the end of the data, so if we leave it the same then it will include the bryte for the compression as well
                                        zlib_size_int = i;
                                        i = zlib_data_size * sector_count_int;
                                }
                                else
                                {
                                        zlib_data[i + 4 + 1] = (byte)zlib_byte; //+4 for length field, +1 for compression type field - +5 is where the data starts
                                }
                        }
                        log("zlib_size_int: " + zlib_size_int);

                        chunk_file.Close();

                        //zlib_size = Parse_Bytes(zlib_size_int, 4);
                        //log("zlib_size[0]: " + zlib_size[0]);
                        //log("zlib_size[1]: " + zlib_size[1]);
                        //log("zlib_size[2]: " + zlib_size[2]);
                        //log("zlib_size[3]: " + zlib_size[3]);

                        //make the first 4 bytes of data the length
                        if (!Copy_Bytes(zlib_data, zlib_size, zlib_data_size * sector_count_int, 4)) return false;
                        zlib_data[4] = 0x2;     //make the 5th byte the compression type which is always 2

                        //write the new size
                        //return false if unable to write the new zlib size to the region file
                        //if (!Write_File(mca_file, zlib_size, offset_data_int, 4)) return false;
                        //if (!Write_File(mca_path, zlib_size, 4, offset_data_int)) return false;

                        //write the length of bytes, the compression type (0x2), and the zlib data to the region file
                        //return false in unable to write new zlib file to region file
                        //if (!Write_File(mca_file, zlib_data, offset_data_int + 5, zlib_size_int - 1)) return false;

                        if (!Write_File(mca_path, zlib_data, zlib_size_int, offset_data_int)) return false;
                        //seemed like this 2nd one fills up C too?!?

                        //return false if unable to delete zlib file
                        if (!Delete_File(zlib_path)) return false;
                        
                        return true;
                }

                private bool Copy_Bytes(byte[] copy_to, byte[] copy_from, int to_size, int from_size)
                {
                        if (from_size > to_size)
                        {
                                log("Source bytes are greater than the destination bytes");
                                return false;
                        }
                        else
                        {
                                for (int i = 0; i < from_size; i++) {
                                        copy_to[i] = copy_from[i];
                                }

                                return true;
                        }
                }

                private int Index_Trailing_Zeros(byte[] data, int size)
                {
                        int i;

                        for (i = size-1; i >- 0; i--)
                        {
                                if (data[i] != 0)
                                {
                                        //the last byte was the start of the trailing zeroes
                                        return i++;
                                }
                        }

                        return -1;
                }

                //returns the region from the chunk given (chunk cordinate)
                public int Get_Region(int chunkCordinate)
                {
                        return chunkCordinate >> 5;
                }

                private void Add_To_Region(Dictionary<int, Dictionary<int, Dictionary<int, int[]>>>  regions, int regionX, int regionZ, int chunkX, int chunkZ)
                {
                        Dictionary<int, Dictionary<int, Dictionary<int, int[]>>>.KeyCollection regionXs = regions.Keys;
                        
                        if (!regionXs.Contains(regionX))
                        {
                                regions.Add(regionX, new Dictionary<int, Dictionary<int, int[]>>());
                        }
                        
                        Dictionary<int, Dictionary<int, int[]>>.KeyCollection regionZs = regions[regionX].Keys;


                        if (!regionZs.Contains(regionZ))
                        {
                                regions[regionX].Add(regionZ, new Dictionary<int, int[]>());
                        }
                        
                        int chunkCount = regions[regionX][regionZ].Count;
                        int[] chunkAry = new int[2];

                        chunkAry[0] = chunkX;
                        chunkAry[1] = chunkZ;

                        regions[regionX][regionZ].Add(chunkCount, chunkAry);

                        max_progress++;
                        log("Added " + chunkX + " " + chunkZ + " to region " + regionX + " " + regionZ);
                }

                public int Get_File_Data(FileStream file, int offset, int num_bytes)
                {
                        byte[] data = new byte[num_bytes + 1];

                        file.Seek(offset, 0);
                        file.Read(data, 0, num_bytes);

                        return Parse_Int(data, num_bytes);
                }

                public int Get_Index(int chunkCordinate)
                {
                        int index = chunkCordinate % 32;
                        if (index < 0)
                        {
                                index += 32;
                        }

                        return index;
                }

                private void Warning(string text)
                {
                        log(text);
                        warning_text += text + Environment.NewLine;
                }

                public int Get_Offset(int chunkX, int chunkZ)
                {
                        return 4 * ((chunkX & 31) + (chunkZ & 31) * 32);
                }

                private bool Copy_File(string old_file, string new_file)
                {
                        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                        try
                        {
                                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                startInfo.FileName = "cmd.exe";
                                startInfo.Arguments = "/C copy " + old_file + " " + new_file;
                                cmd.StartInfo = startInfo;
                                cmd.Start();
                                //wait until the file is deleted
                                cmd.WaitForExit();
                                log("Copied " + old_file + " to " + new_file);
                                return true;
                        }
                        catch (IOException)
                        {
                                log("Could not copy " + old_file + " to " + new_file);
                                return false;
                        }
                }

                private bool Create_Chunk(FileStream mca_file, int chunkX, int chunkZ, string biome_name)
                {

                        //get the offset of the header for the chunk
                        int offset = Get_Offset(chunkX, chunkZ);
                        log("new chunk's header offset: " + offset);

                        //get the location of an empty sector
                        byte[] zlib_offset = new byte[4];
                        byte[] zlib_sectors = new byte[2];
                        int sector_count = 2;   //"A chunk with an offset of 2 begins right after the timestamps table."
                        int sector_offset;
                        byte[] zlib_length = new byte[4];
                        int zlib_length_int;

                        do
                        {
                                sector_offset = sector_count * 4096;    //get length of zlib data (if any) of next sector
                                mca_file.Seek(sector_offset, 0);
                                mca_file.Read(zlib_length, 0, 3);
                                zlib_length_int = Parse_Int(zlib_length, 3);
                                sector_count++; //increase the count to check next sector
                        } while (zlib_length_int != 0 && sector_count < 1024 + 2); //repeat until a sector is empty     1024 + 2 is the max number of chunks per region (plus 1 because it starts at 2)

                        //could not find any empty sectors
                        if (zlib_length_int == 0)
                        {
                                log("No empty sectors for creating chunk " + chunkX + " " + chunkZ);
                                return false;
                        }

                        //copy empty chunk data and rename it
                        string empty_chunk_path = ".\\chunks\\empty_chunk";
                        int regionX = Get_Region(chunkX);
                        int regionZ = Get_Region(chunkZ);
                        int indexX = Get_Index(chunkX);
                        int indexZ = Get_Index(chunkZ);
                        string new_chunk_path = ".\\r" + regionX + "." + regionZ + "." + ".chunk." + indexX + "." + indexZ;

                        if (!Copy_File(empty_chunk_path, new_chunk_path))
                        {
                                Warning("Could not create new chunk for " + chunkX + " " + chunkZ);
                                return false;
                        }

                        //change biome of new chunk to the specified biome
                        if (!Change_Chunk_Biome(new_chunk_path, biome_name))
                        {
                                return false;
                        }

                        //compress the new chunk
                        if (!Compress_Zlib(empty_chunk_path)) {
                                Warning("Could not compress new empty chunk with new biome data");
                                return false;
                        }

                        //store the location of the sector in the header file
                        zlib_offset = Parse_Bytes(sector_offset / 4096, 3);     //have to convert it back into the raw form when storing the offset of the sector
                        if (!Write_File(mca_file, zlib_offset, offset, 3))
                        {
                                Warning("Could not write new offset for " + chunkX + " " + chunkZ);
                        }

                        //store 01 as the number of sectors
                        byte[] one = new byte[2];
                        one[0] = 1;
                        if (!Write_File(mca_file, one, offset+3, 1))
                        {
                                Warning("Could not write new number of sectors for " + chunkX + " " + chunkZ);

                                byte[] empty_bytes = new byte[4];
                                if (!Write_File(mca_file, empty_bytes, offset, 3))
                                {
                                        Warning("Could not remove offset of sector for " + chunkX + " " + chunkZ + " - Created a desync in mca file at offset " + offset + "!");
                                }
                                return false;
                        }

                        //import the new chunk into the empty sector (also need to write compression type between length and the zlib)
                        if (!Import_Zlib(mca_file, new_chunk_path + ".zlib", zlib_offset, 1))
                        {
                                byte[] empty_bytes = new byte[5];
                                if (!Write_File(mca_file, empty_bytes, offset, 4))
                                {
                                        Warning("Could not remove offset and number of sectors for " + chunkX + " " + chunkZ + " - Created a desync in mca file at offset " + offset + "!");
                                }
                                return false;
                        }

                        //if any of the last three fail, then we need to change the data we changed back to 0's

                        return true;
                }

                //calculated_zlib_offset = offset from mca header * 4096
                public bool Import_Zlib(FileStream mca_file, string zlib_path, int calculated_zlib_offset, int number_sectors)
                {
                        byte[] offset = Parse_Bytes(calculated_zlib_offset / 4096, 3);  //3 because the bytes originally holding the data in the mca file is 3 bytes long
                        return Import_Zlib(mca_file, zlib_path, offset, number_sectors);
                }

                public bool Import_Zlib(FileStream mca_file, string zlib_path, byte[] sector_offset, int number_sectors)
                {
                        log("Import_Zlib(" + mca_file + ", " + zlib_path + ", " + sector_offset + ", " + number_sectors + ")");
                        
                        int sector_size = 4096;
                        int zlib_max_size = number_sectors * sector_size;
                        byte[] zlib_data = new byte[zlib_max_size + 1];
                        int zlib_length_int = 0;
                        int sector_offset_int = Parse_Int(sector_offset, 3) * sector_size;
                        int zlib_length_length = 4;     //the numebr of bytes the length field is for the zlib + compression length
                        int compression_length = 1;
                        
                        //get the new zlib data
                        FileStream zlib_file = Open_File(zlib_path);
                        if (zlib_file == null)
                        {
                                Warning("Could not open new zlib file (" + zlib_path + ")");
                                return false;
                        }
                        for (int i = 0; i < zlib_max_size; i++)
                        {
                                int zlib_byte = zlib_file.ReadByte();

                                if (zlib_byte == -1)
                                {
                                        //i points to the byte after the end of the data, so if we leave it the same then it will include the byte for the compression as well
                                        zlib_length_int = i;    //zlib_length_int = length that will be written to the mca file (length of zlib - 1)
                                        i = zlib_max_size;
                                }
                                else
                                {
                                        zlib_data[i] = (byte)zlib_byte;
                                }
                        }
                        
                        //close zlib file
                        zlib_file.Flush();
                        zlib_file.Close();

                        //write new zlib length to the opened mca file
                        byte[] zlib_length_byte = Parse_Bytes(zlib_length_int, zlib_length_length);
                        if (!Write_File(mca_file, zlib_length_byte, sector_offset_int, zlib_length_length))
                        {
                                Warning("Could not write new zlib length to the mca file(" + zlib_path + ")");
                                return false;
                        }

                        //write the compression type
                        byte[] compression = new byte[compression_length + 1];
                        compression[0] = 2;                     //sector_offset_int(zlib sector base) + zlib_length_length(zlib sector size)
                        if (!Write_File(mca_file, compression, sector_offset_int + zlib_length_length + compression_length, 1))
                        {
                                Warning("Could not write compression field to the mca file(" + zlib_path + ")");
                                return false;
                        }

                        //write new zlib to the opened mca file
                        if (!Write_File(mca_file, zlib_data, sector_offset_int + zlib_length_length + compression_length, zlib_length_int - 1))
                        {
                                Warning("Could not write new zlib data to the mca file from zlib file (" + zlib_path + ")");
                                return false;
                        }

                        //Warning("Succusefully Imported new chunk (" + zlib_path + ")");
                        return true;
                }

                private bool Change_Chunk_Biome(string chunk_path, string biome_name)
                {
                        //open the chunk file
                        FileStream chunk_file = Open_File(chunk_path);
                        if (chunk_file == null)
                        {
                                Warning("Could not open chunk file(" + chunk_path + ")");
                                return false;
                        }
                        log("Opened " + chunk_path);

                        //change biome data in chunk
                        byte[] biomes_in_binary = { 0x42, 0x69, 0x6F, 0x6D, 0x65, 0x73 };
                        int biomes_offset = File_Find(chunk_file, biomes_in_binary, 6);  //offset of the biomes header in the file
                        if (biomes_offset == -1)
                        {
                                Warning("Could not find the biomes data inside the chunk file(" + chunk_path + ")");
                                return false;
                        }
                        biomes_offset += 10;    //get passed biome header
                        byte[] biome_single = { 0x0, 0x0, 0x0, 0x0 };
                        biome_single[3] = mc_biomes[biome_name];
                        int biome_size = 1024;
                        byte[] biome_data = new byte[biome_size + 1];
                        Fill_Bytes_From_Bytes(biome_data, biome_single, biome_size, 4);
                        if (!Write_File(chunk_file, biome_data, biomes_offset, biome_size))     //write to the opened chunk file 
                        {
                                Warning("Could not write new chunk data to chunk file(" + chunk_path + ")");
                                return false;
                        }

                        //close chunk file
                        chunk_file.Flush();
                        chunk_file.Close();

                        return true;
                }

                private void Change_Region_Chunks(int regionX, int regionZ, Dictionary<int, int[]> chunks)
                {
                        string self = "thread." + regionX + "." + regionZ;
                        log("Change_Region_Chunks(" + regionX + ", " + regionZ + " " + chunks + ")");

                        string Regions_Folder = mc_saves + SelectedWorld_Label.Text + "\\Region\\";
                        string mca_path = Regions_Folder += "r." + regionX.ToString() + "." + regionZ.ToString() + ".mca";
                        FileStream mca_file;

                        //mca_file = File.Open(mca_path, FileMode.Open);
                        mca_file = Open_File(mca_path);

                        if (mca_file == null)
                        {
                                Warning("Could not open mca file " + mca_path);
                                log(self + " thread stopping");
                                active_threads--;
                                return;
                        }

                        log("Opened " + mca_path);

                        //extract each chunk data from the file
                        for (int ch = 0; ch < chunks.Count; ch++)
                        {
                                //check to see if the user has told us to stop or not
                                if (stop)
                                {
                                        mca_file.Flush();
                                        mca_file.Close();
                                        log(self + " user pushed stop, stopping thread");
                                        active_threads--;
                                        return;
                                }

                                //Warning("Working on region " + regionX + " " + regionZ + " - chunk index: " + ch);
                                int chunkX = chunks[ch][0];
                                int chunkZ = chunks[ch][1];

                                int indexX = Get_Index(chunkX);
                                int indexZ = Get_Index(chunkZ);

                                log(self + " Extracting data for chunk " + chunkX + " " + chunkZ);
                                
                                //offset in the header for the offset for the data and sector count
                                int offset = Get_Offset(chunkX, chunkZ);

                                int zlib_offset = Get_File_Data(mca_file, offset, 3);
                                int zlib_sectors = Get_File_Data(mca_file, offset+3, 1);

                                //Warning("Step 2 of region " + regionX + " " + regionZ + " - chunk index: " + ch);

                                //no chunk data
                                if (zlib_offset == 0)
                                {
                                        //Warning(self + " No chunk found for " + chunkX + " " + chunkZ + " - ignoring rest of region chunks");
                                        Warning("No chunk found for " + chunkX + " " + chunkZ + " - Creating an empty chunk with the specified biome");

                                        //if succesfully created chunk
                                        if (Create_Chunk(mca_file, chunkX, chunkZ, biome_name))
                                        {
                                                //progress++;
                                                Add_Progress(1);
                                        }
                                        else
                                        {
                                                Warning("Could not create empty chunk for " + chunkX + " " + chunkZ);
                                                continue;
                                        }
                                }
                                else
                                {       
                                        //mulpiply by 4096 to get the real offset of the zlib data
                                        zlib_offset *= 4096;

                                        //get the length of the zlib file +1 for the compressrion type
                                        mca_file.Seek(zlib_offset, 0);
                                        int zlib_length_length = 4;
                                        byte[] zlib_length = new byte[zlib_length_length + 1];
                                        mca_file.Read(zlib_length, 0, zlib_length_length);
                                        int zlib_length_int = Parse_Int(zlib_length, zlib_length_length);
                                        zlib_length_int--;      //go ahead and make zlib_length_int equal to the zlib file length (removing the compression type from being counted)
                                        
                                        //get the zlib data
                                        mca_file.Seek(zlib_offset + zlib_length_length + 1, 0); //get passed the length field and compression type field
                                        int zlib_per_sector = 4096;
                                        int zlib_max_size = zlib_sectors * zlib_per_sector;
                                        byte[] zlib_data = new byte[zlib_max_size];
                                        mca_file.Read(zlib_data, 0, zlib_length_int);

                                        //save zlib to file
                                        string chunk_path = "r." + regionX + "." + regionZ + ".chunk." + indexX + "." + indexZ;
                                        string zlib_path = chunk_path + ".zlib";

                                        //Warning("Step 3 of region " + regionX + " " + regionZ + " - chunk index: " + ch);

                                        //failed to save zlib file
                                        if (!Write_File(zlib_path, zlib_data, 0, zlib_length_int))
                                        {
                                                Warning("Could not save zlib file(" + indexX + ", " + indexZ + ")");
                                                continue;
                                        }
                                        else
                                        {
                                                //delete decompressed file if it already exists for some reason, so that zdrop doesn't mess up on us and not tell us it did
                                                if (File.Exists(chunk_path))
                                                {
                                                        if (!Delete_File(chunk_path))
                                                        {
                                                                Warning("There was decompressed file already made(" + indexX + ", " + indexZ + "), and we couldn't delete it");
                                                                continue;
                                                        }
                                                        else
                                                        {
                                                                Warning("There was a decompressed file already made(" + indexX + ", " + indexZ + "), and we were able to delete it before decompressing");
                                                        }
                                                }

                                                //decompress zlib file
                                                if (!Decompress_Zlib(zlib_path))
                                                {
                                                        Warning("Could not decompress zlib file(" + indexX + ", " + indexZ + ")");
                                                        continue;
                                                }
                                                else
                                                {
                                                        //change the biome in the chunk
                                                        if (!Change_Chunk_Biome(chunk_path, biome_name))
                                                        {
                                                                Warning("Could not change the biome in chunk: " + indexX + ", " + indexZ);
                                                                continue;
                                                        }

                                                        //delete the old zlib file
                                                        if (!Delete_File(zlib_path))
                                                        {
                                                                Warning("Could not delete zlib file(" + indexX + ", " + indexZ + ")");
                                                                continue;
                                                        }

                                                        //compress new zlib file
                                                        if (!Compress_Zlib(chunk_path))
                                                        {
                                                                Warning("Could not compress new zlib file(" + indexX + ", " + indexZ + ")");
                                                                continue;
                                                        }

                                                        //import the new zlib file into the mca file
                                                        if (!Import_Zlib(mca_file, zlib_path, zlib_offset, zlib_sectors))
                                                        {
                                                                Warning("Could not import zlib file into the mca file for chunk: " + indexX + ", " + indexZ);
                                                                continue;
                                                        }

                                                        Add_Progress(1);

                                                        //delete the chunk file
                                                        if (!Delete_File(chunk_path))
                                                        {
                                                                Warning("Could not delete new chunk file(" + indexX + ", " + indexZ + ")");
                                                        }

                                                        //delete the zlib file
                                                        if (!Delete_File(zlib_path))
                                                        {
                                                                Warning("Could not delete new zlib file(" + indexX + ", " + indexZ + ")");
                                                        }

                                                        //Warning("Finished region " + regionX + " " + regionZ + " - chunk index: " + ch);
                                                        //progress++;
                                                }
                                        }
                                }
                        }

                        mca_file.Flush();
                        mca_file.Close();

                        log(self + " thread stopping");
                        active_threads--;
                }

                private void Add_Progress(int prog_add)
                {
                        progress_lock.EnterWriteLock();
                        progress += prog_add;
                        progress_lock.ExitWriteLock();
                }

                //creates a rnadom number between min and max (inclusively)
                //uses a global Random named rnd
                private int Random(int min, int max)
                {
                        return rnd.Next(min, max+1);
                }

                private void Wait(int milliseconds)
                {
                        log("Waiting " + milliseconds + " milliseconds");
                        DateTime start = DateTime.Now;
                        DateTime end = start;
                        end.AddMilliseconds(milliseconds);

                        while (start < end)
                        {

                        }
                }

                private int Get_Number_Regions(Dictionary<int, Dictionary<int, Dictionary<int, int[]>>> regions)
                {
                        int count = 0;
                        for (int rx = 0; rx < regions.Count; rx++)
                        {
                                int x_key = regions.Keys.ElementAt(rx);

                                count += regions[x_key].Count;
                        }

                        return count;
                }

                private void Change_Biomes()
                {
                        log("Change_Biomes()");

                        int x;
                        int z;
                        int x2;
                        int z2;
                        int chunkX;
                        int chunkZ;
                        int chunkX2;
                        int chunkZ2;
                        int diff_x = 0;
                        int diff_z = 0;
                        int curX;
                        int curZ;
                        int stopX;
                        int stopZ;
                        string startX;
                        string startZ;
                        string endX;
                        string endZ;

                        startX = StartX_Textbox.Text;
                        startZ = StartZ_Textbox.Text;
                        log("startX: " + startX);
                        log("startZ: " + startZ);

                        endX = EndX_Textbox.Text;
                        endZ = EndZ_Textbox.Text;
                        log("endX: " + endX);
                        log("endZ: " + endZ);

                        if (!Is_Num(startX))
                        {
                                Warning("Invalid starting X");
                                return;
                                //return false;
                        }

                        if (!Is_Num(startZ))
                        {
                                Warning("Invalid starting Z");
                                return;
                                //return false;
                        }

                        if (!Is_Num(endX))
                        {
                                Warning("Invalid ending X");
                                return;
                                //return false;
                        }

                        if (!Is_Num(endZ))
                        {
                                Warning("Invalid ending Z");
                                return;
                                //return false;
                        }

                        //get the starting cordinates in int form
                        x = Parse_Int(startX);
                        z = Parse_Int(startZ);
                        log("x: " + x);
                        log("z: " + z);

                        //get the starting chunk
                        chunkX = x / 16;
                        if (x < 0) chunkX--;
                        chunkZ = z / 16;
                        if (z < 0) chunkZ--;
                        log("chunkX: " + chunkX);
                        log("chunkZ: " + chunkZ);

                        //get the ending cordinates in int form
                        x2 = Parse_Int(endX);
                        z2 = Parse_Int(endZ);
                        log("x2: " + x2);
                        log("z2: " + z2);

                        //get the ending chunk
                        chunkX2 = x2 / 16;
                        if (x2 < 0) chunkX2--;
                        chunkZ2 = z2 / 16;
                        if (z2 < 0) chunkZ2--;
                        log("chunkX2: " + chunkX2);
                        log("chunkZ2: " + chunkZ2);

                        diff_x = difference(chunkX, chunkX2);
                        diff_z = difference(chunkZ, chunkZ2);
                        log("diff_x: " + diff_x);
                        log("diff_z: " + diff_z);

                        if (chunkX > chunkX2)
                        {
                                curX = chunkX2;
                                stopX = chunkX;
                        }
                        else
                        {
                                curX = chunkX;
                                stopX = chunkX2;
                        }
                        if (chunkZ > chunkZ2)
                        {
                                curZ = chunkZ2;
                                stopZ = chunkZ;
                        }
                        else
                        {
                                curZ = chunkZ;
                                stopZ = chunkZ2;
                        }
                        log("curX: " + curX);
                        log("stopX: " + stopX);
                        log("curZ: " + curZ);
                        log("stopZ: " + stopZ);

                        int oriX = curX;
                        int oriZ = curZ;

                        //Start a thread for each region that have chunks being changed
                        Dictionary<int, Dictionary<int, Dictionary<int, int[]>>> regions = new Dictionary<int, Dictionary<int, Dictionary<int, int[]>>>();

                        int startRegionX = curX >> 5;
                        int startRegionZ = curZ >> 5;
                        int stopRegionX = stopX >> 5;
                        int stopRegionZ = stopZ >> 5;

                        int regionX;
                        int regionZ;

                        for (curX = oriX; curX <= stopX; curX++)
                        {
                                for (curZ = oriZ; curZ <= stopZ; curZ++)
                                {
                                        regionX = Get_Region(curX);
                                        regionZ = Get_Region(curZ);

                                        Add_To_Region(regions, regionX, regionZ, curX, curZ);
                                }
                        }

                        //log the regions and their chunks
                        log("regions.Count: " + regions.Count);
                        int num_regions = Get_Number_Regions(regions);
                        //Warning("Number of Regions: " + num_regions);
                        for (int rx = 0; rx < regions.Count; rx++)
                        {
                                int x_key = regions.Keys.ElementAt(rx);

                                for (int zx = 0; zx < regions[x_key].Count; zx++)
                                {
                                        int z_key = regions[x_key].Keys.ElementAt(zx);
                                        //Warning("Number of Chunks in region " + x_key + " " + z_key + ": " + regions[x_key][z_key].Count);

                                        log("Region " + x_key + " " + z_key);
                                        for (int c = 0; c < regions[x_key][z_key].Count; c++)
                                        {
                                                log("Chunk: " + regions[x_key][z_key][c][0] + " " + regions[x_key][z_key][c][1]);
                                        }
                                }
                        }


                        for (int rx = 0; rx < regions.Count; rx++)
                        {
                                int x_key = regions.Keys.ElementAt(rx);

                                for (int zx = 0; zx < regions[x_key].Count; zx++)
                                {
                                        int z_key = regions[x_key].Keys.ElementAt(zx);

                                        //an 'array' of chunkX and chunkZ in the current region
                                        Dictionary<int, int[]> chunks = regions[x_key][z_key];

                                        log("Creating thread for Region " + x_key + " " + z_key);
                                        //Warning("Creating thread to handle " + chunks.Count + " chunks");
                                        new Thread(() => Change_Region_Chunks(x_key, z_key, chunks)).Start();
                                        active_threads++;
                                        //Change_Region_Chunks(x_key, z_key, chunks);
                                }
                        }



                        /*
                        for (regionX = startRegionX; regionX <= stopRegionX; regionX++)
                        {
                                for (regionZ = startRegionZ; regionZ <= stopRegionZ; regionZ++)
                                {
                                        //Thread thread = new Thread(Change_Biomes);
                                        //thread.Start();
                                        //new Thread(Change_Region_Biomes).Start(regionX, regionZ);
                                        //int[] region_chunks_change_x = new int[64];
                                }
                        }
                        */


                        /*
                        //loop for each row of x chunks
                        while (curX <= stopX)
                        {
                                //reset the z value for next row
                                curZ = oriZ;

                                //loop for each z in this x row
                                while (curZ <= stopZ)
                                {
                                        if (Change_Biome(curX, curZ))
                                        {

                                                log("Changed Biome at Chunk: " + curX + " " + curZ);
                                                Update_ProgressBar(oriX, curX, stopX, diff_x, oriZ, curZ, stopZ, diff_z);
                                                log("Updated Progress Bar");
                                                curZ++;
                                        }
                                        else
                                        {
                                                log("Something happened while trying to change biome for " + curX + " " + curZ);
                                                return;
                                                //return false;
                                        }
                                }
                                curX++;
                        }
                        */


                        return;
                        //return true;
                }

                public bool Write_File(FileStream file, byte[] data, int offset_file_file, int data_size)
                {
                        try
                        {
                                file.Seek(offset_file_file, 0);
                                file.Write(data, 0, data_size);
                                file.Flush();
                                log("Wrote " + data + " to " + file.Name);
                                return true;
                        }
                        catch(IOException)
                        {
                                log("Could not write " + data + " to " + file.Name);
                                return false;
                        }
                }

                private void Clear_Logs()
                {
                        Debug_RichTextbox.Text = "";
                        Warnings_RichTextbox.Text = "";
                }

                private void ChangeBiomes_Button_Click(object sender, EventArgs e)
                {
                        log("ChangeBiomes_Button_Click");

                        Clear_Logs();
                        stop = false;
                        
                        if (SelectedWorld_Label.Text == "")
                        {
                                Warning("No World Selected");
                                log("SelectedWorld_Label.Text: " + SelectedWorld_Label.Text);
                                return;
                        }

                        if (Biome_ComboBox.SelectedIndex == -1)
                        {
                                Warning("No Biome Specified");
                                log("Biome_ComboBox.SelectedIndex: " + Biome_ComboBox.SelectedIndex);
                                return;
                        }

                        biome_name = Biome_ComboBox.SelectedItem.ToString();

                        progress = 0;
                        max_progress = 0;
                        Set_Progress(0);

                        //ChangeBiomes_Button.Enabled = false;
                        //finished_chunks = false;
                        //Thread thread = new Thread(Change_Biomes);
                        //thread.Start();
                        Change_Biomes();

                        processing = true;
                        Set_Enabled(false);
                        Done_Label.Text = "";

                        //this.Invalidate();
                        //this.Refresh();

                        //if (Change_Biomes())
                        //{
                        //Done_Label.Text = "Done";
                        //}

                        //Set_Enabled(true);
                }

                public void Set_Enabled(bool value, bool cancel_button = true)
                {
                        ChangeBiomes_Button.Enabled = value;
                        StartX_Textbox.Enabled = value;
                        StartZ_Textbox.Enabled = value;
                        EndX_Textbox.Enabled = value;
                        EndZ_Textbox.Enabled = value;
                        MCSaves_Listbox.Enabled = value;
                        SelectWorld_Button.Enabled = value;
                        Biome_ComboBox.Enabled = value;
                        SubChunk_Button.Enabled = value;

                        Stop_Button.Enabled = cancel_button;
                }

                private void Update_ProgressBar(int oriX, int curX, int stopX, int diff_x, int oriZ, int curZ, int stopZ, int diff_z)
                {
                        int max_progress = (diff_x + 1) * (diff_z + 1);
                        double percentage;
                        int perc_int;

                        progress++;

                        if (max_progress <= 0) max_progress = 1;
                        percentage = progress / (double)max_progress;
                        perc_int = (int)(percentage * 100);

                        progress_bar_text = progress.ToString() + " / " + max_progress.ToString() + " = " + percentage.ToString();
                        //ProgressBar_Label.Text = progress.ToString() + " / " + max_progress.ToString() + " = " + percentage.ToString();

                        log("Current Progress: " + progress);

                        //ProgressBar_Label.Text = progress;
                        if (perc_int > 100) perc_int = 100;

                        //progressBar1.Value = perc_int;
                        Set_Progress(perc_int);
                        //ProgresBar_Text = progress;
                }

                private void Set_Progress(int value)
                {
                        //progressBar1.Value = value;
                        progress_bar_value = value;
                }

                private byte[] Parse_Bytes(int data, int num_bytes)
                {
                        byte[] new_data = new byte[num_bytes];
                        int last_byte = 0;

                        for (int b = num_bytes-1; b >= 0; b--)
                        {
                                last_byte = data & 0xFF;
                                new_data[b] = (byte)last_byte;

                                data = data >> 8;
                        }

                        return new_data;
                }

                private void Fill_Bytes_From_Bytes(byte[] big_data, byte[] filler, int big_data_size, int filler_size)
                {
                        int bb;

                        for (int b = 0; b < big_data_size; b++)
                        {
                                bb = b % filler_size;

                                big_data[b] = filler[bb];
                        }
                }

                private void log(byte[] data, int size)
                {
                        string data_string = Parse_String(data, size);

                        log(data_string);
                }

                private string Parse_String(byte[] data, int size)
                {
                        string str = "";

                        for (int s = 0; s < size; s++)
                        {
                                str += data[s];
                        }

                        return str;
                }

                private bool Bytes_Equal(byte[] one, byte[] two, int size)
                {
                        for (int b = 0; b < size; b++)
                        {
                                if (one[b] != two[b])
                                {
                                        return false;
                                }
                        }

                        return true;
                }

                public int File_Find(FileStream file, byte[] data, int size)
                {
                        byte[] cur_byte = new byte[size];
                        int cur_offset = 0;
                        
                        while(cur_offset < file.Length)
                        {
                                file.Seek(cur_offset, 0);
                                file.Read(cur_byte, 0, size);

                                //log(cur_byte[0]);

                                if (Bytes_Equal(cur_byte, data, size))
                                {
                                        return cur_offset;
                                }

                                cur_offset++;
                        }

                        log("Reached end of file - No Matches for " + data);
                        return -1;
                }

                public bool Write_File(string path, byte[] data, int offset, int size)
                {
                        log("Write_File(" + path + ", " + data + ", " + size + ", " + offset + ")");

                        try
                        {
                                FileStream file;
                                file = new FileStream(path, FileMode.OpenOrCreate);
                                file.Seek(offset, 0);
                                file.Write(data, 0, size);
                                file.Flush();
                                file.Close();
                                return true;
                        }
                        catch(IOException)
                        {
                                log("Could not write to file " + path);
                                return false;
                        }
                }

                private byte[] Parse_Bytes(string data)
                {
                        byte[] new_data = new byte[data.Length+1];

                        for (int i = 0; i < data.Length; i++)
                        {
                                new_data[i] = (byte)data[i];
                        }

                        return new_data;
                }

                
                private bool Write_File(string path, string data)
                {
                        log("Write_File(" + path + ", " + data + ")");
                        byte[] bytes = Parse_Bytes(data);

                        return Write_File(path, bytes, 0, data.Length);

                        /*
                        FileStream file;
                        file = new FileStream(path, FileMode.Create);
                        file.Write(bytes, 0, path.Length);
                        file.Close();*/
                }

                public int Parse_Int(byte[] data, int size)
                {
                        int newData = 0;
                        
                        for (int index = 0; index < size; index++)
                        {
                                newData = newData << 8;
                                newData = newData | data[index];
                        }

                        return newData;
                }
                
                public int Round(double num)
                {
                        Dictionary<int, int> digits = new Dictionary<int, int>();
                        int num_int = (int)num;
                        double new_num = num;
                        int index = 1;

                        digits[0] = num_int;            //index 0 = the whole number part (23.56456 => [0] = 23)
                        new_num = num - num_int;        //new_num = decimal only        (23.56456 => new_num = 0.56456)

                        while (new_num != 0)
                        {
                                new_num *= 10;          //(23.56456 => new_num = 5.6456)
                                num_int = (int)new_num; //(23.56456 => num_int = 5)

                                digits[index] = num_int;        //(23.56456 => [1] = 5)

                                new_num = new_num - num_int;    //(23.56456 => new_num = 0.6456)

                                index++;
                        }

                        for (int i = index - 1; i > 0; i--)
                        {
                                num_int = digits[i];    //(23.5645 6 => num_int = 6)

                                if (num_int >= 5)
                                {
                                        digits[i - 1] += 1;     //(23.564 5 6 => 5 += 1 = 6)
                                }
                        }

                        return digits[0];
                }
                
                public FileStream Open_File(string path)
                {
                        try
                        {
                                FileStream file;
                                file = new FileStream(path, FileMode.Open);
                                return file;
                        }
                        catch(IOException the_problem) {
                                log(the_problem.Message);
                                return null;
                        }
                }

                public int Parse_Int(string data)
                {
                        int number;
                        string dataNext;

                        if (data.Length == 0)
                        {
                                return 0;
                        }
                        else
                        {
                                dataNext = data.Substring(1);

                                //negative number
                                if (data[0] == '-')
                                {
                                        return -1 * Parse_Int(dataNext);
                                }
                                else
                                {
                                        number = (data[0] - '0') * Power(10, data.Length - 1);
                                        
                                        return number + Parse_Int(dataNext);
                                }
                        }
                }

                //use recursion to raise a number to a power
                public int Power(int number, int power)
                {
                        if (power == 0)
                        {
                                return 1;
                        }
                        else
                        {
                                return number * Power(number, --power);
                        }
                }

                public bool Is_Num(string number)
                {
                        //no number is not a number
                        if (number.Length == 0)
                        {
                                return false;
                        }

                        for (int n = 0; n < number.Length; n++)
                        {
                                //if if starts wth '-' and has more data than just that afterwards (could be a string of just '-' for some reason)
                                if (n == 0 && number.Length > 1)
                                {
                                        //if looks like a negative number, then look at the rest of the string
                                        if (number[n] == '-')
                                        {
                                                continue;
                                        }
                                }

                                if (number[n] - '0' < 0 || number[n] - '0' > 9)
                                {
                                        return false;
                                }
                        }

                        return true;
                }

                private void log(string text)
                {
                        if (debug_mode)
                        {
                                //Debug_RichTextbox.AppendText(text + Environment.NewLine);
                                //Debug_RichTextbox.ScrollToCaret();
                                log_text += text + Environment.NewLine;
                        }
                }

                private void logger()
                {
                        if (debug_mode)
                        {
                                string added_text = log_text;
                                log_text = "";
                                if (added_text != "")
                                {
                                        Debug_RichTextbox.AppendText(added_text);
                                        Debug_RichTextbox.ScrollToCaret();
                                }
                        }

                        string warn_text = warning_text;
                        warning_text = "";
                        if (warn_text != "")
                        {
                                //Warnings_RichTextbox.Text += warn_text;
                                Warnings_RichTextbox.AppendText(warn_text);
                                Warnings_RichTextbox.ScrollToCaret();
                        }
                }

                private void log(int text)
                {
                        log(text.ToString());
                }

                //https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.deflatestream?view=netcore-3.1
                public static void Compress(DirectoryInfo directorySelected)
                {

                        foreach (FileInfo file in directorySelected.GetFiles("*.xml"))
                                using (FileStream originalFileStream = file.OpenRead())
                                {
                                        if ((File.GetAttributes(file.FullName) & FileAttributes.Hidden)
                                            != FileAttributes.Hidden & file.Extension != ".cmp")
                                        {
                                                using (FileStream compressedFileStream = File.Create(file.FullName + ".cmp"))
                                                {
                                                        using (DeflateStream compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                                                        {
                                                                originalFileStream.CopyTo(compressionStream);
                                                        }
                                                }

                                                FileInfo info = new FileInfo(directoryPath + "\\" + file.Name + ".cmp");
                                                Console.WriteLine("Compressed {0} from {1} to {2} bytes.", file.Name, file.Length, info.Length);
                                        }
                                }
                }

                public void Decompress(FileInfo fileToDecompress)
                {
                        using (FileStream originalFileStream = fileToDecompress.OpenRead())
                        {
                                string currentFileName = fileToDecompress.FullName;
                                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                                using (FileStream decompressedFileStream = File.Create(newFileName))
                                {
                                        using (DeflateStream decompressionStream = new DeflateStream(originalFileStream, CompressionMode.Decompress))
                                        {
                                                decompressionStream.CopyTo(decompressedFileStream);
                                                Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);

                                                //Write_File(newFileName, decompressionStream);

                                        }
                                }
                        }
                }

                private void Write_File(string filename, DeflateStream decompressed_stream)
                {
                        string decompressed_string = "";
                        byte[] decompressed_byte = new byte[2];
                        int offset = 0;
                        
                        decompressed_stream.Read(decompressed_byte, 0, 1);
                        while (decompressed_stream.ReadByte() != -1)
                        {
                                decompressed_string += decompressed_byte[0];

                                offset++;
                                decompressed_stream.Read(decompressed_byte, 0, 1);
                        }

                        Write_File(filename, decompressed_string);
                }

                private void bar_text()
                {
                        double prog;
                        int prog_int;

                        if (processing)
                        {
                                if (max_progress <= 0)
                                {
                                        prog = 0;
                                }
                                else
                                {
                                        prog = progress / (double)max_progress;
                                }

                                prog_int = (int)(prog * 100);

                                ProgressBar_Label.Text = progress.ToString() + " / " + max_progress.ToString() + " = " + prog.ToString();
                                progressBar1.Value = prog_int;
                                ProgressPercentage_Label.Text = prog_int.ToString() + "%";

                                //update the background of the percentage over the bar
                                TransparetBackground(ProgressPercentage_Label);
                        }
                }

                //https://stackoverflow.com/questions/25830079/how-to-make-the-background-of-a-label-transparent-in-c-sharp
                void TransparetBackground(Control C)
                {
                        //C.Visible = false;

                        C.Refresh();
                        Application.DoEvents();

                        Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
                        int titleHeight = screenRectangle.Top - this.Top;
                        int Right = screenRectangle.Left - this.Left;

                        Bitmap bmp = new Bitmap(this.Width, this.Height);
                        this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                        Bitmap bmpImage = new Bitmap(bmp);
                        bmp = bmpImage.Clone(new Rectangle(C.Location.X + Right, C.Location.Y + titleHeight, C.Width, C.Height), bmpImage.PixelFormat);
                        C.BackgroundImage = bmp;

                        //C.Visible = true;
                }

                private void bar_value()
                {
                        int value = progress_bar_value;
                        progress_bar_value = -1;
                        if (value != -1)
                        {
                                progressBar1.Value = value;
                        }
                }

                private void Update_Threads()
                {
                        ActiveThreads_Label.Text = "Active Threads: " + active_threads.ToString();
                }

                private void UpdateProgressBar_Timer_Tick(object sender, EventArgs e)
                {
                        logger();
                        bar_text();
                        Update_Threads();
                        Update_Finished();
                }

                private void Update_Finished()
                {
                        if (processing)
                        {
                                if (active_threads == 0)
                                {
                                        processing = false;
                                        Set_Enabled(true);

                                        Warning("Finished");

                                        if (progress != max_progress)
                                        {
                                                int unchanged = max_progress - progress;

                                                Warning("Not all chunks were changed");
                                                if (unchanged == 1)
                                                {
                                                        Warning(" " + unchanged.ToString() + " chunk was not changed");
                                                }
                                                else
                                                {
                                                        Warning(" " + unchanged.ToString() + " chunks were not changed");
                                                }
                                                Set_Progress(100);
                                                progress = 1;
                                                max_progress = 1;

                                                //update the background of the percentage over the bar at the very end
                                                TransparetBackground(ProgressPercentage_Label);
                                        }
                                        logger();
                                        bar_text();
                                        Update_Threads();
                                }
                        }
                }

                private void DisplayLog_Checkbox_CheckedChanged(object sender, EventArgs e)
                {
                        Debug_RichTextbox.Visible = DisplayLog_Checkbox.Checked;
                }

                private void UpdatePercentate_Timer_Tick(object sender, EventArgs e)
                {
                        //update the background of the percentage over the bar
                        TransparetBackground(ProgressPercentage_Label);

                        if (!processing)
                        {
                                UpdatePercentate_Timer.Enabled = false;
                        }
                }

                private void Stop_Button_Click(object sender, EventArgs e)
                {
                        stop = true;
                }

                private void SubChunk_Button_Click(object sender, EventArgs e)
                {
                        if (SelectedWorld_Label.Text == "")
                        {
                                Warning("No World Selected");
                                log("SelectedWorld_Label.Text: " + SelectedWorld_Label.Text);
                                return;
                        }

                        SubChunk_Form = new SubChunks_Form();

                        SubChunk_Form.mc_biomes = mc_biomes;
                        SubChunk_Form.mc_biome_names = mc_biome_names;
                        SubChunk_Form.Selected_World = SelectedWorld_Label.Text;
                        SubChunk_Form.Main = this;

                        SubChunk_Form.Show();

                        Set_Enabled(false, false);
                }

                private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
                {
                        //if there is a sub chunk editing form open, then run the closing function
                        if (SubChunk_Form != null)
                        {
                                SubChunk_Form.Close();
                        }
                }
        }
}
