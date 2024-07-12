using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_ThongTinKhac : UserControl
    {
        string query = "";
        adb adb = new adb();
        private Process adbProcess;
        private bool isRunning = false;

        // variable RAM
        double total_Ram_GB = 0;
        double used_Ram_GB = 0;
        double free_Ram_GB = 0;

        // variable ROM
        double total_Rom_GB = 0;
        double speed_Write_MB = 0;
        double app_Rom_GB = 0;
        double photo_Rom_GB = 0;
        double video_Rom_GB = 0;
        double audio_Rom_GB = 0;
        double download_Rom_GB = 0;
        double system_Rom_GB = 0;
        double other_Rom_GB = 0;
        double rom_Free_GB = 0;
        double rom_Used_GB = 0;

        public usr_ThongTinKhac()
        {
            InitializeComponent();
            LoadData();

            listView.Columns.Add("STT", (int)(1 * panelAccount.Width / 10), HorizontalAlignment.Center);
            listView.Columns.Add("Tên tài khoản", (int)(4.5 * panelAccount.Width / 10), HorizontalAlignment.Center);
            listView.Columns.Add("Tên ứng dụng", (int)(4 * panelAccount.Width / 10), HorizontalAlignment.Center);

            listViewConnect.Columns.Add("STT", (int)(1 * panelAccount.Width / 10), HorizontalAlignment.Center);
            listViewConnect.Columns.Add("Tên wifi", (int)(2.5 * panelAccount.Width / 10), HorizontalAlignment.Center);
            listViewConnect.Columns.Add("Thời gian tạo", (int)(2 * panelAccount.Width / 10), HorizontalAlignment.Center);
            listViewConnect.Columns.Add("Địa chỉ MAC", (int)(2 * panelAccount.Width / 10), HorizontalAlignment.Center);
            listViewConnect.Columns.Add("Giao thức", (int)(2 * panelAccount.Width / 10), HorizontalAlignment.Center);
        }

        private async void LoadData()
        {
            await Task.Run(() => Load_InfoNetstat());
            Load_InfoPin();
            Load_InfoPowerDisplay();
            Load_InfoRam();
            Load_InfoRom();
            Load_InfoAccount();
            Load_InfoWifiConnected();
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panelEx1.Width / 2);
                splitContainer2.SplitterDistance = (int)(panelEx1.Width / 4);
                splitContainer3.SplitterDistance = (int)(panelEx1.Width / 4);
                panel1.Width = (int)(panelEx1.Width / 7);

                listView.Columns[0].Width = (int)(1 * panelAccount.Width / 10);
                listView.Columns[1].Width = (int)(4.5 * panelAccount.Width / 10);
                listView.Columns[2].Width = (int)(4 * panelAccount.Width / 10);

                listViewConnect.Columns[0].Width = (int)(1 * panelAccount.Width / 10);
                listViewConnect.Columns[1].Width = (int)(2.5 * panelAccount.Width / 10);
                listViewConnect.Columns[2].Width = (int)(2 * panelAccount.Width / 10);
                listViewConnect.Columns[3].Width = (int)(2 * panelAccount.Width / 10);
                listViewConnect.Columns[4].Width = (int)(2 * panelAccount.Width / 10);

                panel_RamDaDung.Width = (int)(panel_Ram.Width * used_Ram_GB / total_Ram_GB);

                panel_App.Width = (int)(panel_Rom.Width * app_Rom_GB / total_Rom_GB);
                panel_Photo.Width = (int)(panel_Rom.Width * photo_Rom_GB / total_Rom_GB);
                panel_Video.Width = (int)(panel_Rom.Width * video_Rom_GB / total_Rom_GB);
                panel_Audio.Width = (int)(panel_Rom.Width * audio_Rom_GB / total_Rom_GB);
                panel_Download.Width = (int)(panel_Rom.Width * download_Rom_GB / total_Rom_GB);
                panel_System.Width = (int)(panel_Rom.Width * system_Rom_GB / total_Rom_GB);
            }
            catch
            {

            }
        }

        private string GetValue(string input, string key)
        {
            string pattern = $@"{key}: (.*?)\r\n";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private string GetValueNumber(string input, string key)
        {
            string pattern = $@"{key} \(kB/s\) = (\d+)";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private string GetValueMemory(string input, string key)
        {
            string pattern = $@"{key}: (.*?)K";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private List<string> GetValueList1(string input, string key)
        {
            string pattern = $@"{key}=(.*?),";

            List<string> values = new List<string>();

            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches)
            {
                values.Add(match.Groups[1].Value);
            }
            return values;
        }

        private List<string> GetValueList2(string input, string key)
        {
            string pattern = $@"{key}=(.*?)(?=}})";

            List<string> values = new List<string>();

            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches)
            {
                values.Add(match.Groups[1].Value);
            }
            return values;
        }

        private string GetValueListConnect(string input)
        {
            string pattern = @"Dump of WifiConfigManager[\s\S]*?Dump of PasspointManager";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                return "";
            }
        }

        private List<string> GetValueListConnect1(string input, string key)
        {
            string pattern = $@"{key}:(.*?)\r\n";

            List<string> values = new List<string>();

            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches)
            {
                values.Add(match.Groups[1].Value.Trim());
            }
            return values;
        }

        private List<string> GetValueListConnect2(string input, string key)
        {
            string pattern = $@"{key}=(.*?)\r\n";

            List<string> values = new List<string>();

            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches)
            {
                values.Add(match.Groups[1].Value.Trim());
            }
            return values;
        }

        private void Load_InfoPin()
        {
            query = "shell dumpsys battery";
            string str_InfoPin = adb.adbCommand(query);
            if (str_InfoPin == string.Empty)
            {
                str_InfoPin = "Current Battery Service state:\r\n  AC powered: false\r\n  USB powered: true\r\n  Wireless powered: false\r\n  Max charging current: 500000\r\n  Max charging voltage: 5000000\r\n  Charge counter: 2555000\r\n  status: 2\r\n  health: 2\r\n  present: true\r\n  level: 95\r\n  scale: 100\r\n  voltage: 4203\r\n  temperature: 378\r\n  technology: Li-poly";
            }
            str_InfoPin += "\r\n";

            //switch
            string AC_powered = GetValue(str_InfoPin, "AC powered");
            if (AC_powered == "true") { sw_NguonAC.IsOn = true; }
            else { sw_NguonAC.IsOn = false; }

            string USB_powered = GetValue(str_InfoPin, "USB powered");
            if (USB_powered == "true") { sw_NguonUSB.IsOn = true; }
            else { sw_NguonUSB.IsOn = false; }

            string Wireless_powered = GetValue(str_InfoPin, "Wireless powered");
            if (Wireless_powered == "true") { sw_NguonKhongDay.IsOn = true; }
            else { sw_NguonKhongDay.IsOn = false; }

            string present = GetValue(str_InfoPin, "present");
            if (present == "true") { sw_GanPin.IsOn = true; }
            else { sw_GanPin.IsOn = false; }

            //text
            string Max_charging_current = GetValue(str_InfoPin, "Max charging current");
            txtDongDienToiDa.Text = (Double.Parse(Max_charging_current) / 1000000).ToString() + "A";

            string Max_charging_voltage = GetValue(str_InfoPin, "Max charging voltage");
            txtDienApToiDa.Text = (Int32.Parse(Max_charging_voltage) / 1000000).ToString() + "V";

            string Charge_counter = GetValue(str_InfoPin, "Charge counter");
            txtDungLuongPin.Text = (Int32.Parse(Charge_counter) / 1000).ToString() + "mAh";

            string status = GetValue(str_InfoPin, "status");
            txtTrangThaiPin.Text = status;

            string health = GetValue(str_InfoPin, "health");
            txtTinhTrangPin.Text = health;

            string level = GetValue(str_InfoPin, "level");
            txtPinHienTai.Text = level;

            string scale = GetValue(str_InfoPin, "scale");
            txtThangDo.Text = scale;

            string voltage = GetValue(str_InfoPin, "  voltage");
            txtDienApHienTai.Text = (Double.Parse(voltage) / 1000).ToString() + "V";

            string temperature = GetValue(str_InfoPin, "temperature");
            txtNhietDoPin.Text = (Double.Parse(temperature) / 10).ToString() + "°C";

            string technology = GetValue(str_InfoPin, "technology");
            txtCongNghePin.Text = technology;
        }

        private void Load_InfoPowerDisplay()
        {
            //===== Power
            query = "shell dumpsys power";
            string str_InfoPowerDisplay = adb.adbCommand(query);
            if (str_InfoPowerDisplay == string.Empty)
            {
                str_InfoPowerDisplay = "Display Power: state=OFF\r\n\r\nBattery saving stats:\r\n  Battery Saver is currently: ON\r\n    Last ON time: 2024-07-10 08:02:20.831 -33m9s696ms\r\n    Last OFF time: 2024-07-10 08:02:15.595 -33m14s932ms\r\n    Times enabled: 204";
            }
            str_InfoPowerDisplay += "\r\n";

            //switch
            string Display_Power = GetValue(str_InfoPowerDisplay, "Display Power");
            if (Display_Power.Replace("state=", "") == "ON") { sw_TrangThaiManHinh.IsOn = true; }
            else { sw_TrangThaiManHinh.IsOn = false; }

            string Battery_Saver_is_currently = GetValue(str_InfoPowerDisplay, "Battery Saver is currently");
            if (Battery_Saver_is_currently == "ON") { sw_CheDoTKPin.IsOn = true; }
            else { sw_CheDoTKPin.IsOn = false; }

            //text
            string Last_ON_time = GetValue(str_InfoPowerDisplay, "Last ON time");
            txtLanBatCuoiCung.Text = Last_ON_time;

            string Times_enabled = GetValue(str_InfoPowerDisplay, "Times enabled");
            txtSoLanBat.Text = Times_enabled;


            //===== Display
            query = "shell wm size";
            str_InfoPowerDisplay = adb.adbCommand(query);
            if (str_InfoPowerDisplay == string.Empty)
            {
                str_InfoPowerDisplay = "Physical size: 1080x2340";
            }
            str_InfoPowerDisplay += "\r\n";
            //text
            string Physical_size = GetValue(str_InfoPowerDisplay, "Physical size");
            txtKichThuocManHinh.Text = Physical_size;


            //===== Sim
            query = "shell dumpsys telephony.registry";
            str_InfoPowerDisplay = adb.adbCommand(query);
            if (str_InfoPowerDisplay == string.Empty)
            {
                str_InfoPowerDisplay = "mServiceState={mVoiceRegState=0(IN_SERVICE), mDataRegState=0(IN_SERVICE), mChannelNumber=1700, duplexMode()=1, mCellBandwidths=[15000], mVoiceOperatorAlphaLong=Viettel, mVoiceOperatorAlphaShort=Viettel, mDataOperatorAlphaLong=Viettel, mDataOperatorAlphaShort=Viettel, isManualNetworkSelection=false(automatic), getRilVoiceRadioTechnology=14(LTE), getRilDataRadioTechnology=14(LTE), mCssIndicator=unsupported, mNetworkId=-1, mSystemId=-1, mCdmaRoamingIndicator=-1, mCdmaDefaultRoamingIndicator=-1, mIsEmergencyOnly=false, isUsingCarrierAggregation=false, mLteEarfcnRsrpBoost=0, mNetworkRegistrationInfos=[NetworkRegistrationInfo{ domain=CS transportType=WWAN registrationState=HOME roamingType=NOT_ROAMING accessNetworkTechnology=LTE rejectCause=0 emergencyEnabled=false availableServices=[VOICE,SMS,VIDEO] cellIdentity=CellIdentityLte:{ mBandwidth=15000 mMcc=452 mMnc=04 mAlphaLong=Viettel mAlphaShort=Viettel} voiceSpecificInfo=VoiceSpecificRegistrationInfo { mCssSupported=false mRoamingIndicator=1 mSystemIsInPrl=-1 mDefaultRoamingIndicator=-1} dataSpecificInfo=null nrState=NONE}, NetworkRegistrationInfo{ domain=PS transportType=WWAN registrationState=HOME roamingType=NOT_ROAMING accessNetworkTechnology=LTE rejectCause=0 emergencyEnabled=false availableServices=[DATA] cellIdentity=CellIdentityLte:{ mBandwidth=15000 mMcc=452 mMnc=04 mAlphaLong=Viettel mAlphaShort=Viettel} voiceSpecificInfo=null dataSpecificInfo=android.telephony.DataSpecificRegistrationInfo :{ maxDataCalls = 20 isDcNrRestricted = false isNrAvailable = false isEnDcAvailable = false LteVopsSupportInfo :  mVopsSupport = 2 mEmcBearerSupport = 3 mIsUsingCarrierAggregation = false } nrState=NONE}], mNrFrequencyRange=-1, mOperatorAlphaLongRaw=Viettel, mOperatorAlphaShortRaw=Viettel, mIsIwlanPreferred=false}\r\nmServiceState={mVoiceRegState=0(IN_SERVICE), mDataRegState=0(IN_SERVICE), mChannelNumber=1700, duplexMode()=1, mCellBandwidths=[15000], mVoiceOperatorAlphaLong=Viettel, mVoiceOperatorAlphaShort=Vinaphone, mDataOperatorAlphaLong=Viettel, mDataOperatorAlphaShort=Viettel, isManualNetworkSelection=false(automatic), getRilVoiceRadioTechnology=14(LTE), getRilDataRadioTechnology=14(LTE), mCssIndicator=unsupported, mNetworkId=-1, mSystemId=-1, mCdmaRoamingIndicator=-1, mCdmaDefaultRoamingIndicator=-1, mIsEmergencyOnly=false, isUsingCarrierAggregation=false, mLteEarfcnRsrpBoost=0, mNetworkRegistrationInfos=[NetworkRegistrationInfo{ domain=CS transportType=WWAN registrationState=HOME roamingType=NOT_ROAMING accessNetworkTechnology=LTE rejectCause=0 emergencyEnabled=false availableServices=[VOICE,SMS,VIDEO] cellIdentity=CellIdentityLte:{ mBandwidth=15000 mMcc=452 mMnc=04 mAlphaLong=Viettel mAlphaShort=Viettel} voiceSpecificInfo=VoiceSpecificRegistrationInfo { mCssSupported=false mRoamingIndicator=1 mSystemIsInPrl=-1 mDefaultRoamingIndicator=-1} dataSpecificInfo=null nrState=NONE}, NetworkRegistrationInfo{ domain=PS transportType=WWAN registrationState=HOME roamingType=NOT_ROAMING accessNetworkTechnology=LTE rejectCause=0 emergencyEnabled=false availableServices=[DATA] cellIdentity=CellIdentityLte:{ mBandwidth=15000 mMcc=452 mMnc=04 mAlphaLong=Viettel mAlphaShort=Viettel} voiceSpecificInfo=null dataSpecificInfo=android.telephony.DataSpecificRegistrationInfo :{ maxDataCalls = 20 isDcNrRestricted = false isNrAvailable = false isEnDcAvailable = false LteVopsSupportInfo :  mVopsSupport = 2 mEmcBearerSupport = 3 mIsUsingCarrierAggregation = false } nrState=NONE}], mNrFrequencyRange=-1, mOperatorAlphaLongRaw=Viettel, mOperatorAlphaShortRaw=Viettel, mIsIwlanPreferred=false}";
            }
            //text
            List<string> list_sim = GetValueList1(str_InfoPowerDisplay, "mVoiceOperatorAlphaShort");
            string sim1 = list_sim[0];
            txtSim1.Text = sim1;

            string sim2 = list_sim[1];
            txtSim2.Text = sim2;
        }

        private double ConvertKbToGb(long kilobytes)
        {
            const long KilobytesPerGigabyte = 1024 * 1024;
            double gigabytes = (double)kilobytes / KilobytesPerGigabyte;
            return Math.Round(gigabytes, 1);
        }

        private double ConvertKbToMb(long kilobytes)
        {
            const long MegabytesPerGigabyte = 1024;
            double gigabytes = (double)kilobytes / MegabytesPerGigabyte;
            return Math.Round(gigabytes, 1);
        }

        private double ConvertByteToGb(long bytes)
        {
            const long BytesPerGigabyte = 1024 * 1024 * 1024;
            double gigabytes = (double)bytes / BytesPerGigabyte;
            return Math.Round(gigabytes, 1);
        }

        private void Load_InfoRam()
        {
            query = "shell dumpsys meminfo";
            string str_InfoRam = adb.adbCommand(query);
            if (str_InfoRam == string.Empty)
            {
                str_InfoRam = "Total RAM: 5,691,120K (status normal)\r\n Free RAM: 3,221,555K (  702,551K cached pss + 2,397,228K cached kernel +       200K cached ion +   121,576K free)\r\n Used RAM: 3,508,155K (2,848,863K used pss +   659,292K kernel)\r\n Lost RAM:   782,604K\r\n     ZRAM:   317,988K physical used for 1,242,584K in swap (2,621,436K total swap)\r\n   Tuning: 256 (large 512), oom   322,560K, restore limit   107,520K (high-end-gfx)";
            }

            //text
            string Total_RAM = GetValueMemory(str_InfoRam, "Total RAM");
            int number_Total_RAM = Int32.Parse(Total_RAM.Replace(",", ""));
            double number_Total_RAM_GB = ConvertKbToGb(number_Total_RAM);

            string Free_RAM = GetValueMemory(str_InfoRam, "Free RAM");
            int number_Free_RAM = Int32.Parse(Free_RAM.Replace(",", ""));
            double number_Free_RAM_GB = ConvertKbToGb(number_Free_RAM);

            string Used_RAM = GetValueMemory(str_InfoRam, "Used RAM");
            int number_Used_RAM = Int32.Parse(Used_RAM.Replace(",", ""));
            double number_Used_RAM_GB = ConvertKbToGb(number_Used_RAM);

            string Lost_RAM = GetValueMemory(str_InfoRam, "Lost RAM");
            int number_Lost_RAM = Int32.Parse(Lost_RAM.Replace(",", ""));
            double number_Lost_RAM_GB = ConvertKbToGb(number_Lost_RAM);

            string ZRAM = GetValueMemory(str_InfoRam, "ZRAM");
            int ZRAM_RAM = Int32.Parse(ZRAM.Replace(",", ""));
            double ZRAM_RAM_GB = ConvertKbToGb(ZRAM_RAM);

            total_Ram_GB = number_Free_RAM_GB + number_Used_RAM_GB - number_Lost_RAM_GB + ZRAM_RAM_GB;
            txtRamTotal.Text = total_Ram_GB.ToString() + "GB";
            used_Ram_GB = number_Used_RAM_GB - ZRAM_RAM_GB;
            txtRamUsed.Text = used_Ram_GB.ToString() + "GB";

            free_Ram_GB = total_Ram_GB - used_Ram_GB;
            txtRamFree.Text = free_Ram_GB.ToString() + "GB";

            // resize
            panel_RamDaDung.Width = (int)(panel_Ram.Width * used_Ram_GB / total_Ram_GB);
        }

        private void Load_InfoRom()
        {
            query = "shell dumpsys diskstats";
            string str_InfoRom = adb.adbCommand(query);
            if (str_InfoRom == string.Empty)
            {
                str_InfoRom = "Latency: 0ms [512B Data Write]\r\nRecent Disk Write Speed (kB/s) = 27438\r\nData-Free: 16570084K / 116543452K total = 14% free\r\nCache-Free: 16570084K / 116543452K total = 14% free\r\nSystem-Free: 175404K / 3555292K total = 4% free\r\nFile-based Encryption: true\r\nApp Size: 11583023104\r\nApp Data Size: 33171407872\r\nApp Cache Size: 7033290752\r\nPhotos Size: 6013239296\r\nVideos Size: 42832719872\r\nAudio Size: 21393408\r\nDownloads Size: 0\r\nSystem Size: 8659505152\r\nOther Size: 19586949120";
            }
            str_InfoRom += "\r\n";

            // speed
            string Recent_Disk_Write_Speed = GetValueNumber(str_InfoRom, "Recent Disk Write Speed");
            int number_Recent_Disk_Write_Speed_ROM = Int32.Parse(Recent_Disk_Write_Speed);
            double number_Recent_Disk_Write_Speed_ROM_GB = ConvertKbToMb(number_Recent_Disk_Write_Speed_ROM);

            // Giá trị từng loại
            string App_Size = GetValue(str_InfoRom, "App Size");
            long number_App_Size_ROM = long.Parse(App_Size);
            double number_App_Size_ROM_GB = ConvertByteToGb(number_App_Size_ROM);

            string App_Data_Size = GetValue(str_InfoRom, "App Data Size");
            long number_App_Data_Size_ROM = long.Parse(App_Data_Size);
            double number_App_Data_Size_ROM_GB = ConvertByteToGb(number_App_Data_Size_ROM);

            string App_Cache_Size = GetValue(str_InfoRom, "App Cache Size");
            long number_App_Cache_Size_ROM = long.Parse(App_Cache_Size);
            double number_App_Cache_Size_ROM_GB = ConvertByteToGb(number_App_Cache_Size_ROM);

            string Photos_Size = GetValue(str_InfoRom, "Photos Size");
            long number_Photos_Size_ROM = long.Parse(Photos_Size);
            double number_Photos_Size_ROM_GB = ConvertByteToGb(number_Photos_Size_ROM);

            string Videos_Size = GetValue(str_InfoRom, "Videos Size");
            long number_Videos_Size_ROM = long.Parse(Videos_Size);
            double number_Videos_Size_ROM_GB = ConvertByteToGb(number_Videos_Size_ROM);

            string Audio_Size = GetValue(str_InfoRom, "Audio Size");
            long number_Audio_Size_ROM = long.Parse(Audio_Size);
            double number_Audio_Size_ROM_GB = ConvertByteToGb(number_Audio_Size_ROM);

            string Downloads_Size = GetValue(str_InfoRom, "Downloads Size");
            long number_Downloads_Size_ROM = long.Parse(Downloads_Size);
            double number_Downloads_Size_ROM_GB = ConvertByteToGb(number_Downloads_Size_ROM);

            string System_Size = GetValue(str_InfoRom, "System Size");
            long number_System_Size_ROM = long.Parse(System_Size);
            double number_System_Size_ROM_GB = ConvertByteToGb(number_System_Size_ROM);

            string Other_Size = GetValue(str_InfoRom, "Other Size");
            long number_Other_Size_ROM = long.Parse(Other_Size);
            double number_Other_Size_ROM_GB = ConvertByteToGb(number_Other_Size_ROM);

            // xử lý
            app_Rom_GB = number_App_Size_ROM_GB + number_App_Data_Size_ROM_GB + number_App_Cache_Size_ROM_GB;
            photo_Rom_GB = number_Photos_Size_ROM_GB;
            video_Rom_GB = number_Videos_Size_ROM_GB;
            audio_Rom_GB = number_Audio_Size_ROM_GB;
            download_Rom_GB = number_Downloads_Size_ROM_GB;
            system_Rom_GB = number_System_Size_ROM_GB;
            other_Rom_GB = number_Other_Size_ROM_GB;
            speed_Write_MB = number_Recent_Disk_Write_Speed_ROM_GB;

            total_Rom_GB = app_Rom_GB + photo_Rom_GB + video_Rom_GB + audio_Rom_GB + download_Rom_GB + system_Rom_GB + other_Rom_GB;
            rom_Used_GB = total_Rom_GB - other_Rom_GB;
            rom_Free_GB = other_Rom_GB;

            txtRomTotal.Text = total_Rom_GB.ToString() + "GB";
            txtSpeedWrite.Text = speed_Write_MB.ToString() + "MB/s";
            txtRomApp.Text = app_Rom_GB.ToString() + "GB";
            txtRomPhoto.Text = photo_Rom_GB.ToString() + "GB";
            txtRomVideo.Text = video_Rom_GB.ToString() + "GB";
            txtRomAudio.Text = audio_Rom_GB.ToString() + "GB";
            txtRomDownload.Text = download_Rom_GB.ToString() + "GB";
            txtRomSystem.Text = system_Rom_GB.ToString() + "GB";
            txtRomFree.Text = rom_Free_GB.ToString() + "GB";

            // resize
            panel_App.Width = (int)(panel_Rom.Width * app_Rom_GB / total_Rom_GB);
            panel_Photo.Width = (int)(panel_Rom.Width * photo_Rom_GB / total_Rom_GB);
            panel_Video.Width = (int)(panel_Rom.Width * video_Rom_GB / total_Rom_GB);
            panel_Audio.Width = (int)(panel_Rom.Width * audio_Rom_GB / total_Rom_GB);
            panel_Download.Width = (int)(panel_Rom.Width * download_Rom_GB / total_Rom_GB);
            panel_System.Width = (int)(panel_Rom.Width * system_Rom_GB / total_Rom_GB);
        }

        private void Load_InfoAccount()
        {
            query = "shell dumpsys account";
            string str_InfoAccount = adb.adbCommand(query);
            if (str_InfoAccount == string.Empty)
            {
                str_InfoAccount = "User UserInfo{0:Chủ sở hữu:13}:\r\n  Accounts: 10\r\n    Account {name=hacongquoc***@gmail.com, type=com.google}\r\n    Account {name=hacongquocl***@gmail.com, type=com.google}\r\n    Account {name=6230***535, type=com.xiaomi}\r\n    Account {name=WPS ***ice, type=cn.wps.moffice}\r\n    Account {name=Zalo***ount, type=com.zing.zalo}\r\n    Account {name=Ti***k, type=com.ss.android.ugc.trill}\r\n    Account {name=5280***196, type=org.telegram.messenger}\r\n    Account {name=Mes***ger, type=com.facebook.messenger}\r\n    Account {name=Fac***ok, type=com.facebook.auth.login}\r\n    Account {name=100011***265424, type=com.facebook.auth.login}";
            }

            listView.Items.Clear();
            List<string> list_name = GetValueList1(str_InfoAccount, "name");
            List<string> list_type = GetValueList2(str_InfoAccount, "type");
            for (int i = 0; i < list_name.Count; i++)
            {
                ListViewItem item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(list_name[i]);
                item.SubItems.Add(list_type[i]);

                listView.Items.Add(item);
            }
        }

        private void Load_InfoWifiConnected()
        {
            query = "shell dumpsys wifi";
            string str_InfoWifiConnected = adb.adbCommand(query);
            if (str_InfoWifiConnected == string.Empty)
            {
                str_InfoWifiConnected = "Dump of WifiConfigManager\r\nWifiConfigManager - Log Begin ----\r\n2024-06-17T21:00:15.986 - clearInternalData: Clearing all internal data\r\n\r\nEnterprise config:\r\n\r\nDPP config:\r\nIP config:\r\nIP assignment: DHCP\r\nProxy settings: NONE\r\n cuid=1000 cname=android.uid.system:1000 luid=1000 lname=android.uid.system:1000 lcuid=1000 userApproved=USER_UNSPECIFIED noInternetAccessExpected=false\r\nrecentFailure: Association Rejection code: 0\r\nShareThisAp: false\r\n\r\nID: 1 SSID: \"Oc cau gao\" PROVIDER-NAME: null BSSID: null FQDN: null PRIO: 0 HIDDEN: false PMF: false OWE Transition mode Iface: null\r\n NetworkSelectionStatus NETWORK_SELECTION_ENABLED\r\n hasEverConnected: true\r\n numAssociation 1\r\n creation time=05-01 15:04:37.975\r\n validatedInternetAccess trusted\r\n macRandomizationSetting: 0\r\n mRandomizedMacAddress: 8e:06:e5:e0:d1:14\r\n KeyMgmt: WPA_PSK Protocols: WPA RSN WAPI\r\n AuthAlgorithms: OPEN\r\n PairwiseCiphers: TKIP CCMP GCMP_256\r\n GroupCiphers: WEP40 WEP104 TKIP CCMP GCMP_256\r\n GroupMgmtCiphers:\r\n SuiteBCiphers: ECDHE_ECDSA\r\n PSK/SAE: *\r\n\r\n\r\n\r\n\r\nEnterprise config:\r\n\r\nDPP config:\r\nIP config:\r\nIP assignment: DHCP\r\nProxy settings: NONE\r\n cuid=1000 cname=android.uid.system:1000 luid=1000 lname=android.uid.system:1000 lcuid=1000 userApproved=USER_UNSPECIFIED noInternetAccessExpected=false\r\nrecentFailure: Association Rejection code: 0\r\nShareThisAp: false\r\n\r\nID: 2 SSID: \"LoiTuyet\" PROVIDER-NAME: null BSSID: null FQDN: null PRIO: 0 HIDDEN: false PMF: false OWE Transition mode Iface: null\r\n NetworkSelectionStatus NETWORK_SELECTION_ENABLED\r\n hasEverConnected: true\r\n numAssociation 7\r\n creation time=05-04 12:22:02.007\r\n validatedInternetAccess trusted\r\n macRandomizationSetting: 0\r\n mRandomizedMacAddress: 22:1a:75:6d:a6:29\r\n KeyMgmt: WPA_PSK Protocols: WPA RSN WAPI\r\n AuthAlgorithms: OPEN\r\n PairwiseCiphers: TKIP CCMP GCMP_256\r\n GroupCiphers: WEP40 WEP104 TKIP CCMP GCMP_256\r\n GroupMgmtCiphers:\r\n SuiteBCiphers: ECDHE_ECDSA\r\n PSK/SAE: *\r\n\r\n\r\n\r\nDump of PasspointManager\r\nPasspointManager - Providers Begin ---\r\nPasspointManager - Providers End ---\r\nPasspointManager - Next provider ID to be assigned 0\r\nLast sweep 128:36:35.759 ago.";
            }

            str_InfoWifiConnected = GetValueListConnect(str_InfoWifiConnected);
            str_InfoWifiConnected = str_InfoWifiConnected.Replace("PROVIDER", "\r\nPROVIDER");

            listViewConnect.Items.Clear();
            List<string> list_SSID = GetValueListConnect1(str_InfoWifiConnected, " SSID");
            List<string> list_mRandomizedMacAddress = GetValueListConnect1(str_InfoWifiConnected, "mRandomizedMacAddress");
            List<string> list_creation_time = GetValueListConnect2(str_InfoWifiConnected, "creation time");
            List<string> list_Protocols = GetValueListConnect1(str_InfoWifiConnected, " Protocols");

            for (int i = 0; i < list_SSID.Count; i++)
            {
                ListViewItem item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(list_SSID[i].Replace("\"", ""));
                item.SubItems.Add(list_mRandomizedMacAddress[i]);
                item.SubItems.Add(list_creation_time[i]);
                item.SubItems.Add(list_Protocols[i]);

                listViewConnect.Items.Add(item);
            }
        }

        private void Load_InfoNetstat()
        {
            //txtKetQuaNetstat.Clear();

            StartAdbCommand_Netstat("netstat");
        }

        private void StartAdbCommand_Netstat(string command)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c adb shell {command}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                adbProcess = new Process();
                adbProcess.StartInfo = startInfo;
                adbProcess.OutputDataReceived += Process_OutputDataReceived;

                adbProcess.Start();
                adbProcess.BeginOutputReadLine();

                isRunning = true;  // Đã bắt đầu thực thi
                UpdateButtonsState();  // Cập nhật trạng thái các nút
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực thi lệnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopAdbCommand()
        {
            if (adbProcess != null && !adbProcess.HasExited)
            {
                adbProcess.Kill();
                isRunning = false;  // Đã ngừng thực thi
                UpdateButtonsState();  // Cập nhật trạng thái các nút
            }
        }

        private void ContinueAdbCommand()
        {
            if (!isRunning)
            {
                StartAdbCommand_Netstat("netstat");
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                // Thêm dòng kết quả vào TextBox
                AppendTextSafe(e.Data + Environment.NewLine);
            }
        }

        private void AppendTextSafe(string text)
        {
            if (txtKetQuaNetstat.InvokeRequired)
            {
                txtKetQuaNetstat.Invoke(new Action<string>(AppendTextSafe), text);
            }
            else
            {
                txtKetQuaNetstat.AppendText(text);
            }
        }

        private void UpdateButtonsState()
        {
            btnStop.Enabled = isRunning;       // Cho phép dừng nếu có tiến trình đang chạy
            btnContinue.Enabled = !isRunning;  // Cho phép tiếp tục nếu đã dừng
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ContinueAdbCommand();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopAdbCommand();
        }

        private void btnLuuKetNoiMang_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void SaveToFile()
        {
            // Tạo tên file dựa trên thời gian hiện tại
            string fileName = $"Netstat_{DateTime.Now.ToString("HH_mm_ss_dd_MM_yyyy")}.txt";

            // Hiển thị hộp thoại lưu file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = fileName;
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Lưu file Netstat";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lưu nội dung vào file được chọn
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.WriteAllText(filePath, txtKetQuaNetstat.Text);
                    MessageBox.Show("Lưu file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            listViewConnect.Items.Clear();
            StopAdbCommand();
            LoadData();
        }
    }
}
