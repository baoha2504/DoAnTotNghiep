using MTA_Mobile_Forensic.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTA_Mobile_Forensic.GUI.Android
{
    public partial class usr_ThayDoiMatKhau : UserControl
    {
        string query = "";
        adb adb = new adb();
        public usr_ThayDoiMatKhau()
        {
            InitializeComponent();
            LoadTab_1();
            LoadTab_2();
            LoadTab_3();
            LoadTab_4();
        }

        private void Load_Size_MaPin()
        {
            try
            {
                splitContainer1.SplitterDistance = (int)(panel_MaPin.Width / 2);
                panel1_MaPin.Height = (int)(panel_MaPin.Height / 4);
                panel2_MaPin.Height = (int)(panel_MaPin.Height / 4);
            }
            catch { }
        }

        private void Load_Size_MatMa()
        {
            try
            {
                splitContainer2.SplitterDistance = (int)(panel_MatMa.Width / 2);
                panel1_MatMa.Height = (int)(panel_MatMa.Height / 4);
                panel2_MatMa.Height = (int)(panel_MatMa.Height / 4);
            }
            catch { }
        }

        private void Load_Size_MauHinh()
        {
            try
            {
                splitContainer3.SplitterDistance = (int)(panel_MauHinh.Width / 2);
                panel1_MauHinh.Height = (int)(panel_MauHinh.Height / 4);
                panel2_MauHinh.Height = (int)(panel_MauHinh.Height / 4);
            }
            catch { }
        }

        private void Load_Size_SinhTracHoc()
        {
            try
            {
                splitContainer4.SplitterDistance = (int)(panel_SinhTracHoc.Width / 2);
                panel1_SinhTracHoc.Height = (int)(panel_SinhTracHoc.Height / 4);
                panel2_SinhTracHoc.Height = (int)(panel_SinhTracHoc.Height / 4);
            }
            catch { }
        }

        private void Load_Size()
        {
            Load_Size_MaPin();
            Load_Size_MatMa();
            Load_Size_MauHinh();
            Load_Size_SinhTracHoc();
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

        private void LoadTab_1()
        {

        }

        private void LoadTab_2()
        {

        }

        private void LoadTab_3()
        {

        }

        private void LoadTab_4()
        {
            query = "shell settings list secure";
            string str_BaoMat = adb.adbCommand(query);
            if (str_BaoMat == string.Empty)
            {
                str_BaoMat = "FBO_RULES=\r\nFBO_STATE_OPEN=1\r\nPOWER_SAVE_GUIDE_ENABLE=0\r\nSILENT_MODE_FOR_MIUI9=3\r\naccess_control_lock_enabled=1\r\naccessibility_display_magnification_enabled=0\r\naccessibility_display_magnification_scale=2.0\r\naccessibility_enabled=1\r\naccessibility_shortcut_on_lock_screen=1\r\nallowed_geolocation_origins=http://www.google.com http://www.google.co.uk\r\nam_show_system_apps=1\r\nandroid_id=e42edce4c9bc01f6\r\nanr_show_background=0\r\naod_mode_time=1\r\naod_mode_user_set=0\r\naod_show_style_update=1\r\naod_time_update=1\r\naod_using_super_wallpaper=0\r\napp_lock_add_account_md5=51e39ddb3bc3a73b7d1fe0f1ae3cc095\r\napplock_countDownTimer_deadline=0\r\nassistant=com.google.android.googlequicksearchbox/com.google.android.voiceinteraction.GsaVoiceInteractionService\r\naudio_game_4d=0\r\nauto_download=0\r\nauto_update=0\r\nautofill_field_classification=1\r\nautofill_service=com.google.android.gms/.autofill.service.AutofillService\r\nautofill_service_search_uri=https://play.google.com/store/apps/collection/promotion_30029f7_autofillservices_apps\r\nautofill_user_data_max_category_count=20\r\naware_enabled=not_set\r\naware_lock_enabled=0\r\nbackup_enabled:com.android.calllogbackup=1\r\nbackup_enabled:com.android.providers.telephony=1\r\nbackup_enabled=0\r\nbackup_encryption_opt_in_displayed=1\r\nbackup_transport=com.google.android.gms/.backup.BackupTransportService\r\nban_lock_list_change_pkg=com.android.browser,com.android.calendar\r\nbluetooth_address=A8:9C:ED:22:99:14\r\nbluetooth_name=Quốc Bảo\r\ncast_mode=0\r\ncast_mode_package=\r\nchange_index2name=1\r\ncharging_sounds_enabled=0\r\ncharging_vibration_enabled=1\r\ncleaner_dc_shortcut_recall_invalid=1\r\ncleaner_main_shortcut_recall_invalid=1\r\ncleaner_main_shortcut_recall_status=0\r\nclipboard_cipher_list=\r\ncloud_clipboard_cipher_content_saved=\r\ncom.xiaomi.market.need_update_app_count=10\r\ncom.xiaomi.market.need_update_game_count=0\r\ncom_android_settings_privacypassword_fingerprint_upgrade=1\r\ncom_miui_applicatinlock_use_fingerprint_state=2\r\ncom_miui_applicationlock_fingerprint_upgrade=1\r\nconfig_update_certificate=MIID4TCCAsmgAwIBAgIJAMsyDT+FwMuvMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYDVQQGEwJVUzETMBEGA1UECAwKQ2FsaWZvcm5pYTEWMBQGA1UEBwwNTW91bnRhaW4gVmlldzETMBEGA1UECgwKR29vZ2xlIEluYzEQMA4GA1UECwwHQW5kcm9pZDEjMCEGCSqGSIb3DQEJARYUc2VjdXJpdHlAYW5kcm9pZC5jb20wHhcNMTIwOTEwMjA0NDQxWhcNMTIxMDEwMjA0NDQxWjCBhjELMAkGA1UEBhMCVVMxEzARBgNVBAgMCkNhbGlmb3JuaWExFjAUBgNVBAcMDU1vdW50YWluIFZpZXcxEzARBgNVBAoMCkdvb2dsZSBJbmMxEDAOBgNVBAsMB0FuZHJvaWQxIzAhBgkqhkiG9w0BCQEWFHNlY3VyaXR5QGFuZHJvaWQuY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwuFNNPal1QpUSH8MU5T+BSSbCv/jTU8pbRKwYfq4ZmsjG1BxX8GJS/itdHr5IgG5YGSRpB9JkWJTAne54Qv2xX3ck8yMySjOKbsCuF3Qnrv1HiM1w0PmMvPC5GvDJz9alrrsq6uj40Hd5ina8pTKieYO1PNIf1MByBnrq/615cPOwb4UAYUwx4ChFG8aUNoChgYk0Ain7YoX4Pw7/tXIiVArsPWRbtzXVaMNWONRUM3SGVT5NO67LEEk0vIkDVjmGnJaDkkF+yqAr24ekl2qdMQb9cEnc1wNCMtVdyxUelZoVxFnot3m4ZhYIPzGI2E88TJ6c30Qs1ocHuPXW8KfGQIDAQABo1AwTjAdBgNVHQ4EFgQUXlT7Zj4V/hHVbyL54kmRYizK/powHwYDVR0jBBgwFoAUXlT7Zj4V/hHVbyL54kmRYizK/powDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOCAQEAE5D6pMFMJMwVTtrzZXExgn6r2q7LugmdIpaoFjiU/Iv/bk3VvhDU5jjE54G/fqi/g415yowkFQ02/ZkrTm/q3anvWsppN3NEns2YDi4moEhwLZcCVBzhjkppHGgkLHadzlJZdUFLnzZbDuiZrDNPEkjldVejF1qGt6LbW6d8xNmscerUdKJSUqryOi3U8SXjlTWu5d/hmajQLEe7VJ3y8q5rEsCfsb15EyYxNLfTZuf4PXVU/+kSciTyJxkPZAaz2J1zXQegzC+k3gzNnovlHYlDc5EaVnprsBgOCkaWQfjr6/5YyJysM96WMjndIjyFZSHnj9yF4j7JDwB9qEfxvA==\r\nconvert_foreground_location=0\r\nconvert_perm_GetInstall=0\r\ndefault_input_method=com.google.android.inputmethod.latin/com.android.inputmethod.latin.LatinIME\r\ndisable_voicetrigger=0\r\ndouble_tap_to_wake=1\r\ndoze_always_on=0\r\ndoze_enabled=0\r\ndropbox:data_app_anr=disabled\r\ndropbox:data_app_crash=disabled\r\ndropbox:data_app_wtf=disabled\r\nenabled_accessibility_services=com.android.settings/com.android.settings.accessibility.accessibilitymenu.AccessibilityMenuService\r\nenabled_input_methods=com.google.android.googlequicksearchbox/com.google.android.voicesearch.ime.VoiceInputMethodService:com.google.android.inputmethod.latin/com.android.inputmethod.latin.LatinIME\r\nenabled_notification_assistant=com.miui.notification/com.miui.notification.NotificationListener\r\nenabled_notification_listeners=com.google.android.projection.gearhead/com.google.android.gearhead.notifications.SharedNotificationListenerManager$ListenerService:com.google.android.googlequicksearchbox/com.google.android.apps.gsa.notificationlistener.GsaNotificationListenerService:com.crrepa.band.hero/com.crrepa.band.my.notify.service.NotificationCollectorService\r\nenabled_notification_policy_access_packages=com.google.android.apps.wellbeing\r\nenabled_widgets=com.google.android.googlequicksearchbox\r\nface_unlcok_apply_for_lock=1\r\nface_unlock_by_notification_screen_on=1\r\nface_unlock_has_feature=2\r\nface_unlock_model=1\r\nface_unlock_success_show_message=0\r\nface_unlock_success_stay_screen=0\r\nface_unlock_valid_feature=1\r\nfacelock_detection_threshold=0.0\r\nfacelock_liveliness_recognition_threshold=2.2\r\nfacelock_max_center_movement=10.0\r\nfaceunlock_support_superpower=1\r\nfbo_app_size=5000000\r\nfingerprint_apply_to_privacy_password=1\r\nfirst_use_freeform=1\r\nfirst_use_tip_confirm_times=1\r\nfod_auth_fingerprint=0\r\nfreeform_package_name=null\r\nfreeform_timestamps=[\"{com.vng.pubgmobile=[1596206576764], com.lxqd.myhotpotstroy.abroad=[1670586991627, 1670587000692], com.facebook.katana=[1634964008263, 1684237410411, 1684329086303, 1697632560470, 1699332480893, 1704469988499, 1707315132864, 1711461005632, 1714650526190, 1717675356697], com.vzcreations.lightignite=[1618662886790], cn.wps.moffice_eng=[1617197017557, 1617197062207], com.zing.zalo=[1709812324494, 1710418297552, 1712825362151, 1712826340823, 1712826397448, 1713874614311, 1715849541059, 1718802810550, 1719674807239, 1719947153374], com.android.mms=[1678202904223, 1703678917937, 1705464806951, 1707054934479, 1710421648064, 1715764043797, 1715873579377, 1715966005570, 1716119931010, 1716463464563], com.android.chrome=[1619926755305, 1671166183670, 1678471545411, 1681448403055], com.facebook.lite=[1596208475279, 1596208496817, 1608911478224], com.miui.gallery=[1617888592693], com.ss.android.ugc.trill=[1689397155156, 1697632569169, 1704470080550, 1711461017785, 1718774088002], org.telegram.messenger=[1669005618375, 1669005751286], com.facebook.orca=[1717071518397, 1717202727690, 1717202798252, 1717202968675, 1717203787705, 1717256658403, 1717395684255, 1717477557558, 1717589012218, 1717676796731]}\"]\r\nfreeform_window_state=-1\r\nfw_fsgesture_support_superpower=1\r\ngamebooster_data_migration=1\r\ngamebooster_remove_desktop_icon=1\r\ngb_boosting=0\r\ngb_handsfree=0\r\ngb_notification=0\r\nglobal_satisfaction_miui_version=10\r\ngxzw_icon_aod_show_enable=1\r\nhas_new_antispam=1\r\nhuanji_used=1\r\nhush_gesture_used=0\r\nimmersive_mode_confirmations=\r\nin_call_notification_enabled=0\r\ninput_methods_subtype_history=com.google.android.inputmethod.latin/com.android.inputmethod.latin.LatinIME;1891618174:com.vng.inputmethod.labankey/.LatinIME;1891618174\r\ninstall_non_market_apps=1\r\ninvalidCleanAlertNotificationCount=65\r\nkey_app_lock_state_data_migrated=1\r\nkey_clean_alert_switch_state=1\r\nkey_garbage_danger_in_flag=1\r\nkey_garbage_danger_in_size=2000133545\r\nkey_garbage_deepclean_size=13077204932\r\nkey_garbage_facebook_size=2803620\r\nkey_garbage_installed_app_count=50\r\nkey_garbage_multi_video_size=713936920\r\nkey_garbage_normal_size=2000133545\r\nkey_garbage_not_used_app_count=0\r\nkey_garbage_whatsapp_size=0\r\nkey_homelist_cache_deleted=1\r\nkey_is_in_miui_sos_mode=0\r\nkey_latest_battery_status=false;false;false;true;false;false;false;false;false\r\nkey_latest_virus_scan_date=1656422564190\r\nkey_latest_wifi_ssid_bssid=(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(b653590ed4cb0c91224914a41c6c9336998fdd99d69d6eefe9925b2973a2f323,0f4c91568765603d3b852f2049d546b99364fde2cf2a22ab6576828eb892a410);(a67aa7def01786f3c71f978623c5ea92ac62cca273f999826b9a27a3e3834256,4e2168efd870d01a035e87bca1aeb40a107a84efecc821e095253faf7efa78e6);(2e27fb853314ad95048e12344113f9078a5396c5af45fd2e4c315f82fce00931,6507132733c14d9353a055525a644715910672bd18ce6e43b50d6d92b39ad269);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e);(e410e9e3232e180c8ba56c7aa1912a62ae7ac049359bbcdb19c48dbf718b3c05,b63c436e086c7b322e743099ecbfc3b5826f6fa9f0fca0227b59e48d010c3d9e)\r\nkey_launcher_loading_finished=1\r\nkey_long_press_volume_down=none\r\nkey_notificaiton_general_clean_need=1\r\nkey_notificaiton_general_clean_size=2000133545\r\nkey_notificaiton_whatsapp_clean_size=0\r\nkey_notification_wechat_size=0\r\nkey_notification_wechat_size_need=0\r\nkey_notification_whatsapp_clean_need=0\r\nkey_open_earthquake_warning=0\r\nkey_score_in_security=70\r\nkey_xspace_boot_guide_times=1\r\nlast_setup_shown=eclair_1\r\nlocation_changer=2\r\nlocation_mode=0\r\nlocation_providers_allowed=\r\nlock_screen_allow_private_notifications=1\r\nlock_screen_magazine_status=1\r\nlock_screen_owner_info_enabled=0\r\nlock_screen_show_notifications=1\r\nlockscreen.disabled=0\r\nlockscreen.options=enable_facelock\r\nlong_press_timeout=400\r\nmanual_ringer_toggle_count=0\r\nmark_time_agent=0\r\nmark_time_agent_sim_2=0\r\nmark_time_fraud=0\r\nmark_time_fraud_sim_2=0\r\nmark_time_sell=0\r\nmark_time_sell_sim_2=0\r\nmasterLocationPackagePrefixBlacklist=com.google.,com.semaphoremobile.zagat.android\r\nmasterLocationPackagePrefixWhitelist=com.google.android.gms\r\nmedia_button_receiver=\r\nmiui_bubbles_pinned_apps=\r\nmiui_updater_enable=1\r\nmms_backup_enabled=1\r\nmms_backup_in_progress=0\r\nmms_backup_last_completed=1718480240776\r\nmms_restore_account_name=null\r\nmobile_download=0\r\nmock_location=0\r\nmount_play_not_snd=1\r\nmount_ums_autostart=0\r\nmount_ums_notify_enabled=1\r\nmount_ums_prompt=1\r\nmulti_press_timeout=300\r\nnearby_sharing_component=com.google.android.gms/com.google.android.gms.nearby.sharing.ShareSheetActivity\r\nneed_convert_virtual_phone=0\r\nnfc_payment_default_component=com.google.android.gms/com.google.android.gms.tapandpay.hce.service.TpHceService\r\nnotification_badging=1\r\nnotification_bubbles=0\r\nold_clipboard_content_need_clear_new=1\r\noptimizer_scan_cloud=1\r\npackage_changed_browser=1\r\npackage_changed_calendar=1\r\npackage_verifier_state=1\r\npai_selection_page_complete=1\r\npassword_has_promotioned=0\r\npc_security_center_extreme_mode_enter=0\r\npermission_revocation_first_enabled_timestamp_ms=1648485416208\r\nphotos_restore_account_name=null\r\nplay_determined_dma_eligibility=0\r\npower_supersave_tile_enabled=1\r\npref_key_cleaner_uuid=90f5e0bb-a361-4fdb-b5e8-caf9a14c146f\r\npref_open_game_booster=1\r\npreview_plan=0\r\nprint_service_search_uri=https://play.google.com/store/apps/collection/promotion_3000abc_print_services\r\nprivacy_add_account_md5=51e39ddb3bc3a73b7d1fe0f1ae3cc095\r\nprivacy_mode_enabled=0\r\nprivacy_password_countDownTimer_deadline=0\r\nprivacy_password_finger_authentication_num=0\r\nprivacy_password_is_open=1\r\nprivacy_password_status=1\r\nprivacy_password_status_is_record=1\r\nprivacy_status_com.android.providers.downloads.ui=1\r\nprivacy_status_com.miui.securitycenter=1\r\nprivacy_wrong_attempt_num=0\r\npubkey_blacklist=410f36363258f30b347d12ce4863e433437806a8,c4f9663716cd5e71d6950b5f33ce041c95b435d1,e23b8d105f87710a68d9248050ebefc627be4ca6,7b2e16bc39bcd72b456e9f055d1de615b74945db,e8f91200c65cee16e039b9f883841661635f81c5,0129bcd5b448ae8d2496d1c3e19723919088e152,d33c5b41e45cc4b3be9ad6952c4ecc2528032981,e12d89f56d2276f830e6ceafa66c725c0b41a932,d9f5c6ce57ffaa39cc7ed172bd53e0d307834bd1,3ecf4bbbe46096d514bb539bb913d77aa4ef31bf,685eec0a39f668ae8fd8964f987476b4504fd2be,0e502d4dd1e160368a31f06a810431ba6f72c041,93d1532229cc2abd21dff597ee320fe4246f3d0c,f57179faea10c5438cb0c6e1cc277b6e0db2ff54,077ac7de8da558643a06c5369e554faeb3dfa166,e58e315baaeeaac6e72ec9573670ca2f254ec347,384d0c1dc477a7b3f86786d018519f589f1e9e25,c1f9f2894f2e8521507c089f362d5cd9feff7e21,11f75f354126c2c849f53f29cf929604233cf869,e0e90e95afab970b897bfb503f72cb4c06cfb4f3\r\nquick_reply=0\r\nquiet_mode_enable=0\r\nscreen_buttons_has_been_disabled=1\r\nscreen_buttons_state=0\r\nscreen_project_caller=com.miui.securitycenter:ui\r\nscreen_project_hang_up_on=0\r\nscreen_project_in_screening=0\r\nscreen_project_private_on=1\r\nscreen_project_small_window_on=0\r\nscreensaver_activate_on_dock=1\r\nscreensaver_activate_on_sleep=0\r\nscreensaver_components=com.google.android.deskclock/com.android.deskclock.Screensaver\r\nscreensaver_default_component=com.google.android.deskclock/com.android.deskclock.Screensaver\r\nscreensaver_enabled=1\r\nselected_input_method_subtype=-1\r\nselected_spell_checker=com.google.android.inputmethod.latin/com.android.inputmethod.latin.spellcheck.AndroidSpellCheckerService\r\nselected_spell_checker_subtype=0\r\nsend_action_app_error=1\r\nserial_blacklist=\r\nsettings_face_id_prefix_5=Khuôn mặt 1\r\nsettings_face_id_prefix_6=Khuôn mặt 2\r\nsettings_face_id_prefix_7=Khuôn mặt 1\r\nsettings_face_id_prefix_8=Khuôn mặt 2\r\nsettings_fingerprint_id_prefix_-128000784=Vân tay4\r\nsettings_fingerprint_id_prefix_-1383024586=Vân tay5\r\nsettings_fingerprint_id_prefix_-1615912973=Vân tay5\r\nsettings_fingerprint_id_prefix_-1820902113=Vân tay5\r\nsettings_fingerprint_id_prefix_-1954678606=Vân tay1\r\nsettings_fingerprint_id_prefix_-2109803503=Vân tay1\r\nsettings_fingerprint_id_prefix_-312740984=Vân tay2\r\nsettings_fingerprint_id_prefix_-34436474=Vân tay5\r\nsettings_fingerprint_id_prefix_-469956686=Vân tay4\r\nsettings_fingerprint_id_prefix_-506813775=Vân tay2\r\nsettings_fingerprint_id_prefix_-786886107=Vân tay3\r\nsettings_fingerprint_id_prefix_-83753695=Vân tay2\r\nsettings_fingerprint_id_prefix_1271325661=Vân tay3\r\nsettings_fingerprint_id_prefix_1292573291=Vân tay5\r\nsettings_fingerprint_id_prefix_1327387519=Vân tay4\r\nsettings_fingerprint_id_prefix_1718788240=Vân tay2\r\nsettings_fingerprint_id_prefix_1801536666=Vân tay1\r\nsettings_fingerprint_id_prefix_2141630964=Vân tay3\r\nsettings_fingerprint_id_prefix_36833109=Vân tay2\r\nsettings_fingerprint_id_prefix_526937310=Vân tay3\r\nsettings_fingerprint_id_prefix_681645577=Vân tay1\r\nsettings_fingerprint_id_prefix_769120380=Vân tay1\r\nsettings_fingerprint_id_prefix_801484520=Vân tay3\r\nsettings_fingerprint_id_prefix_848125878=Vân tay4\r\nshare_tail_disable=1\r\nshield_super_save_bar=1\r\nshortcut_recall_interval=604800000\r\nshortcut_recall_invalid=1\r\nshortcut_recall_max_count=2\r\nshow_first_crash_dialog_dev_option=0\r\nshow_ime_with_hard_keyboard=0\r\nshow_zen_settings_suggestion=0\r\nsidebar_bounds=\r\nsilence_gesture=not_set\r\nskip_gesture=not_set\r\nsleep_timeout=-1\r\nsnoozed_schedule_condition_provider=\r\nspeak_password=1\r\nssl_session_cache=file\r\nstatus_memory_clean_shortcut=1\r\nstatus_whatsapp_shortcut=0\r\nsync_parent_sounds=1\r\nsystemui_fsgesture_support_superpower=1\r\nsysui_qs_tiles=cell,bt,wifi,flashlight,autobrightness,mute,screenshot,airplane,screenlock,gps,rotation,hotspot,scanner,papermode,night,quietmode,batterysaver,custom(com.miui.securitycenter/com.miui.superpower.notification.SuperPowerTileService),custom(com.milink.service/com.milink.ui.service.MiLinkTileService),custom(com.miui.mishare.connectivity/.tile.MiShareTileService),freeformhang,custom(com.miui.screenrecorder/.service.QuickService),vibrate,nfc,edit\r\nsysui_tuner_version=1\r\ntouch_exploration_enabled=0\r\ntouch_exploration_granted_accessibility_services=null\r\ntrust_agents_initialized=1\r\ntts_default_synth=com.google.android.tts\r\nui_night_mode=1\r\nunknown_sources_default_reversed=1\r\nunsafe_volume_music_active_ms=3120000\r\nupload_debug_log_pref=1\r\nupload_log_pref=1\r\nusb_audio_automatic_routing_disabled=0\r\nuser_full_data_backup_aware=1\r\nuser_setup_complete=1\r\nuser_setup_personalization_state=10\r\nvoice_interaction_service=com.google.android.googlequicksearchbox/com.google.android.voiceinteraction.GsaVoiceInteractionService\r\nvoice_recognition_service=com.google.android.googlequicksearchbox/com.google.android.voicesearch.serviceapi.GoogleRecognitionService\r\nvolume_hush_gesture=1\r\nvpn_password_enable=1\r\nvtb_boosting=0\r\nwake_gesture_enabled=0\r\nweb_autofill_query_url=http://android.clients.google.com/proxy/webautofill\r\nxspace_enabled=0\r\nzen_duration=0\r\nzen_settings_suggestion_viewed=0\r\nzen_settings_updated=1\r\nzman_cloud_disable=0\r\nzman_share_hide_camera=1\r\nzman_share_hide_camera_default=1\r\nzman_share_hide_location=1\r\nzman_share_hide_location_default=1";
            }

            string settings_face_id_prefix = GetCount(str_BaoMat, "Khuôn mặt ").ToString();
            txtSoKhuonMat.Text = settings_face_id_prefix;

            string settings_fingerprint_id_prefix = GetCount(str_BaoMat, "Vân tay").ToString();
            txtSoVanTay.Text = settings_fingerprint_id_prefix;
        }

        private void panel_MaPin_Resize(object sender, EventArgs e)
        {
            Load_Size_MaPin();
        }

        private void panel_MatMa_Resize(object sender, EventArgs e)
        {
            Load_Size_MatMa();
        }

        private void panel_MauHinh_Resize(object sender, EventArgs e)
        {
            Load_Size_MauHinh();
        }

        private void panel_SinhTracHoc_Resize(object sender, EventArgs e)
        {
            Load_Size_SinhTracHoc();
        }

        private void groupPanel_DoiMatKhau_Resize(object sender, EventArgs e)
        {
            Load_Size();
        }
    }
}
