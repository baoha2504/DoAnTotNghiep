using MTA_Mobile_Forensic.Support;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_CaiDat : UserControl
    {
        int width = 0;
        int setuptab0 = 0;
        int setuptab1 = 0;
        int setuptab2 = 0;
        string query = "";
        adb adb = new adb();

        public usr_CaiDat()
        {
            InitializeComponent();

            setuptab0 = 1;
            width = tabControlPanel1.Width;
            try
            {
                splitContainer1.SplitterDistance = (int)(width / 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi kích thước: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadTab_HeThong();
            LoadTab_BaoMat();
            LoadTab_CucBo();
        }


        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            try
            {
                int selectedIndex = tabControl1.SelectedTabIndex;
                if (selectedIndex == 0 && setuptab0 == 0)
                {
                    setuptab0 = 1;
                    width = tabControlPanel1.Width;
                    splitContainer1.SplitterDistance = (int)(width / 2);
                }
                else if (selectedIndex == 1 && setuptab1 == 0)
                {
                    setuptab1 = 1;
                    width = tabControlPanel2.Width;
                    splitContainer2.SplitterDistance = (int)(width / 2);
                }
                else if (selectedIndex == 2 && setuptab2 == 0)
                {
                    setuptab2 = 1;
                    width = tabControlPanel4.Width;
                    panel63.Width = (int)(width * 0.3);
                    splitContainer3.SplitterDistance = (int)(width / 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi kích thước: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetValue(string input, string key)
        {
            string pattern = $@"{key}=(.*?)\r\n";

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

        private string GetValueByKeyContain(string input, string key)
        {
            input = GetValue(input, key);
            string pattern = @"=(\d+)$";

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

        private int GetCount(string input, string key)
        {
            int count = 0;
            for (int i = 9; i > 0; i--)
            {
                if (input.Contains($"{key}{i}"))
                {
                    count = i;
                    break;
                }
            }
            return count;
        }

        private void LoadTab_HeThong()
        {
            query = "shell settings list system";
            string str_HeThong = adb.adbCommand(query);
            if (str_HeThong == string.Empty)
            {
                str_HeThong = "Channel_Msg_Default=Channel_Msg_Default1586676994027\r\nPOWER_SAVE_MODE_OPEN=0\r\nPOWER_SAVE_PRE_CLEAN_MEMORY_TIME=600\r\nPOWER_SAVE_PRE_HIDE_MODE=enhance\r\nPOWER_SAVE_PRE_SYNCHRONIZE_ENABLE=1\r\naccelerometer_rotation=0\r\nalarm_alert=file:///system/media/audio/ui/dynamic_alarm_speech.ogg\r\nalarm_alert_set=1\r\nalways_enable_mms=0\r\nandroid.contacts.smart_group_by_company=1\r\nandroid.contacts.smart_group_by_frequency=1\r\nandroid.contacts.smart_group_by_location=1\r\nanr_pkgs=[\"com.tencent.qqlive\",\"com.android.systemui\",\"com.miui.systemAdSolution\",\"com.milink.service\",\"com.miui.gallery\",\"com.miui.gallery\",\"com.miui.cloudservice\",\"com.miui.cloudservice\",\"com.miui.cloudservice\",\"com.miui.video\",\"com.miui.home\",\"com.miui.home\"]\r\napptimer_load_data_time=1720112627059\r\nauto_dual_clock=1\r\nbattery_indicator_style=1\r\ncalendar_alert=file:///system/media/audio/ui/WaterDrop_preview.ogg\r\ncamera_boost={\"nowFree\":1387168,\"killNum\":10,\"willFree\":459205,\"totalMem\":5691120,\"time\":569585370}\r\nclock_changed_time_1x2=1650423328695\r\nclock_changed_time_2x2=1650423328695\r\nclock_changed_time_2x4=1650423328695\r\nclock_changed_time_3x4=1650423328695\r\nclock_changed_time_4x4=1650423328695\r\ncom.miui.home.enable_share_progress_status=true\r\ncom.miui.yellowpage_preferences.pref_last_database_region=VN\r\ncom.xiaomi.discover.auto_update_enabled=1\r\ncom.xiaomi.discover.metered_system_update_confirm_needed_by_region=0\r\ncom.xiaomi.discover.metered_update_answered=0\r\ncom.xiaomi.discover.metered_update_confirm_needed_by_region=0\r\ncom.xiaomi.market.enable_app_chooser_recommend=0\r\ncom.xiaomi.market.lastRegion=VN\r\ncontrast_alpha=0\r\ndark_mode_enable=0\r\ndefault_alarm_alert=\r\ndelete_sound_effect=1\r\ndim_screen=1\r\ndisable_wifi_auto_connect_ssid=IlBhZmF08J+SjiI=,\r\ndisplay_color_mode=0\r\ndisplay_yellowpage_tab=1\r\ndtmf_tone=0\r\ndtmf_tone_type=0\r\ndxCRMxhQkdGePGnp=EF4E9E811866ECF39EBD4F9941D792461EE755F72052E9E9BB39DFAB618DFCE6\r\nenable_three_gesture=0\r\nend_button_behavior=2\r\nfirewall_calllog_displayed=1\r\nfod_animation_type=2\r\nfont_scale=1.0\r\ngearhead:driving_mode_settings_enabled=0\r\ngesture_wakeup=1\r\nhaptic_feedback_enabled=0\r\nhas_screenshot_sound=1\r\nhas_update_application_state=true\r\nhearing_aid=0\r\nhide_rotation_lock_toggle_for_accessibility=0\r\nhotspot_mac_black_set=NTA6ZWI6NzE6ZDk6NGU6ZDI=,\r\nhotspot_max_station_num=4\r\nhotspot_vendor_specific=DD0A0017F206010103010000\r\nis_darkmode_switch_show=1\r\nis_fingerprint_unlock=0\r\nis_small_window=0\r\nkey_garbage_danger_in_flag=1\r\nkey_garbage_danger_in_size=2000133545\r\nkey_mqs_uuid=4a10c676-537b-41da-8079-8b119bbfa844\r\nkey_notificaiton_general_clean_need=1\r\nkey_notificaiton_general_clean_size=2000133545\r\nkey_notificaiton_whatsapp_clean_size=0\r\nkey_notification_wechat_size=0\r\nkey_notification_wechat_size_need=0\r\nkey_notification_whatsapp_clean_need=0\r\nkey_updated=1\r\nlaunchMiBrowserWhileSwipe=1\r\nlauncher_state=1\r\nlock_wallpaper_provider_authority=com.miui.android.fashiongallery.lockscreen_magazine_provider\r\nlocked_apps=[{\"u\":0,\"pkgs\":[\"com.lxqd.myhotpotstroy.abroad\",\"com.google.android.youtube\",\"com.coccoc.trinhduyet\",\"com.instagram.android\",\"com.zing.zalo\"]},{\"u\":-100,\"pkgs\":[\"com.jeejen.family.miui\"]}]\r\nlockscreen_sounds_enabled=1\r\nlong_press_back_key=none\r\nlong_press_home_key=turn_on_torch\r\nmibridge_authorized_pkg_list=com.mi.testmibridge,com.ss.android.article.news\r\nmifg_online_content=1\r\nmiprofile.settings.miprofile_badge_notice=0\r\nmiprofile.settings.miprofile_on=0\r\nmiprofile.settings.miprofile_set=0\r\nmiprofile.settings.miprofile_user_notice=0\r\nmiprofile.settings.miprofile_visible=0\r\nmisettings_st_enable_sm=1\r\nmisettings_support_repost=1\r\nmishare_wifi_connect_state=0\r\nmispeed_authorized_pkg_list=com.tencent.mm,mm.tencent.com.comtencentmmhardcodertest\r\nmiui_home_enable_auto_fill_empty_cells=1\r\nmiui_home_screen_cells_size=4x6\r\nmiui_popup_sound_index=0\r\nmiui_recents_privacy_thumbnail_blur=cn.com.cmbc.newmbank,com.mt.mtxx.mtxx,com.cebbank.mobile.cemb,com.benqu.wuta,com.gorgeous.lite,cn.com.spdb.mobilebank.per,vStudio.Android.Camera360,com.chinamworld.main,com.icbc,com.chinamworld.bocmbci,com.meitu.meiyancamera,com.bankcomm.Bankcomm,com.android.bankabc,com.android.camera,cmb.pb,com.ecitic.bank.mobile,com.miui.gallery,com.campmobile.snowcamera,com.lemon.faceu\r\nmiui_recents_show_mem_info=1\r\nmms_private_address_marker=0\r\nmms_sync_wild_msg_state=0\r\nmms_sync_wild_numbers=null\r\nmms_thread_marker=0\r\nmms_upload_old_msg_accounts=null\r\nmms_upload_old_msg_state=0\r\nmode_ringer_streams_affected=2342\r\nmqBRboGZkQPcAkyk=YOIKoemkRMsDAFLoUm0iDow+\r\nmute_streams_affected=2159\r\nnavigation_bar_window_loaded=0\r\nnew_numeric_password_type=1\r\nnext_alarm_clock_formatted=Th 7 05:28\r\nnext_alarm_formatted=Th 6 14:37\r\nnotification_light_pulse=1\r\nnotification_sound=file:///system/media/audio/ui/WaterDrop_preview.ogg\r\nnotification_sound_set=1\r\nonly_phones_v2=1\r\npick_up_gesture_wakeup_mode=0\r\npointer_location=0\r\npointer_speed=0\r\npower_center_finger_aod=-1\r\npower_center_haptic_feed_back_mode=-1\r\npower_center_sound_mode_click=-1\r\npower_center_sound_mode_delete=-1\r\npower_center_sound_mode_dialer=-1\r\npower_center_sound_mode_lock=-1\r\npower_center_sound_mode_screenshot=-1\r\npower_center_wakeup_double_click=-1\r\npower_center_wakeup_notification=-1\r\npower_center_wakeup_pickup=-1\r\npower_supersave_mode_open=0\r\npref_key_wallpaper_screen_span=1.0\r\nradio.data.stall.recovery.action=0\r\nremote_control_pkg_name=null\r\nremote_control_proc_name=null\r\nresident_timezone=Asia/Ho_Chi_Minh\r\nringtone=file:///system/media/audio/ringtones/Celesta.ogg\r\nringtone_default=file:///system/media/audio/ringtones/MiRemix.ogg\r\nringtone_set=1\r\nringtone_sound_slot_1=file:///system/media/audio/ringtones/Celesta.ogg\r\nringtone_sound_slot_2=file:///system/media/audio/ringtones/Celesta.ogg\r\nscreen_auto_brightness_adj=0.0\r\nscreen_brightness=634\r\nscreen_brightness_for_vr=86\r\nscreen_brightness_mode=0\r\nscreen_color_level=2\r\nscreen_game_mode=0\r\nscreen_key_order=2 1 3\r\nscreen_off_timeout=300000\r\nscreen_optimize_mode=1\r\nscreen_paper_mode_enabled=0\r\nscreen_paper_mode_level=155\r\nselected_keyguard_clock_position=1\r\nsettings.notify.key.settings.modified=2\r\nshow_touches=0\r\nshutdown_alarm_clock_offset=300\r\nsmart_dark_enable=0\r\nsms_delivered_sound=file:///system/media/audio/notifications/MessageSent.ogg\r\nsms_delivered_sound_slot_1=file:///system/media/audio/notifications/MessageSent.ogg\r\nsms_delivered_sound_slot_2=file:///system/media/audio/notifications/MessageSent.ogg\r\nsms_received_sound=file:///system/media/audio/ui/WaterDrop_preview.ogg\r\nsms_received_sound_slot_1=file:///system/media/audio/ui/WaterDrop_preview.ogg\r\nsms_received_sound_slot_2=file:///system/media/audio/ui/WaterDrop_preview.ogg\r\nsoftap_reported_frequency=2472\r\nsound_effects_enabled=0\r\nstatus_bar_expandable_under_keyguard=1\r\nstatus_bar_in_call_notification_floating=0\r\nstatus_bar_notification_style=1\r\nstatus_bar_show_network_assistant=1\r\nstatus_bar_show_network_speed=1\r\nstatus_bar_window_loaded=0\r\nsunrise=23160000\r\nsunrise_update=123\r\nsunset=69720000\r\nsystem_locales=vi-VN\r\nsysui_tuner_demo_on=0\r\ntouch.stats={\"min\":0,\"max\":1}\r\ntty_mode=0\r\nupdatable_system_app_count=3\r\nuse_control_panel=0\r\nuser_network_priority_enabled=0\r\nuser_rotation=0\r\nvibrate_in_normal=1\r\nvibrate_in_silent=1\r\nvibrate_when_ringing=1\r\nvolume_alarm=10\r\nvolume_alarm_speaker=15\r\nvolume_bluetooth_sco=7\r\nvolume_bluetooth_sco_bt_a2dp=7\r\nvolume_bluetooth_sco_bt_sco=13\r\nvolume_bluetooth_sco_earpiece=15\r\nvolume_bluetooth_sco_headset=15\r\nvolume_bluetooth_sco_speaker=7\r\nvolume_music=10\r\nvolume_music_analog_dock=10\r\nvolume_music_aux_line=10\r\nvolume_music_before_mute_analog_dock=10\r\nvolume_music_before_mute_aux_line=10\r\nvolume_music_before_mute_bt_a2dp=7\r\nvolume_music_before_mute_bt_a2dp_hp=10\r\nvolume_music_before_mute_bt_a2dp_spk=10\r\nvolume_music_before_mute_bt_sco=10\r\nvolume_music_before_mute_bt_sco_carkit=10\r\nvolume_music_before_mute_bt_sco_hs=10\r\nvolume_music_before_mute_bus=10\r\nvolume_music_before_mute_digital_dock=10\r\nvolume_music_before_mute_earpiece=15\r\nvolume_music_before_mute_fm_transmitter=10\r\nvolume_music_before_mute_hdmi=10\r\nvolume_music_before_mute_headphone=7\r\nvolume_music_before_mute_headset=10\r\nvolume_music_before_mute_hearing_aid_out=10\r\nvolume_music_before_mute_hmdi_arc=10\r\nvolume_music_before_mute_ip=10\r\nvolume_music_before_mute_line=7\r\nvolume_music_before_mute_proxy=15\r\nvolume_music_before_mute_remote_submix=10\r\nvolume_music_before_mute_spdif=10\r\nvolume_music_before_mute_speaker=0\r\nvolume_music_before_mute_speaker_safe=10\r\nvolume_music_before_mute_telephony_tx=10\r\nvolume_music_before_mute_usb_accessory=10\r\nvolume_music_before_mute_usb_device=10\r\nvolume_music_before_mute_usb_headset=4\r\nvolume_music_bt_a2dp=7\r\nvolume_music_bt_a2dp_hp=10\r\nvolume_music_bt_a2dp_spk=10\r\nvolume_music_bt_sco=10\r\nvolume_music_bt_sco_carkit=10\r\nvolume_music_bt_sco_hs=10\r\nvolume_music_bus=10\r\nvolume_music_digital_dock=10\r\nvolume_music_earpiece=15\r\nvolume_music_fm_transmitter=10\r\nvolume_music_hdmi=10\r\nvolume_music_headphone=7\r\nvolume_music_headset=15\r\nvolume_music_hearing_aid_out=10\r\nvolume_music_hmdi_arc=10\r\nvolume_music_ip=10\r\nvolume_music_line=6\r\nvolume_music_proxy=15\r\nvolume_music_remote_submix=10\r\nvolume_music_spdif=10\r\nvolume_music_speaker=0\r\nvolume_music_speaker_safe=10\r\nvolume_music_telephony_tx=10\r\nvolume_music_usb_accessory=10\r\nvolume_music_usb_device=10\r\nvolume_music_usb_headset=4\r\nvolume_notification=10\r\nvolume_ring=10\r\nvolume_ring_bt_sco=10\r\nvolume_ring_earpiece=15\r\nvolume_ring_headset=14\r\nvolume_ring_speaker=0\r\nvolume_system=10\r\nvolume_voice=4\r\nvolume_voice_bt_a2dp=8\r\nvolume_voice_bt_sco=11\r\nvolume_voice_earpiece=11\r\nvolume_voice_headphone=6\r\nvolume_voice_headset=9\r\nvolume_voice_line=8\r\nvolume_voice_speaker=10\r\nvr_mode=0\r\nwakeup_for_keyguard_notification=2\r\nwc_enable_source=oobe\r\nwifi_assistant=1\r\nwifi_assistant_data_prompt=1\r\nwifi_p2p_accept_mac=\r\nwindow_type_layer={\"2000\":18,\"2001\":4,\"2013\":1}";
            }

            // 1-0
            string accelerometer_rotation = GetValue(str_HeThong, "accelerometer_rotation");
            if (accelerometer_rotation == "1") { sw1_XoayMH.IsOn = true; }
            else { sw1_XoayMH.IsOn = false; }

            string auto_dual_clock = GetValue(str_HeThong, "auto_dual_clock");
            if (auto_dual_clock == "1") { sw1_HaiMuiGio.IsOn = true; }
            else { sw1_HaiMuiGio.IsOn = false; }

            string dark_mode_enable = GetValue(str_HeThong, "dark_mode_enable");
            if (dark_mode_enable == "1") { sw1_CheDoToi.IsOn = true; }
            else { sw1_CheDoToi.IsOn = false; }

            string status_bar_show_network_speed = GetValue(str_HeThong, "status_bar_show_network_speed");
            if (status_bar_show_network_speed == "1") { sw1_TocDoMang.IsOn = true; }
            else { sw1_TocDoMang.IsOn = false; }

            string wifi_assistant = GetValue(str_HeThong, "wifi_assistant");
            if (wifi_assistant == "1") { sw1_TinhNangWifi.IsOn = true; }
            else { sw1_TinhNangWifi.IsOn = false; }

            string alarm_alert = GetValue(str_HeThong, "alarm_alert");
            if (alarm_alert == "1") { sw1_BaoThuc.IsOn = true; }
            else { sw1_BaoThuc.IsOn = false; }

            string lockscreen_sounds_enabled = GetValue(str_HeThong, "lockscreen_sounds_enabled");
            if (lockscreen_sounds_enabled == "1") { sw1_AmThanhKhoaMH.IsOn = true; }
            else { sw1_AmThanhKhoaMH.IsOn = false; }

            // Text
            string screen_brightness = GetValue(str_HeThong, "screen_brightness");
            txtDoSangManHinh.Text = screen_brightness;
            string screen_off_timeout = GetValue(str_HeThong, "screen_off_timeout");
            txtThoiGianManHinhCho.Text = screen_off_timeout;
            string next_alarm_formatted = GetValue(str_HeThong, "next_alarm_formatted");
            txtBaoThucTiepTheo.Text = next_alarm_formatted;

        }

        private void UpdateTab_HeThong()
        {

        }

        private void LoadTab_BaoMat()
        {
            query = "shell settings list secure";
            string str_BaoMat = adb.adbCommand(query);
            if (str_BaoMat == string.Empty)
            {
                str_BaoMat = "FBO_RULES=\r\nFBO_STATE_OPEN=1\r\nPOWER_SAVE_GUIDE_ENABLE=0\r\nSILENT_MODE_FOR_MIUI9=3\r\naccess_control_lock_enabled=1\r\naccessibility_display_magnification_enabled=0\r\naccessibility_display_magnification_scale=2.0\r\naccessibility_enabled=1\r\naccessibility_shortcut_on_lock_screen=1\r\nallowed_geolocation_origins=http://www.google.com http://www.google.co.uk\r\nam_show_system_apps=1\r\nandroid_id=e42edce4c9bc01f6\r\nanr_show_background=0\r\naod_mode_time=1\r\naod_mode_user_set=0\r\naod_show_style_update=1\r\naod_time_update=1\r\naod_using_super_wallpaper=0\r\napp_lock_add_account_md5=51e39ddb3bc3a73b7d1fe0f1ae3cc095\r\napplock_countDownTimer_deadline=0\r\nassistant=com.google.android.googlequicksearchbox/com.google.android.voiceinteraction.GsaVoiceInteractionService\r\naudio_game_4d=0\r\nauto_download=0\r\nauto_update=0\r\nautofill_field_classification=1\r\nautofill_service=com.google.android.gms/.autofill.service.AutofillService\r\nautofill_service_search_uri=https://play.google.com/store/apps/collection/promotion_30029f7_autofillservices_apps\r\nautofill_user_data_max_category_count=20\r\naware_enabled=not_set\r\naware_lock_enabled=0\r\nbackup_enabled:com.android.calllogbackup=1\r\nbackup_enabled:com.android.providers.telephony=1\r\nbackup_enabled=0\r\nbackup_encryption_opt_in_displayed=1\r\nbackup_transport=com.google.android.gms/.backup.BackupTransportService\r\nban_lock_list_change_pkg=com.android.browser,com.android.calendar\r\nbluetooth_address=A8:9C:ED:22:99:14\r\nbluetooth_name=Quốc Bảo\r\ncast_mode=0\r\ncast_mode_package=\r\nchange_index2name=1\r\ncharging_sounds_enabled=0\r\ncharging_vibration_enabled=1\r\ncleaner_dc_shortcut_recall_invalid=1\r\ncleaner_main_shortcut_recall_invalid=1\r\ncleaner_main_shortcut_recall_status=0\r\nclipboard_cipher_list=\r\ncloud_clipboard_cipher_content_saved=\r\ncom.xiaomi.market.need_update_app_count=10\r\ncom.xiaomi.market.need_update_game_count=0\r\ncom_android_settings_privacypassword_fingerprint_upgrade=1\r\ncom_miui_applicatinlock_use_fingerprint_state=2\r\ncom_miui_applicationlock_fingerprint_upgrade=1\r\nconfig_update_certificate=MIID4TCCAsmgAwIBAgIJAMsyDT+FwMuvMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYDVQQGEwJVUzETMBEGA1UECAwKQ2FsaWZvcm5pYTEWMBQGA1UEBwwNTW91bnRhaW4gVmlldzETMBEGA1UECgwKR29vZ2xlIEluYzEQMA4GA1UECwwHQW5kcm9pZDEjMCEGCSqGSIb3DQEJARYUc2VjdXJpdHlAYW5kcm9pZC5jb20wHhcNMTIwOTEwMjA0NDQxWhcNMTIxMDEwMjA0NDQxWjCBhjELMAkGA1UEBhMCVVMxEzARBgNVBAgMCkNhbGlmb3JuaWExFjAUBgNVBAcMDU1vdW50YWluIFZpZXcxEzARBgNVBAoMCkdvb2dsZSBJbmMxEDAOBgNVBAsMB0FuZHJvaWQxIzAhBgkqhkiG9w0BCQEWFHNlY3VyaXR5QGFuZHJvaWQuY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwuFNNPal1QpUSH8MU5T+BSSbCv/jTU8pbRKwYfq4ZmsjG1BxX8GJS/itdHr5IgG5YGSRpB9JkWJTAne54Qv2xX3ck8yMySjOKbsCuF3Qnrv1HiM1w0PmMvPC5GvDJz9alrrsq6uj40Hd5ina8pTKieYO1PNIf1MByBnrq/615cPOwb4UAYUwx4ChFG8aUNoChgYk0Ain7YoX4Pw7/tXIiVArsPWRbtzXVaMNWONRUM3SGVT5NO67LEEk0vIkDVjmGnJaDkkF+yqAr24ekl2qdMQb9cEnc1wNCMtVdyxUelZoVxFnot3m4ZhYIPzGI2E88TJ6c30Qs1ocHuPXW8KfGQIDAQABo1AwTjAdBgNVHQ4EFgQUXlT7Zj4V/hHVbyL54kmRYizK/powHwYDVR0jBBgwFoAUXlT7Zj4V/hHVbyL54kmRYizK/powDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOCAQEAE5D6pMFMJMwVTtrzZXExgn6r2q7LugmdIpaoFjiU/Iv/bk3VvhDU5jjE54G/fqi/g415yowkFQ02/ZkrTm/q3anvWsppN3NEns2YDi4moEhwLZcCVBzhjkppHGgkLHadzlJZdUFLnzZbDuiZrDNPEkjldVejF1qGt6LbW6d8xNmscerUdKJSUqryOi3U8SXjlTWu5d/hmajQLEe7VJ3y8q5rEsCfsb15EyYxNLfTZuf4PXVU/+kSciTyJxkPZAaz2J1zXQegzC+k3gzNnovlHYlDc5EaVnprsBgOCkaWQfjr6/5YyJysM96WMjndIjyFZSHnj9yF4j7JDwB9qEfxvA==\r\nconvert_foreground_location=0\r\nconvert_perm_GetInstall=0\r\ndefault_input_method=com.google.android.inputmethod.latin/com.android.inputmethod.latin.LatinIME\r\ndisable_voicetrigger=0\r\ndouble_tap_to_wake=1\r\ndoze_always_on=0\r\ndoze_enabled=0\r\ndropbox:data_app_anr=disabled\r\ndropbox:data_app_crash=disabled\r\ndropbox:data_app_wtf=disabled\r\nenabled_accessibility_services=com.android.settings/com.android.settings.accessibility.accessibilitymenu.AccessibilityMenuService\r\nenabled_input_methods=com.google.android.googlequicksearchbox/com.google.android.voicesearch.ime.VoiceInputMethodService:com.google.android.inputmethod.latin/com.android.inputmethod.latin.LatinIME\r\nenabled_notification_assistant=com.miui.notification/com.miui.notification.NotificationListener\r\nenabled_notification_listeners=com.google.android.projection.gearhead/com.google.android.gearhead.notifications.SharedNotificationListenerManager$ListenerService:com.google.android.googlequicksearchbox/com.google.android.apps.gsa.notificationlistener.GsaNotificationListenerService:com.crrepa.band.hero/com.crrepa.band.my.notify.service.NotificationCollectorService\r\nenabled_notification_policy_access_packages=com.google.android.apps.wellbeing\r\nenabled_widgets=com.google.android.googlequicksearchbox\r\nface_unlcok_apply_for_lock=1\r\nface_unlock_by_notification_screen_on=1\r\nface_unlock_has_feature=2\r\nface_unlock_model=1\r\nface_unlock_success_show_message=0\r\nface_unlock_success_stay_screen=0\r\nface_unlock_valid_feature=1\r\nfacelock_detection_threshold=0.0\r\nfacelock_liveliness_recognition_threshold=2.2\r\nfacelock_max_center_movement=10.0\r\nfaceunlock_support_superpower=1\r\nfbo_app_size=5000000\r\nfingerprint_apply_to_privacy_password=1\r\nfirst_use_freeform=1\r\nfirst_use_tip_confirm_times=1\r\nfod_auth_fingerprint=0\r\nfreeform_package_name=null\r\nfreeform_timestamps=[\"{com.vng.pubgmobile=[1596206576764], com.lxqd.myhotpotstroy.abroad=[1670586991627, 1670587000692], com.facebook.katana=[1634964008263, 1684237410411, 1684329086303, 1697632560470, 1699332480893, 1704469988499, 1707315132864, 1711461005632, 1714650526190, 1717675356697], com.vzcreations.lightignite=[1618662886790], cn.wps.moffice_eng=[1617197017557, 1617197062207], com.zing.zalo=[1709812324494, 1710418297552, 1712825362151, 1712826340823, 1712826397448, 1713874614311, 1715849541059, 1718802810550, 1719674807239, 1719947153374], com.android.mms=[1678202904223, 1703678917937, 1705464806951, 1707054934479, 1710421648064, 1715764043797, 1715873579377, 1715966005570, 1716119931010, 1716463464563], com.android.chrome=[1619926755305, 1671166183670, 1678471545411, 1681448403055], com.facebook.lite=[1596208475279, 1596208496817, 1608911478224], com.miui.gallery=[1617888592693], com.ss.android.ugc.trill=[1689397155156, 1697632569169, 1704470080550, 1711461017785, 1718774088002], org.telegram.messenger=[1669005618375, 1669005751286], com.facebook.orca=[1717071518397, 1717202727690, 1717202798252, 1717202968675, 1717203787705, 1717256658403, 1717395684255, 1717477557558, 1717589012218, 1717676796731]}\"]\r\nfreeform_window_state=-1\r\nfw_fsgesture_support_superpower=1\r\ngamebooster_data_migration=1\r\ngamebooster_remove_desktop_icon=1\r\ngb_boosting=0\r\ngb_handsfree=0\r\ngb_notification=0\r\nglobal_satisfaction_miui_version=10\r\ngxzw_icon_aod_show_enable=1\r\nhas_new_antispam=1\r\nhuanji_used=1\r\nhush_gesture_used=0\r\nimmersive_mode_confirmations=\r\nin_call_notification_enabled=0\r\ninput_methods_subtype_history=com.google.android.inputmethod.latin/com.android.inputmethod.latin.LatinIME;1891618174:com.vng.inputmethod.labankey/.LatinIME;1891618174\r\ninstall_non_market_apps=1\r\ninvalidCleanAlertNotificationCount=65\r\nkey_app_lock_state_data_migrated=1\r\nkey_clean_alert_switch_state=1\r\nkey_garbage_danger_in_flag=1\r\nkey_garbage_danger_in_size=2000133545\r\nkey_garbage_deepclean_size=13077204932\r\nkey_garbage_facebook_size=2803620\r\nkey_garbage_installed_app_count=50\r\nkey_garbage_multi_video_size=713936920\r\nkey_garbage_normal_size=2000133545\r\nkey_garbage_not_used_app_count=0\r\nkey_garbage_whatsapp_size=0\r\nkey_homelist_cache_deleted=1\r\nkey_is_in_miui_sos_mode=0\r\nkey_latest_battery_status=false;false;false;true;false;false;false;false;false\r\nkey_latest_virus_scan_date=1656422564190\r\nkey_latest_wifi_ssid_bssid=(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(b653590ed4cb0c91224914a41c6c9336998fdd99d69d6eefe9925b2973a2f323,0f4c91568765603d3b852f2049d546b99364fde2cf2a22ab6576828eb892a410);(a67aa7def01786f3c71f978623c5ea92ac62cca273f999826b9a27a3e3834256,4e2168efd870d01a035e87bca1aeb40a107a84efecc821e095253faf7efa78e6);(2e27fb853314ad95048e12344113f9078a5396c5af45fd2e4c315f82fce00931,6507132733c14d9353a055525a644715910672bd18ce6e43b50d6d92b39ad269);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e)\r\nkey_launcher_loading_finished=1\r\nkey_long_press_volume_down=none\r\nkey_notificaiton_general_clean_need=1\r\nkey_notificaiton_general_clean_size=2000133545\r\nkey_notificaiton_whatsapp_clean_size=0\r\nkey_notification_wechat_size=0\r\nkey_notification_wechat_size_need=0\r\nkey_notification_whatsapp_clean_need=0\r\nkey_open_earthquake_warning=0\r\nkey_score_in_security=70\r\nkey_xspace_boot_guide_times=1\r\nlast_setup_shown=eclair_1\r\nlocation_changer=2\r\nlocation_mode=0\r\nlocation_providers_allowed=\r\nlock_screen_allow_private_notifications=1\r\nlock_screen_magazine_status=1\r\nlock_screen_owner_info_enabled=0\r\nlock_screen_show_notifications=1\r\nlockscreen.disabled=0\r\nlockscreen.options=enable_facelock\r\nlong_press_timeout=400\r\nmanual_ringer_toggle_count=0\r\nmark_time_agent=0\r\nmark_time_agent_sim_2=0\r\nmark_time_fraud=0\r\nmark_time_fraud_sim_2=0\r\nmark_time_sell=0\r\nmark_time_sell_sim_2=0\r\nmasterLocationPackagePrefixBlacklist=com.google.,com.semaphoremobile.zagat.android\r\nmasterLocationPackagePrefixWhitelist=com.google.android.gms\r\nmedia_button_receiver=\r\nmiui_bubbles_pinned_apps=\r\nmiui_updater_enable=1\r\nmms_backup_enabled=1\r\nmms_backup_in_progress=0\r\nmms_backup_last_completed=1718480240776\r\nmms_restore_account_name=null\r\nmobile_download=0\r\nmock_location=0\r\nmount_play_not_snd=1\r\nmount_ums_autostart=0\r\nmount_ums_notify_enabled=1\r\nmount_ums_prompt=1\r\nmulti_press_timeout=300\r\nnearby_sharing_component=com.google.android.gms/com.google.android.gms.nearby.sharing.ShareSheetActivity\r\nneed_convert_virtual_phone=0\r\nnfc_payment_default_component=com.google.android.gms/com.google.android.gms.tapandpay.hce.service.TpHceService\r\nnotification_badging=1\r\nnotification_bubbles=0\r\nold_clipboard_content_need_clear_new=1\r\noptimizer_scan_cloud=1\r\npackage_changed_browser=1\r\npackage_changed_calendar=1\r\npackage_verifier_state=1\r\npai_selection_page_complete=1\r\npassword_has_promotioned=0\r\npc_security_center_extreme_mode_enter=0\r\npermission_revocation_first_enabled_timestamp_ms=1648485416208\r\nphotos_restore_account_name=null\r\nplay_determined_dma_eligibility=0\r\npower_supersave_tile_enabled=1\r\npref_key_cleaner_uuid=90f5e0bb-a361-4fdb-b5e8-caf9a14c146f\r\npref_open_game_booster=1\r\npreview_plan=0\r\nprint_service_search_uri=https://play.google.com/store/apps/collection/promotion_3000abc_print_services\r\nprivacy_add_account_md5=51e39ddb3bc3a73b7d1fe0f1ae3cc095\r\nprivacy_mode_enabled=0\r\nprivacy_password_countDownTimer_deadline=0\r\nprivacy_password_finger_authentication_num=0\r\nprivacy_password_is_open=1\r\nprivacy_password_status=1\r\nprivacy_password_status_is_record=1\r\nprivacy_status_com.android.providers.downloads.ui=1\r\nprivacy_status_com.miui.securitycenter=1\r\nprivacy_wrong_attempt_num=0\r\npubkey_blacklist=410f36363258f30b347d12ce4863e433437806a8,c4f9663716cd5e71d6950b5f33ce041c95b435d1,e23b8d105f87710a68d9248050ebefc627be4ca6,7b2e16bc39bcd72b456e9f055d1de615b74945db,e8f91200c65cee16e039b9f883841661635f81c5,0129bcd5b448ae8d2496d1c3e19723919088e152,d33c5b41e45cc4b3be9ad6952c4ecc2528032981,e12d89f56d2276f830e6ceafa66c725c0b41a932,d9f5c6ce57ffaa39cc7ed172bd53e0d307834bd1,3ecf4bbbe46096d514bb539bb913d77aa4ef31bf,685eec0a39f668ae8fd8964f987476b4504fd2be,0e502d4dd1e160368a31f06a810431ba6f72c041,93d1532229cc2abd21dff597ee320fe4246f3d0c,f57179faea10c5438cb0c6e1cc277b6e0db2ff54,077ac7de8da558643a06c5369e554faeb3dfa166,e58e315baaeeaac6e72ec9573670ca2f254ec347,384d0c1dc477a7b3f86786d018519f589f1e9e25,c1f9f2894f2e8521507c089f362d5cd9feff7e21,11f75f354126c2c849f53f29cf929604233cf869,e0e90e95afab970b897bfb503f72cb4c06cfb4f3\r\nquick_reply=0\r\nquiet_mode_enable=0\r\nscreen_buttons_has_been_disabled=1\r\nscreen_buttons_state=0\r\nscreen_project_caller=com.miui.securitycenter:ui\r\nscreen_project_hang_up_on=0\r\nscreen_project_in_screening=0\r\nscreen_project_private_on=1\r\nscreen_project_small_window_on=0\r\nscreensaver_activate_on_dock=1\r\nscreensaver_activate_on_sleep=0\r\nscreensaver_components=com.google.android.deskclock/com.android.deskclock.Screensaver\r\nscreensaver_default_component=com.google.android.deskclock/com.android.deskclock.Screensaver\r\nscreensaver_enabled=1\r\nselected_input_method_subtype=-1\r\nselected_spell_checker=com.google.android.inputmethod.latin/com.android.inputmethod.latin.spellcheck.AndroidSpellCheckerService\r\nselected_spell_checker_subtype=0\r\nsend_action_app_error=1\r\nserial_blacklist=\r\nsettings_face_id_prefix_5=Khuôn mặt 1\r\nsettings_face_id_prefix_6=Khuôn mặt 2\r\nsettings_face_id_prefix_7=Khuôn mặt 1\r\nsettings_face_id_prefix_8=Khuôn mặt 2\r\nsettings_fingerprint_id_prefix_-128000784=Vân tay4\r\nsettings_fingerprint_id_prefix_-1383024586=Vân tay5\r\nsettings_fingerprint_id_prefix_-1615912973=Vân tay5\r\nsettings_fingerprint_id_prefix_-1820902113=Vân tay5\r\nsettings_fingerprint_id_prefix_-1954678606=Vân tay1\r\nsettings_fingerprint_id_prefix_-2109803503=Vân tay1\r\nsettings_fingerprint_id_prefix_-312740984=Vân tay2\r\nsettings_fingerprint_id_prefix_-34436474=Vân tay5\r\nsettings_fingerprint_id_prefix_-469956686=Vân tay4\r\nsettings_fingerprint_id_prefix_-506813775=Vân tay2\r\nsettings_fingerprint_id_prefix_-786886107=Vân tay3\r\nsettings_fingerprint_id_prefix_-83753695=Vân tay2\r\nsettings_fingerprint_id_prefix_1271325661=Vân tay3\r\nsettings_fingerprint_id_prefix_1292573291=Vân tay5\r\nsettings_fingerprint_id_prefix_1327387519=Vân tay4\r\nsettings_fingerprint_id_prefix_1718788240=Vân tay2\r\nsettings_fingerprint_id_prefix_1801536666=Vân tay1\r\nsettings_fingerprint_id_prefix_2141630964=Vân tay3\r\nsettings_fingerprint_id_prefix_36833109=Vân tay2\r\nsettings_fingerprint_id_prefix_526937310=Vân tay3\r\nsettings_fingerprint_id_prefix_681645577=Vân tay1\r\nsettings_fingerprint_id_prefix_769120380=Vân tay1\r\nsettings_fingerprint_id_prefix_801484520=Vân tay3\r\nsettings_fingerprint_id_prefix_848125878=Vân tay4\r\nshare_tail_disable=1\r\nshield_super_save_bar=1\r\nshortcut_recall_interval=604800000\r\nshortcut_recall_invalid=1\r\nshortcut_recall_max_count=2\r\nshow_first_crash_dialog_dev_option=0\r\nshow_ime_with_hard_keyboard=0\r\nshow_zen_settings_suggestion=0\r\nsidebar_bounds=\r\nsilence_gesture=not_set\r\nskip_gesture=not_set\r\nsleep_timeout=-1\r\nsnoozed_schedule_condition_provider=\r\nspeak_password=1\r\nssl_session_cache=file\r\nstatus_memory_clean_shortcut=1\r\nstatus_whatsapp_shortcut=0\r\nsync_parent_sounds=1\r\nsystemui_fsgesture_support_superpower=1\r\nsysui_qs_tiles=cell,bt,wifi,flashlight,autobrightness,mute,screenshot,airplane,screenlock,gps,rotation,hotspot,scanner,papermode,night,quietmode,batterysaver,custom(com.miui.securitycenter/com.miui.superpower.notification.SuperPowerTileService),custom(com.milink.service/com.milink.ui.service.MiLinkTileService),custom(com.miui.mishare.connectivity/.tile.MiShareTileService),freeformhang,custom(com.miui.screenrecorder/.service.QuickService),vibrate,nfc,edit\r\nsysui_tuner_version=1\r\ntouch_exploration_enabled=0\r\ntouch_exploration_granted_accessibility_services=null\r\ntrust_agents_initialized=1\r\ntts_default_synth=com.google.android.tts\r\nui_night_mode=1\r\nunknown_sources_default_reversed=1\r\nunsafe_volume_music_active_ms=3120000\r\nupload_debug_log_pref=1\r\nupload_log_pref=1\r\nusb_audio_automatic_routing_disabled=0\r\nuser_full_data_backup_aware=1\r\nuser_setup_complete=1\r\nuser_setup_personalization_state=10\r\nvoice_interaction_service=com.google.android.googlequicksearchbox/com.google.android.voiceinteraction.GsaVoiceInteractionService\r\nvoice_recognition_service=com.google.android.googlequicksearchbox/com.google.android.voicesearch.serviceapi.GoogleRecognitionService\r\nvolume_hush_gesture=1\r\nvpn_password_enable=1\r\nvtb_boosting=0\r\nwake_gesture_enabled=0\r\nweb_autofill_query_url=http://android.clients.google.com/proxy/webautofill\r\nxspace_enabled=0\r\nzen_duration=0\r\nzen_settings_suggestion_viewed=0\r\nzen_settings_updated=1\r\nzman_cloud_disable=0\r\nzman_share_hide_camera=1\r\nzman_share_hide_camera_default=1\r\nzman_share_hide_location=1\r\nzman_share_hide_location_default=1";
            }

            // 1-0
            string FBO_STATE_OPEN = GetValue(str_BaoMat, "FBO_STATE_OPEN");
            if (FBO_STATE_OPEN == "1") { sw2_TrangThaiFBO.IsOn = true; }
            else { sw2_TrangThaiFBO.IsOn = false; }

            string POWER_SAVE_GUIDE_ENABLE = GetValue(str_BaoMat, "POWER_SAVE_GUIDE_ENABLE");
            if (POWER_SAVE_GUIDE_ENABLE == "1") { sw2_CheDoTietKiemPin.IsOn = true; }
            else { sw2_CheDoTietKiemPin.IsOn = false; }

            string access_control_lock_enabled = GetValue(str_BaoMat, "access_control_lock_enabled");
            if (access_control_lock_enabled == "1") { sw2_KiemSoatTruyCap.IsOn = true; }
            else { sw2_KiemSoatTruyCap.IsOn = false; }

            string lock_screen_show_notifications = GetValue(str_BaoMat, "lock_screen_show_notifications");
            if (lock_screen_show_notifications == "1") { sw2_HienThiTBMHKhoa.IsOn = true; }
            else { sw2_HienThiTBMHKhoa.IsOn = false; }

            string lockscreen = GetValue(str_BaoMat, "lockscreen.disabled");
            if (lockscreen == "1") { sw2_ManHinhKhoa.IsOn = false; }
            else if (lockscreen == "0") { sw2_ManHinhKhoa.IsOn = true; }

            string auto_download = GetValue(str_BaoMat, "auto_download");
            if (auto_download == "1") { sw2_TuDongCapNhat.IsOn = true; }
            else { sw2_TuDongCapNhat.IsOn = false; }

            string auto_update = GetValue(str_BaoMat, "auto_update");
            if (auto_update == "1") { sw2_TuDongTai.IsOn = true; }
            else { sw2_TuDongTai.IsOn = false; }

            // Text
            string bluetooth_name = GetValue(str_BaoMat, "bluetooth_name");
            txtTenBluetooth.Text = bluetooth_name;

            string bluetooth_address = GetValue(str_BaoMat, "bluetooth_address");
            txtDiaChiBluetooth.Text = bluetooth_address;

            string default_input_method = GetValue(str_BaoMat, "default_input_method");
            txtPhuongThucNhap.Text = default_input_method;

            string allowed_geolocation_origins = GetValue(str_BaoMat, "allowed_geolocation_origins");
            txtNguonSuDungDinhVi.Text = allowed_geolocation_origins;

            string settings_face_id_prefix = GetCount(str_BaoMat, "Khuôn mặt ").ToString();
            txtSoKhuonMat.Text = settings_face_id_prefix;

            string settings_fingerprint_id_prefix = GetCount(str_BaoMat, "Vân tay").ToString();
            txtSoVanTay.Text = settings_fingerprint_id_prefix;

        }

        private void UpdateTab_BaoMat()
        {

        }

        private void LoadTab_CucBo()
        {
            query = "shell settings list global";
            string str_CucBo = adb.adbCommand(query);
            if (str_CucBo == string.Empty)
            {
                str_CucBo = "BtMihomeDataTrafficMonth=2024-07,1377436\r\nPhenotype_boot_count=140\r\nPhenotype_flags=activity_starts_logging_enabled:alarm_manager_constants:alarm_manager_dummy_flags:always_on_display_constants:anomaly_config:anomaly_config_version:anomaly_detection_constants:app_idle_constants:app_standby_enabled:appop_history_parameters:backup_agent_timeout_parameters:battery_stats_constants:battery_tip_constants:binder_calls_stats:ble_scan_balanced_interval_ms:ble_scan_balanced_window_ms:ble_scan_low_power_interval_ms:ble_scan_low_power_window_ms:blocking_helper_dismiss_to_view_ratio:blocking_helper_streak_limit:device_idle_constants:emergency_call_codes_data:gnss_satellite_blacklist:hybrid_sysui_battery_warning_flags:job_scheduler_constants:job_scheduler_quota_controller_constants:job_scheduler_time_controller_constants:location_background_throttle_interval_ms:location_ignore_settings_package_whitelist:network_watchlist_enabled:night_display_forced_auto_mode_available:notification_snooze_options:phenotype_test_setting:settings_use_external_provider_api:settings_use_psd_api:smart_replies_in_notifications_flags:sqlite_compatibility_wal_flags:sys_uidcpupower:text_classifier_constants:zram_enabled\r\n_boot_Phenotype_flags=\r\nactivity_starts_logging_enabled=1\r\nad_aaid=14dc11f7-c872-45b2-a2ed-5b0735122269\r\nadb_enabled=1\r\nadb_wifi_auth_required=0\r\nadb_wifi_enabled=1\r\nadd_users_when_locked=0\r\nairplane_mode_on=0\r\nairplane_mode_radios=cell,bluetooth,wifi,nfc,wimax\r\nairplane_mode_toggleable_radios=bluetooth,wifi,nfc\r\nalarm_manager_constants=\r\nalarm_manager_dummy_flags=null\r\nalways_finish_activities=0\r\nalways_on_display_constants=null\r\nanimator_duration_scale=1.0\r\nanomaly_config=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\r\nanomaly_config_version=10\r\nanomaly_detection_constants=null\r\napp_idle_constants=\r\napp_standby_enabled=1\r\napply_ramping_ringer=0\r\nappop_history_parameters=mode=HISTORICAL_MODE_DISABLED,baseIntervalMillis=1000,intervalMultiplier=10\r\nart_verifier_verify_debuggable=1\r\nassisted_gps_enabled=1\r\naudio_safe_volume_state=3\r\nauto_test_mode_on=0\r\nauto_time=1\r\nauto_time_zone=1\r\nautofill_compat_mode_allowed_packages=com.android.chrome[url_bar]:com.brave.browser[url_bar]:com.brave.browser_beta[url_bar]:com.brave.browser_nightly[url_bar]:com.chrome.beta[url_bar]:com.chrome.dev[url_bar]:com.chrome.canary[url_bar]:com.microsoft.emmx[url_bar]:com.microsoft.emmx.beta[url_bar]:com.microsoft.emmx.canary[url_bar]:com.microsoft.emmx.dev[url_bar]:com.opera.browser[url_field]:com.opera.browser.beta[url_bar]:com.opera.mini.native[url_bar]:com.opera.mini.native.beta[url_bar]:com.sec.android.app.sbrowser[location_bar_edit_text]:com.sec.android.app.sbrowser.beta[location_bar_edit_text]:org.mozilla.fennec_aurora[url_bar]:org.mozilla.firefox[url_bar]:org.mozilla.firefox_beta[url_bar]\r\nautofill_logging_level=0\r\nbackup_agent_timeout_parameters=\r\nbattery_saver_constants=null\r\nbattery_stats_constants=track_cpu_times_by_proc_state=false\r\nbattery_tip_constants=app_restriction_enabled=true\r\nbinder_calls_stats=\r\nble_scan_always_enabled=1\r\nble_scan_balanced_interval_ms=730\r\nble_scan_balanced_window_ms=183\r\nble_scan_low_power_interval_ms=1200\r\nble_scan_low_power_window_ms=80\r\nblocking_helper_dismiss_to_view_ratio=null\r\nblocking_helper_streak_limit=null\r\nbluetooth_disabled_profiles=0\r\nbluetooth_on=0\r\nboot_count=140\r\ncall_auto_retry=0\r\ncan_nav_bar_hide=0\r\ncar_dock_sound=/system/media/audio/ui/Dock.ogg\r\ncar_undock_sound=/system/media/audio/ui/Undock.ogg\r\ncarrier_app_names=com.google.android.apps.tycho:Google Fi\r\ncarrier_app_whitelist=4C36AF4A5BDAD97C1F3D8B283416D244496C2AC5EAFE8226079EF6F676FD1859:com.google.android.apps.tycho\r\ncdma_cell_broadcast_sms=1\r\ncell_on=1\r\ncert_pin_content_url=https://www.gstatic.com/android/config_update/08202014-pins.txt\r\ncert_pin_metadata_url=https://www.gstatic.com/android/config_update/08202014-metadata.txt\r\ncom.android.settings.superscript_count=1\r\ncom.android.settings.superscript_map=1\r\ncom.android.thememanager.preferences.IS_THEME_ANIMATION_PREVIEW_ENABLED=true\r\ncom.android.thememanager.preferences.LAST_USER_PRIVACY_AGREEMENT_REGION=VN\r\ncom.android.thememanager.preferences.PRIVACY_NOT_ASK_REGION=\r\ncom.android.thememanager.preferences.apply_theme_with_ringtones=false\r\ncom.android.thememanager.preferences.update_using_theme_automatically=false\r\ncom.android.thememanager.preferences.using_personal=true\r\ncom.android.thememanager.preferences.using_theme_show_ad=true\r\ncom.mi.android.globalpersonalassistant.preferences.ad_refresh=1\r\ncom.mi.android.globalpersonalassistant.preferences.app_recommend_more_deep_linkVN=\r\ncom.mi.android.globalpersonalassistant.preferences.app_recommend_refresh_interval=10\r\ncom.mi.android.globalpersonalassistant.preferences.app_recommend_strategy_config={\"strategy\":2,\"cache\":24}\r\ncom.mi.android.globalpersonalassistant.preferences.cache_size=5\r\ncom.mi.android.globalpersonalassistant.preferences.flag_app_listener_machine=0\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_SETTINGS_ORDER=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_VNshortcuts.user=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.apk_new_version_auto_upgrade=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.cricket_card_config=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.has_click_calendar_permission_button=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.head_icon_config=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.key_cricket_match=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.key_football=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.market_update_switch_data=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.recommend_ad_animation_config=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.twitter_ad_configVN=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.utility_card_data=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.videos_config=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_com.mi.android.globalminusscreen.videos_config_support_v3=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_content.function.user=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_function.user=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_health_has_goal_moved=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_health_has_privacy_aggreed=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_noteboard=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_recommend_games_card_data_VNvi_VN=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_recommend_games_card_data_v2_VNvi_VN=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_shortcuts.source=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_shortcuts.user=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_stock.user=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_move_data_videos_youtubeVN=1\r\ncom.mi.android.globalpersonalassistant.preferences.has_used_system_agree_time=1\r\ncom.mi.android.globalpersonalassistant.preferences.icon_ads_preload_ad_id=1.337.4.1\r\ncom.mi.android.globalpersonalassistant.preferences.icon_ads_request_mode=0\r\ncom.mi.android.globalpersonalassistant.preferences.key_agenda_assistant=3\r\ncom.mi.android.globalpersonalassistant.preferences.key_app_recomment=3\r\ncom.mi.android.globalpersonalassistant.preferences.key_booking=7\r\ncom.mi.android.globalpersonalassistant.preferences.key_games_card_type_VNvi_VN=-1\r\ncom.mi.android.globalpersonalassistant.preferences.key_has_request_calendar_permission=1\r\ncom.mi.android.globalpersonalassistant.preferences.key_is_minus_first_show=0\r\ncom.mi.android.globalpersonalassistant.preferences.key_market_notification_view=8\r\ncom.mi.android.globalpersonalassistant.preferences.key_newsfeed=3\r\ncom.mi.android.globalpersonalassistant.preferences.key_noteboard=3\r\ncom.mi.android.globalpersonalassistant.preferences.key_rate=8\r\ncom.mi.android.globalpersonalassistant.preferences.key_recommend_games_has_customized_card_order=1\r\ncom.mi.android.globalpersonalassistant.preferences.key_recommended_deals=7\r\ncom.mi.android.globalpersonalassistant.preferences.key_security_center=3\r\ncom.mi.android.globalpersonalassistant.preferences.key_social=2\r\ncom.mi.android.globalpersonalassistant.preferences.key_stock=8\r\ncom.mi.android.globalpersonalassistant.preferences.key_videos=3\r\ncom.mi.android.globalpersonalassistant.preferences.key_weather=2\r\ncom.mi.android.globalpersonalassistant.preferences.load_ad_interval=480\r\ncom.mi.android.globalpersonalassistant.preferences.load_ad_num=5\r\ncom.mi.android.globalpersonalassistant.preferences.load_strategy=on_entry\r\ncom.mi.android.globalpersonalassistant.preferences.merge_times=2\r\ncom.mi.android.globalpersonalassistant.preferences.stock_title_schema=0\r\ncom.mi.globalbrowser.preferences.AnonymousIdTransfer=1\r\ncom.mi.globalbrowser.preferences.BookmarksTransfer=1\r\ncom.mi.globalbrowser.preferences.CollectFlowTransfer=1\r\ncom.mi.globalbrowser.preferences.DefaultPrefsTransfer=1\r\ncom.mi.globalbrowser.preferences.DefaultPrefsTransfer_preload=1\r\ncom.mi.globalbrowser.preferences.DownloadTransfer=1\r\ncom.mi.globalbrowser.preferences.HistorySyncTransfer=1\r\ncom.mi.globalbrowser.preferences.HistoryTransfer=1\r\ncom.mi.globalbrowser.preferences.HomePageTransfer=1\r\ncom.mi.globalbrowser.preferences.ImagesTransfer=1\r\ncom.mi.globalbrowser.preferences.KVPrefsTransfer=1\r\ncom.mi.globalbrowser.preferences.KVPrefsTransfer_preload=1\r\ncom.mi.globalbrowser.preferences.LanguageKVPrefsTransfer=1\r\ncom.mi.globalbrowser.preferences.MostVisitedTransfer=1\r\ncom.mi.globalbrowser.preferences.PartnerBookmarksTransfer=1\r\ncom.mi.globalbrowser.preferences.PrivateFolderTransfer=1\r\ncom.mi.globalbrowser.preferences.QuickLinkTransfer=1\r\ncom.mi.globalbrowser.preferences.SearchHistoryTransfer=1\r\ncom.xiaomi.mipicks.superscript_count=10\r\ncom.xiaomi.system.devicelock.locked=0\r\ncontent_capture_service_explicitly_enabled=default\r\nconversation_actions_content_url=https://www.gstatic.com/android/text_classifier/actions/q/v103/default.model\r\nconversation_actions_metadata_url=https://www.gstatic.com/android/text_classifier/actions/q/v103/default.model.metadata\r\ndata_roaming1=0\r\ndata_roaming2=0\r\ndata_roaming3=0\r\ndata_roaming4=0\r\ndata_roaming5=0\r\ndata_roaming6=0\r\ndata_roaming7=0\r\ndata_roaming8=0\r\ndata_roaming=0\r\ndatabase_creation_buildid=QKQ1.190825.002\r\ndebug.force_rtl=0\r\ndebug_app=null\r\ndebug_view_attributes=0\r\ndefault_install_location=0\r\ndefault_restrict_background_data=0\r\ndesk_dock_sound=/system/media/audio/ui/Dock.ogg\r\ndesk_undock_sound=/system/media/audio/ui/Undock.ogg\r\ndevelopment_settings_enabled=1\r\ndevice_first_using_data=1\r\ndevice_idle_constants=light_after_inactive_to=180000,light_pre_idle_to=180000,\r\ndevice_name=Mi 9T\r\ndevice_provisioned=1\r\ndevice_provisioning_mobile_data=1\r\ndock_audio_media_enabled=1\r\ndock_sounds_enabled=0\r\ndock_sounds_enabled_when_accessbility=0\r\ndownload_manager_recommended_max_bytes_over_mobile=2147483647\r\nemergency_affordance_needed=0\r\nemergency_call_codes_data=null\r\nemergency_tone=0\r\nenable_cellular_on_boot=1\r\nenable_fileexplorer_private_folder=0\r\nenable_freeform_support=0\r\nenable_gnss_raw_meas_full_tracking=0\r\nenable_gpu_debug_layers=0\r\nenable_screen_on_proximity_sensor=1\r\nfast_connect_ble_scan_mode=0\r\nfast_connect_support_mode=1\r\nforce_allow_on_external=0\r\nforce_desktop_mode_on_external_displays=0\r\nforce_fsg_nav_bar=0\r\nforce_immersive_nav_bar=0\r\nforce_resizable_activities=0\r\ngnss_satellite_blacklist=\r\nheads_up_notifications_enabled=1\r\nhide_gesture_line=0\r\nhide_nav_bar=0\r\nhybrid_sysui_battery_warning_flags=\r\nis_default_icon=0\r\nisolated_storage_remote=null\r\njob_scheduler_constants=\r\njob_scheduler_quota_controller_constants=max_job_count_per_rate_limiting_window=10,rate_limiting_window_ms=60000,max_job_count_active=75,max_session_count_active=75\r\njob_scheduler_time_controller_constants=\r\nkey_securitycenter_never_show_vb_box=0\r\nlang_id_content_url=https://www.gstatic.com/android/text_classifier/langid/q/v1/model.smfb\r\nlang_id_metadata_url=https://www.gstatic.com/android/text_classifier/langid/q/v1/model.smfb.metadata\r\nlid_behavior=1\r\nlocation_background_throttle_interval_ms=600000\r\nlocation_global_kill_switch=0\r\nlocation_ignore_settings_package_whitelist=com.google.android.gms,com.google.android.dialer\r\nlock_sound=/system/media/audio/ui/Lock.ogg\r\nlow_battery_sound=/system/media/audio/ui/LowBattery.ogg\r\nlow_battery_sound_timeout=0\r\nlow_power=0\r\nmax_sound_trigger_detection_service_ops_per_day=1000\r\nmilink_setting_entrance_compat=1\r\nmiui_carrier_region=normal\r\nmiui_nearby_download_device_time=00000000:1707802485949,\r\nmiui_new_version=V12.0.9.0.QFJMIXM\r\nmiui_pre_big_miui_version=12\r\nmiui_pre_codebase=10\r\nmiui_pre_version=V12.0.5.0.QFJMIXM\r\nmiui_pre_version_miservice=12\r\nmiui_update_ready=0\r\nmobile_data0=1\r\nmobile_data1=1\r\nmobile_data=0\r\nmobile_data_always_on=1\r\nmode_ringer=1\r\nmulti_cb=1\r\nmulti_sim_data_call=7\r\nmulti_sim_sms=7\r\nmulti_sim_sms_prompt=0\r\nmulti_sim_voice_call=-1\r\nmusic_in_white_list=0\r\nnetstats_enabled=1\r\nnetstats_uid_bucket_duration=3600000\r\nnetstats_uid_tag_bucket_duration=3600000\r\nnetwork_recommendations_enabled=0\r\nnetwork_recommendations_package=com.google.android.gms\r\nnetwork_scoring_ui_enabled=1\r\nnetwork_watchlist_enabled=\r\nnetwork_watchlist_last_report_time=1720112400000\r\nnew_device_after_support_notification_animation=1\r\nnight_display_forced_auto_mode_available=0\r\nnotification_snooze_options=null\r\nntp_server_2=persist.vendor.ntp.svr_2\r\nota_disable_automatic_update=1\r\noverlay_display_devices=null\r\npackage_verifier_enable=1\r\npackage_verifier_user_consent=1\r\npersonalized_ad_enabled=3\r\npersonalized_ad_time=1586676819149\r\nphenotype_test_setting=V15AboveDefault\r\npolicy_control=immersive.preconfirms=*\r\npower_sounds_enabled=1\r\npref_key_sys_app_auto_update=1\r\npref_key_wallpaper_screen_scrolled_span=0\r\npreferred_network_mode1=20\r\npreferred_network_mode2=18\r\npreferred_network_mode3=20\r\npreferred_network_mode4=20\r\npreferred_network_mode5=20\r\npreferred_network_mode6=20\r\npreferred_network_mode7=20\r\npreferred_network_mode8=20\r\npreferred_network_mode=20,20\r\nrequire_password_to_decrypt=0\r\nsecurity_center_pc_save_mode_data={\"a\":0,\"b\":-1,\"c\":-1,\"d\":-1}\r\nsend_action_app_error=1\r\nset_install_location=0\r\nsettings_network_and_internet_v2=false\r\nsettings_use_external_provider_api=1\r\nsettings_use_psd_api=1\r\nshow_notification_channel_warnings=0\r\nsmall_window_shown_notification=1\r\nsmart_replies_in_notifications_flags=enabled=true,max_squeeze_remeasure_attempts=3,requires_targeting_p=true\r\nsmart_selection_content_url=https://www.gstatic.com/android/text_classifier/q/v714/universal.fb\r\nsmart_selection_metadata_url=https://www.gstatic.com/android/text_classifier/q/v714/universal.fb.metadata\r\nsms_short_codes_content_url=https://www.gstatic.com/android/config_update/05222024-sms-denylist.txt\r\nsms_short_codes_metadata_url=https://www.gstatic.com/android/config_update/05222024-sms-denylist-metadata.txt\r\nsoft_ap_timeout_enabled=0\r\nsound_trigger_detection_service_op_timeout=15000\r\nsqlite_compatibility_wal_flags=\r\nstay_on_while_plugged_in=0\r\nsubscription_mode=0\r\nsys_uidcpupower=\r\nsystemui_float_whitelist=-1830023641\r\nsystemui_fsg_version=10\r\nsystemui_keyguard_whitelist=-1318924517\r\nsystemui_local_score=2099579515\r\nsysui_float_version=1718632814463\r\nsysui_keyguard_version=1718632814476\r\nsysui_powerui_enabled=0\r\ntether_offload_disabled=1\r\ntext_classifier_constants=generate_links_max_text_length=10000\r\ntheater_mode_on=0\r\ntorch_state=0\r\ntransition_animation_scale=1.0\r\ntrusted_sound=/system/media/audio/ui/Trusted.ogg\r\nuimode_timing=0\r\nunlock_sound=/system/media/audio/ui/Unlock.ogg\r\nupload_apk_enable=1\r\nusb_mass_storage_enabled=1\r\nuse_gesture_version_three=1\r\nuser_aggregate=-3\r\nuser_fold=-3\r\nverifier_timeout=17000\r\nverifier_verify_adb_installs=0\r\nwait_for_debugger=0\r\nwebview_fallback_logic_enabled=0\r\nwifi_coverage_extend_feature_enabled=0\r\nwifi_display_certification_on=0\r\nwifi_display_on=1\r\nwifi_max_dhcp_retry_count=9\r\nwifi_networks_available_notification_on=0\r\nwifi_on=0\r\nwifi_saved_state=0\r\nwifi_scan_always_enabled=1\r\nwifi_scan_throttle_enabled=1\r\nwifi_sleep_policy=2\r\nwifi_verbose_logging_enabled=0\r\nwifi_wakeup_enabled=0\r\nwindow_animation_scale=1.0\r\nwireless_charging_started_sound=/system/media/audio/ui/charging.ogg\r\nwireless_reverse_charging=30\r\nxiaomi_account_is_child=0\r\nzen_duration=null\r\nzen_mode=4\r\nzen_mode_config_etag=885663743\r\nzen_mode_ringer_level=null\r\nzram_enabled=1";
            }

            // 1-0
            string adb_enabled = GetValue(str_CucBo, "adb_enabled");
            if (adb_enabled == "1") { sw3_adb.IsOn = true; }
            else { sw3_adb.IsOn = false; }

            string adb_wifi_enabled = GetValue(str_CucBo, "adb_wifi_enabled");
            if (adb_wifi_enabled == "1") { sw3_adbWifi.IsOn = true; }
            else { sw3_adbWifi.IsOn = false; }

            string adb_wifi_auth_required = GetValue(str_CucBo, "adb_wifi_auth_required");
            if (adb_wifi_auth_required == "1") { sw3_adbWifiAuth.IsOn = true; }
            else { sw3_adbWifiAuth.IsOn = false; }

            string airplane_mode_on = GetValue(str_CucBo, "airplane_mode_on");
            if (airplane_mode_on == "1") { sw3_CheDoMayBay.IsOn = true; }
            else { sw3_CheDoMayBay.IsOn = false; }

            string wifi_on = GetValue(str_CucBo, "wifi_on");
            if (wifi_on == "1") { sw3_Wifi.IsOn = true; }
            else { sw3_Wifi.IsOn = false; }

            string mobile_data0 = GetValue(str_CucBo, "mobile_data0");
            if (mobile_data0 == "1") { sw3_MangDiDong.IsOn = true; }
            else { sw3_MangDiDong.IsOn = false; }

            string bluetooth_on = GetValue(str_CucBo, "bluetooth_on");
            if (bluetooth_on == "1") { sw3_Bluetooth.IsOn = true; }
            else { sw3_Bluetooth.IsOn = false; }

            string netstats_enabled = GetValue(str_CucBo, "netstats_enabled");
            if (netstats_enabled == "1") { sw3_ThongKeLLMang.IsOn = true; }
            else { sw3_ThongKeLLMang.IsOn = false; }

            string auto_time = GetValue(str_CucBo, "auto_time");
            if (auto_time == "1") { sw3_CapNhatThoiGian.IsOn = true; }
            else { sw3_CapNhatThoiGian.IsOn = false; }

            string auto_time_zone = GetValue(str_CucBo, "auto_time_zone");
            if (auto_time_zone == "1") { sw3_CapNhatMuiGio.IsOn = true; }
            else { sw3_CapNhatMuiGio.IsOn = false; }

            string ble_scan_always_enabled = GetValue(str_CucBo, "ble_scan_always_enabled");
            if (ble_scan_always_enabled == "1") { sw3_QuetBle.IsOn = true; }
            else { sw3_QuetBle.IsOn = false; }

            string call_auto_retry = GetValue(str_CucBo, "call_auto_retry");
            if (call_auto_retry == "1") { sw3_ThuLaiCuocLoi.IsOn = true; }
            else { sw3_ThuLaiCuocLoi.IsOn = false; }

            string add_users_when_locked = GetValue(str_CucBo, "add_users_when_locked");
            if (add_users_when_locked == "1") { sw3_ThemNguoiDung.IsOn = true; }
            else { sw3_ThemNguoiDung.IsOn = false; }


            // Text
            string boot_count = GetValue(str_CucBo, "boot_count");
            txtSoLanKhoiDong.Text = boot_count;

            string netstats_uid_bucket_duration = GetValue(str_CucBo, "netstats_uid_bucket_duration");
            txtChuKyThuThap.Text = netstats_uid_bucket_duration;

            string wifi_max_dhcp_retry_count = GetValue(str_CucBo, "wifi_max_dhcp_retry_count");
            txtSoLanKetNoiLai.Text = wifi_max_dhcp_retry_count;
        }

        private void UpdateTab_CucBo()
        {

        }

        private void btnXuat_Click(object sender, System.EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, System.EventArgs e)
        {
            UpdateTab_HeThong();
            UpdateTab_BaoMat();
            UpdateTab_CucBo();
        }

        private void btnLamMoi_Click(object sender, System.EventArgs e)
        {
            LoadTab_HeThong();
            LoadTab_BaoMat();
            LoadTab_CucBo();
        }
    }
}
